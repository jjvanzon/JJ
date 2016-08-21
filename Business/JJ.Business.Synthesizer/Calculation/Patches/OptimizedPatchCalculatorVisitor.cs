﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JJ.Business.Synthesizer.Calculation.Arrays;
using JJ.Business.Synthesizer.Calculation.Curves;
using JJ.Business.Synthesizer.Calculation.Operators;
using JJ.Business.Synthesizer.Calculation.Random;
using JJ.Business.Synthesizer.Calculation.Samples;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Validation.Operators;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Common;
using JJ.Framework.Common.Exceptions;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Calculation.Patches
{
    /// <summary>
    /// The way this class works, is that the base visitor visits an Operator's Inlets,
    /// which will lead to Calculator objects to be put on a stack.
    /// Then the base class calls the appropriate specialized visit method for the Operator, e.g. VisitAdd,
    /// which can then pop its operands from this stack, 
    /// and decide which Calculator to push onto the stack again.
    /// </summary>
    internal partial class OptimizedPatchCalculatorVisitor : OperatorVisitorBase
    {
        /// <summary>
        /// Feature switch: not being able to vary bundle dimension values
        /// during the calculation is faster, but would not give the expected behavior.
        /// However, we keep the code alive, to be able to experiment with the performance impact.
        /// </summary>
        private const bool BUNDLE_POSITIONS_ARE_INVARIANT = false;
        private const double DEFAULT_DIMENSION_VALUE = 0.0;

        private readonly Outlet _outlet;
        private readonly int _samplingRate;
        private readonly double _nyquistFrequency;
        private readonly int _channelCount;
        private readonly int _samplesBetweenApplyFilterVariables;
        private readonly CalculatorCache _calculatorCache;
        private readonly ICurveRepository _curveRepository;
        private readonly ISampleRepository _sampleRepository;
        private readonly IPatchRepository _patchRepository;
        private readonly ISpeakerSetupRepository _speakerSetupRepository;

        private Stack<OperatorCalculatorBase> _stack;
        private DimensionStackCollection _dimensionStackCollection;

        private Dictionary<Operator, double> _operator_NoiseOffsetInSeconds_Dictionary;
        private Dictionary<Operator, int> _operator_RandomOffsetInSeconds_Dictionary;
        private Dictionary<Operator, VariableInput_OperatorCalculator> _patchInlet_Calculator_Dictionary;
        private IList<ResettableOperatorTuple> _resettableOperatorTuples;

        public OptimizedPatchCalculatorVisitor(
            Outlet outlet, 
            int samplingRate,
            int channelCount,
            double secondsBetweenApplyFilterVariables,
            CalculatorCache calculatorCache,
            ICurveRepository curveRepository,
            ISampleRepository sampleRepository,
            IPatchRepository patchRepository,
            ISpeakerSetupRepository speakerSetupRepository)
        {
            if (outlet == null) throw new NullException(() => outlet);
            if (calculatorCache == null) throw new NullException(() => calculatorCache);
            if (curveRepository == null) throw new NullException(() => curveRepository);
            if (sampleRepository == null) throw new NullException(() => sampleRepository);
            if (patchRepository == null) throw new NullException(() => patchRepository);
            if (speakerSetupRepository == null) throw new NullException(() => speakerSetupRepository);

            _outlet = outlet;
            _samplingRate = samplingRate;
            _channelCount = channelCount;
            _calculatorCache = calculatorCache;
            _curveRepository = curveRepository;
            _sampleRepository = sampleRepository;
            _patchRepository = patchRepository;
            _speakerSetupRepository = speakerSetupRepository;

            _nyquistFrequency = _samplingRate / 2.0;

            double samplesBetweenApplyFilterVariablesDouble = secondsBetweenApplyFilterVariables * samplingRate;
            if (!ConversionHelper.CanCastToPositiveInt32(samplesBetweenApplyFilterVariablesDouble))
            {
                throw new Exception(String.Format("samplesBetweenApplyFilterVariablesDouble {0} cannot be cast to positive Int32.", samplesBetweenApplyFilterVariablesDouble));
            }
            _samplesBetweenApplyFilterVariables = (int)(secondsBetweenApplyFilterVariables * samplingRate);
        }

        /// <param name="channelCount">Used for e.g. mixing channels of samples into one channel.</param>
        public OptimizedPatchCalculatorVisitorResult Execute()
        {
            IValidator validator = new Recursive_OperatorValidator(
                _outlet.Operator,
                _curveRepository, _sampleRepository, _patchRepository,
                alreadyDone: new HashSet<object>());
            validator.Assert();

            _stack = new Stack<OperatorCalculatorBase>();
            _dimensionStackCollection = new DimensionStackCollection();
            _operator_NoiseOffsetInSeconds_Dictionary = new Dictionary<Operator, double>();
            _operator_RandomOffsetInSeconds_Dictionary = new Dictionary<Operator, int>();
            _patchInlet_Calculator_Dictionary = new Dictionary<Operator, VariableInput_OperatorCalculator>();
            _resettableOperatorTuples = new List<ResettableOperatorTuple>();

            VisitOutlet(_outlet);

            OperatorCalculatorBase outputOperatorCalculator = _stack.Pop();

            if (_stack.Count != 0)
            {
                throw new NotEqualException(() => _stack.Count, 0);
            }

            foreach (DimensionEnum dimensionEnum in EnumHelper.GetValues<DimensionEnum>())
            {
                DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
                if (dimensionStack.Count != 1) // 1, because a single item is added by default as when the DimensionStackCollection is initialized.
                {
                    throw new Exception(String.Format("DimensionStack.Count for DimensionEnum '{0}' should be 1 but it is {1}.", dimensionEnum, dimensionStack.Count));
                }
            }

            return new OptimizedPatchCalculatorVisitorResult(
                _dimensionStackCollection,
                outputOperatorCalculator,
                _patchInlet_Calculator_Dictionary.Values.ToArray(),
                _resettableOperatorTuples);
        }

        protected override void VisitAbsolute(Operator op)
        {
            base.VisitAbsolute(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase calculatorX = _stack.Pop();
            calculatorX = calculatorX ?? new Zero_OperatorCalculator();
            double x = calculatorX.Calculate();
            bool xIsConst = calculatorX is Number_OperatorCalculator;

            if (xIsConst)
            {
                double value;

                if (x >= 0.0)
                {
                    value = x;
                }
                else
                {
                    value = -x;
                }

                calculator = new Number_OperatorCalculator(value);
            }
            else if (!xIsConst)
            {
                calculator = new Absolute_OperatorCalculator(calculatorX);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitAdd(Operator op)
        {
            base.VisitAdd(op);

            OperatorCalculatorBase calculator;

            IList<OperatorCalculatorBase> operandCalculators = new List<OperatorCalculatorBase>(op.Inlets.Count);
            for (int i = 0; i < op.Inlets.Count; i++)
            {
                OperatorCalculatorBase operandCalculator = _stack.Pop();
                operandCalculators.Add(operandCalculator);
            }

            operandCalculators = TruncateOperandCalculatorList(operandCalculators, x => x.Sum());

            // Get rid of const zero.
            operandCalculators.TryRemoveFirst(x => x is Number_OperatorCalculator &&
                                                   x.Calculate() == 0.0);

            switch (operandCalculators.Count)
            {
                case 0:
                    calculator = new Zero_OperatorCalculator();
                    break;

                case 1:
                    calculator = operandCalculators[0];
                    break;

                default:
                    OperatorCalculatorBase constOperandCalculator = operandCalculators.Where(x => x is Number_OperatorCalculator)
                                                                                      .SingleOrDefault();
                    if (constOperandCalculator == null)
                    {
                        calculator = OperatorCalculatorFactory.CreateAddCalculatorOnlyVars(operandCalculators);
                    }
                    else
                    {
                        IList<OperatorCalculatorBase> varOperandCalculators = operandCalculators.Except(constOperandCalculator).ToArray();
                        double constValue = constOperandCalculator.Calculate();

                        calculator = OperatorCalculatorFactory.CreateAddCalculatorWithConst(constValue, varOperandCalculators);
                    }
                    break;
            }

            _stack.Push(calculator);
        }

        protected override void VisitAllPassFilter(Operator op)
        {
            base.VisitAllPassFilter(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase centerFrequencyCalculator = _stack.Pop();
            OperatorCalculatorBase bandWidthCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            centerFrequencyCalculator = centerFrequencyCalculator ?? new Zero_OperatorCalculator();
            bandWidthCalculator = bandWidthCalculator ?? new Zero_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool centerFrequencyIsConst = centerFrequencyCalculator is Number_OperatorCalculator;
            bool bandWidthIsConst = bandWidthCalculator is Number_OperatorCalculator;

            double signal = signalIsConst ? signalCalculator.Calculate() : 0.0;
            double centerFrequency = centerFrequencyIsConst ? centerFrequencyCalculator.Calculate() : 0.0;
            double bandWidth = bandWidthIsConst ? bandWidthCalculator.Calculate() : 0.0;

            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool centerFrequencyIsConstSpecialValue = centerFrequencyIsConst && DoubleHelper.IsSpecialValue(centerFrequency);
            bool bandWidthIsConstSpecialValue = bandWidthIsConst && DoubleHelper.IsSpecialValue(bandWidth);

            if (centerFrequency > _nyquistFrequency) centerFrequency = _nyquistFrequency;

            if (signalIsConstSpecialValue || centerFrequencyIsConstSpecialValue || bandWidthIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConst)
            {
                // There are no frequencies. So you a filter should do nothing.
                calculator = signalCalculator;
            }
            else if (centerFrequencyIsConst && bandWidthIsConst)
            {
                calculator = new AllPassFilter_OperatorCalculator_ManyConsts(
                    signalCalculator,
                    centerFrequency,
                    bandWidth,
                    _samplingRate);
            }
            else
            {
                calculator = new AllPassFilter_OperatorCalculator_AllVars(
                    signalCalculator,
                    centerFrequencyCalculator,
                    bandWidthCalculator,
                    _samplingRate,
                    _samplesBetweenApplyFilterVariables);
            }

            _stack.Push(calculator);
        }

        protected override void VisitAnd(Operator op)
        {
            base.VisitAnd(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase calculatorA = _stack.Pop();
            OperatorCalculatorBase calculatorB = _stack.Pop();

            calculatorA = calculatorA ?? new Zero_OperatorCalculator();
            calculatorB = calculatorB ?? new Zero_OperatorCalculator();

            bool aIsConst = calculatorA is Number_OperatorCalculator;
            bool bIsConst = calculatorB is Number_OperatorCalculator;

            double a = aIsConst ? calculatorA.Calculate() : 0.0;
            double b = bIsConst ? calculatorB.Calculate() : 0.0;

            bool aIsConstZero = aIsConst && a == 0.0;
            bool bIsConstZero = bIsConst && b == 0.0;
            bool aIsConstNonZero = aIsConst && a != 0.0;
            bool bIsConstNonZero = bIsConst && b != 0.0;

            if (!aIsConst && !bIsConst)
            {
                calculator = new And_OperatorCalculator_VarA_VarB(calculatorA, calculatorB);
            }
            else if (aIsConstNonZero && bIsConstNonZero)
            {
                calculator = new One_OperatorCalculator();
            }
            else if (aIsConstZero || bIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (aIsConstNonZero && !bIsConst)
            {
                calculator = calculatorB;
            }
            else if (!aIsConst && bIsConstNonZero)
            {
                calculator = calculatorA;
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitAverage(Operator op)
        {
            base.VisitAverage(op);

            OperatorCalculatorBase calculator;

            IList<OperatorCalculatorBase> operandCalculators = new List<OperatorCalculatorBase>(op.Inlets.Count);
            for (int i = 0; i < op.Inlets.Count; i++)
            {
                OperatorCalculatorBase operandCalculator = _stack.Pop();
                operandCalculators.Add(operandCalculator);
            }

            operandCalculators = TruncateOperandCalculatorList(operandCalculators, x => x.Average());

            switch (operandCalculators.Count)
            {
                case 0:
                    calculator = new Zero_OperatorCalculator();
                    break;

                case 1:
                    // Also covers the 'all are const' situation, since all consts are aggregated to one in earlier code.
                    calculator = operandCalculators[0];
                    break;

                default:
                    calculator = new Average_OperatorCalculator(operandCalculators);
                    break;
            }

            _stack.Push(calculator);
        }

        protected override void VisitAverageOverDimension(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitAverageOverDimension(op);

            OperatorCalculatorBase operatorCalculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase fromCalculator = _stack.Pop();
            OperatorCalculatorBase tillCalculator = _stack.Pop();
            OperatorCalculatorBase stepCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            fromCalculator = fromCalculator ?? new Zero_OperatorCalculator();
            tillCalculator = tillCalculator ?? new Zero_OperatorCalculator();
            stepCalculator = stepCalculator ?? new One_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool fromIsConst = fromCalculator is Number_OperatorCalculator;
            bool tillIsConst = tillCalculator is Number_OperatorCalculator;
            bool stepIsConst = stepCalculator is Number_OperatorCalculator;

            double from = fromIsConst ? fromCalculator.Calculate() : 0.0;
            double till = tillIsConst ? tillCalculator.Calculate() : 0.0;
            double step = stepIsConst ? stepCalculator.Calculate() : 0.0;

            bool stepIsConstZero = stepIsConst && step == 0.0;
            bool stepIsConstNegative = stepIsConst && step < 0.0;
            bool fromIsConstSpecialValue = fromIsConst && DoubleHelper.IsSpecialValue(from);
            bool tillIsConstSpecialValue = tillIsConst && DoubleHelper.IsSpecialValue(till);
            bool stepIsConstSpecialValue = stepIsConst && DoubleHelper.IsSpecialValue(step);

            if (signalIsConst)
            {
                operatorCalculator = signalCalculator;
            }
            else if (stepIsConstZero)
            {
                operatorCalculator = new Zero_OperatorCalculator();
            }
            else if (stepIsConstNegative)
            {
                operatorCalculator = new Zero_OperatorCalculator();
            }
            else if (fromIsConstSpecialValue || tillIsConstSpecialValue || stepIsConstSpecialValue)
            {
                operatorCalculator = new Number_OperatorCalculator(Double.NaN);
            }
            else
            {
                var wrapper = new AverageOverDimension_OperatorWrapper(op);
                CollectionRecalculationEnum collectionRecalculationEnum = wrapper.CollectionRecalculation;
                switch (collectionRecalculationEnum)
                {
                    case CollectionRecalculationEnum.Continuous:
                        operatorCalculator = new AverageOverDimension_OperatorCalculator_CollectionRecalculationContinuous(
                            signalCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        break;

                    case CollectionRecalculationEnum.UponReset:
                        operatorCalculator = new AverageOverDimension_OperatorCalculator_CollectionRecalculationUponReset(
                            signalCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        break;

                    default:
                        throw new ValueNotSupportedException(collectionRecalculationEnum);
                }
            }

            _stack.Push(operatorCalculator);
        }

        protected override void VisitAverageFollower(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitAverageFollower(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase sliceLengthCalculator = _stack.Pop();
            OperatorCalculatorBase sampleCountCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            sliceLengthCalculator = sliceLengthCalculator ?? new One_OperatorCalculator();
            sampleCountCalculator = sampleCountCalculator ?? new One_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;

            if (signalIsConst)
            {
                calculator = signalCalculator;
            }
            else
            {
                calculator = new AverageFollower_OperatorCalculator(signalCalculator, sliceLengthCalculator, sampleCountCalculator, dimensionStack);
            }

            _stack.Push(calculator);
        }

        protected override void VisitBandPassFilterConstantPeakGain(Operator op)
        {
            base.VisitBandPassFilterConstantPeakGain(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase centerFrequencyCalculator = _stack.Pop();
            OperatorCalculatorBase bandWidthCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            centerFrequencyCalculator = centerFrequencyCalculator ?? new Zero_OperatorCalculator();
            bandWidthCalculator = bandWidthCalculator ?? new Zero_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool centerFrequencyIsConst = centerFrequencyCalculator is Number_OperatorCalculator;
            bool bandWidthIsConst = bandWidthCalculator is Number_OperatorCalculator;

            double signal = signalIsConst ? signalCalculator.Calculate() : 0.0;
            double centerFrequency = centerFrequencyIsConst ? centerFrequencyCalculator.Calculate() : 0.0;
            double bandWidth = bandWidthIsConst ? bandWidthCalculator.Calculate() : 0.0;

            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool centerFrequencyIsConstZero = centerFrequencyIsConst && centerFrequency == 0.0;
            bool centerFrequencyIsConstSpecialValue = centerFrequencyIsConst && DoubleHelper.IsSpecialValue(centerFrequency);
            bool bandWidthIsConstSpecialValue = bandWidthIsConst && DoubleHelper.IsSpecialValue(bandWidth);

            if (centerFrequency > _nyquistFrequency) centerFrequency = _nyquistFrequency;

            if (signalIsConstSpecialValue || centerFrequencyIsConstSpecialValue || bandWidthIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConst)
            {
                // There are no frequencies. So you a filter should do nothing.
                calculator = signalCalculator;
            }
            else if (centerFrequencyIsConstZero)
            {
                // No filtering
                calculator = signalCalculator;
            }
            else if (centerFrequencyIsConst && bandWidthIsConst)
            {
                calculator = new BandPassFilterConstantPeakGain_OperatorCalculator_ConstCenterFrequency_ConstBandWidth(
                    signalCalculator,
                    centerFrequency,
                    bandWidth,
                    _samplingRate);
            }
            else
            {
                calculator = new BandPassFilterConstantPeakGain_OperatorCalculator_VarCenterFrequency_VarBandWidth(
                    signalCalculator,
                    centerFrequencyCalculator,
                    bandWidthCalculator,
                    _samplingRate,
                    _samplesBetweenApplyFilterVariables);
            }

            _stack.Push(calculator);
        }

        protected override void VisitBandPassFilterConstantTransitionGain(Operator op)
        {
            base.VisitBandPassFilterConstantTransitionGain(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase centerFrequencyCalculator = _stack.Pop();
            OperatorCalculatorBase bandWidthCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            centerFrequencyCalculator = centerFrequencyCalculator ?? new Zero_OperatorCalculator();
            bandWidthCalculator = bandWidthCalculator ?? new Zero_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool centerFrequencyIsConst = centerFrequencyCalculator is Number_OperatorCalculator;
            bool bandWidthIsConst = bandWidthCalculator is Number_OperatorCalculator;

            double signal = signalIsConst ? signalCalculator.Calculate() : 0.0;
            double centerFrequency = centerFrequencyIsConst ? centerFrequencyCalculator.Calculate() : 0.0;
            double bandWidth = bandWidthIsConst ? bandWidthCalculator.Calculate() : 0.0;

            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool centerFrequencyIsConstZero = centerFrequencyIsConst && centerFrequency == 0.0;
            bool centerFrequencyIsConstSpecialValue = centerFrequencyIsConst && DoubleHelper.IsSpecialValue(centerFrequency);
            bool bandWidthIsConstSpecialValue = bandWidthIsConst && DoubleHelper.IsSpecialValue(bandWidth);

            if (centerFrequency > _nyquistFrequency) centerFrequency = _nyquistFrequency;

            if (signalIsConstSpecialValue || centerFrequencyIsConstSpecialValue || bandWidthIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConst)
            {
                // There are no frequencies. So you a filter should do nothing.
                calculator = signalCalculator;
            }
            else if (centerFrequencyIsConstZero)
            {
                // No filtering
                calculator = signalCalculator;
            }
            else if (centerFrequencyIsConst && bandWidthIsConst)
            {
                calculator = new BandPassFilterConstantTransitionGain_OperatorCalculator_ConstCenterFrequency_ConstBandWidth(
                    signalCalculator,
                    centerFrequency,
                    bandWidth,
                    _samplingRate);
            }
            else
            {
                calculator = new BandPassFilterConstantTransitionGain_OperatorCalculator_VarCenterFrequency_VarBandWidth(
                    signalCalculator,
                    centerFrequencyCalculator,
                    bandWidthCalculator,
                    _samplingRate,
                    _samplesBetweenApplyFilterVariables);
            }

            _stack.Push(calculator);
        }

        protected override void VisitBundle(Operator op)
        {
            if (BUNDLE_POSITIONS_ARE_INVARIANT)
            {
                throw new Exception("VisitBundle should not execute if BUNDLE_POSITIONS_ARE_INVARIANT.");
            }

            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            // No pushing and popping from the dimension stack here.

            base.VisitBundle(op);

            var operandCalculators = new List<OperatorCalculatorBase>(op.Inlets.Count);

            for (int i = 0; i < op.Inlets.Count; i++)
            {
                OperatorCalculatorBase operandCalculator = _stack.Pop();

                operandCalculator = operandCalculator ?? new Zero_OperatorCalculator();

                operandCalculators.Add(operandCalculator);
            }

            OperatorCalculatorBase calculator = new Bundle_OperatorCalculator(operandCalculators, dimensionStack);

            _stack.Push(calculator);
        }

        protected override void VisitCache(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            DimensionStack channelDimensionStack = _dimensionStackCollection.GetDimensionStack(DimensionEnum.Channel);

            base.VisitCache(op);

            OperatorCalculatorBase calculator = null;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase startCalculator = _stack.Pop();
            OperatorCalculatorBase endCalculator = _stack.Pop();
            OperatorCalculatorBase samplingRateCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            startCalculator = startCalculator ?? new Zero_OperatorCalculator();
            endCalculator = endCalculator ?? new One_OperatorCalculator();
            samplingRateCalculator = samplingRateCalculator ?? new One_OperatorCalculator();

            double start = startCalculator.Calculate();
            double end = endCalculator.Calculate();
            double samplingRate = samplingRateCalculator.Calculate();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;

            // We need a lot of lenience in this code, because validity is dependent on user input,
            // and this validity cannot be checked on the entity level, only when starting the calculation.
            // In theory I could generate additional messages in the calculation optimization process,
            // but we should keep it possible to reoptimize in runtime, and we cannot obtrusively interrupt
            // the user with validation messages, because he is busy making music and the show must go on.
            bool startIsValid = !DoubleHelper.IsSpecialValue(start);
            bool endIsValid = !DoubleHelper.IsSpecialValue(end);
            bool samplingRateIsValid = ConversionHelper.CanCastToInt32(samplingRate) && (int)samplingRate > 0;
            bool startComparedToEndIsValid = end > start;
            bool valuesAreValid = startIsValid &&
                                  endIsValid &&
                                  samplingRateIsValid &&
                                  startComparedToEndIsValid;
            if (!valuesAreValid)
            {
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConst)
            {
                calculator = signalCalculator;
            }
            else
            {
                IList<ArrayCalculatorBase> arrayCalculators = _calculatorCache.GetCacheArrayCalculators(
                    op,
                    signalCalculator,
                    start,
                    end,
                    samplingRate,
                    dimensionStack,
                    channelDimensionStack,
                    _speakerSetupRepository);

                bool hasMinPosition = start != 0.0;
                var wrapper = new Cache_OperatorWrapper(op);
                InterpolationTypeEnum interpolationTypeEnum = wrapper.InterpolationType;

                if (hasMinPosition)
                {
                    if (arrayCalculators.Count == 1)
                    {
                        ArrayCalculatorBase arrayCalculator = arrayCalculators[0];
                        {
                            var castedArrayCalculator = arrayCalculator as ArrayCalculator_MinPosition_Block;
                            if (castedArrayCalculator != null)
                            {
                                calculator = new Cache_OperatorCalculator_SingleChannel<ArrayCalculator_MinPosition_Block>(castedArrayCalculator, dimensionStack);
                            }
                        }
                        {
                            var castedArrayCalculator = arrayCalculator as ArrayCalculator_MinPosition_Cubic;
                            if (castedArrayCalculator != null)
                            {
                                calculator = new Cache_OperatorCalculator_SingleChannel<ArrayCalculator_MinPosition_Cubic>(castedArrayCalculator, dimensionStack);
                            }
                        }
                        {
                            var castedArrayCalculator = arrayCalculator as ArrayCalculator_MinPosition_Hermite;
                            if (castedArrayCalculator != null)
                            {
                                calculator = new Cache_OperatorCalculator_SingleChannel<ArrayCalculator_MinPosition_Hermite>(castedArrayCalculator, dimensionStack);
                            }
                        }
                        {
                            var castedArrayCalculator = arrayCalculator as ArrayCalculator_MinPosition_Line;
                            if (castedArrayCalculator != null)
                            {
                                calculator = new Cache_OperatorCalculator_SingleChannel<ArrayCalculator_MinPosition_Line>(castedArrayCalculator, dimensionStack);
                            }
                        }
                        {
                            var castedArrayCalculator = arrayCalculator as ArrayCalculator_MinPosition_Stripe;
                            if (castedArrayCalculator != null)
                            {
                                calculator = new Cache_OperatorCalculator_SingleChannel<ArrayCalculator_MinPosition_Stripe>(castedArrayCalculator, dimensionStack);
                            }
                        }
                    }
                    else
                    {
                        // arrayCalculators.Count != 1

                        switch (interpolationTypeEnum)
                        {
                            case InterpolationTypeEnum.Block:
                                {
                                    IList<ArrayCalculator_MinPosition_Block> castedArrayCalculators = arrayCalculators.Select(x => (ArrayCalculator_MinPosition_Block)x).ToArray();
                                    calculator = new Cache_OperatorCalculator_MultiChannel<ArrayCalculator_MinPosition_Block>(castedArrayCalculators, dimensionStack, channelDimensionStack);
                                    break;
                                }

                            case InterpolationTypeEnum.Cubic:
                                {
                                    IList<ArrayCalculator_MinPosition_Cubic> castedArrayCalculators = arrayCalculators.Select(x => (ArrayCalculator_MinPosition_Cubic)x).ToArray();
                                    calculator = new Cache_OperatorCalculator_MultiChannel<ArrayCalculator_MinPosition_Cubic>(castedArrayCalculators, dimensionStack, channelDimensionStack);
                                    break;
                                }

                            case InterpolationTypeEnum.Hermite:
                                {
                                    IList<ArrayCalculator_MinPosition_Hermite> castedArrayCalculators = arrayCalculators.Select(x => (ArrayCalculator_MinPosition_Hermite)x).ToArray();
                                    calculator = new Cache_OperatorCalculator_MultiChannel<ArrayCalculator_MinPosition_Hermite>(castedArrayCalculators, dimensionStack, channelDimensionStack);
                                    break;
                                }

                            case InterpolationTypeEnum.Line:
                                {
                                    IList<ArrayCalculator_MinPosition_Line> castedArrayCalculators = arrayCalculators.Select(x => (ArrayCalculator_MinPosition_Line)x).ToArray();
                                    calculator = new Cache_OperatorCalculator_MultiChannel<ArrayCalculator_MinPosition_Line>(castedArrayCalculators, dimensionStack, channelDimensionStack);
                                    break;
                                }

                            case InterpolationTypeEnum.Stripe:
                                {
                                    IList<ArrayCalculator_MinPosition_Stripe> castedArrayCalculators = arrayCalculators.Select(x => (ArrayCalculator_MinPosition_Stripe)x).ToArray();
                                    calculator = new Cache_OperatorCalculator_MultiChannel<ArrayCalculator_MinPosition_Stripe>(castedArrayCalculators, dimensionStack, channelDimensionStack);
                                    break;
                                }

                            default:
                                throw new ValueNotSupportedException(interpolationTypeEnum);
                        }
                    }
                }
                else
                {
                    // !hasMinPosition
                    if (arrayCalculators.Count == 1)
                    {
                        ArrayCalculatorBase arrayCalculator = arrayCalculators[0];
                        {
                            var castedArrayCalculator = arrayCalculator as ArrayCalculator_MinPositionZero_Block;
                            if (castedArrayCalculator != null)
                            {
                                calculator = new Cache_OperatorCalculator_SingleChannel<ArrayCalculator_MinPositionZero_Block>(castedArrayCalculator, dimensionStack);
                            }
                        }
                        {
                            var castedArrayCalculator = arrayCalculator as ArrayCalculator_MinPositionZero_Cubic;
                            if (castedArrayCalculator != null)
                            {
                                calculator = new Cache_OperatorCalculator_SingleChannel<ArrayCalculator_MinPositionZero_Cubic>(castedArrayCalculator, dimensionStack);
                            }
                        }
                        {
                            var castedArrayCalculator = arrayCalculator as ArrayCalculator_MinPositionZero_Hermite;
                            if (castedArrayCalculator != null)
                            {
                                calculator = new Cache_OperatorCalculator_SingleChannel<ArrayCalculator_MinPositionZero_Hermite>(castedArrayCalculator, dimensionStack);
                            }
                        }
                        {
                            var castedArrayCalculator = arrayCalculator as ArrayCalculator_MinPositionZero_Line;
                            if (castedArrayCalculator != null)
                            {
                                calculator = new Cache_OperatorCalculator_SingleChannel<ArrayCalculator_MinPositionZero_Line>(castedArrayCalculator, dimensionStack);
                            }
                        }
                        {
                            var castedArrayCalculator = arrayCalculator as ArrayCalculator_MinPositionZero_Stripe;
                            if (castedArrayCalculator != null)
                            {
                                calculator = new Cache_OperatorCalculator_SingleChannel<ArrayCalculator_MinPositionZero_Stripe>(castedArrayCalculator, dimensionStack);
                            }
                        }
                    }
                    else
                    {
                        // arrayCalculators.Count != 1

                        switch (interpolationTypeEnum)
                        {
                            case InterpolationTypeEnum.Block:
                                {
                                    IList<ArrayCalculator_MinPositionZero_Block> castedArrayCalculators = arrayCalculators.Select(x => (ArrayCalculator_MinPositionZero_Block)x).ToArray();
                                    calculator = new Cache_OperatorCalculator_MultiChannel<ArrayCalculator_MinPositionZero_Block>(castedArrayCalculators, dimensionStack, channelDimensionStack);
                                    break;
                                }

                            case InterpolationTypeEnum.Cubic:
                                {
                                    IList<ArrayCalculator_MinPositionZero_Cubic> castedArrayCalculators = arrayCalculators.Select(x => (ArrayCalculator_MinPositionZero_Cubic)x).ToArray();
                                    calculator = new Cache_OperatorCalculator_MultiChannel<ArrayCalculator_MinPositionZero_Cubic>(castedArrayCalculators, dimensionStack, channelDimensionStack);
                                    break;
                                }

                            case InterpolationTypeEnum.Hermite:
                                {
                                    IList<ArrayCalculator_MinPositionZero_Hermite> castedArrayCalculators = arrayCalculators.Select(x => (ArrayCalculator_MinPositionZero_Hermite)x).ToArray();
                                    calculator = new Cache_OperatorCalculator_MultiChannel<ArrayCalculator_MinPositionZero_Hermite>(castedArrayCalculators, dimensionStack, channelDimensionStack);
                                    break;
                                }

                            case InterpolationTypeEnum.Line:
                                {
                                    IList<ArrayCalculator_MinPositionZero_Line> castedArrayCalculators = arrayCalculators.Select(x => (ArrayCalculator_MinPositionZero_Line)x).ToArray();
                                    calculator = new Cache_OperatorCalculator_MultiChannel<ArrayCalculator_MinPositionZero_Line>(castedArrayCalculators, dimensionStack, channelDimensionStack);
                                    break;
                                }

                            case InterpolationTypeEnum.Stripe:
                                {
                                    IList<ArrayCalculator_MinPositionZero_Stripe> castedArrayCalculators = arrayCalculators.Select(x => (ArrayCalculator_MinPositionZero_Stripe)x).ToArray();
                                    calculator = new Cache_OperatorCalculator_MultiChannel<ArrayCalculator_MinPositionZero_Stripe>(castedArrayCalculators, dimensionStack, channelDimensionStack);
                                    break;
                                }

                            default:
                                throw new ValueNotSupportedException(interpolationTypeEnum);
                        }
                    }
                }
            }

            if (calculator == null)
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitChangeTrigger(Operator op)
        {
            base.VisitChangeTrigger(op);

            OperatorCalculatorBase operatorCalculator;

            OperatorCalculatorBase calculationCalculator = _stack.Pop();
            OperatorCalculatorBase resetCalculator = _stack.Pop();

            calculationCalculator = calculationCalculator ?? new Zero_OperatorCalculator();
            resetCalculator = resetCalculator ?? new Zero_OperatorCalculator();

            bool calculationIsConst = calculationCalculator is Number_OperatorCalculator;
            bool resetIsConst = resetCalculator is Number_OperatorCalculator;

            if (calculationIsConst)
            {
                operatorCalculator = calculationCalculator;
            }
            else if (resetIsConst)
            {
                operatorCalculator = calculationCalculator;
            }
            else
            {
                operatorCalculator = new ChangeTrigger_OperatorCalculator(calculationCalculator, resetCalculator);
            }

            _stack.Push(operatorCalculator);
        }

        protected override void VisitClosest(Operator op)
        {
            base.VisitClosest(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase inputCalculator = _stack.Pop();

            int itemCount = op.Inlets.Count - 1;
            IList<OperatorCalculatorBase> itemCalculators = new OperatorCalculatorBase[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                OperatorCalculatorBase itemCalculator = _stack.Pop();
                itemCalculators[i] = itemCalculator;
            }

            inputCalculator = inputCalculator ?? new Zero_OperatorCalculator();
            itemCalculators = itemCalculators.Where(x => x != null).ToArray();

            bool inputIsConst = inputCalculator is Number_OperatorCalculator;
            bool itemsIsEmpty = itemCalculators.Count == 0;
            bool allItemsAreConst = itemCalculators.All(x => x is Number_OperatorCalculator);

            double input = inputIsConst ? inputCalculator.Calculate() : 0.0;
            IList<double> items = null;
            if (allItemsAreConst)
            {
                items = itemCalculators.Select(x => x.Calculate()).ToArray();
            }

            if (itemsIsEmpty)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (inputIsConst && allItemsAreConst)
            {
                double result = AggregateCalculator.Closest(input, items);
                calculator = new Number_OperatorCalculator(result);
            }
            else if (allItemsAreConst)
            {
                if (items.Count == 2)
                {
                    calculator = new Closest_OperatorCalculator_VarInput_2ConstItems(inputCalculator, items[0], items[1]);
                }
                else
                {
                    calculator = new Closest_OperatorCalculator_VarInput_ConstItems(inputCalculator, items);
                }
            }
            else
            {
                calculator = new Closest_OperatorCalculator_AllVars(inputCalculator, itemCalculators);
            }

            _stack.Push(calculator);
        }

        protected override void VisitClosestExp(Operator op)
        {
            base.VisitClosestExp(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase inputCalculator = _stack.Pop();

            int itemCount = op.Inlets.Count - 1;
            IList<OperatorCalculatorBase> itemCalculators = new OperatorCalculatorBase[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                OperatorCalculatorBase itemCalculator = _stack.Pop();
                itemCalculators[i] = itemCalculator;
            }

            inputCalculator = inputCalculator ?? new Zero_OperatorCalculator();
            itemCalculators = itemCalculators.Where(x => x != null).ToArray();

            bool inputIsConst = inputCalculator is Number_OperatorCalculator;
            bool itemsIsEmpty = itemCalculators.Count == 0;
            bool allItemsAreConst = itemCalculators.All(x => x is Number_OperatorCalculator);

            double input = inputIsConst ? inputCalculator.Calculate() : 0.0;
            IList<double> items = null;
            if (allItemsAreConst)
            {
                items = itemCalculators.Select(x => x.Calculate()).ToArray();
            }

            if (itemsIsEmpty)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (inputIsConst && allItemsAreConst)
            {
                double result = AggregateCalculator.ClosestExp(input, items);
                calculator = new Number_OperatorCalculator(result);
            }
            else if (allItemsAreConst)
            {
                if (items.Count == 2)
                {
                    calculator = new ClosestExp_OperatorCalculator_VarInput_2ConstItems(inputCalculator, items[0], items[1]);
                }
                else
                {
                    calculator = new ClosestExp_OperatorCalculator_VarInput_ConstItems(inputCalculator, items);
                }
            }
            else
            {
                calculator = new ClosestExp_OperatorCalculator_AllVars(inputCalculator, itemCalculators);
            }

            _stack.Push(calculator);
        }

        protected override void VisitClosestOverDimension(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitClosestOverDimension(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase inputCalculator = _stack.Pop();
            OperatorCalculatorBase collectionCalculator = _stack.Pop();
            OperatorCalculatorBase fromCalculator = _stack.Pop();
            OperatorCalculatorBase tillCalculator = _stack.Pop();
            OperatorCalculatorBase stepCalculator = _stack.Pop();

            inputCalculator = inputCalculator ?? new Zero_OperatorCalculator();
            collectionCalculator = collectionCalculator ?? new Zero_OperatorCalculator();
            fromCalculator = fromCalculator ?? new Zero_OperatorCalculator();
            tillCalculator = tillCalculator ?? new Zero_OperatorCalculator();
            stepCalculator = stepCalculator ?? new One_OperatorCalculator();

            bool inputIsConst = inputCalculator is Number_OperatorCalculator;
            bool collectionIsConst = collectionCalculator is Number_OperatorCalculator;
            bool fromIsConst = fromCalculator is Number_OperatorCalculator;
            bool tillIsConst = tillCalculator is Number_OperatorCalculator;
            bool stepIsConst = stepCalculator is Number_OperatorCalculator;

            double input = inputIsConst ? inputCalculator.Calculate() : 0.0;
            double collection = collectionIsConst ? collectionCalculator.Calculate() : 0.0;
            double from = fromIsConst ? fromCalculator.Calculate() : 0.0;
            double till = tillIsConst ? tillCalculator.Calculate() : 0.0;
            double step = stepIsConst ? stepCalculator.Calculate() : 0.0;

            bool stepIsConstZero = stepIsConst && step == 0.0;
            bool stepIsConstNegative = stepIsConst && step < 0.0;
            bool inputIsConstSpecialValue = inputIsConst && DoubleHelper.IsSpecialValue(input);
            bool collectionIsConstSpecialValue = collectionIsConst && DoubleHelper.IsSpecialValue(collection);
            bool fromIsConstSpecialValue = fromIsConst && DoubleHelper.IsSpecialValue(from);
            bool tillIsConstSpecialValue = tillIsConst && DoubleHelper.IsSpecialValue(till);
            bool stepIsConstSpecialValue = stepIsConst && DoubleHelper.IsSpecialValue(step);

            if (inputIsConstSpecialValue ||
                collectionIsConstSpecialValue ||
                fromIsConstSpecialValue ||
                tillIsConstSpecialValue ||
                stepIsConstSpecialValue)
            {
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (stepIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (stepIsConstNegative)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (collectionIsConst)
            {
                calculator = collectionCalculator;
            }
            else
            {
                var wrapper = new ClosestOverDimension_OperatorWrapper(op);
                CollectionRecalculationEnum collectionRecalculationEnum = wrapper.CollectionRecalculation;
                switch (collectionRecalculationEnum)
                {
                    case CollectionRecalculationEnum.Continuous:
                        calculator = new ClosestOverDimension_OperatorCalculator_CollectionRecalculationContinuous(
                            inputCalculator,
                            collectionCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        break;

                    case CollectionRecalculationEnum.UponReset:
                        calculator = new ClosestOverDimension_OperatorCalculator_CollectionRecalculationUponReset(
                            inputCalculator,
                            collectionCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        break;

                    default:
                        throw new ValueNotSupportedException(collectionRecalculationEnum);
                }
            }

            _stack.Push(calculator);
        }

        protected override void VisitClosestOverDimensionExp(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitClosestOverDimensionExp(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase inputCalculator = _stack.Pop();
            OperatorCalculatorBase collectionCalculator = _stack.Pop();
            OperatorCalculatorBase fromCalculator = _stack.Pop();
            OperatorCalculatorBase tillCalculator = _stack.Pop();
            OperatorCalculatorBase stepCalculator = _stack.Pop();

            inputCalculator = inputCalculator ?? new Zero_OperatorCalculator();
            collectionCalculator = collectionCalculator ?? new Zero_OperatorCalculator();
            fromCalculator = fromCalculator ?? new Zero_OperatorCalculator();
            tillCalculator = tillCalculator ?? new Zero_OperatorCalculator();
            stepCalculator = stepCalculator ?? new One_OperatorCalculator();

            bool inputIsConst = inputCalculator is Number_OperatorCalculator;
            bool collectionIsConst = collectionCalculator is Number_OperatorCalculator;
            bool fromIsConst = fromCalculator is Number_OperatorCalculator;
            bool tillIsConst = tillCalculator is Number_OperatorCalculator;
            bool stepIsConst = stepCalculator is Number_OperatorCalculator;

            double input = inputIsConst ? inputCalculator.Calculate() : 0.0;
            double collection = collectionIsConst ? collectionCalculator.Calculate() : 0.0;
            double from = fromIsConst ? fromCalculator.Calculate() : 0.0;
            double till = tillIsConst ? tillCalculator.Calculate() : 0.0;
            double step = stepIsConst ? stepCalculator.Calculate() : 0.0;

            bool stepIsConstZero = stepIsConst && step == 0.0;
            bool stepIsConstNegative = stepIsConst && step < 0.0;
            bool inputIsConstSpecialValue = inputIsConst && DoubleHelper.IsSpecialValue(input);
            bool collectionIsConstSpecialValue = collectionIsConst && DoubleHelper.IsSpecialValue(collection);
            bool fromIsConstSpecialValue = fromIsConst && DoubleHelper.IsSpecialValue(from);
            bool tillIsConstSpecialValue = tillIsConst && DoubleHelper.IsSpecialValue(till);
            bool stepIsConstSpecialValue = stepIsConst && DoubleHelper.IsSpecialValue(step);

            if (inputIsConstSpecialValue ||
                collectionIsConstSpecialValue ||
                fromIsConstSpecialValue ||
                tillIsConstSpecialValue ||
                stepIsConstSpecialValue)
            {
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (stepIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (stepIsConstNegative)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (collectionIsConst)
            {
                calculator = collectionCalculator;
            }
            else
            {
                var wrapper = new ClosestOverDimensionExp_OperatorWrapper(op);
                CollectionRecalculationEnum collectionRecalculationEnum = wrapper.CollectionRecalculation;
                switch (collectionRecalculationEnum)
                {
                    case CollectionRecalculationEnum.Continuous:
                        calculator = new ClosestOverDimensionExp_OperatorCalculator_CollectionRecalculationContinuous(
                            inputCalculator,
                            collectionCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        break;

                    case CollectionRecalculationEnum.UponReset:
                        calculator = new ClosestOverDimensionExp_OperatorCalculator_CollectionRecalculationUponReset(
                            inputCalculator,
                            collectionCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        break;

                    default:
                        throw new ValueNotSupportedException(collectionRecalculationEnum);
                }
            }

            _stack.Push(calculator);
        }

        protected override void VisitCurveOperator(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitCurveOperator(op);

            OperatorCalculatorBase calculator = null;

            var wrapper = new Curve_OperatorWrapper(op, _curveRepository);
            Curve curve = wrapper.Curve;
            if (curve == null)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else
            {
                ICurveCalculator curveCalculator = _calculatorCache.GetCurveCalculator(curve);

                var curveCalculator_MinPosition = curveCalculator as CurveCalculator_MinX;
                if (curveCalculator_MinPosition != null)
                {
                    if (dimensionEnum == DimensionEnum.Time)
                    {
                        calculator = new Curve_OperatorCalculator_MinX_WithOriginShifting(curveCalculator_MinPosition, dimensionStack);
                    }
                    else
                    {
                        calculator = new Curve_OperatorCalculator_MinX_NoOriginShifting(curveCalculator_MinPosition, dimensionStack);
                    }
                }

                var curveCalculator_MinPositionZero = curveCalculator as CurveCalculator_MinXZero;
                if (curveCalculator_MinPositionZero != null)
                {
                    if (dimensionEnum == DimensionEnum.Time)
                    {
                        calculator = new Curve_OperatorCalculator_MinXZero_WithOriginShifting(curveCalculator_MinPositionZero, dimensionStack);
                    }
                    else
                    {
                        calculator = new Curve_OperatorCalculator_MinXZero_NoOriginShifting(curveCalculator_MinPositionZero, dimensionStack);
                    }
                }
            }

            if (calculator == null)
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitShift(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            dimensionStack.Push(DEFAULT_DIMENSION_VALUE);

            base.VisitShift(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase differenceCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            differenceCalculator = differenceCalculator ?? new Zero_OperatorCalculator();

            double signal = signalCalculator.Calculate();
            double difference = differenceCalculator.Calculate();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool differenceIsConst = differenceCalculator is Number_OperatorCalculator;
            bool signalIsConstZero = signalIsConst && signal == 0;
            bool differenceIsConstZero = differenceIsConst && difference == 0;

            dimensionStack.Pop();

            if (signalIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (differenceIsConstZero)
            {
                calculator = signalCalculator;
            }
            else if (signalIsConst)
            {
                calculator = signalCalculator;
            }
            else if (differenceIsConst)
            {
                calculator = new Shift_OperatorCalculator_VarSignal_ConstDifference(signalCalculator, difference, dimensionStack);
            }
            else
            {
                calculator = new Shift_OperatorCalculator_VarSignal_VarDifference(signalCalculator, differenceCalculator, dimensionStack);
            }

            _stack.Push(calculator);
        }

        protected override void VisitDivide(Operator op)
        {
            base.VisitDivide(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase numeratorCalculator = _stack.Pop();
            OperatorCalculatorBase denominatorCalculator = _stack.Pop();
            OperatorCalculatorBase originCalculator = _stack.Pop();

            // When nulls should make the operator do nothing but pass the signal.
            if (denominatorCalculator == null && numeratorCalculator != null)
            {
                _stack.Push(numeratorCalculator);
                return;
            }

            numeratorCalculator = numeratorCalculator ?? new Zero_OperatorCalculator();
            denominatorCalculator = denominatorCalculator ?? new One_OperatorCalculator();
            originCalculator = originCalculator ?? new Zero_OperatorCalculator();

            double numerator = numeratorCalculator.Calculate();
            double denominator = denominatorCalculator.Calculate();
            double origin = originCalculator.Calculate();
            bool denominatorIsConst = denominatorCalculator is Number_OperatorCalculator;
            bool numeratorIsConst = numeratorCalculator is Number_OperatorCalculator;
            bool originIsConst = originCalculator is Number_OperatorCalculator;
            bool numeratorIsConstZero = numeratorIsConst && numerator == 0;
            bool denominatorIsConstZero = denominatorIsConst && denominator == 0;
            bool originIsConstZero = originIsConst && origin == 0;
            bool denominatorIsConstOne = denominatorIsConst && denominator == 1;

            if (denominatorIsConstZero)
            {
                // Special Value
                calculator = new Zero_OperatorCalculator();
            }
            else if (numeratorIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (denominatorIsConstOne)
            {
                calculator = numeratorCalculator;
            }
            else if (originIsConstZero && numeratorIsConst & denominatorIsConst)
            {
                calculator = new Number_OperatorCalculator(numerator / denominator);
            }
            else if (originIsConst && numeratorIsConst && denominatorIsConst)
            {
                double value = (numerator - origin) / denominator + origin;
                calculator = new Number_OperatorCalculator(value);
            }
            else if (originIsConstZero && numeratorIsConst && !denominatorIsConst)
            {
                calculator = new Divide_ConstNumerator_VarDenominator_ZeroOrigin_OperatorCalculator(numerator, denominatorCalculator);
            }
            else if (originIsConstZero && !numeratorIsConst && denominatorIsConst)
            {
                calculator = new Divide_VarNumerator_ConstDenominator_ZeroOrigin_OperatorCalculator(numeratorCalculator, denominator);
            }
            else if (originIsConstZero && !numeratorIsConst && !denominatorIsConst)
            {
                calculator = new Divide_WithoutOrigin_OperatorCalculator(numeratorCalculator, denominatorCalculator);
            }
            else if (originIsConst && numeratorIsConst && !denominatorIsConst)
            {
                calculator = new Divide_ConstNumerator_VarDenominator_ConstOrigin_OperatorCalculator(numerator, denominatorCalculator, origin);
            }
            else if (originIsConst && !numeratorIsConst && denominatorIsConst)
            {
                calculator = new Divide_VarNumerator_ConstDenominator_ConstOrigin_OperatorCalculator(numeratorCalculator, denominator, origin);
            }
            else if (originIsConst && !numeratorIsConst && !denominatorIsConst)
            {
                calculator = new Divide_VarNumerator_VarDenominator_ConstOrigin_OperatorCalculator(numeratorCalculator, denominatorCalculator, origin);
            }
            else if (!originIsConst && numeratorIsConst && denominatorIsConst)
            {
                calculator = new Divide_WithOrigin_AndConstNumerator_AndDenominator_OperatorCalculator(numerator, denominator, originCalculator);
            }
            else if (!originIsConst && numeratorIsConst && !denominatorIsConst)
            {
                calculator = new Divide_WithOrigin_AndConstNumerator_OperatorCalculator(numerator, denominatorCalculator, originCalculator);
            }
            else if (!originIsConst && !numeratorIsConst && denominatorIsConst)
            {
                calculator = new Divide_WithOrigin_AndConstDenominator_OperatorCalculator(numeratorCalculator, denominator, originCalculator);
            }
            else
            {
                calculator = new Divide_WithOrigin_OperatorCalculator(numeratorCalculator, denominatorCalculator, originCalculator);
            }

            _stack.Push(calculator);
        }

        protected override void VisitEqual(Operator op)
        {
            base.VisitEqual(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase calculatorA = _stack.Pop();
            OperatorCalculatorBase calculatorB = _stack.Pop();

            calculatorA = calculatorA ?? new Zero_OperatorCalculator();
            calculatorB = calculatorB ?? new Zero_OperatorCalculator();

            double a = calculatorA.Calculate();
            double b = calculatorB.Calculate();

            bool aIsConst = calculatorA is Number_OperatorCalculator;
            bool bIsConst = calculatorB is Number_OperatorCalculator;

            if (aIsConst && bIsConst)
            {
                double value;

                if (a == b) value = 1.0;
                else value = 0.0;

                calculator = new Number_OperatorCalculator(value);
            }
            else if (!aIsConst && bIsConst)
            {
                calculator = new Equal_ConstA_VarB_OperatorCalculator(b, calculatorA);
            }
            else if (aIsConst && !bIsConst)
            {
                calculator = new Equal_ConstA_VarB_OperatorCalculator(a, calculatorB);
            }
            else if (!aIsConst && !bIsConst)
            {
                calculator = new Equal_VarA_VarB_OperatorCalculator(calculatorA, calculatorB);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitExponent(Operator op)
        {
            base.VisitExponent(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase lowCalculator = _stack.Pop();
            OperatorCalculatorBase highCalculator = _stack.Pop();
            OperatorCalculatorBase ratioCalculator = _stack.Pop();

            lowCalculator = lowCalculator ?? new Zero_OperatorCalculator();
            highCalculator = highCalculator ?? new Zero_OperatorCalculator();
            ratioCalculator = ratioCalculator ?? new Zero_OperatorCalculator();

            double low = lowCalculator.Calculate();
            double high = highCalculator.Calculate();
            double ratio = ratioCalculator.Calculate();
            bool lowIsConst = lowCalculator is Number_OperatorCalculator;
            bool highIsConst = highCalculator is Number_OperatorCalculator;
            bool ratioIsConst = ratioCalculator is Number_OperatorCalculator;

            // TODO: Program more specialized cases?
            bool lowIsConstZero = lowIsConst && low == 0;
            bool highIsConstZero = lowIsConst && high == 0;
            //bool ratioIsConstZero = ratioIsConst && ratio == 0;
            //bool lowIsConstOne = lowIsConst && low == 1;
            //bool highIsConstOne = lowIsConst && high == 1;
            //bool ratioIsConstOne = ratioIsConst && ratio == 1;

            if (lowIsConstZero)
            {
                // Special Value
                calculator = new Zero_OperatorCalculator();
            }
            else if (highIsConstZero)
            {
                // Would result in 0. See formula further down
                calculator = new Zero_OperatorCalculator();
            }
            else if (lowIsConst && highIsConst && ratioIsConst)
            {
                double value = low * Math.Pow(high / low, ratio);
                calculator = new Number_OperatorCalculator(value);
            }
            else if (!lowIsConst && highIsConst && ratioIsConst)
            {
                calculator = new Exponent_OperatorCalculator_VarLow_ConstHigh_ConstRatio(lowCalculator, high, ratio);
            }
            else if (lowIsConst && !highIsConst && ratioIsConst)
            {
                calculator = new Exponent_OperatorCalculator_ConstLow_VarHigh_ConstRatio(low, highCalculator, ratio);
            }
            else if (!lowIsConst && !highIsConst && ratioIsConst)
            {
                calculator = new Exponent_OperatorCalculator_VarLow_VarHigh_ConstRatio(lowCalculator, highCalculator, ratio);
            }
            else if (lowIsConst && highIsConst && !ratioIsConst)
            {
                calculator = new Exponent_OperatorCalculator_ConstLow_ConstHigh_VarRatio(low, high, ratioCalculator);
            }
            else if (!lowIsConst && highIsConst && !ratioIsConst)
            {
                calculator = new Exponent_OperatorCalculator_VarLow_ConstHigh_VarRatio(lowCalculator, high, ratioCalculator);
            }
            else if (lowIsConst && !highIsConst && !ratioIsConst)
            {
                calculator = new Exponent_OperatorCalculator_ConstLow_VarHigh_VarRatio(low, highCalculator, ratioCalculator);
            }
            else
            {
                calculator = new Exponent_OperatorCalculator_VarLow_VarHigh_VarRatio(lowCalculator, highCalculator, ratioCalculator);
            }

            _stack.Push(calculator);
        }

        protected override void VisitGetDimension(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitGetDimension(op);

            var calculator = new GetDimension_OperatorCalculator(dimensionStack);
            _stack.Push(calculator);
        }

        protected override void VisitGreaterThan(Operator op)
        {
            base.VisitGreaterThan(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase calculatorA = _stack.Pop();
            OperatorCalculatorBase calculatorB = _stack.Pop();

            calculatorA = calculatorA ?? new Zero_OperatorCalculator();
            calculatorB = calculatorB ?? new Zero_OperatorCalculator();

            double a = calculatorA.Calculate();
            double b = calculatorB.Calculate();

            bool aIsConst = calculatorA is Number_OperatorCalculator;
            bool bIsConst = calculatorB is Number_OperatorCalculator;

            if (aIsConst && bIsConst)
            {
                double value;

                if (a > b) value = 1.0;
                else value = 0.0;

                calculator = new Number_OperatorCalculator(value);
            }
            else if (!aIsConst && bIsConst)
            {
                calculator = new GreaterThan_VarA_ConstB_OperatorCalculator(calculatorA, b);
            }
            else if (aIsConst && !bIsConst)
            {
                calculator = new GreaterThan_ConstA_VarB_OperatorCalculator(a, calculatorB);
            }
            else if (!aIsConst && !bIsConst)
            {
                calculator = new GreaterThan_VarA_VarB_OperatorCalculator(calculatorA, calculatorB);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitGreaterThanOrEqual(Operator op)
        {
            base.VisitGreaterThanOrEqual(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase calculatorA = _stack.Pop();
            OperatorCalculatorBase calculatorB = _stack.Pop();

            calculatorA = calculatorA ?? new Zero_OperatorCalculator();
            calculatorB = calculatorB ?? new Zero_OperatorCalculator();

            double a = calculatorA.Calculate();
            double b = calculatorB.Calculate();

            bool aIsConst = calculatorA is Number_OperatorCalculator;
            bool bIsConst = calculatorB is Number_OperatorCalculator;

            if (aIsConst && bIsConst)
            {
                double value;

                if (a >= b) value = 1.0;
                else value = 0.0;

                calculator = new Number_OperatorCalculator(value);
            }
            else if (!aIsConst && bIsConst)
            {
                calculator = new GreaterThanOrEqual_VarA_ConstB_OperatorCalculator(calculatorA, b);
            }
            else if (aIsConst && !bIsConst)
            {
                calculator = new GreaterThanOrEqual_ConstA_VarB_OperatorCalculator(a, calculatorB);
            }
            else if (!aIsConst && !bIsConst)
            {
                calculator = new GreaterThanOrEqual_VarA_VarB_OperatorCalculator(calculatorA, calculatorB);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitHighPassFilter(Operator op)
        {
            base.VisitHighPassFilter(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase minFrequencyCalculator = _stack.Pop();
            OperatorCalculatorBase bandWidthCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            minFrequencyCalculator = minFrequencyCalculator ?? new Zero_OperatorCalculator();
            bandWidthCalculator = bandWidthCalculator ?? new Zero_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool minFrequencyIsConst = minFrequencyCalculator is Number_OperatorCalculator;
            bool bandWidthIsConst = bandWidthCalculator is Number_OperatorCalculator;

            double signal = signalIsConst ? signalCalculator.Calculate() : 0.0;
            double minFrequency = minFrequencyIsConst ? minFrequencyCalculator.Calculate() : 0.0;
            double bandWidth = bandWidthIsConst ? bandWidthCalculator.Calculate() : 0.0;

            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool minFrequencyIsConstZero = minFrequencyIsConst && minFrequency == 0.0;
            bool minFrequencyIsConstSpecialValue = minFrequencyIsConst && DoubleHelper.IsSpecialValue(minFrequency);
            bool bandWidthIsConstSpecialValue = bandWidthIsConst && DoubleHelper.IsSpecialValue(bandWidth);

            if (minFrequency > _nyquistFrequency) minFrequency = _nyquistFrequency;

            if (signalIsConstSpecialValue || minFrequencyIsConstSpecialValue || bandWidthIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConst)
            {
                calculator = signalCalculator;
            }
            else if (minFrequencyIsConstZero)
            {
                // No filtering
                calculator = signalCalculator;
            }
            else if (minFrequencyIsConst && bandWidthIsConst)
            {
                calculator = new HighPassFilter_OperatorCalculator_ManyConsts(
                    signalCalculator, 
                    minFrequency, 
                    bandWidth,
                    _samplingRate);
            }
            else
            {
                calculator = new HighPassFilter_OperatorCalculator_AllVars(
                    signalCalculator, 
                    minFrequencyCalculator, 
                    bandWidthCalculator,
                    _samplingRate,
                    _samplesBetweenApplyFilterVariables);
            }

            _stack.Push(calculator);
        }

        protected override void VisitHighShelfFilter(Operator op)
        {
            base.VisitHighShelfFilter(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase transitionFrequencyCalculator = _stack.Pop();
            OperatorCalculatorBase transitionSlopeCalculator = _stack.Pop();
            OperatorCalculatorBase dbGainCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            transitionFrequencyCalculator = transitionFrequencyCalculator ?? new Zero_OperatorCalculator();
            transitionSlopeCalculator = transitionSlopeCalculator ?? new Zero_OperatorCalculator();
            dbGainCalculator = dbGainCalculator ?? new Zero_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool transitionFrequencyIsConst = transitionFrequencyCalculator is Number_OperatorCalculator;
            bool dbGainIsConst = dbGainCalculator is Number_OperatorCalculator;
            bool transitionSlopeIsConst = transitionSlopeCalculator is Number_OperatorCalculator;

            double signal = signalIsConst ? signalCalculator.Calculate() : 0.0;
            double transitionFrequency = transitionFrequencyIsConst ? transitionFrequencyCalculator.Calculate() : 0.0;
            double transitionSlope = transitionSlopeIsConst ? transitionSlopeCalculator.Calculate() : 0.0;
            double dbGain = dbGainIsConst ? dbGainCalculator.Calculate() : 0.0;

            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool transitionFrequencyIsConstSpecialValue = transitionFrequencyIsConst && DoubleHelper.IsSpecialValue(transitionFrequency);
            bool transitionSlopeIsConstSpecialValue = transitionSlopeIsConst && DoubleHelper.IsSpecialValue(transitionSlope);
            bool dbGainIsConstSpecialValue = dbGainIsConst && DoubleHelper.IsSpecialValue(dbGain);

            if (transitionFrequency > _nyquistFrequency) transitionFrequency = _nyquistFrequency;

            if (signalIsConstSpecialValue || transitionFrequencyIsConstSpecialValue || transitionSlopeIsConstSpecialValue || dbGainIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConst)
            {
                // There are no frequencies. So you a filter should do nothing.
                calculator = signalCalculator;
            }
            else if (transitionFrequencyIsConst && dbGainIsConst && transitionSlopeIsConst)
            {
                calculator = new HighShelfFilter_OperatorCalculator_ManyConsts(
                    signalCalculator,
                    transitionFrequency,
                    transitionSlope,
                    dbGain,
                    _samplingRate);
            }
            else
            {
                calculator = new HighShelfFilter_OperatorCalculator_AllVars(
                    signalCalculator,
                    transitionFrequencyCalculator,
                    transitionSlopeCalculator,
                    dbGainCalculator,
                    _samplingRate,
                    _samplesBetweenApplyFilterVariables);
            }

            _stack.Push(calculator);
        }

        protected override void VisitHold(Operator op)
        {
            base.VisitHold(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            bool signalIsConst = signalCalculator is Number_OperatorCalculator;

            if (signalIsConst)
            {
                calculator = signalCalculator;
            }
            else
            {
                calculator = new Hold_OperatorCalculator(signalCalculator);
            }

            _stack.Push(calculator);
        }

        protected override void VisitIf(Operator op)
        {
            base.VisitIf(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase conditionCalculator = _stack.Pop();
            OperatorCalculatorBase thenCalculator = _stack.Pop();
            OperatorCalculatorBase elseCalculator = _stack.Pop();

            conditionCalculator = conditionCalculator ?? new Zero_OperatorCalculator();
            thenCalculator = thenCalculator ?? new Zero_OperatorCalculator();
            elseCalculator = elseCalculator ?? new Zero_OperatorCalculator();

            double condition = conditionCalculator.Calculate();
            double then = thenCalculator.Calculate();
            double @else = elseCalculator.Calculate();

            bool conditionIsConst = conditionCalculator is Number_OperatorCalculator;
            bool thenIsConst = thenCalculator is Number_OperatorCalculator;
            bool elseIsConst = elseCalculator is Number_OperatorCalculator;

            if (conditionIsConst)
            {
                bool conditionIsTrue = condition != 0.0;
                if (conditionIsTrue)
                {
                    calculator = thenCalculator;
                }
                else
                {
                    calculator = elseCalculator;
                }
            }
            else if (thenIsConst && elseIsConst)
            {
                calculator = new If_VarCondition_ConstThen_ConstElse_OperatorCalculator(conditionCalculator, then, @else);
            }
            else if (thenIsConst && !elseIsConst)
            {
                calculator = new If_VarCondition_ConstThen_VarElse_OperatorCalculator(conditionCalculator, then, elseCalculator);
            }
            else if (!thenIsConst && elseIsConst)
            {
                calculator = new If_VarCondition_VarThen_ConstElse_OperatorCalculator(conditionCalculator, thenCalculator, @else);
            }
            else if (!thenIsConst && !elseIsConst)
            {
                calculator = new If_VarCondition_VarThen_VarElse_OperatorCalculator(conditionCalculator, thenCalculator, elseCalculator);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitLessThan(Operator op)
        {
            base.VisitLessThan(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase calculatorA = _stack.Pop();
            OperatorCalculatorBase calculatorB = _stack.Pop();

            calculatorA = calculatorA ?? new Zero_OperatorCalculator();
            calculatorB = calculatorB ?? new Zero_OperatorCalculator();

            double a = calculatorA.Calculate();
            double b = calculatorB.Calculate();

            bool aIsConst = calculatorA is Number_OperatorCalculator;
            bool bIsConst = calculatorB is Number_OperatorCalculator;

            if (aIsConst && bIsConst)
            {
                double value;

                if (a < b) value = 1.0;
                else value = 0.0;

                calculator = new Number_OperatorCalculator(value);
            }
            else if (!aIsConst && bIsConst)
            {
                calculator = new LessThan_VarA_ConstB_OperatorCalculator(calculatorA, b);
            }
            else if (aIsConst && !bIsConst)
            {
                calculator = new LessThan_ConstA_VarB_OperatorCalculator(a, calculatorB);
            }
            else if (!aIsConst && !bIsConst)
            {
                calculator = new LessThan_VarA_VarB_OperatorCalculator(calculatorA, calculatorB);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitLessThanOrEqual(Operator op)
        {
            base.VisitLessThanOrEqual(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase calculatorA = _stack.Pop();
            OperatorCalculatorBase calculatorB = _stack.Pop();

            calculatorA = calculatorA ?? new Zero_OperatorCalculator();
            calculatorB = calculatorB ?? new Zero_OperatorCalculator();

            double a = calculatorA.Calculate();
            double b = calculatorB.Calculate();

            bool aIsConst = calculatorA is Number_OperatorCalculator;
            bool bIsConst = calculatorB is Number_OperatorCalculator;

            if (aIsConst && bIsConst)
            {
                double value;

                if (a <= b) value = 1.0;
                else value = 0.0;

                calculator = new Number_OperatorCalculator(value);
            }
            else if (!aIsConst && bIsConst)
            {
                calculator = new LessThanOrEqual_VarA_ConstB_OperatorCalculator(calculatorA, b);
            }
            else if (aIsConst && !bIsConst)
            {
                calculator = new LessThanOrEqual_ConstA_VarB_OperatorCalculator(a, calculatorB);
            }
            else if (!aIsConst && !bIsConst)
            {
                calculator = new LessThanOrEqual_VarA_VarB_OperatorCalculator(calculatorA, calculatorB);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitLoop(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            dimensionStack.Push(DEFAULT_DIMENSION_VALUE);

            base.VisitLoop(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase skipCalculator = _stack.Pop();
            OperatorCalculatorBase loopStartMarkerCalculator = _stack.Pop();
            OperatorCalculatorBase loopEndMarkerCalculator = _stack.Pop();
            OperatorCalculatorBase releaseEndMarkerCalculator = _stack.Pop();
            OperatorCalculatorBase noteDurationCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            skipCalculator = skipCalculator ?? new Zero_OperatorCalculator();
            loopStartMarkerCalculator = loopStartMarkerCalculator ?? new Zero_OperatorCalculator();
            loopEndMarkerCalculator = loopEndMarkerCalculator ?? new Zero_OperatorCalculator();
            releaseEndMarkerCalculator = releaseEndMarkerCalculator ?? new Number_OperatorCalculator(CalculationHelper.VERY_HIGH_VALUE);
            noteDurationCalculator = noteDurationCalculator ?? new Number_OperatorCalculator(CalculationHelper.VERY_HIGH_VALUE);

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool skipIsConst = skipCalculator is Number_OperatorCalculator;
            bool loopStartMarkerIsConst = loopStartMarkerCalculator is Number_OperatorCalculator;
            bool loopEndMarkerIsConst = loopEndMarkerCalculator is Number_OperatorCalculator;
            bool releaseEndMarkerIsConst = releaseEndMarkerCalculator is Number_OperatorCalculator;
            bool noteDurationIsConst = noteDurationCalculator is Number_OperatorCalculator;

            double signal = signalCalculator.Calculate();
            double skip = skipCalculator.Calculate();
            double loopStartMarker = loopStartMarkerCalculator.Calculate();
            double loopEndMarker = loopEndMarkerCalculator.Calculate();
            double releaseEndMarker = releaseEndMarkerCalculator.Calculate();
            double noteDuration = noteDurationCalculator.Calculate();

            bool skipIsConstZero = skipIsConst && skip == 0.0;
            bool noteDurationIsConstVeryHighValue = noteDurationIsConst && noteDuration == CalculationHelper.VERY_HIGH_VALUE;
            bool releaseEndMarkerIsConstVeryHighValue = releaseEndMarkerIsConst && releaseEndMarker == CalculationHelper.VERY_HIGH_VALUE;

            dimensionStack.Pop();

            if (signalIsConst)
            {
                // Note that the signalCalculator is not used here,
                // because behavior consistent with the other loop
                // calculators would mean that before and after the
                // attack, loop and release, the signal is 0,
                // while during attack, loop and release it would be
                // the value of constant value.
                // To make this behavior consistent would require another calculator,
                // that is just not worth is, because it makes no sense to apply a loop to a constant.
                // So to not return e.g. the number 2 before and after the loop
                // just return 0. Nobody wants to loop throught a constant.
                calculator = new Zero_OperatorCalculator();
            }
            else
            {
                if (skipIsConst &&
                    loopStartMarkerIsConst &&
                    skip == loopStartMarker &&
                    noteDurationIsConstVeryHighValue)
                {
                    if (loopEndMarkerIsConst)
                    {
                        calculator = new Loop_OperatorCalculator_ConstSkip_WhichEqualsLoopStartMarker_ConstLoopEndMarker_NoNoteDuration(
                            signalCalculator, loopStartMarker, loopEndMarker, dimensionStack);
                    }
                    else
                    {
                        calculator = new Loop_OperatorCalculator_ConstSkip_WhichEqualsLoopStartMarker_VarLoopEndMarker_NoNoteDuration(
                            signalCalculator, loopStartMarker, loopEndMarkerCalculator, dimensionStack);
                    }
                }
                else if (skipIsConstZero && releaseEndMarkerIsConstVeryHighValue)
                {
                    if (loopStartMarkerIsConst && loopEndMarkerIsConst)
                    {
                        calculator = new Loop_OperatorCalculator_NoSkipOrRelease_ManyConstants(
                            signalCalculator, loopStartMarker, loopEndMarker, noteDurationCalculator, dimensionStack);
                    }
                    else
                    {
                        calculator = new Loop_OperatorCalculator_NoSkipOrRelease(
                            signalCalculator, loopStartMarkerCalculator, loopEndMarkerCalculator, noteDurationCalculator, dimensionStack);
                    }
                }
                else
                {
                    if (skipIsConst && loopStartMarkerIsConst && loopEndMarkerIsConst && releaseEndMarkerIsConst)
                    {
                        calculator = new Loop_OperatorCalculator_ManyConstants(
                            signalCalculator, skip, loopStartMarker, loopEndMarker, releaseEndMarker, noteDurationCalculator, dimensionStack);
                    }
                    else
                    {
                        calculator = new Loop_OperatorCalculator_AllVars(
                            signalCalculator,
                            skipCalculator,
                            loopStartMarkerCalculator,
                            loopEndMarkerCalculator,
                            releaseEndMarkerCalculator,
                            noteDurationCalculator,
                            dimensionStack);
                    }
                }
            }

            _stack.Push(calculator);
        }

        protected override void VisitLowPassFilter(Operator op)
        {
            base.VisitLowPassFilter(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase maxFrequencyCalculator = _stack.Pop();
            OperatorCalculatorBase bandWidthCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            maxFrequencyCalculator = maxFrequencyCalculator ?? new Zero_OperatorCalculator();
            bandWidthCalculator = bandWidthCalculator ?? new Zero_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool maxFrequencyIsConst = maxFrequencyCalculator is Number_OperatorCalculator;
            bool bandWidthIsConst = bandWidthCalculator is Number_OperatorCalculator;

            double signal = signalIsConst ? signalCalculator.Calculate() : 0.0;
            double maxFrequency = maxFrequencyIsConst ? maxFrequencyCalculator.Calculate() : 0.0;
            double bandWidth = bandWidthIsConst ? bandWidthCalculator.Calculate() : 0.0;

            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool maxFrequencyIsConstZero = maxFrequencyIsConst && maxFrequency == 0.0;
            bool maxFrequencyIsConstSpecialValue = maxFrequencyIsConst && DoubleHelper.IsSpecialValue(maxFrequency);
            bool bandWidthIsConstSpecialValue = bandWidthIsConst && DoubleHelper.IsSpecialValue(bandWidth);

            if (maxFrequency > _nyquistFrequency) maxFrequency = _nyquistFrequency;

            if (signalIsConstSpecialValue || maxFrequencyIsConstSpecialValue || bandWidthIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConst)
            {
                calculator = signalCalculator;
            }
            else if (maxFrequencyIsConstZero)
            {
                // Special Value: time stands still.
                calculator = new Number_OperatorCalculator(signal);
            }
            else if (maxFrequencyIsConst && bandWidthIsConst)
            {
                calculator = new LowPassFilter_OperatorCalculator_ManyConsts(
                    signalCalculator, 
                    maxFrequency, 
                    bandWidth,
                    _samplingRate);
            }
            else
            {
                calculator = new LowPassFilter_OperatorCalculator_AllVars(
                    signalCalculator, 
                    maxFrequencyCalculator, 
                    bandWidthCalculator,
                    _samplingRate,
                    _samplesBetweenApplyFilterVariables);
            }

            _stack.Push(calculator);
        }

        protected override void VisitLowShelfFilter(Operator op)
        {
            base.VisitLowShelfFilter(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase transitionFrequencyCalculator = _stack.Pop();
            OperatorCalculatorBase transitionSlopeCalculator = _stack.Pop();
            OperatorCalculatorBase dbGainCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            transitionFrequencyCalculator = transitionFrequencyCalculator ?? new Zero_OperatorCalculator();
            transitionSlopeCalculator = transitionSlopeCalculator ?? new Zero_OperatorCalculator();
            dbGainCalculator = dbGainCalculator ?? new Zero_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool transitionFrequencyIsConst = transitionFrequencyCalculator is Number_OperatorCalculator;
            bool transitionSlopeIsConst = transitionSlopeCalculator is Number_OperatorCalculator;
            bool dbGainIsConst = dbGainCalculator is Number_OperatorCalculator;

            double signal = signalIsConst ? signalCalculator.Calculate() : 0.0;
            double transitionFrequency = transitionFrequencyIsConst ? transitionFrequencyCalculator.Calculate() : 0.0;
            double transitionSlope = transitionSlopeIsConst ? transitionSlopeCalculator.Calculate() : 0.0;
            double dbGain = dbGainIsConst ? dbGainCalculator.Calculate() : 0.0;

            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool transitionFrequencyIsConstSpecialValue = transitionFrequencyIsConst && DoubleHelper.IsSpecialValue(transitionFrequency);
            bool transitionSlopeIsConstSpecialValue = transitionSlopeIsConst && DoubleHelper.IsSpecialValue(transitionSlope);
            bool dbGainIsConstSpecialValue = dbGainIsConst && DoubleHelper.IsSpecialValue(dbGain);

            if (transitionFrequency > _nyquistFrequency) transitionFrequency = _nyquistFrequency;

            if (signalIsConstSpecialValue || transitionFrequencyIsConstSpecialValue || transitionSlopeIsConstSpecialValue || dbGainIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConst)
            {
                // There are no frequencies. So you a filter should do nothing.
                calculator = signalCalculator;
            }
            else if (transitionFrequencyIsConst && dbGainIsConst && transitionSlopeIsConst)
            {
                calculator = new LowShelfFilter_OperatorCalculator_ManyConsts(
                    signalCalculator,
                    transitionFrequency,
                    transitionSlope,
                    dbGain,
                    _samplingRate);
            }
            else
            {
                calculator = new LowShelfFilter_OperatorCalculator_AllVars(
                    signalCalculator,
                    transitionFrequencyCalculator,
                    transitionSlopeCalculator,
                    dbGainCalculator,
                    _samplingRate,
                    _samplesBetweenApplyFilterVariables);
            }

            _stack.Push(calculator);
        }

        protected override void VisitMakeContinuous(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            // No pushing and popping from the dimension stack here.

            base.VisitMakeContinuous(op);

            var operandCalculators = new List<OperatorCalculatorBase>(op.Inlets.Count);

            for (int i = 0; i < op.Inlets.Count; i++)
            {
                OperatorCalculatorBase operandCalculator = _stack.Pop();

                operandCalculator = operandCalculator ?? new Zero_OperatorCalculator();

                operandCalculators.Add(operandCalculator);
            }

            var wrapper = new MakeContinuous_OperatorWrapper(op);
            OperatorCalculatorBase calculator = new MakeContinuous_OperatorCalculator(operandCalculators, wrapper.InterpolationType, dimensionStack);

            _stack.Push(calculator);
        }

        protected override void VisitMakeDiscrete(Operator op)
        {
            // Exactly the same behavior as Unbundle.
            VisitUnbundle(op);
        }

        protected override void VisitMax(Operator op)
        {
            base.VisitMax(op);

            OperatorCalculatorBase calculator;

            IList<OperatorCalculatorBase> operandCalculators = new List<OperatorCalculatorBase>(op.Inlets.Count);
            for (int i = 0; i < op.Inlets.Count; i++)
            {
                OperatorCalculatorBase operandCalculator = _stack.Pop();
                operandCalculators.Add(operandCalculator);
            }

            operandCalculators = TruncateOperandCalculatorList(operandCalculators, x => x.Max());

            OperatorCalculatorBase constOperandCalculator = operandCalculators.Where(x => x is Number_OperatorCalculator)
                                                                              .SingleOrDefault();
            switch (operandCalculators.Count)
            {
                case 0:
                    calculator = new Zero_OperatorCalculator();
                    break;

                case 1:
                    // Also covers the 'all are const' situation, since all consts are aggregated to one in earlier code.
                    calculator = operandCalculators[0];
                    break;

                case 2:
                    if (constOperandCalculator == null)
                    {
                        OperatorCalculatorBase aCalculator = operandCalculators[0];
                        OperatorCalculatorBase bCalculator = operandCalculators[1];
                        calculator = new Max_OperatorCalculator_TwoVars(aCalculator, bCalculator);
                    }
                    else
                    {
                        double constValue = constOperandCalculator.Calculate();
                        OperatorCalculatorBase varCalculator = operandCalculators.Except(constOperandCalculator).Single();
                        calculator = new Max_OperatorCalculator_OneConst_OneVar(constValue, varCalculator);
                    }
                    break;

                default:

                    if (constOperandCalculator == null)
                    {
                        calculator = new Max_OperatorCalculator_AllVars(operandCalculators);
                    }
                    else
                    {
                        IList<OperatorCalculatorBase> varOperandCalculators = operandCalculators.Except(constOperandCalculator).ToArray();
                        double constValue = constOperandCalculator.Calculate();

                        calculator =  new Max_OperatorCalculator_WithConst_AndVarArray(constValue, varOperandCalculators);
                    }
                    break;
            }

            _stack.Push(calculator);
        }

        protected override void VisitMaxFollower(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            dimensionStack.Push(DEFAULT_DIMENSION_VALUE);

            base.VisitMaxFollower(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase sliceLengthCalculator = _stack.Pop();
            OperatorCalculatorBase sampleCountCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            sliceLengthCalculator = sliceLengthCalculator ?? new One_OperatorCalculator();
            sampleCountCalculator = sampleCountCalculator ?? new One_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;

            dimensionStack.Pop();

            if (signalIsConst)
            {
                calculator = signalCalculator;
            }
            else
            {
                calculator = new MaxFollower_OperatorCalculator(signalCalculator, sliceLengthCalculator, sampleCountCalculator, dimensionStack);
            }

            _stack.Push(calculator);
        }

        protected override void VisitMaxOverDimension(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitMaxOverDimension(op);

            OperatorCalculatorBase operatorCalculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase fromCalculator = _stack.Pop();
            OperatorCalculatorBase tillCalculator = _stack.Pop();
            OperatorCalculatorBase stepCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            fromCalculator = fromCalculator ?? new Zero_OperatorCalculator();
            tillCalculator = tillCalculator ?? new Zero_OperatorCalculator();
            stepCalculator = stepCalculator ?? new One_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool fromIsConst = fromCalculator is Number_OperatorCalculator;
            bool tillIsConst = tillCalculator is Number_OperatorCalculator;
            bool stepIsConst = stepCalculator is Number_OperatorCalculator;

            double from = fromIsConst ? fromCalculator.Calculate() : 0.0;
            double till = tillIsConst ? tillCalculator.Calculate() : 0.0;
            double step = stepIsConst ? stepCalculator.Calculate() : 0.0;

            bool stepIsConstZero = stepIsConst && step == 0.0;
            bool stepIsConstNegative = stepIsConst && step < 0.0;
            bool fromIsConstSpecialValue = fromIsConst && DoubleHelper.IsSpecialValue(from);
            bool tillIsConstSpecialValue = tillIsConst && DoubleHelper.IsSpecialValue(till);
            bool stepIsConstSpecialValue = stepIsConst && DoubleHelper.IsSpecialValue(step);

            if (signalIsConst)
            {
                operatorCalculator = signalCalculator;
            }
            else if (stepIsConstZero)
            {
                operatorCalculator = new Zero_OperatorCalculator();
            }
            else if (stepIsConstNegative)
            {
                operatorCalculator = new Zero_OperatorCalculator();
            }
            else if (fromIsConstSpecialValue || tillIsConstSpecialValue || stepIsConstSpecialValue)
            {
                operatorCalculator = new Number_OperatorCalculator(Double.NaN);
            }
            else
            {
                var wrapper = new MaxOverDimension_OperatorWrapper(op);
                CollectionRecalculationEnum collectionRecalculationEnum = wrapper.CollectionRecalculation;
                switch (collectionRecalculationEnum)
                {
                    case CollectionRecalculationEnum.Continuous:
                        operatorCalculator = new MaxOverDimension_OperatorCalculator_CollectionRecalculationContinuous(
                            signalCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        break;

                    case CollectionRecalculationEnum.UponReset:
                        operatorCalculator = new MaxOverDimension_OperatorCalculator_CollectionRecalculationUponReset(
                            signalCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        break;

                    default:
                        throw new InvalidValueException(collectionRecalculationEnum);
                }
            }

            _stack.Push(operatorCalculator);
        }

        protected override void VisitMin(Operator op)
        {
            base.VisitMin(op);

            OperatorCalculatorBase calculator;

            IList<OperatorCalculatorBase> operandCalculators = new List<OperatorCalculatorBase>(op.Inlets.Count);
            for (int i = 0; i < op.Inlets.Count; i++)
            {
                OperatorCalculatorBase operandCalculator = _stack.Pop();
                operandCalculators.Add(operandCalculator);
            }

            operandCalculators = TruncateOperandCalculatorList(operandCalculators, x => x.Min());

            OperatorCalculatorBase constOperandCalculator = operandCalculators.Where(x => x is Number_OperatorCalculator)
                                                                              .SingleOrDefault();
            switch (operandCalculators.Count)
            {
                case 0:
                    calculator = new Zero_OperatorCalculator();
                    break;

                case 1:
                    // Also covers the 'all are const' situation, since all consts are aggregated to one in earlier code.
                    calculator = operandCalculators[0];
                    break;

                case 2:
                    if (constOperandCalculator == null)
                    {
                        OperatorCalculatorBase aCalculator = operandCalculators[0];
                        OperatorCalculatorBase bCalculator = operandCalculators[1];
                        calculator = new Min_OperatorCalculator_TwoVars(aCalculator, bCalculator);
                    }
                    else
                    {
                        double constValue = constOperandCalculator.Calculate();
                        OperatorCalculatorBase varCalculator = operandCalculators.Except(constOperandCalculator).Single();
                        calculator = new Min_OperatorCalculator_OneConst_OneVar(constValue, varCalculator);
                    }
                    break;

                default:
                    if (constOperandCalculator == null)
                    {
                        calculator = new Min_OperatorCalculator_AllVars(operandCalculators);
                    }
                    else
                    {
                        IList<OperatorCalculatorBase> varOperandCalculators = operandCalculators.Except(constOperandCalculator).ToArray();
                        double constValue = constOperandCalculator.Calculate();

                        calculator = new Min_OperatorCalculator_WithConst_AndVarArray(constValue, varOperandCalculators);
                    }

                    break;
            }

            _stack.Push(calculator);
        }

        protected override void VisitMinFollower(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            dimensionStack.Push(DEFAULT_DIMENSION_VALUE);

            base.VisitMinFollower(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase sliceLengthCalculator = _stack.Pop();
            OperatorCalculatorBase sampleCountCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            sliceLengthCalculator = sliceLengthCalculator ?? new One_OperatorCalculator();
            sampleCountCalculator = sampleCountCalculator ?? new One_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;

            dimensionStack.Pop();

            if (signalIsConst)
            {
                calculator = signalCalculator;
            }
            else
            {
                calculator = new MinFollower_OperatorCalculator(signalCalculator, sliceLengthCalculator, sampleCountCalculator, dimensionStack);
            }

            _stack.Push(calculator);
        }

        protected override void VisitMinOverDimension(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitMinOverDimension(op);

            OperatorCalculatorBase operatorCalculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase fromCalculator = _stack.Pop();
            OperatorCalculatorBase tillCalculator = _stack.Pop();
            OperatorCalculatorBase stepCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            fromCalculator = fromCalculator ?? new Zero_OperatorCalculator();
            tillCalculator = tillCalculator ?? new Zero_OperatorCalculator();
            stepCalculator = stepCalculator ?? new One_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool fromIsConst = fromCalculator is Number_OperatorCalculator;
            bool tillIsConst = tillCalculator is Number_OperatorCalculator;
            bool stepIsConst = stepCalculator is Number_OperatorCalculator;

            double from = fromIsConst ? fromCalculator.Calculate() : 0.0;
            double till = tillIsConst ? tillCalculator.Calculate() : 0.0;
            double step = stepIsConst ? stepCalculator.Calculate() : 0.0;

            bool stepIsConstZero = stepIsConst && step == 0.0;
            bool stepIsConstNegative = stepIsConst && step < 0.0;
            bool fromIsConstSpecialValue = fromIsConst && DoubleHelper.IsSpecialValue(from);
            bool tillIsConstSpecialValue = tillIsConst && DoubleHelper.IsSpecialValue(till);
            bool stepIsConstSpecialValue = stepIsConst && DoubleHelper.IsSpecialValue(step);

            if (signalIsConst)
            {
                operatorCalculator = signalCalculator;
            }
            else if (stepIsConstZero)
            {
                operatorCalculator = new Zero_OperatorCalculator();
            }
            else if (stepIsConstNegative)
            {
                operatorCalculator = new Zero_OperatorCalculator();
            }
            else if (fromIsConstSpecialValue || tillIsConstSpecialValue || stepIsConstSpecialValue)
            {
                operatorCalculator = new Number_OperatorCalculator(Double.NaN);
            }
            else
            {
                var wrapper = new MinOverDimension_OperatorWrapper(op);
                CollectionRecalculationEnum collectionRecalculationEnum = wrapper.CollectionRecalculation;
                switch (collectionRecalculationEnum)
                {
                    case CollectionRecalculationEnum.Continuous:
                        operatorCalculator = new MinOverDimension_OperatorCalculator_CollectionRecalculationContinuous(
                            signalCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        break;

                    case CollectionRecalculationEnum.UponReset:
                        operatorCalculator = new MinOverDimension_OperatorCalculator_CollectionRecalculationUponReset(
                            signalCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        break;

                    default:
                        throw new InvalidValueException(collectionRecalculationEnum);
                }
            }

            _stack.Push(operatorCalculator);
        }

        protected override void VisitMultiply(Operator op)
        {
            base.VisitMultiply(op);

            OperatorCalculatorBase calculator;

            IList<OperatorCalculatorBase> operandCalculators = new List<OperatorCalculatorBase>(op.Inlets.Count);
            for (int i = 0; i < op.Inlets.Count; i++)
            {
                OperatorCalculatorBase operandCalculator = _stack.Pop();
                operandCalculators.Add(operandCalculator);
            }
            operandCalculators = TruncateOperandCalculatorList(operandCalculators, x => x.Product());

            // Get the constant, to handle 1 and 0.
            OperatorCalculatorBase constOperandCalculator = operandCalculators.Where(x => x is Number_OperatorCalculator).SingleOrDefault();

            bool isZero = false;
            double? constValue = null;

            if (constOperandCalculator != null)
            {
                constValue = constOperandCalculator.Calculate();

                if (constValue.Value == 0.0)
                {
                    isZero = true;
                }
                else if (constValue.Value == 1.0)
                {
                    // Exclude ones
                    operandCalculators = operandCalculators.Except(constOperandCalculator).ToArray();
                }
            }

            IList<OperatorCalculatorBase> varOperandCalculators = operandCalculators.Except(constOperandCalculator).ToArray();

            // Handle zero
            if (isZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else
            {
                switch (operandCalculators.Count)
                {
                    case 0:
                        calculator = new Zero_OperatorCalculator();
                        break;

                    case 1:
                        calculator = operandCalculators[0];
                        break;

                    default:
                        if (constOperandCalculator == null)
                        {
                            calculator = OperatorCalculatorFactory.CreateMultiplyCalculatorOnlyVars(operandCalculators);
                        }
                        else
                        {
                            calculator = OperatorCalculatorFactory.CreateMultiplyCalculatorWithConst(constValue.Value, varOperandCalculators);
                        }
                        break;
                }
            }

            _stack.Push(calculator);
        }

        protected override void VisitMultiplyWithOrigin(Operator op)
        {
            base.VisitMultiplyWithOrigin(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase aCalculator = _stack.Pop();
            OperatorCalculatorBase bCalculator = _stack.Pop();
            OperatorCalculatorBase originCalculator = _stack.Pop();

            aCalculator = aCalculator ?? new One_OperatorCalculator();
            bCalculator = bCalculator ?? new One_OperatorCalculator();
            originCalculator = originCalculator ?? new Zero_OperatorCalculator();

            double a = aCalculator.Calculate();
            double b = bCalculator.Calculate();
            double origin = originCalculator.Calculate();
            bool aIsConst = aCalculator is Number_OperatorCalculator;
            bool bIsConst = bCalculator is Number_OperatorCalculator;
            bool originIsConst = originCalculator is Number_OperatorCalculator;
            bool aIsConstZero = aIsConst && a == 0;
            bool bIsConstZero = bIsConst && b == 0;
            bool originIsConstZero = originIsConst && origin == 0;
            bool aIsConstOne = aIsConst && a == 1;
            bool bIsConstOne = bIsConst && b == 1;

            if (aIsConstZero || bIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (aIsConstOne)
            {
                calculator = bCalculator;
            }
            else if (bIsConstOne)
            {
                calculator = aCalculator;
            }
            else if (originIsConstZero && aIsConst && bIsConst)
            {
                calculator = new Number_OperatorCalculator(a * b);
            }
            else if (aIsConst && bIsConst && originIsConst)
            {
                double value = (a - origin) * b + origin;
                calculator = new Number_OperatorCalculator(value);
            }
            else if (aIsConst && !bIsConst && originIsConstZero)
            {
                calculator = new Multiply_OperatorCalculator_1Const_1Var(a, bCalculator);
            }
            else if (!aIsConst && bIsConst && originIsConstZero)
            {
                calculator = new Multiply_OperatorCalculator_1Const_1Var(b, aCalculator);
            }
            else if (!aIsConst && !bIsConst && originIsConstZero)
            {
                calculator = new Multiply_OperatorCalculator_2Vars(aCalculator, bCalculator);
            }
            else if (aIsConst && !bIsConst && originIsConst)
            {
                calculator = new MultiplyWithOrigin_OperatorCalculator_ConstA_VarB_ConstOrigin(a, bCalculator, origin);
            }
            else if (!aIsConst && bIsConst && originIsConst)
            {
                calculator = new MultiplyWithOrigin_OperatorCalculator_VarA_ConstB_ConstOrigin(aCalculator, b, origin);
            }
            else if (!aIsConst && !bIsConst && originIsConst)
            {
                calculator = new MultiplyWithOrigin_OperatorCalculator_VarA_VarB_ConstOrigin(aCalculator, bCalculator, origin);
            }
            else if (aIsConst && bIsConst && !originIsConst)
            {
                calculator = new MultiplyWithOrigin_OperatorCalculator_ConstA_ConstB_VarOrigin(a, b, originCalculator);
            }
            else if (aIsConst && !bIsConst && !originIsConst)
            {
                calculator = new MultiplyWithOrigin_OperatorCalculator_ConstA_VarB_VarOrigin(a, bCalculator, originCalculator);
            }
            else if (!aIsConst && bIsConst && !originIsConst)
            {
                calculator = new MultiplyWithOrigin_OperatorCalculator_VarA_ConstB_VarOrigin(aCalculator, b, originCalculator);
            }
            else
            {
                calculator = new MultiplyWithOrigin_OperatorCalculator_VarA_VarB_VarOrigin(aCalculator, bCalculator, originCalculator);
            }

            _stack.Push(calculator);
        }

        protected override void VisitNegative(Operator op)
        {
            base.VisitNegative(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase xCalculator = _stack.Pop();

            xCalculator = xCalculator ?? new Zero_OperatorCalculator();
            double x = xCalculator.Calculate();
            bool xIsConst = xCalculator is Number_OperatorCalculator;

            if (xIsConst)
            {
                calculator = new Number_OperatorCalculator(-x);
            }
            else
            {
                calculator = new Negative_OperatorCalculator(xCalculator);
            }

            _stack.Push(calculator);
        }

        protected override void VisitNoise(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitNoise(op);

            double offset;
            if (!_operator_NoiseOffsetInSeconds_Dictionary.TryGetValue(op, out offset))
            {
                offset = NoiseCalculator.GetRandomOffset();
                _operator_NoiseOffsetInSeconds_Dictionary.Add(op, offset);
            }

            var calculator = new Noise_OperatorCalculator(offset, dimensionStack);
            _stack.Push(calculator);
        }

        protected override void VisitNot(Operator op)
        {
            base.VisitNot(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase xCalculator = _stack.Pop();

            xCalculator = xCalculator ?? new Zero_OperatorCalculator();

            double x = xCalculator.Calculate();

            bool xIsConst = xCalculator is Number_OperatorCalculator;

            if (xIsConst)
            {
                double value;

                bool aIsFalse = x == 0.0;

                if (aIsFalse) value = 1.0;
                else value = 0.0;

                calculator = new Number_OperatorCalculator(value);
            }
            else if (!xIsConst)
            {
                calculator = new Not_OperatorCalculator(xCalculator);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitNotchFilter(Operator op)
        {
            base.VisitNotchFilter(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase centerFrequencyCalculator = _stack.Pop();
            OperatorCalculatorBase bandWidthCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            centerFrequencyCalculator = centerFrequencyCalculator ?? new Zero_OperatorCalculator();
            bandWidthCalculator = bandWidthCalculator ?? new Zero_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool centerFrequencyIsConst = centerFrequencyCalculator is Number_OperatorCalculator;
            bool bandWidthIsConst = bandWidthCalculator is Number_OperatorCalculator;

            double signal = signalIsConst ? signalCalculator.Calculate() : 0.0;
            double centerFrequency = centerFrequencyIsConst ? centerFrequencyCalculator.Calculate() : 0.0;
            double bandWidth = bandWidthIsConst ? bandWidthCalculator.Calculate() : 0.0;

            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool centerFrequencyIsConstSpecialValue = centerFrequencyIsConst && DoubleHelper.IsSpecialValue(centerFrequency);
            bool bandWidthIsConstSpecialValue = bandWidthIsConst && DoubleHelper.IsSpecialValue(bandWidth);

            if (centerFrequency > _nyquistFrequency) centerFrequency = _nyquistFrequency;

            if (signalIsConstSpecialValue || centerFrequencyIsConstSpecialValue || bandWidthIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConst)
            {
                // There are no frequencies. So you a filter should do nothing.
                calculator = signalCalculator;
            }
            else if (centerFrequencyIsConst && bandWidthIsConst)
            {
                calculator = new NotchFilter_OperatorCalculator_ManyConsts(
                    signalCalculator,
                    centerFrequency,
                    bandWidth,
                    _samplingRate);
            }
            else
            {
                calculator = new NotchFilter_OperatorCalculator_AllVars(
                    signalCalculator,
                    centerFrequencyCalculator,
                    bandWidthCalculator,
                    _samplingRate,
                    _samplesBetweenApplyFilterVariables);
            }

            _stack.Push(calculator);
        }

        protected override void VisitNotEqual(Operator op)
        {
            base.VisitNotEqual(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase calculatorA = _stack.Pop();
            OperatorCalculatorBase calculatorB = _stack.Pop();

            calculatorA = calculatorA ?? new Zero_OperatorCalculator();
            calculatorB = calculatorB ?? new Zero_OperatorCalculator();

            double a = calculatorA.Calculate();
            double b = calculatorB.Calculate();

            bool aIsConst = calculatorA is Number_OperatorCalculator;
            bool bIsConst = calculatorB is Number_OperatorCalculator;

            if (aIsConst && bIsConst)
            {
                double value;

                if (a != b) value = 1.0;
                else value = 0.0;

                calculator = new Number_OperatorCalculator(value);
            }
            else if (!aIsConst && bIsConst)
            {
                calculator = new NotEqual_ConstA_VarB_OperatorCalculator(b, calculatorA);
            }
            else if (aIsConst && !bIsConst)
            {
                calculator = new NotEqual_ConstA_VarB_OperatorCalculator(a, calculatorB);
            }
            else if (!aIsConst && !bIsConst)
            {
                calculator = new NotEqual_VarA_VarB_OperatorCalculator(calculatorA, calculatorB);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitNumber(Operator op)
        {
            base.VisitNumber(op);

            OperatorCalculatorBase calculator;

            var wrapper = new Number_OperatorWrapper(op);
            double number = wrapper.Number;

            if (number == 0.0)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (number == 1.0)
            {
                calculator = new One_OperatorCalculator();
            }
            else
            {
                calculator = new Number_OperatorCalculator(number);
            }

            _stack.Push(calculator);
        }

        protected override void VisitOneOverX(Operator op)
        {
            base.VisitOneOverX(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase xCalculator = _stack.Pop();

            xCalculator = xCalculator ?? new One_OperatorCalculator();
            double x = xCalculator.Calculate();
            bool xIsConst = xCalculator is Number_OperatorCalculator;

            if (xIsConst)
            {
                calculator = new Number_OperatorCalculator(1 / x);
            }
            else
            {
                calculator = new OneOverX_OperatorCalculator(xCalculator);
            }

            _stack.Push(calculator);
        }

        protected override void VisitOr(Operator op)
        {
            base.VisitOr(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase calculatorA = _stack.Pop();
            OperatorCalculatorBase calculatorB = _stack.Pop();

            calculatorA = calculatorA ?? new Zero_OperatorCalculator();
            calculatorB = calculatorB ?? new Zero_OperatorCalculator();

            bool aIsConst = calculatorA is Number_OperatorCalculator;
            bool bIsConst = calculatorB is Number_OperatorCalculator;

            double a = aIsConst ? calculatorA.Calculate() : 0.0;
            double b = bIsConst ? calculatorB.Calculate() : 0.0;

            bool aIsConstZero = aIsConst && a == 0.0;
            bool bIsConstZero = bIsConst && b == 0.0;
            bool aIsConstNonZero = aIsConst && a != 0.0;
            bool bIsConstNonZero = bIsConst && b != 0.0;

            if (!aIsConst && !bIsConst)
            {
                calculator = new Or_VarA_VarB_OperatorCalculator(calculatorA, calculatorB);
            }
            else if (aIsConstNonZero)
            {
                calculator = new One_OperatorCalculator();
            }
            else if (bIsConstNonZero)
            {
                calculator = new One_OperatorCalculator();
            }
            else if (aIsConstZero && bIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (aIsConstZero && !bIsConst)
            {
                calculator = calculatorB;
            }
            else if (bIsConstZero && !aIsConst)
            {
                calculator = calculatorA;
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitPeakingEQFilter(Operator op)
        {
            base.VisitPeakingEQFilter(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase centerFrequencyCalculator = _stack.Pop();
            OperatorCalculatorBase bandWidthCalculator = _stack.Pop();
            OperatorCalculatorBase dbGainCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            centerFrequencyCalculator = centerFrequencyCalculator ?? new Zero_OperatorCalculator();
            bandWidthCalculator = bandWidthCalculator ?? new Zero_OperatorCalculator();
            dbGainCalculator = dbGainCalculator ?? new Zero_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool centerFrequencyIsConst = centerFrequencyCalculator is Number_OperatorCalculator;
            bool bandWidthIsConst = bandWidthCalculator is Number_OperatorCalculator;
            bool dbGainIsConst = dbGainCalculator is Number_OperatorCalculator;

            double signal = signalIsConst ? signalCalculator.Calculate() : 0.0;
            double centerFrequency = centerFrequencyIsConst ? centerFrequencyCalculator.Calculate() : 0.0;
            double bandWidth = bandWidthIsConst ? bandWidthCalculator.Calculate() : 0.0;
            double dbGain = dbGainIsConst ? dbGainCalculator.Calculate() : 0.0;

            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool centerFrequencyIsConstZero = centerFrequencyIsConst && centerFrequency == 0.0;
            bool centerFrequencyIsConstSpecialValue = centerFrequencyIsConst && DoubleHelper.IsSpecialValue(centerFrequency);
            bool bandWidthIsConstSpecialValue = bandWidthIsConst && DoubleHelper.IsSpecialValue(bandWidth);
            bool dbGainIsConstSpecialValue = dbGainIsConst && DoubleHelper.IsSpecialValue(dbGain);

            if (centerFrequency > _nyquistFrequency) centerFrequency = _nyquistFrequency;

            if (signalIsConstSpecialValue || centerFrequencyIsConstSpecialValue || bandWidthIsConstSpecialValue || dbGainIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConst)
            {
                // There are no frequencies. So you a filter should do nothing.
                calculator = signalCalculator;
            }
            else if (centerFrequencyIsConstZero)
            {
                // No filtering
                calculator = signalCalculator;
            }







            if (signalIsConst)
            {
                // There are no frequencies. So you a filter should do nothing.
                calculator = signalCalculator;
            }
            else if (centerFrequencyIsConst && bandWidthIsConst && dbGainIsConst)
            {
                calculator = new PeakingEQFilter_OperatorCalculator_ManyConsts(
                    signalCalculator,
                    centerFrequency,
                    bandWidth,
                    dbGain,
                    _samplingRate);
            }
            else
            {
                calculator = new PeakingEQFilter_OperatorCalculator_AllVars(
                    signalCalculator,
                    centerFrequencyCalculator,
                    bandWidthCalculator,
                    dbGainCalculator,
                    _samplingRate,
                    _samplesBetweenApplyFilterVariables);
            }

            _stack.Push(calculator);
        }

        protected override void VisitPower(Operator op)
        {
            base.VisitPower(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase baseCalculator = _stack.Pop();
            OperatorCalculatorBase exponentCalculator = _stack.Pop();

            // When nulls should make the operator do nothing but pass the signal.
            if (exponentCalculator == null && baseCalculator != null)
            {
                _stack.Push(baseCalculator);
                return;
            }

            baseCalculator = baseCalculator ?? new Zero_OperatorCalculator();
            exponentCalculator = exponentCalculator ?? new Zero_OperatorCalculator();
            double @base = baseCalculator.Calculate();
            double exponent = exponentCalculator.Calculate();
            bool baseIsConst = baseCalculator is Number_OperatorCalculator;
            bool exponentIsConst = exponentCalculator is Number_OperatorCalculator;
            bool baseIsConstZero = baseIsConst && @base == 0;
            bool exponentIsConstZero = exponentIsConst && exponent == 0;

            if (baseIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (exponentIsConstZero)
            {
                calculator = baseCalculator;
            }
            else if (baseIsConst && exponentIsConst)
            {
                calculator = new Number_OperatorCalculator(Math.Pow(@base, exponent));
            }
            else if (baseIsConst)
            {
                calculator = new Power_OperatorCalculator_ConstBase_VarExponent(@base, exponentCalculator);
            }
            else if (exponentIsConst)
            {
                calculator = new Power_OperatorCalculator_VarBase_ConstExponent(baseCalculator, exponent);
            }
            else
            {
                calculator = new Power_OperatorCalculator_VarBase_VarExponent(baseCalculator, exponentCalculator);
            }

            _stack.Push(calculator);
        }

        protected override void VisitPulse(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitPulse(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase frequencyCalculator = _stack.Pop();
            OperatorCalculatorBase widthCalculator = _stack.Pop();

            frequencyCalculator = frequencyCalculator ?? new Zero_OperatorCalculator();
            widthCalculator = widthCalculator ?? new Number_OperatorCalculator(0.5);

            double frequency = frequencyCalculator.Calculate();
            double width = widthCalculator.Calculate() % 1.0;

            bool frequencyIsConst = frequencyCalculator is Number_OperatorCalculator;
            bool widthIsConst = widthCalculator is Number_OperatorCalculator;

            bool frequencyIsConstZero = frequencyIsConst && frequency == 0.0;
            bool widthIsConstZero = widthIsConst && width == 0.0;

            bool frequencyIsConstSpecialValue = frequencyIsConst && DoubleHelper.IsSpecialValue(frequency);
            bool widthIsConstSpecialValue = widthIsConst && DoubleHelper.IsSpecialValue(width);

            bool widthIsConstHalf = widthIsConst && width == 0.5;

            if (frequencyIsConstSpecialValue || widthIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (frequencyIsConstZero)
            {
                // Special Value
                // (Frequency 0 means time stands still. In theory this could produce a different value than 0.
                //  but I feel I would have to make disproportionate effort to take that into account.)
                calculator = new Zero_OperatorCalculator();
            }
            else if (widthIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (frequencyIsConst && widthIsConstHalf && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_ConstFrequency_HalfWidth_WithOriginShifting(frequency, dimensionStack);
            }
            else if (frequencyIsConst && widthIsConstHalf && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_ConstFrequency_HalfWidth_NoOriginShifting(frequency, dimensionStack);
            }
            else if (frequencyIsConst && widthIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_ConstFrequency_ConstWidth_WithOriginShifting(frequency, width, dimensionStack);
            }
            else if (frequencyIsConst && widthIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_ConstFrequency_ConstWidth_NoOriginShifting(frequency, width, dimensionStack);
            }
            else if (frequencyIsConst && !widthIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_ConstFrequency_VarWidth_WithOriginShifting(frequency, widthCalculator, dimensionStack);
            }
            else if (frequencyIsConst && !widthIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_ConstFrequency_VarWidth_NoOriginShifting(frequency, widthCalculator,dimensionStack);
            }
            else if (!frequencyIsConst && widthIsConstHalf && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_VarFrequency_HalfWidth_WithPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else if (!frequencyIsConst && widthIsConstHalf && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_VarFrequency_HalfWidth_NoPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else if (!frequencyIsConst && widthIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_VarFrequency_ConstWidth_WithPhaseTracking(frequencyCalculator, width, dimensionStack);
            }
            else if (!frequencyIsConst && widthIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_VarFrequency_ConstWidth_NoPhaseTracking(frequencyCalculator, width, dimensionStack);
            }
            else if (!frequencyIsConst && !widthIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_VarFrequency_VarWidth_WithPhaseTracking(frequencyCalculator, widthCalculator, dimensionStack);
            }
            else if (!frequencyIsConst && !widthIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_VarFrequency_VarWidth_NoPhaseTracking(frequencyCalculator, widthCalculator, dimensionStack);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitPulseTrigger(Operator op)
        {
            base.VisitPulseTrigger(op);

            OperatorCalculatorBase operatorCalculator;

            OperatorCalculatorBase calculationCalculator = _stack.Pop();
            OperatorCalculatorBase resetCalculator = _stack.Pop();

            calculationCalculator = calculationCalculator ?? new Zero_OperatorCalculator();
            resetCalculator = resetCalculator ?? new Zero_OperatorCalculator();

            bool calculationIsConst = calculationCalculator is Number_OperatorCalculator;
            bool resetIsConst = resetCalculator is Number_OperatorCalculator;

            if (calculationIsConst)
            {
                operatorCalculator = calculationCalculator;
            }
            else if (resetIsConst)
            {
                operatorCalculator = calculationCalculator;
            }
            else
            {
                operatorCalculator = new PulseTrigger_OperatorCalculator(calculationCalculator, resetCalculator);
            }

            _stack.Push(operatorCalculator);
        }

        protected override void VisitRandom(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitRandom(op);

            int randomCalculatorOffset;
            if (!_operator_RandomOffsetInSeconds_Dictionary.TryGetValue(op, out randomCalculatorOffset))
            {
                randomCalculatorOffset = RandomCalculatorBase.GetRandomOffset();
                _operator_RandomOffsetInSeconds_Dictionary.Add(op, randomCalculatorOffset);
            }

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase rateCalculator = _stack.Pop();

            rateCalculator = rateCalculator ?? new Zero_OperatorCalculator();

            double rate = rateCalculator.Calculate();

            bool rateIsConst = rateCalculator is Number_OperatorCalculator;

            bool rateIsConstZero = rateIsConst && rate == 0;

            bool rateIsConstSpecialValue = rateIsConst && DoubleHelper.IsSpecialValue(rate);

            if (rateIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (rateIsConstZero)
            {
                // Special Value
                calculator = new Zero_OperatorCalculator();
            }
            // TODO: Add more variations.
            else
            {
                var wrapper = new Random_OperatorWrapper(op);
                ResampleInterpolationTypeEnum resampleInterpolationTypeEnum = wrapper.InterpolationType;

                switch (resampleInterpolationTypeEnum)
                {
                    case ResampleInterpolationTypeEnum.Block:
                        {
                            var randomCalculator = new RandomCalculator_BlockInterpolation();

                            calculator = new Random_OperatorCalculator_BlockAndStripe_VarFrequency(
                                randomCalculator,
                                randomCalculatorOffset,
                                rateCalculator,
                                dimensionStack);

                            break;
                        }

                    case ResampleInterpolationTypeEnum.Stripe:
                        {
                            var randomCalculator = new RandomCalculator_StripeInterpolation();

                            calculator = new Random_OperatorCalculator_BlockAndStripe_VarFrequency(
                                randomCalculator,
                                randomCalculatorOffset,
                                rateCalculator,
                                dimensionStack);

                            break;
                        }

                    case ResampleInterpolationTypeEnum.LineRememberT0:
                    case ResampleInterpolationTypeEnum.LineRememberT1:
                    case ResampleInterpolationTypeEnum.CubicEquidistant:
                    case ResampleInterpolationTypeEnum.CubicAbruptSlope:
                    case ResampleInterpolationTypeEnum.CubicSmoothSlope:
                    case ResampleInterpolationTypeEnum.Hermite:
                        {
                            var randomCalculator = new RandomCalculator_StripeInterpolation();

                            calculator = new Random_OperatorCalculator_OtherInterpolationTypes(
                                randomCalculator,
                                randomCalculatorOffset,
                                rateCalculator,
                                resampleInterpolationTypeEnum,
                                dimensionStack);

                            break;
                        }
                    
                    default:
                        throw new ValueNotSupportedException(resampleInterpolationTypeEnum);
                }
            }

            _stack.Push(calculator);
        }

        protected override void VisitRange(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitRange(op);

            OperatorCalculatorBase operatorCalculator;

            OperatorCalculatorBase fromCalculator = _stack.Pop();
            OperatorCalculatorBase tillCalculator = _stack.Pop();
            OperatorCalculatorBase stepCalculator = _stack.Pop();

            fromCalculator = fromCalculator ?? new Zero_OperatorCalculator();
            tillCalculator = tillCalculator ?? new Zero_OperatorCalculator();
            stepCalculator = stepCalculator ?? new One_OperatorCalculator();

            bool fromIsConst = fromCalculator is Number_OperatorCalculator;
            bool tillIsConst = tillCalculator is Number_OperatorCalculator;
            bool stepIsConst = stepCalculator is Number_OperatorCalculator;

            double from = fromIsConst ? fromCalculator.Calculate() : 0.0;
            double till = tillIsConst ? tillCalculator.Calculate() : 0.0;
            double step = stepIsConst ? stepCalculator.Calculate() : 0.0;

            bool stepIsConstZero = stepIsConst && step == 0.0;
            bool stepIsConstOne = stepIsConst && step == 1.0;
            bool fromIsConstSpecialValue = fromIsConst && DoubleHelper.IsSpecialValue(from);
            bool tillIsConstSpecialValue = tillIsConst && DoubleHelper.IsSpecialValue(till);
            bool stepIsConstSpecialValue = stepIsConst && DoubleHelper.IsSpecialValue(step);

            if (stepIsConstZero)
            {
                // Would eventually lead to divide by zero and an infinite amount of index positions.
                operatorCalculator = new Number_OperatorCalculator(Double.NaN);
            }
            if (fromIsConstSpecialValue || tillIsConstSpecialValue || stepIsConstSpecialValue)
            {
                operatorCalculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (fromIsConst && tillIsConst && stepIsConstOne)
            {
                operatorCalculator = new Range_OperatorCalculator_WithConstants_AndStepOne(from, till, dimensionStack);
            }
            else if (fromIsConst && tillIsConst && stepIsConst)
            {
                operatorCalculator = new Range_OperatorCalculator_WithConstants(from, till, step, dimensionStack);
            }
            else if (!fromIsConst && !tillIsConst && !stepIsConst)
            {
                operatorCalculator = new Range_OperatorCalculator_WithVariables(fromCalculator, tillCalculator, stepCalculator, dimensionStack);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(operatorCalculator);
        }

        protected override void VisitResample(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            dimensionStack.Push(DEFAULT_DIMENSION_VALUE);

            base.VisitResample(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase samplingRateCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            samplingRateCalculator = samplingRateCalculator ?? new Zero_OperatorCalculator();

            double signal = signalCalculator.Calculate();
            double samplingRate = samplingRateCalculator.Calculate();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool samplingRateIsConst = samplingRateCalculator is Number_OperatorCalculator;

            bool samplingRateIsConstZero = samplingRateIsConst && samplingRate == 0;

            bool samplingRateIsConstSpecialValue = samplingRateIsConst && DoubleHelper.IsSpecialValue(samplingRate);

            dimensionStack.Pop();

            if (signalIsConst)
            {
                // Const signal Preceeds weird numbers.
                calculator = new Number_OperatorCalculator(signal);
            }
            else if (samplingRateIsConstZero)
            {
                // Special Value: Time stands still.
                calculator = new Number_OperatorCalculator(signal);
            }
            else if (samplingRateIsConstSpecialValue)
            {
                // Wierd number
                // Note that if signal is const,
                // an indeterminate sampling rate can still render a determinite result.
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else
            {
                var wrapper = new Resample_OperatorWrapper(op);
                ResampleInterpolationTypeEnum resampleInterpolationTypeEnum = wrapper.InterpolationType;

                calculator = OperatorCalculatorFactory.CreateResample_OperatorCalculator(resampleInterpolationTypeEnum, signalCalculator, samplingRateCalculator, dimensionStack);
            }

            _stack.Push(calculator);
        }

        protected override void VisitReverse(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            dimensionStack.Push(DEFAULT_DIMENSION_VALUE);

            base.VisitReverse(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase speedCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            speedCalculator = speedCalculator ?? new One_OperatorCalculator();

            double signal = signalCalculator.Calculate();
            double speed = speedCalculator.Calculate();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool speedIsConst = speedCalculator is Number_OperatorCalculator;

            bool speedIsConstZero = speedIsConst && speed == 0;

            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool speedIsConstSpecialValue = speedIsConst && DoubleHelper.IsSpecialValue(speed);

            dimensionStack.Pop();

            if (speedIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(signal);
            }
            else if (speedIsConstZero)
            {
                // Special Value
                // Speed-up of 0 means time stands still.
                calculator = new Number_OperatorCalculator(signal);
            }
            else if (signalIsConst)
            {
                calculator = signalCalculator;
            }
            else if (speedIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Reverse_OperatorCalculator_ConstSpeed_WithOriginShifting(signalCalculator, speed, dimensionStack);
            }
            else if (speedIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Reverse_OperatorCalculator_ConstSpeed_NoOriginShifting(signalCalculator, speed, dimensionStack);
            }
            else if (!speedIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Reverse_OperatorCalculator_VarSpeed_WithPhaseTracking(signalCalculator, speedCalculator, dimensionStack);
            }
            else if (!speedIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Reverse_OperatorCalculator_VarSpeed_NoPhaseTracking(signalCalculator, speedCalculator, dimensionStack);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitRound(Operator op)
        {
            base.VisitRound(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase stepCalculator = _stack.Pop();
            OperatorCalculatorBase offsetCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            stepCalculator = stepCalculator ?? new One_OperatorCalculator();
            offsetCalculator = offsetCalculator ?? new Zero_OperatorCalculator();

            double signal = signalCalculator.Calculate();
            double step = stepCalculator.Calculate();
            double offset = offsetCalculator.Calculate();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool stepIsConst = stepCalculator is Number_OperatorCalculator;
            bool offsetIsConst = offsetCalculator is Number_OperatorCalculator;

            bool offsetIsConstZero = offsetIsConst && offset % 1.0 == 0;

            bool stepIsConstOne = stepIsConst && step == 1;

            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool stepIsConstSpecialValue = stepIsConst && DoubleHelper.IsSpecialValue(step);
            bool offsetIsConstSpecialValue = offsetIsConst && DoubleHelper.IsSpecialValue(offset);

            if (signalIsConstSpecialValue || stepIsConstSpecialValue || offsetIsConstSpecialValue)
            {
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConst)
            {
                calculator = new Round_ConstSignal_OperatorCalculator(signal, stepCalculator, offsetCalculator);
            }
            else if (stepIsConstOne && offsetIsConstZero)
            {
                calculator = new Round_VarSignal_StepOne_OffsetZero(signalCalculator);
            }
            else
            {
                if (stepIsConst && offsetIsConstZero)
                {
                    calculator = new Round_ConstStep_ZeroOffSet_OperatorCalculator(signalCalculator, step);
                }
                else if (stepIsConst && offsetIsConst)
                {
                    calculator = new Round_ConstStep_ConstOffSet_OperatorCalculator(signalCalculator, step, offset);
                }
                else if (stepIsConst && !offsetIsConst)
                {
                    calculator = new Round_ConstStep_VarOffSet_OperatorCalculator(signalCalculator, step, offsetCalculator);
                }
                else if (!stepIsConst && offsetIsConstZero)
                {
                    calculator = new Round_VarStep_ZeroOffSet_OperatorCalculator(signalCalculator, stepCalculator);
                }
                else if (!stepIsConst && offsetIsConst)
                {
                    calculator = new Round_VarStep_ConstOffSet_OperatorCalculator(signalCalculator, stepCalculator, offset);
                }
                else if (!stepIsConst && !offsetIsConst)
                {
                    calculator = new Round_VarStep_VarOffSet_OperatorCalculator(signalCalculator, stepCalculator, offsetCalculator);
                }
                else
                {
                    throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
                }
            }

            _stack.Push(calculator);
        }

        protected override void VisitSampleOperator(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            DimensionStack channelDimensionStack = _dimensionStackCollection.GetDimensionStack(DimensionEnum.Channel);

            base.VisitSampleOperator(op);

            OperatorCalculatorBase calculator = null;

            OperatorCalculatorBase frequencyCalculator = _stack.Pop();

            frequencyCalculator = frequencyCalculator ?? new Zero_OperatorCalculator();
            double frequency = frequencyCalculator.Calculate();
            bool frequencyIsConst = frequencyCalculator is Number_OperatorCalculator;
            bool frequencyIsConstZero = frequencyIsConst && frequency == 0.0;

            var wrapper = new Sample_OperatorWrapper(op, _sampleRepository);
            SampleInfo sampleInfo = wrapper.SampleInfo;
            if (sampleInfo.Sample == null)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (frequencyIsConstZero)
            {
                // Special Value
                calculator = new Zero_OperatorCalculator();
            }
            else
            {
                ISampleCalculator sampleCalculator = _calculatorCache.GetSampleCalculator(sampleInfo);

                int sampleChannelCount = sampleInfo.Sample.GetChannelCount();

                if (sampleChannelCount == _channelCount)
                {
                    if (frequencyIsConst && dimensionEnum == DimensionEnum.Time)
                    {
                        calculator = new Sample_OperatorCalculator_ConstFrequency_WithOriginShifting(frequency, sampleCalculator, dimensionStack, channelDimensionStack);
                    }
                    else if (frequencyIsConst && dimensionEnum != DimensionEnum.Time)
                    {
                        calculator = new Sample_OperatorCalculator_ConstFrequency_NoOriginShifting(frequency, sampleCalculator, dimensionStack, channelDimensionStack);
                    }
                    else if (!frequencyIsConst && dimensionEnum == DimensionEnum.Time)
                    {
                        calculator = new Sample_OperatorCalculator_VarFrequency_WithPhaseTracking(frequencyCalculator, sampleCalculator, dimensionStack, channelDimensionStack);
                    }
                    else if (!frequencyIsConst && dimensionEnum != DimensionEnum.Time)
                    {
                        calculator = new Sample_OperatorCalculator_VarFrequency_NoPhaseTracking(frequencyCalculator, sampleCalculator, dimensionStack, channelDimensionStack);
                    }
                }
                else if (sampleChannelCount == 1 && _channelCount == 2)
                {
                    if (frequencyIsConst && dimensionEnum == DimensionEnum.Time)
                    {
                        calculator = new Sample_OperatorCalculator_ConstFrequency_MonoToStereo_WithOriginShifting(frequency, sampleCalculator, dimensionStack);
                    }
                    else if (frequencyIsConst && dimensionEnum != DimensionEnum.Time)
                    {
                        calculator = new Sample_OperatorCalculator_ConstFrequency_MonoToStereo_NoOriginShifting(frequency, sampleCalculator, dimensionStack);
                    }
                    else if (!frequencyIsConst && dimensionEnum == DimensionEnum.Time)
                    {
                        calculator = new Sample_OperatorCalculator_VarFrequency_MonoToStereo_WithPhaseTracking(frequencyCalculator, sampleCalculator, dimensionStack);
                    }
                    else if (!frequencyIsConst && dimensionEnum != DimensionEnum.Time)
                    {
                        calculator = new Sample_OperatorCalculator_VarFrequency_MonoToStereo_NoPhaseTracking(frequencyCalculator, sampleCalculator, dimensionStack);
                    }
                }
                else if (sampleChannelCount == 2 && _channelCount == 1)
                {
                    if (frequencyIsConst && dimensionEnum == DimensionEnum.Time)
                    {
                        calculator = new Sample_OperatorCalculator_ConstFrequency_StereoToMono_WithOriginShifting(frequency, sampleCalculator, dimensionStack);
                    }
                    else if (frequencyIsConst && dimensionEnum != DimensionEnum.Time)
                    {
                        calculator = new Sample_OperatorCalculator_ConstFrequency_StereoToMono_NoOriginShifting(frequency, sampleCalculator, dimensionStack);
                    }
                    else if (!frequencyIsConst && dimensionEnum == DimensionEnum.Time)
                    {
                        calculator = new Sample_OperatorCalculator_VarFrequency_StereoToMono_WithPhaseTracking(frequencyCalculator, sampleCalculator, dimensionStack);
                    }
                    else if (!frequencyIsConst && dimensionEnum != DimensionEnum.Time)
                    {
                        calculator = new Sample_OperatorCalculator_VarFrequency_StereoToMono_NoPhaseTracking(frequencyCalculator, sampleCalculator, dimensionStack);
                    }
                }
            }

            if (calculator == null)
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitSawDown(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitSawDown(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase frequencyCalculator = _stack.Pop();

            frequencyCalculator = frequencyCalculator ?? new Zero_OperatorCalculator();

            double frequency = frequencyCalculator.Calculate();

            bool frequencyIsConst = frequencyCalculator is Number_OperatorCalculator;
            bool frequencyIsConstZero = frequencyIsConst && frequency == 0;

            if (frequencyIsConstZero)
            {
                // Special Value
                calculator = new Zero_OperatorCalculator();
            }
            else if (frequencyIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new SawDown_OperatorCalculator_ConstFrequency_WithOriginShifting(frequency, dimensionStack);
            }
            else if (frequencyIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new SawDown_OperatorCalculator_ConstFrequency_NoOriginShifting(frequency, dimensionStack);
            }
            else if (!frequencyIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new SawDown_OperatorCalculator_VarFrequency_WithPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else if (!frequencyIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new SawDown_OperatorCalculator_VarFrequency_NoPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitSawUp(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitSawUp(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase frequencyCalculator = _stack.Pop();

            frequencyCalculator = frequencyCalculator ?? new Zero_OperatorCalculator();
            double frequency = frequencyCalculator.Calculate();
            bool frequencyIsConst = frequencyCalculator is Number_OperatorCalculator;
            bool frequencyIsConstZero = frequencyIsConst && frequency == 0;

            if (frequencyIsConstZero)
            {
                // Special Value
                calculator = new Zero_OperatorCalculator();
            }
            else if (frequencyIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new SawUp_OperatorCalculator_ConstFrequency_WithOriginShifting(frequency, dimensionStack);
            }
            else if (frequencyIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new SawUp_OperatorCalculator_ConstFrequency_NoOriginShifting(frequency, dimensionStack);
            }
            else if (!frequencyIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new SawUp_OperatorCalculator_VarFrequency_WithPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else if (!frequencyIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new SawUp_OperatorCalculator_VarFrequency_NoPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else if (frequencyIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new SawUp_OperatorCalculator_ConstFrequency_WithOriginShifting(frequency, dimensionStack);
            }
            else if (frequencyIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new SawUp_OperatorCalculator_ConstFrequency_NoOriginShifting(frequency, dimensionStack);
            }
            else if (!frequencyIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new SawUp_OperatorCalculator_VarFrequency_WithPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else if (!frequencyIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new SawUp_OperatorCalculator_VarFrequency_NoPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitScaler(Operator op)
        {
            base.VisitScaler(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase sourceValueACalculator = _stack.Pop();
            OperatorCalculatorBase sourceValueBCalculator = _stack.Pop();
            OperatorCalculatorBase targetValueACalculator = _stack.Pop();
            OperatorCalculatorBase targetValueBCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            sourceValueACalculator = sourceValueACalculator ?? new Zero_OperatorCalculator();
            sourceValueBCalculator = sourceValueBCalculator ?? new One_OperatorCalculator();
            targetValueACalculator = targetValueACalculator ?? new Zero_OperatorCalculator();
            targetValueBCalculator = targetValueBCalculator ?? new One_OperatorCalculator();

            bool sourceValueAIsConst = sourceValueACalculator is Number_OperatorCalculator;
            bool sourceValueBIsConst = sourceValueBCalculator is Number_OperatorCalculator;
            bool targetValueAIsConst = targetValueACalculator is Number_OperatorCalculator;
            bool targetValueBIsConst = targetValueBCalculator is Number_OperatorCalculator;

            double sourceValueA = sourceValueAIsConst ? sourceValueACalculator.Calculate() : 0.0;
            double sourceValueB = sourceValueBIsConst ? sourceValueBCalculator.Calculate() : 0.0;
            double targetValueA = targetValueAIsConst ? targetValueACalculator.Calculate() : 0.0;
            double targetValueB = targetValueBIsConst ? targetValueBCalculator.Calculate() : 0.0;

            if (sourceValueAIsConst && sourceValueBIsConst && targetValueAIsConst && targetValueBIsConst)
            {
                calculator = new Scaler_OperatorCalculator_ManyConstants(signalCalculator, sourceValueA, sourceValueB, targetValueA, targetValueB);
            }
            else
            {
                calculator = new Scaler_OperatorCalculator_AllVariables(signalCalculator, sourceValueACalculator, sourceValueBCalculator, targetValueACalculator, targetValueBCalculator);
            }

            _stack.Push(calculator);
        }

        protected override void VisitSelect(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            dimensionStack.Push(DEFAULT_DIMENSION_VALUE);

            base.VisitSelect(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase positionCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            positionCalculator = positionCalculator ?? new Zero_OperatorCalculator();

            double signal = signalCalculator.Calculate();
            double position = positionCalculator.Calculate();
            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool positionIsConst = positionCalculator is Number_OperatorCalculator;

            dimensionStack.Pop();

            if (signalIsConst)
            {
                calculator = new Number_OperatorCalculator(signal);
            }
            else if (positionIsConst)
            {
                calculator = new Select_OperatorCalculator_ConstPosition(signalCalculator, position, dimensionStack);
            }
            else
            {
                calculator = new Select_OperatorCalculator_VarPosition(signalCalculator, positionCalculator, dimensionStack);
            }

            _stack.Push(calculator);
        }

        protected override void VisitSetDimension(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            dimensionStack.Push(DEFAULT_DIMENSION_VALUE);

            base.VisitSetDimension(op);

            OperatorCalculatorBase operatorCalculator;

            OperatorCalculatorBase calculationCalculator = _stack.Pop();
            OperatorCalculatorBase valueCalculator = _stack.Pop();

            calculationCalculator = calculationCalculator ?? new Zero_OperatorCalculator();
            valueCalculator = valueCalculator ?? new Zero_OperatorCalculator();

            bool calculationIsConst = calculationCalculator is Number_OperatorCalculator;
            bool valueIsConst = valueCalculator is Number_OperatorCalculator;

            double value = valueCalculator.Calculate();

            dimensionStack.Pop();

            if (calculationIsConst)
            {
                operatorCalculator = calculationCalculator;
            }
            else if (valueIsConst)
            {
                operatorCalculator = new SetDimension_OperatorCalculator_ConstValue(calculationCalculator, value, dimensionStack);
            }
            else
            {
                operatorCalculator = new SetDimension_OperatorCalculator_VarValue(calculationCalculator, valueCalculator, dimensionStack);
            }

            _stack.Push(operatorCalculator);
        }

        protected override void VisitSine(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitSine(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase frequencyCalculator = _stack.Pop();
            frequencyCalculator = frequencyCalculator ?? new Zero_OperatorCalculator();
            double frequency = frequencyCalculator.Calculate();
            bool frequencyIsConst = frequencyCalculator is Number_OperatorCalculator;
            bool frequencyIsConstZero = frequencyIsConst && frequency == 0.0;
            bool frequencyIsConstSpecialValue = frequencyIsConst && DoubleHelper.IsSpecialValue(frequency);

            if (frequencyIsConstSpecialValue)
            {
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            if (frequencyIsConstZero)
            {
                // Special value
                // Frequency 0 means time stands still.
                calculator = new Zero_OperatorCalculator();
            }
            else if (frequencyIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Sine_OperatorCalculator_ConstFrequency_WithOriginShifting(frequency, dimensionStack);
            }
            else if (frequencyIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Sine_OperatorCalculator_ConstFrequency_NoOriginShifting(frequency, dimensionStack);
            }
            else if (!frequencyIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Sine_OperatorCalculator_VarFrequency_WithPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else if (!frequencyIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Sine_OperatorCalculator_VarFrequency_NoPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitSort(Operator op)
        {
            //DimensionEnum dimensionEnum = op.GetDimensionEnum();
            //DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            throw new NotImplementedException();

            base.VisitSort(op);

            OperatorCalculatorBase calculator;

            IList<OperatorCalculatorBase> operandCalculators = new List<OperatorCalculatorBase>(op.Inlets.Count);
            for (int i = 0; i < op.Inlets.Count; i++)
            {
                OperatorCalculatorBase operandCalculator = _stack.Pop();
                operandCalculators.Add(operandCalculator);
            }

            operandCalculators = operandCalculators.Where(x => x != null).ToArray();

            switch (operandCalculators.Count)
            {
                case 0:
                    calculator = new Zero_OperatorCalculator();
                    break;

                default:
                    throw new NotImplementedException();
                    //calculator = new Sort_OperatorCalculator(operandCalculators, dimensionStack);
                    break;
            }

            _stack.Push(calculator);
        }

        protected override void VisitSortOverDimension(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitSortOverDimension(op);

            OperatorCalculatorBase operatorCalculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase fromCalculator = _stack.Pop();
            OperatorCalculatorBase tillCalculator = _stack.Pop();
            OperatorCalculatorBase stepCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            fromCalculator = fromCalculator ?? new Zero_OperatorCalculator();
            tillCalculator = tillCalculator ?? new Zero_OperatorCalculator();
            stepCalculator = stepCalculator ?? new One_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool fromIsConst = fromCalculator is Number_OperatorCalculator;
            bool tillIsConst = tillCalculator is Number_OperatorCalculator;
            bool stepIsConst = stepCalculator is Number_OperatorCalculator;

            double from = fromIsConst ? fromCalculator.Calculate() : 0.0;
            double till = tillIsConst ? tillCalculator.Calculate() : 0.0;
            double step = stepIsConst ? stepCalculator.Calculate() : 0.0;

            bool stepIsConstZero = stepIsConst && step == 0.0;
            bool stepIsConstNegative = stepIsConst && step < 0.0;
            bool fromIsConstSpecialValue = fromIsConst && DoubleHelper.IsSpecialValue(from);
            bool tillIsConstSpecialValue = tillIsConst && DoubleHelper.IsSpecialValue(till);
            bool stepIsConstSpecialValue = stepIsConst && DoubleHelper.IsSpecialValue(step);

            if (signalIsConst)
            {
                operatorCalculator = signalCalculator;
            }
            else if (stepIsConstZero)
            {
                operatorCalculator = new Zero_OperatorCalculator();
            }
            else if (stepIsConstNegative)
            {
                operatorCalculator = new Zero_OperatorCalculator();
            }
            else if (fromIsConstSpecialValue || tillIsConstSpecialValue || stepIsConstSpecialValue)
            {
                operatorCalculator = new Number_OperatorCalculator(Double.NaN);
            }
            else
            {
                var wrapper = new SortOverDimension_OperatorWrapper(op);
                CollectionRecalculationEnum collectionRecalculationEnum = wrapper.CollectionRecalculation;
                switch (collectionRecalculationEnum)
                {
                    case CollectionRecalculationEnum.Continuous:
                        operatorCalculator = new SortOverDimension_OperatorCalculator_CollectionRecalculationContinuous(
                            signalCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        break;

                    case CollectionRecalculationEnum.UponReset:
                        operatorCalculator = new SortOverDimension_OperatorCalculator_CollectionRecalculationUponReset(
                            signalCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        break;

                    default:
                        throw new InvalidValueException(collectionRecalculationEnum);
                }
            }

            _stack.Push(operatorCalculator);
        }

        protected override void VisitSpectrum(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            dimensionStack.Push(DEFAULT_DIMENSION_VALUE);

            base.VisitSpectrum(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase startCalculator = _stack.Pop();
            OperatorCalculatorBase endCalculator = _stack.Pop();
            OperatorCalculatorBase frequencyCountCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            startCalculator = startCalculator ?? new Zero_OperatorCalculator();
            endCalculator = endCalculator ?? new One_OperatorCalculator();
            frequencyCountCalculator = frequencyCountCalculator ?? new Number_OperatorCalculator(2);

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool startIsConst = startCalculator is Number_OperatorCalculator;
            bool endIsConst = endCalculator is Number_OperatorCalculator;
            bool frequencyCountIsConst = frequencyCountCalculator is Number_OperatorCalculator;

            double signal = signalIsConst ? signalCalculator.Calculate() : 0.0;
            double start = startIsConst ? startCalculator.Calculate() : 0.0;
            double end = endIsConst ? endCalculator.Calculate() : 0.0;
            double frequencyCount = frequencyCountIsConst ? frequencyCountCalculator.Calculate() : 0.0;

            bool signalIsConstZero = signalIsConst && signal == 0;
            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool startIsConstSpecialValue = startIsConst && DoubleHelper.IsSpecialValue(start);
            bool endIsConstSpecialValue = endIsConst && DoubleHelper.IsSpecialValue(end);
            bool frequencyCountIsConstSpecialValue = frequencyCountIsConst && DoubleHelper.IsSpecialValue(frequencyCount);

            dimensionStack.Pop();

            if (signalIsConstSpecialValue || startIsConstSpecialValue || endIsConstSpecialValue || frequencyCountIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (signalIsConst)
            {
                // Zero frequencies
                calculator = new Zero_OperatorCalculator();
            }
            else
            {
                calculator = new Spectrum_OperatorCalculator(
                    signalCalculator,
                    startCalculator,
                    endCalculator,
                    frequencyCountCalculator,
                    dimensionStack);
            }

            _stack.Push(calculator);
        }

        protected override void VisitSquare(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitSquare(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase frequencyCalculator = _stack.Pop();
            frequencyCalculator = frequencyCalculator ?? new Zero_OperatorCalculator();
            double frequency = frequencyCalculator.Calculate();
            bool frequencyIsConst = frequencyCalculator is Number_OperatorCalculator;
            bool frequencyIsConstZero = frequencyIsConst && frequency == 0;
            bool frequencyIsConstSpecialValue = frequencyIsConst && DoubleHelper.IsSpecialValue(frequency);

            if (frequencyIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            if (frequencyIsConstZero)
            {
                // Special Value
                calculator = new Zero_OperatorCalculator();
            }
            else if (frequencyIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_ConstFrequency_HalfWidth_WithOriginShifting(frequency, dimensionStack);
            }
            else if (frequencyIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_ConstFrequency_HalfWidth_NoOriginShifting(frequency, dimensionStack);
            }
            else if (!frequencyIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_VarFrequency_HalfWidth_WithPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else if (!frequencyIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Pulse_OperatorCalculator_VarFrequency_HalfWidth_NoPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitSquash(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            dimensionStack.Push(DEFAULT_DIMENSION_VALUE);

            base.VisitSquash(op);

            OperatorCalculatorBase calculator = null;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase factorCalculator = _stack.Pop();
            OperatorCalculatorBase originCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            factorCalculator = factorCalculator ?? new One_OperatorCalculator();
            originCalculator = originCalculator ?? new Zero_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool factorIsConst = factorCalculator is Number_OperatorCalculator;
            bool originIsConst = originCalculator is Number_OperatorCalculator;

            double signal = signalIsConst ? signalCalculator.Calculate() : 0.0;
            double factor = factorIsConst ? factorCalculator.Calculate() : 0.0;
            double origin = originIsConst ? originCalculator.Calculate() : 0.0;

            bool signalIsConstZero = signalIsConst && signal == 0.0;
            bool factorIsConstZero = factorIsConst && factor == 0.0;
            bool originIsConstZero = originIsConst && origin == 0.0;

            bool factorIsConstOne = factorIsConst && factor == 1.0;

            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool factorIsConstSpecialValue = factorIsConst && DoubleHelper.IsSpecialValue(factor);
            bool originIsConstSpecialValue = originIsConst && DoubleHelper.IsSpecialValue(origin);

            dimensionStack.Pop();

            if (signalIsConstSpecialValue || factorIsConstSpecialValue || originIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (factorIsConstZero)
            {
                // Special Value
                // Speed-up of 0 means time stands still.
                calculator = new Number_OperatorCalculator(signal);
            }
            else if (signalIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (factorIsConstOne)
            {
                calculator = signalCalculator;
            }
            else if (signalIsConst)
            {
                calculator = signalCalculator;
            }
            else if (dimensionEnum == DimensionEnum.Time)
            {
                if (!signalIsConst && factorIsConst)
                {
                    calculator = new Squash_OperatorCalculator_VarSignal_ConstFactor_WithOriginShifting(signalCalculator, factor, dimensionStack);
                }
                else if (!signalIsConst && !factorIsConst)
                {
                    calculator = new Squash_OperatorCalculator_VarSignal_VarFactor_WithPhaseTracking(signalCalculator, factorCalculator, dimensionStack);
                }
            }
            else
            {
                if (!signalIsConst && factorIsConst && originIsConstZero)
                {
                    calculator = new Squash_OperatorCalculator_VarSignal_ConstFactor_ZeroOrigin(signalCalculator, factor, dimensionStack);
                }
                else if (!signalIsConst && !factorIsConst && originIsConstZero)
                {
                    calculator = new Squash_OperatorCalculator_VarSignal_VarFactor_ZeroOrigin(signalCalculator, factorCalculator, dimensionStack);
                }
                else if (!signalIsConst && factorIsConst && originIsConst)
                {
                    calculator = new Squash_OperatorCalculator_VarSignal_ConstFactor_ConstOrigin(signalCalculator, factor, origin, dimensionStack);
                }
                else if (!signalIsConst && factorIsConst && !originIsConst)
                {
                    calculator = new Squash_OperatorCalculator_VarSignal_ConstFactor_VarOrigin(signalCalculator, factor, originCalculator, dimensionStack);
                }
                else if (!signalIsConst && !factorIsConst && originIsConst)
                {
                    calculator = new Squash_OperatorCalculator_VarSignal_VarFactor_ConstOrigin(signalCalculator, factorCalculator, origin, dimensionStack);
                }
                else if (!signalIsConst && !factorIsConst && !originIsConst)
                {
                    calculator = new Squash_OperatorCalculator_VarSignal_VarFactor_VarOrigin(signalCalculator, factorCalculator, originCalculator, dimensionStack);
                }
            }

            if (calculator == null)
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitStretch(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            dimensionStack.Push(DEFAULT_DIMENSION_VALUE);

            base.VisitStretch(op);

            OperatorCalculatorBase calculator = null;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase factorCalculator = _stack.Pop();
            OperatorCalculatorBase originCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            factorCalculator = factorCalculator ?? new One_OperatorCalculator();
            originCalculator = originCalculator ?? new Zero_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool factorIsConst = factorCalculator is Number_OperatorCalculator;
            bool originIsConst = originCalculator is Number_OperatorCalculator;

            double signal = signalIsConst ? signalCalculator.Calculate() : 0.0;
            double factor = factorIsConst ? factorCalculator.Calculate() : 0.0;
            double origin = originIsConst ? originCalculator.Calculate() : 0.0;

            bool signalIsConstZero = signalIsConst && signal == 0;
            bool factorIsConstZero = factorIsConst && factor == 0;
            bool originIsConstZero = originIsConst && origin == 0;

            bool factorIsConstOne = factorIsConst && factor == 1;

            bool signalIsConstSpecialValue = signalIsConst && DoubleHelper.IsSpecialValue(signal);
            bool factorIsConstSpecialValue = factorIsConst && DoubleHelper.IsSpecialValue(factor);
            bool originIsConstSpecialValue = originIsConst && DoubleHelper.IsSpecialValue(origin);

            dimensionStack.Pop();

            if (factorIsConstSpecialValue || signalIsConstSpecialValue || originIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            if (factorIsConstZero)
            {
                // Special Value
                // Slow down 0 times, means speed up to infinity, equals undefined.
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            else if (signalIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (factorIsConstOne)
            {
                calculator = signalCalculator;
            }
            else if (signalIsConst)
            {
                calculator = signalCalculator;
            }
            else if (dimensionEnum == DimensionEnum.Time)
            {
                if (!signalIsConst && factorIsConst)
                {
                    calculator = new Stretch_OperatorCalculator_VarSignal_ConstFactor_WithOriginShifting(signalCalculator, factor, dimensionStack);
                }
                else if (!signalIsConst && !factorIsConst)
                {
                    calculator = new Stretch_OperatorCalculator_VarSignal_VarFactor_WithPhaseTracking(signalCalculator, factorCalculator, dimensionStack);
                }
            }
            else
            {
                if (!signalIsConst && factorIsConst && originIsConstZero)
                {
                    calculator = new Stretch_OperatorCalculator_VarSignal_ConstFactor_ZeroOrigin(signalCalculator, factor, dimensionStack);
                }
                else if (!signalIsConst && !factorIsConst && originIsConstZero)
                {
                    calculator = new Stretch_OperatorCalculator_VarSignal_VarFactor_ZeroOrigin(signalCalculator, factorCalculator, dimensionStack);
                }
                else if (!signalIsConst && factorIsConst && originIsConst)
                {
                    calculator = new Stretch_OperatorCalculator_VarSignal_ConstFactor_ConstOrigin(signalCalculator, factor, origin, dimensionStack);
                }
                else if (!signalIsConst && factorIsConst && !originIsConst)
                {
                    calculator = new Stretch_OperatorCalculator_VarSignal_ConstFactor_VarOrigin(signalCalculator, factor, originCalculator, dimensionStack);
                }
                else if (!signalIsConst && !factorIsConst && originIsConst)
                {
                    calculator = new Stretch_OperatorCalculator_VarSignal_VarFactor_ConstOrigin(signalCalculator, factorCalculator, origin, dimensionStack);
                }
                else if (!signalIsConst && !factorIsConst && !originIsConst)
                {
                    calculator = new Stretch_OperatorCalculator_VarSignal_VarFactor_VarOrigin(signalCalculator, factorCalculator, originCalculator, dimensionStack);
                }
            }

            if (calculator == null)
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitSubtract(Operator op)
        {
            base.VisitSubtract(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase aCalculator = _stack.Pop();
            OperatorCalculatorBase bCalculator = _stack.Pop();

            aCalculator = aCalculator ?? new Zero_OperatorCalculator();
            bCalculator = bCalculator ?? new Zero_OperatorCalculator();

            double a = aCalculator.Calculate();
            double b = bCalculator.Calculate();
            bool aIsConst = aCalculator is Number_OperatorCalculator;
            bool bIsConst = bCalculator is Number_OperatorCalculator;
            bool aIsConstZero = aIsConst && a == 0;
            bool bIsConstZero = bIsConst && b == 0;

            if (aIsConstZero && bIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (bIsConstZero)
            {
                calculator = aCalculator;
            }
            else if (aIsConst && bIsConst)
            {
                calculator = new Number_OperatorCalculator(a - b);
            }
            else if (aIsConst)
            {
                calculator = new Subtract_ConstA_VarB_OperatorCalculator(a, bCalculator);
            }
            else if (bIsConst)
            {
                calculator = new Subtract_VarA_ConstB_OperatorCalculator(aCalculator, b);
            }
            else
            {
                calculator = new Subtract_VarA_VarB_OperatorCalculator(aCalculator, bCalculator);
            }

            _stack.Push(calculator);
        }

        protected override void VisitSumOverDimension(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitSumOverDimension(op);

            OperatorCalculatorBase operatorCalculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase fromCalculator = _stack.Pop();
            OperatorCalculatorBase tillCalculator = _stack.Pop();
            OperatorCalculatorBase stepCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            fromCalculator = fromCalculator ?? new Zero_OperatorCalculator();
            tillCalculator = tillCalculator ?? new Zero_OperatorCalculator();
            stepCalculator = stepCalculator ?? new One_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool fromIsConst = fromCalculator is Number_OperatorCalculator;
            bool tillIsConst = tillCalculator is Number_OperatorCalculator;
            bool stepIsConst = stepCalculator is Number_OperatorCalculator;

            double from = fromIsConst ? fromCalculator.Calculate() : 0.0;
            double till = tillIsConst ? tillCalculator.Calculate() : 0.0;
            double step = stepIsConst ? stepCalculator.Calculate() : 0.0;

            bool stepIsConstZero = stepIsConst && step == 0.0;
            bool stepIsConstOne = stepIsConst && step == 1.0;
            bool stepIsConstNegative = stepIsConst && step < 0.0;
            bool fromIsConstZero = fromIsConst && from == 0.0;
            bool fromIsConstSpecialValue = fromIsConst && DoubleHelper.IsSpecialValue(from);
            bool tillIsConstSpecialValue = tillIsConst && DoubleHelper.IsSpecialValue(till);
            bool stepIsConstSpecialValue = stepIsConst && DoubleHelper.IsSpecialValue(step);

            if (signalIsConst)
            {
                operatorCalculator = signalCalculator;
            }
            else if (stepIsConstZero)
            {
                operatorCalculator = new Zero_OperatorCalculator();
            }
            else if (stepIsConstNegative)
            {
                operatorCalculator = new Zero_OperatorCalculator();
            }
            else if (fromIsConstSpecialValue || tillIsConstSpecialValue || stepIsConstSpecialValue)
            {
                operatorCalculator = new Number_OperatorCalculator(Double.NaN);
            }
            else
            {
                var wrapper = new SumOverDimension_OperatorWrapper(op);
                CollectionRecalculationEnum collectionRecalculationEnum = wrapper.CollectionRecalculation;
                switch (collectionRecalculationEnum)
                {
                    case CollectionRecalculationEnum.Continuous:
                        if (fromIsConstZero && tillIsConst && stepIsConstOne)
                        {
                            operatorCalculator = new SumOverDimension_OperatorCalculator_ByUnbundleAndAdd(
                                signalCalculator,
                                till,
                                dimensionStack);
                        }
                        else
                        {
                            operatorCalculator = new SumOverDimension_OperatorCalculator_CollectionRecalculationContinuous(
                            signalCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        }
                            break;

                    case CollectionRecalculationEnum.UponReset:
                        operatorCalculator = new SumOverDimension_OperatorCalculator_CollectionRecalculationUponReset(
                            signalCalculator,
                            fromCalculator,
                            tillCalculator,
                            stepCalculator,
                            dimensionStack);
                        break;

                    default:
                        throw new ValueNotSupportedException(collectionRecalculationEnum);
                }
            }

            _stack.Push(operatorCalculator);
        }

        protected override void VisitSumFollower(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitSumFollower(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase sliceLengthCalculator = _stack.Pop();
            OperatorCalculatorBase sampleCountCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            sliceLengthCalculator = sliceLengthCalculator ?? new One_OperatorCalculator();
            sampleCountCalculator = sampleCountCalculator ?? new One_OperatorCalculator();

            bool signalIsConst = signalCalculator is Number_OperatorCalculator;

            if (signalIsConst)
            {
                calculator = signalCalculator;
            }
            else
            {
                calculator = new SumFollower_OperatorCalculator(signalCalculator, sliceLengthCalculator, sampleCountCalculator, dimensionStack);
            }

            _stack.Push(calculator);
        }

        protected override void VisitTimePower(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);
            dimensionStack.Push(DEFAULT_DIMENSION_VALUE);

            base.VisitTimePower(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase signalCalculator = _stack.Pop(); ;
            OperatorCalculatorBase exponentCalculator = _stack.Pop();
            OperatorCalculatorBase originCalculator = _stack.Pop();

            signalCalculator = signalCalculator ?? new Zero_OperatorCalculator();
            exponentCalculator = exponentCalculator ?? new One_OperatorCalculator();
            originCalculator = originCalculator ?? new Zero_OperatorCalculator();

            double signal = signalCalculator.Calculate();
            double exponent = exponentCalculator.Calculate();
            double origin = originCalculator.Calculate();
            bool signalIsConst = signalCalculator is Number_OperatorCalculator;
            bool exponentIsConst = exponentCalculator is Number_OperatorCalculator;
            bool originIsConst = originCalculator is Number_OperatorCalculator;
            bool signalIsConstZero = signalIsConst && signal == 0;
            bool exponentIsConstZero = exponentIsConst && exponent == 0;
            bool originIsConstZero = originIsConst && origin == 0;
            bool exponentIsConstOne = exponentIsConst && exponent == 1;

            dimensionStack.Pop();

            if (signalIsConstZero)
            {
                calculator = new Zero_OperatorCalculator();
            }
            else if (exponentIsConstZero)
            {
                calculator = new One_OperatorCalculator();
            }
            else if (exponentIsConstOne)
            {
                calculator = signalCalculator;
            }
            else if (originIsConstZero)
            {
                calculator = new TimePower_OperatorCalculator_NoOrigin(signalCalculator, exponentCalculator, dimensionStack);
            }
            else
            {
                calculator = new TimePower_OperatorCalculator_WithOrigin(signalCalculator, exponentCalculator, originCalculator, dimensionStack);
            }

            _stack.Push(calculator);
        }

        protected override void VisitToggleTrigger(Operator op)
        {
            base.VisitToggleTrigger(op);

            OperatorCalculatorBase operatorCalculator;

            OperatorCalculatorBase calculationCalculator = _stack.Pop();
            OperatorCalculatorBase resetCalculator = _stack.Pop();

            calculationCalculator = calculationCalculator ?? new Zero_OperatorCalculator();
            resetCalculator = resetCalculator ?? new Zero_OperatorCalculator();

            bool calculationIsConst = calculationCalculator is Number_OperatorCalculator;
            bool resetIsConst = resetCalculator is Number_OperatorCalculator;

            if (calculationIsConst)
            {
                operatorCalculator = calculationCalculator;
            }
            else if (resetIsConst)
            {
                operatorCalculator = calculationCalculator;
            }
            else
            {
                operatorCalculator = new ToggleTrigger_OperatorCalculator(calculationCalculator, resetCalculator);
            }

            _stack.Push(operatorCalculator);
        }

        protected override void VisitTriangle(Operator op)
        {
            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            base.VisitTriangle(op);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase frequencyCalculator = _stack.Pop();
            frequencyCalculator = frequencyCalculator ?? new Zero_OperatorCalculator();
            double frequency = frequencyCalculator.Calculate();
            bool frequencyIsConst = frequencyCalculator is Number_OperatorCalculator;
            bool frequencyIsConstZero = frequencyIsConst && frequency == 0.0;
            bool frequencyIsConstSpecialValue = frequencyIsConst && DoubleHelper.IsSpecialValue(frequency);

            if (frequencyIsConstSpecialValue)
            {
                // Special Value
                calculator = new Number_OperatorCalculator(Double.NaN);
            }
            if (frequencyIsConstZero)
            {
                // Special Value
                calculator = new Zero_OperatorCalculator();
            }
            else if (frequencyIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Triangle_OperatorCalculator_ConstFrequency_WithOriginShifting(frequency, dimensionStack);
            }
            else if (frequencyIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Triangle_OperatorCalculator_ConstFrequency_NoOriginShifting(frequency, dimensionStack);
            }
            else if (!frequencyIsConst && dimensionEnum == DimensionEnum.Time)
            {
                calculator = new Triangle_OperatorCalculator_VarFrequency_WithPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else if (!frequencyIsConst && dimensionEnum != DimensionEnum.Time)
            {
                calculator = new Triangle_OperatorCalculator_VarFrequency_NoPhaseTracking(frequencyCalculator, dimensionStack);
            }
            else
            {
                throw new CalculatorNotFoundException(MethodBase.GetCurrentMethod());
            }

            _stack.Push(calculator);
        }

        protected override void VisitUnbundle(Operator op)
        {
            if (BUNDLE_POSITIONS_ARE_INVARIANT)
            {
                throw new Exception("VisitUnbundle should not execute if BUNDLE_POSITIONS_ARE_INVARIANT.");
            }

            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            // NOTE: Pushing and popping from the dimension stack is done in VisitUnbundleOutlet_WithVariableBundlePositions.

            base.VisitUnbundle(op);

            OperatorCalculatorBase operatorCalculator;

            OperatorCalculatorBase operandCalculator = _stack.Pop();

            operandCalculator = operandCalculator ?? new Zero_OperatorCalculator();

            bool operandIsConst = operandCalculator is Number_OperatorCalculator;
            
            if (operandIsConst)
            {
                operatorCalculator = operandCalculator;
            }   
            else
            {
                double position = dimensionStack.Get();

                operatorCalculator = new Unbundle_OperatorCalculator(operandCalculator, position, dimensionStack);
            }

            _stack.Push(operatorCalculator);
        }

        // Special Visitation

        /// <summary> Overridden to push null-inlets or default values for those inlets. </summary>
        [DebuggerHidden]
        protected override void VisitInlet(Inlet inlet)
        {
            if (inlet.InputOutlet == null)
            {
                if (inlet.DefaultValue.HasValue)
                {
                    _stack.Push(new Number_OperatorCalculator(inlet.DefaultValue.Value));
                }
                else
                {
                    _stack.Push(null);
                }
            }

            base.VisitInlet(inlet);
        }

        protected override void VisitPatchInlet(Operator patchInlet)
        {
            base.VisitPatchInlet(patchInlet);

            OperatorCalculatorBase calculator;

            OperatorCalculatorBase inputCalculator = _stack.Pop();

            var wrapper = new PatchInlet_OperatorWrapper(patchInlet);

            inputCalculator = inputCalculator ?? new Zero_OperatorCalculator();
            double input = inputCalculator.Calculate();
            double defaultValue = wrapper.Inlet.DefaultValue ?? 0.0;
            bool inputIsConst = inputCalculator is Number_OperatorCalculator;
            bool inputIsConstDefaultValue = inputIsConst && input == defaultValue;
            bool isTopLevelPatchInlet = IsTopLevelPatchInlet(patchInlet);

            // Only top-level PatchInlets are values controllable from the outside.
            // For compatibility we only apply that rule if nothing else was filled in
            // into the (non-visible) PatchInlet Inlet.
            if (isTopLevelPatchInlet && inputIsConstDefaultValue)
            {
                VariableInput_OperatorCalculator variableInputCalculator;
                if (!_patchInlet_Calculator_Dictionary.TryGetValue(patchInlet, out variableInputCalculator))
                {
                    Inlet inlet = wrapper.Inlet;

                    variableInputCalculator = new VariableInput_OperatorCalculator
                    (
                        dimensionEnum: inlet.GetDimensionEnum(),
                        name: wrapper.Name,
                        listIndex: wrapper.ListIndex ?? 0,
                        defaultValue: inlet.DefaultValue ?? 0.0
                    );

                    _patchInlet_Calculator_Dictionary.Add(patchInlet, variableInputCalculator);
                }

                calculator = variableInputCalculator;
            }
            else
            {
                calculator = inputCalculator;
            }

            _stack.Push(calculator);
        }

        [DebuggerHidden]
        protected override void VisitOutlet(Outlet outlet)
        {
            OperatorTypeEnum operatorTypeEnum = outlet.Operator.GetOperatorTypeEnum();

            if (operatorTypeEnum == OperatorTypeEnum.CustomOperator)
            {
                VisitCustomOperatorOutlet(outlet);
            }
            else if (operatorTypeEnum == OperatorTypeEnum.Bundle)
            {
                VisitBundleOutlet(outlet);
            }
            else if (operatorTypeEnum == OperatorTypeEnum.Unbundle)
            {
                VisitUnbundleOutlet(outlet);
            }
            else if (operatorTypeEnum == OperatorTypeEnum.MakeContinuous)
            {
                VisitMakeContinuousOutlet(outlet);
            }
            else if (operatorTypeEnum == OperatorTypeEnum.MakeDiscrete)
            {
                VisitMakeDiscreteOutlet(outlet);
            }
            else
            {
                base.VisitOutlet(outlet);
            }
        }

        // TODO: Low Priority: Get rid of the asymmetry in the Operators with one outlet and the ones with multiple outlets.

        private void VisitCustomOperatorOutlet(Outlet outlet)
        {
            // As soon as you encounter a CustomOperator's Outlet,
            // the evaluation has to take a completely different course.
            Outlet customOperatorOutlet = outlet;
            Outlet patchOutlet_Outlet = PatchCalculationHelper.TryApplyCustomOperatorToUnderlyingPatch(customOperatorOutlet, _patchRepository);

            if (patchOutlet_Outlet == null)
            {
                throw new Exception("patchOutlet_Outlet was null after TryApplyCustomOperatorToUnderlyingPatch.");
            }

            VisitOperatorPolymorphic(patchOutlet_Outlet.Operator);
        }

        private void VisitBundleOutlet(Outlet outlet)
        {
            if (BUNDLE_POSITIONS_ARE_INVARIANT)
            {
                VisitBundleOutlet_WithInvariantBundlePositions(outlet);
            }
            else
            {
                VisitBundleOutlet_WithVariableBundlePositions(outlet);
            }
        }

        private void VisitBundleOutlet_WithInvariantBundlePositions(Outlet outlet)
        {
            DimensionEnum dimensionEnum = outlet.Operator.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            if (dimensionStack.Count == 0)
            {
                throw new NotSupportedException(String.Format(
                    "Bundle Operator with ID '{0}' and Dimension '{1}' encountered without first encountering an Unbundle Operator. This is not yet supported.",
                    outlet.Operator.ID,
                    outlet.Operator.GetDimensionEnum()));
            }

            double bundleIndexDouble = dimensionStack.PopAndGet();

            if (!ConversionHelper.CanCastToNonNegativeInt32(bundleIndexDouble))
            {
                throw new Exception(String.Format(
                    "Index '{0}' cannot be cast to non-negative Int32 for Bundle Operator with ID '{1}' and Dimension '{2}'.",
                    bundleIndexDouble,
                    outlet.Operator.ID,
                    outlet.Operator.GetDimensionEnum()));
            }

            if (bundleIndexDouble >= outlet.Operator.Inlets.Count)
            {
                throw new Exception(String.Format(
                    "Index '{0}' does not exist in Bundle Operator with ID '{1}' and Dimension '{2}'.",
                    bundleIndexDouble,
                    outlet.Operator.ID,
                    outlet.Operator.GetDimensionEnum()));
            }

            int bundleIndexInt32 = (int)bundleIndexDouble;

            Inlet inlet = outlet.Operator.Inlets.OrderBy(x => x.ListIndex).ElementAt(bundleIndexInt32);
            if (inlet.InputOutlet == null)
            {
                double defaultValue = inlet.DefaultValue ?? 0.0;
                _stack.Push(new Number_OperatorCalculator(defaultValue));
            }
            else
            {
                VisitOutlet(inlet.InputOutlet);
            }

            dimensionStack.Push(bundleIndexDouble);
        }

        private void VisitBundleOutlet_WithVariableBundlePositions(Outlet outlet)
        {
            base.VisitOutlet(outlet);
        }

        private void VisitUnbundleOutlet(Outlet outlet)
        {
            if (BUNDLE_POSITIONS_ARE_INVARIANT)
            {
                VisitUnbundleOutlet_WithInvariantBundlePositions(outlet);
            }
            else
            {
                VisitUnbundleOutlet_WithVariableBundlePositions(outlet);
            }
        }

        private void VisitUnbundleOutlet_WithInvariantBundlePositions(Outlet outlet)
        {
            Operator op = outlet.Operator;
            Inlet inlet = op.Inlets.Single();
            Outlet inputOutlet = inlet.InputOutlet;
            if (inputOutlet == null)
            {
                _stack.Push(new Zero_OperatorCalculator());
                return;
            }

            int outletIndex = outlet.Operator.Outlets
                                             .OrderBy(x => x.ListIndex)
                                             .IndexOf(x => x == outlet);

            DimensionEnum dimensionEnum = op.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            dimensionStack.Push(outletIndex);

            VisitOutlet(inputOutlet);

            dimensionStack.Pop();
        }

        private void VisitUnbundleOutlet_WithVariableBundlePositions(Outlet outlet)
        {
            int outletIndex = outlet.Operator.Outlets
                                             .OrderBy(x => x.ListIndex)
                                             .IndexOf(x => x == outlet);

            DimensionEnum dimensionEnum = outlet.Operator.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            dimensionStack.Push(outletIndex);

            base.VisitOutlet(outlet);

            dimensionStack.Pop();
        }

        private void VisitMakeContinuousOutlet(Outlet outlet)
        {
            base.VisitOutlet(outlet);
        }

        private void VisitMakeDiscreteOutlet(Outlet outlet)
        {
            int outletIndex = outlet.Operator.Outlets
                                             .OrderBy(x => x.ListIndex)
                                             .IndexOf(x => x == outlet);

            DimensionEnum dimensionEnum = outlet.Operator.GetDimensionEnum();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dimensionEnum);

            dimensionStack.Push(outletIndex);

            base.VisitOutlet(outlet);

            dimensionStack.Pop();
        }

        protected override void VisitReset(Operator op)
        {
            base.VisitReset(op);

            OperatorCalculatorBase calculator = _stack.Peek();

            var wrapper = new Reset_OperatorWrapper(op);

            // Be forgiving when it comes to name being filled in. A warning is generated instead.
            _resettableOperatorTuples.Add(new ResettableOperatorTuple(calculator, op.Name, wrapper.ListIndex));
        }

        // Helpers

        private bool IsTopLevelPatchInlet(Operator op)
        {
            if (op.GetOperatorTypeEnum() != OperatorTypeEnum.PatchInlet)
            {
                return false;
            }

            return op.Patch.ID == _outlet.Operator.Patch.ID;
        }

        /// <summary> Collapses invariant operands to one and gets rid of nulls. </summary>
        /// <param name="constantsCombiningDelegate">The method that combines multiple invariant doubles to one.</param>
        private static IList<OperatorCalculatorBase> TruncateOperandCalculatorList(
            IList<OperatorCalculatorBase> operandCalculators,
            Func<IEnumerable<double>, double> constantsCombiningDelegate)
        {
            IList<OperatorCalculatorBase> constOperandCalculators = operandCalculators.Where(x => x is Number_OperatorCalculator).ToArray();
            IList<OperatorCalculatorBase> varOperandCalculators = operandCalculators.Except(constOperandCalculators).ToArray();

            OperatorCalculatorBase aggregatedConstOperandCalculator = null;
            if (constOperandCalculators.Count != 0)
            {
                IEnumerable<double> consts = constOperandCalculators.Select(x => x.Calculate());
                double aggregatedConsts = constantsCombiningDelegate(consts);
                aggregatedConstOperandCalculator = new Number_OperatorCalculator(aggregatedConsts);
            }

            IList<OperatorCalculatorBase> truncatedOperandCalculatorList = varOperandCalculators.Union(aggregatedConstOperandCalculator)
                                                                                                .Where(x => x != null)
                                                                                                .ToList();
            return truncatedOperandCalculatorList;
        }

    }
}