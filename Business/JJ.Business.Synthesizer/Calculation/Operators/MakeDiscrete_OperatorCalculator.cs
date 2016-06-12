﻿// MIND THE HACKS IN THIS FILE! IT MAY BE THE CAUSE OF YOUR PROBLEMS!

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    // TODO: Also add block and stripe interpolation.

    internal class MakeDiscrete_OperatorCalculator_LineRememberT0 : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _resampleOperator;

        public MakeDiscrete_OperatorCalculator_LineRememberT0(
            OperatorCalculatorBase operandCalculator,
            double position,
            ResampleInterpolationTypeEnum resampleInterpolationTypeEnum,
            DimensionStack dimensionStack)
            : base(new OperatorCalculatorBase[] { operandCalculator })
        {
            // HACK in a piece of patch, to reuse the Resample_OperatorCalculator's capability of
            // handling many types of interpolation.

            var unbundleCalculator = new Unbundle_OperatorCalculator(operandCalculator, position, dimensionStack);

            _resampleOperator = new Resample_OperatorCalculator_LineRememberT0(
                signalCalculator: unbundleCalculator,
                samplingRateCalculator: new Number_OperatorCalculator(1),
                dimensionStack: dimensionStack);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            return _resampleOperator.Calculate();
        }

        public override void Reset()
        {
            // HACK
            _resampleOperator.Reset();
        }
    }

    internal class MakeDiscrete_OperatorCalculator_LineRememberT1 : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _resampleOperator;

        public MakeDiscrete_OperatorCalculator_LineRememberT1(
            OperatorCalculatorBase operandCalculator,
            double position,
            ResampleInterpolationTypeEnum resampleInterpolationTypeEnum,
            DimensionStack dimensionStack)
            : base(new OperatorCalculatorBase[] { operandCalculator })
        {
            // HACK in a piece of patch, to reuse the Resample_OperatorCalculator's capability of
            // handling many types of interpolation.

            var unbundleCalculator = new Unbundle_OperatorCalculator(operandCalculator, position, dimensionStack);

            _resampleOperator = new Resample_OperatorCalculator_LineRememberT1(
                signalCalculator: unbundleCalculator,
                samplingRateCalculator: new Number_OperatorCalculator(1),
                dimensionStack: dimensionStack);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            return _resampleOperator.Calculate();
        }

        public override void Reset()
        {
            // HACK
            _resampleOperator.Reset();
        }
    }

}