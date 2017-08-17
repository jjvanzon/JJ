﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Calculation;
using JJ.Business.Synthesizer.Calculation.Arrays;
using JJ.Business.Synthesizer.Calculation.Operators;
using JJ.Business.Synthesizer.Calculation.Patches;
using JJ.Business.Synthesizer.Calculation.Random;
using JJ.Business.Synthesizer.Dto;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer.RepositoryInterfaces;
using JJ.Framework.Exceptions;
// ReSharper disable ImplicitlyCapturedClosure

namespace JJ.Business.Synthesizer.Visitors
{
    internal abstract class OperatorDtoToCalculatorVisitorBase : OperatorDtoVisitorBase_AfterProgrammerLaziness
    {
        private readonly int _samplesBetweenApplyFilterVariables;
        private Dictionary<VariableInput_OperatorDto, VariableInput_OperatorCalculator> _variableInput_OperatorDto_To_Calculator_Dictionary;
        private IList<ResettableOperatorTuple> _resettableOperatorTuples;

        protected readonly CalculatorCache _calculatorCache;
        protected DimensionStackCollection _dimensionStackCollection;
        protected Stack<OperatorCalculatorBase> _stack;

        public OperatorDtoToCalculatorVisitorBase(
            int targetSamplingRate, 
            double secondsBetweenApplyFilterVariables,
            CalculatorCache calculatorCache,
            ICurveRepository curveRepository,
            ISampleRepository sampleRepository)
        {
            if (curveRepository == null) throw new NullException(() => curveRepository);
            if (sampleRepository == null) throw new NullException(() => sampleRepository);

            _calculatorCache = calculatorCache ?? throw new NullException(() => calculatorCache);

            _samplesBetweenApplyFilterVariables = VisitorHelper.GetSamplesBetweenApplyFilterVariables(secondsBetweenApplyFilterVariables, targetSamplingRate);
        }

        public ToCalculatorResult Execute(IOperatorDto dto)
        {
            if (dto == null) throw new NullException(() => dto);

            _stack = new Stack<OperatorCalculatorBase>();
            _dimensionStackCollection = new DimensionStackCollection();
            _variableInput_OperatorDto_To_Calculator_Dictionary = new Dictionary<VariableInput_OperatorDto, VariableInput_OperatorCalculator>();
            _resettableOperatorTuples = new List<ResettableOperatorTuple>();

            Visit_OperatorDto_Polymorphic(dto);

            OperatorCalculatorBase rootCalculator = _stack.Pop();

            VisitorHelper.AssertDimensionStacksCountsAre1(_dimensionStackCollection);

            var result = new ToCalculatorResult(
                rootCalculator,
                _dimensionStackCollection,
                _variableInput_OperatorDto_To_Calculator_Dictionary.Values.ToArray(),
                _resettableOperatorTuples);

            return result;
        }

        protected override IOperatorDto Visit_OperatorDto_Polymorphic(IOperatorDto dto)
        {
            VisitorHelper.WithStackCheck(_stack, () => base.Visit_OperatorDto_Polymorphic(dto));

            return dto;
        }

        protected override IOperatorDto Visit_Absolute_OperatorDto(Absolute_OperatorDto dto)
        {
            return ProcessOperatorDto(dto, () => new Absolute_OperatorCalculator_VarNumber(_stack.Pop()));
        }

        protected override IOperatorDto Visit_Add_OperatorDto(Add_OperatorDto dto)
        {
            switch (dto.AggregateInfo.Consts.Count)
            {
                case 0:
                    return ProcessOperatorDto(dto, () => OperatorCalculatorFactory.CreateAddCalculator_Vars(dto.AggregateInfo.Vars.Select(x => _stack.Pop()).ToArray()));

                case 1:
                    return ProcessOperatorDto(
                        dto,
                        () => OperatorCalculatorFactory.CreateAddCalculator_Vars_1Const(dto.AggregateInfo.Vars.Select(x => _stack.Pop()).ToArray(), dto.AggregateInfo.Const.Const));

                default:
                    throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_AllPassFilter_OperatorDto_SoundVarOrConst_OtherInputsVar(AllPassFilter_OperatorDto_SoundVarOrConst_OtherInputsVar dto)
        {
            return ProcessOperatorDto(dto, () => new AllPassFilter_OperatorCalculator_SoundVarOrConst_OtherInputsVar(_stack.Pop(), _stack.Pop(), _stack.Pop(), dto.TargetSamplingRate, _samplesBetweenApplyFilterVariables));
        }

        protected override IOperatorDto Visit_AllPassFilter_OperatorDto_SoundVarOrConst_OtherInputsConst(AllPassFilter_OperatorDto_SoundVarOrConst_OtherInputsConst dto)
        {
            return ProcessOperatorDto(dto, () => new AllPassFilter_OperatorCalculator_SoundVarOrConst_OtherInputsConst(_stack.Pop(), dto.CenterFrequency.Const, dto.Width.Const, dto.TargetSamplingRate));
        }
        
        protected override IOperatorDto Visit_And_OperatorDto(And_OperatorDto dto)
        {
            return ProcessOperatorDto(dto, () => new And_OperatorCalculator_VarA_VarB(_stack.Pop(), _stack.Pop()));
        }

        protected override IOperatorDto Visit_AverageFollower_OperatorDto(AverageFollower_OperatorDto dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new AverageFollower_OperatorCalculator(_stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_AverageOverDimension_OperatorDto_CollectionRecalculationContinuous(AverageOverDimension_OperatorDto_CollectionRecalculationContinuous dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new AverageOverDimension_OperatorCalculator_CollectionRecalculationContinuous(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_AverageOverDimension_OperatorDto_CollectionRecalculationUponReset(AverageOverDimension_OperatorDto_CollectionRecalculationUponReset dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new AverageOverDimension_OperatorCalculator_CollectionRecalculationUponReset(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_AverageOverInlets_OperatorDto(AverageOverInlets_OperatorDto dto)
        {
            return ProcessOperatorDto(dto, () => new AverageOverInlets_OperatorCalculator(dto.Inputs.Select(x => _stack.Pop()).ToArray()));
        }

        protected override IOperatorDto Visit_BandPassFilterConstantPeakGain_OperatorDto_SoundVarOrConst_OtherInputsConst(BandPassFilterConstantPeakGain_OperatorDto_SoundVarOrConst_OtherInputsConst dto)
        {
            return ProcessOperatorDto(dto, () => new BandPassFilterConstantPeakGain_OperatorCalculator_SoundVarOrConst_OtherInputsConst(_stack.Pop(), dto.CenterFrequency.Const, dto.Width.Const, dto.TargetSamplingRate));
        }

        protected override IOperatorDto Visit_BandPassFilterConstantPeakGain_OperatorDto_SoundVarOrConst_OtherInputsVar(BandPassFilterConstantPeakGain_OperatorDto_SoundVarOrConst_OtherInputsVar dto)
        {
            return ProcessOperatorDto(dto, () => new BandPassFilterConstantPeakGain_OperatorCalculator_SoundVarOrConst_OtherInputsVar(_stack.Pop(), _stack.Pop(), _stack.Pop(), dto.TargetSamplingRate, _samplesBetweenApplyFilterVariables));
        }

        protected override IOperatorDto Visit_BandPassFilterConstantTransitionGain_OperatorDto_SoundVarOrConst_OtherInputsConst(BandPassFilterConstantTransitionGain_OperatorDto_SoundVarOrConst_OtherInputsConst dto)
        {
            return ProcessOperatorDto(dto, () => new BandPassFilterConstantTransitionGain_OperatorCalculator_SoundVarOrConst_OtherInputsConst(_stack.Pop(), dto.CenterFrequency.Const, dto.Width.Const, dto.TargetSamplingRate));
        }

        protected override IOperatorDto Visit_BandPassFilterConstantTransitionGain_OperatorDto_SoundVarOrConst_OtherInputsVar(BandPassFilterConstantTransitionGain_OperatorDto_SoundVarOrConst_OtherInputsVar dto)
        {
            return ProcessOperatorDto(dto, () => new BandPassFilterConstantTransitionGain_OperatorCalculator_SoundVarOrConst_OtherInputsVar(_stack.Pop(), _stack.Pop(), _stack.Pop(), dto.TargetSamplingRate, _samplesBetweenApplyFilterVariables));
        }

        protected override IOperatorDto Visit_ChangeTrigger_OperatorDto(ChangeTrigger_OperatorDto dto)
        {
            return ProcessOperatorDto(dto, () => new ChangeTrigger_OperatorCalculator_VarPassThrough_VarReset(_stack.Pop(), _stack.Pop()));
        }

        protected override IOperatorDto Visit_ClosestOverDimensionExp_OperatorDto_CollectionRecalculationContinuous(ClosestOverDimensionExp_OperatorDto_CollectionRecalculationContinuous dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new ClosestOverDimensionExp_OperatorCalculator_CollectionRecalculationContinuous(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_ClosestOverDimensionExp_OperatorDto_CollectionRecalculationUponReset(ClosestOverDimensionExp_OperatorDto_CollectionRecalculationUponReset dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new ClosestOverDimensionExp_OperatorCalculator_CollectionRecalculationUponReset(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_ClosestOverDimension_OperatorDto_CollectionRecalculationContinuous(ClosestOverDimension_OperatorDto_CollectionRecalculationContinuous dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new ClosestOverDimension_OperatorCalculator_CollectionRecalculationContinuous(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_ClosestOverDimension_OperatorDto_CollectionRecalculationUponReset(ClosestOverDimension_OperatorDto_CollectionRecalculationUponReset dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new ClosestOverDimension_OperatorCalculator_CollectionRecalculationUponReset(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_ClosestOverInlets_OperatorDto(ClosestOverInlets_OperatorDto dto)
        {
            if (dto.AggregateInfo.OnlyConsts && dto.AggregateInfo.Consts.Count == 2)
            {
                return ProcessOperatorDto(dto, () => new ClosestOverInlets_OperatorCalculator_VarInput_2ConstItems(_stack.Pop(), dto.AggregateInfo.Consts[0].Const, dto.AggregateInfo.Consts[1].Const));
            }
            else if (dto.AggregateInfo.OnlyConsts)
            {
                return ProcessOperatorDto(dto, () => new ClosestOverInlets_OperatorCalculator_VarInput_ConstItems(_stack.Pop(), dto.Items.Select(x => x.Const).ToArray()));
            }
            else if (dto.AggregateInfo.OnlyVars)
            {
                return ProcessOperatorDto(dto, () => new ClosestOverInlets_OperatorCalculator_VarInput_VarItems(_stack.Pop(), dto.Items.Select(x => _stack.Pop()).ToArray()));
            }

            return base.Visit_ClosestOverInlets_OperatorDto(dto);
        }

        protected override IOperatorDto Visit_ClosestOverInletsExp_OperatorDto(ClosestOverInletsExp_OperatorDto dto)
        {
            if (dto.AggregateInfo.OnlyConsts && dto.AggregateInfo.Consts.Count == 2)
            {
                return ProcessOperatorDto(dto, () => new ClosestOverInletsExp_OperatorCalculator_VarInput_2ConstItems(_stack.Pop(), dto.AggregateInfo.Consts[0].Const, dto.AggregateInfo.Consts[1].Const));
            }
            else if (dto.AggregateInfo.OnlyConsts)
            {
                return ProcessOperatorDto(dto, () => new ClosestOverInletsExp_OperatorCalculator_VarInput_ConstItems(_stack.Pop(), dto.Items.Select(x => x.Const).ToArray()));
            }
            else if (dto.AggregateInfo.OnlyVars)
            {
                return ProcessOperatorDto(dto, () => new ClosestOverInletsExp_OperatorCalculator_VarInput_VarItems(_stack.Pop(), dto.Items.Select(x => _stack.Pop()).ToArray()));
            }

            return base.Visit_ClosestOverInletsExp_OperatorDto(dto);
        }

        protected override IOperatorDto Visit_Curve_OperatorDto_MinXZero_NoOriginShifting(Curve_OperatorDto_MinXZero_NoOriginShifting dto)
        {
            return ProcessCurveOperator(dto);
        }

        protected override IOperatorDto Visit_Curve_OperatorDto_MinXZero_WithOriginShifting(Curve_OperatorDto_MinXZero_WithOriginShifting dto)
        {
            return ProcessCurveOperator(dto);
        }

        protected override IOperatorDto Visit_Curve_OperatorDto_MinX_NoOriginShifting(Curve_OperatorDto_MinX_NoOriginShifting dto)
        {
            return ProcessCurveOperator(dto);
        }

        protected override IOperatorDto Visit_Curve_OperatorDto_MinX_WithOriginShifting(Curve_OperatorDto_MinX_WithOriginShifting dto)
        {
            return ProcessCurveOperator(dto);
        }

        private IOperatorDto ProcessCurveOperator(Curve_OperatorDto dto)
        {
            base.Visit_OperatorDto_Base(dto);

            OperatorCalculatorBase calculator = OperatorCalculatorFactory.Create_Curve_OperatorCalculator(
                dto.ArrayDto,
                dto.StandardDimensionEnum,
                dto.CanonicalCustomDimensionName,
                _dimensionStackCollection);

            _stack.Push(calculator);

            return dto;
        }

        protected override IOperatorDto Visit_DimensionToOutlets_Outlet_OperatorDto(DimensionToOutlets_Outlet_OperatorDto dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new DimensionToOutlets_OperatorCalculator(_stack.Pop(), dto.OutletPosition, dimensionStack));
        }

        protected override IOperatorDto Visit_Divide_OperatorDto(Divide_OperatorDto dto)
        {
            if (dto.A.IsConst && dto.B.IsVar)
            {
                return ProcessOperatorDto(dto, () => new Divide_OperatorCalculator_ConstA_VarB(dto.A.Const, _stack.Pop()));
            }
            else if (dto.A.IsVar && dto.B.IsConst)
            {
                return ProcessOperatorDto(dto, () => new Divide_OperatorCalculator_VarA_ConstB(_stack.Pop(), dto.B.Const));
            }
            else if (dto.A.IsVar && dto.B.IsVar)
            {
                return ProcessOperatorDto(dto, () => new Divide_OperatorCalculator_VarA_VarB(_stack.Pop(), _stack.Pop()));
            }
            else
            {
                throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_Equal_OperatorDto(Equal_OperatorDto dto)
        {
            if (dto.A.IsVar && dto.B.IsConst)
            {
                return ProcessOperatorDto(dto, () => new Equal_OperatorCalculator_VarA_ConstB(_stack.Pop(), dto.B.Const));
            }
            else if (dto.A.IsVar && dto.B.IsConst)
            {
                return ProcessOperatorDto(dto, () => new Equal_OperatorCalculator_VarA_VarB(_stack.Pop(), _stack.Pop()));
            }
            else
            {
                throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_GetDimension_OperatorDto(GetDimension_OperatorDto dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new GetDimension_OperatorCalculator(dimensionStack));
        }

        protected override IOperatorDto Visit_GreaterThanOrEqual_OperatorDto(GreaterThanOrEqual_OperatorDto dto)
        {
            if (dto.A.IsVar && dto.B.IsConst)
            {
                return ProcessOperatorDto(dto, () => new GreaterThanOrEqual_OperatorCalculator_VarA_ConstB(_stack.Pop(), dto.B.Const));
            }
            else if (dto.A.IsVar && dto.B.IsConst)
            {
                return ProcessOperatorDto(dto, () => new GreaterThanOrEqual_OperatorCalculator_VarA_VarB(_stack.Pop(), _stack.Pop()));
            }
            else
            {
                throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_GreaterThan_OperatorDto(GreaterThan_OperatorDto dto)
        {
            if (dto.A.IsVar && dto.B.IsConst)
            {
                return ProcessOperatorDto(dto, () => new GreaterThan_OperatorCalculator_VarA_ConstB(_stack.Pop(), dto.B.Const));
            }
            else if (dto.A.IsVar && dto.B.IsVar)
            {
                return ProcessOperatorDto(dto, () => new GreaterThan_OperatorCalculator_VarA_VarB(_stack.Pop(), _stack.Pop()));
            }
            else
            {
                throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_HighPassFilter_OperatorDto_SoundVarOrConst_OtherInputsVar(HighPassFilter_OperatorDto_SoundVarOrConst_OtherInputsVar dto)
        {
            return ProcessOperatorDto(dto, () => new HighPassFilter_OperatorCalculator_SoundVarOrConst_OtherInputsVar(_stack.Pop(), _stack.Pop(), _stack.Pop(), dto.TargetSamplingRate, _samplesBetweenApplyFilterVariables));
        }

        protected override IOperatorDto Visit_HighPassFilter_OperatorDto_SoundVarOrConst_OtherInputsConst(HighPassFilter_OperatorDto_SoundVarOrConst_OtherInputsConst dto)
        {
            return ProcessOperatorDto(dto, () => new HighPassFilter_OperatorCalculator_SoundVarOrConst_OtherInputsConst(_stack.Pop(), dto.MinFrequency.Const, dto.BlobVolume.Const, dto.TargetSamplingRate));
        }

        protected override IOperatorDto Visit_HighShelfFilter_OperatorDto_SoundVarOrConst_OtherInputsVar(HighShelfFilter_OperatorDto_SoundVarOrConst_OtherInputsVar dto)
        {
            return ProcessOperatorDto(dto, () => new HighShelfFilter_OperatorCalculator_SoundVarOrConst_OtherInputsVar(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dto.TargetSamplingRate, _samplesBetweenApplyFilterVariables));
        }

        protected override IOperatorDto Visit_HighShelfFilter_OperatorDto_SoundVarOrConst_OtherInputsConst(HighShelfFilter_OperatorDto_SoundVarOrConst_OtherInputsConst dto)
        {
            return ProcessOperatorDto(dto, () => new HighShelfFilter_OperatorCalculator_SoundVarOrConst_OtherInputsConst(_stack.Pop(), dto.TransitionFrequency.Const, dto.TransitionSlope.Const, dto.DBGain.Const, dto.TargetSamplingRate));
        }

        protected override IOperatorDto Visit_Hold_OperatorDto(Hold_OperatorDto dto)
        {
            return ProcessOperatorDto(dto, () => new Hold_OperatorCalculator(_stack.Pop()));
        }

        protected override IOperatorDto Visit_If_OperatorDto(If_OperatorDto dto)
        {
            if (dto.Condition.IsVar && dto.Then.IsConst && dto.Else.IsConst)
            {
                return ProcessOperatorDto(dto, () => new If_OperatorCalculator_VarCondition_ConstThen_ConstElse(_stack.Pop(), dto.Then.Const, dto.Else.Const));
            }
            else if (dto.Condition.IsVar && dto.Then.IsConst && dto.Else.IsVar)
            {
                return ProcessOperatorDto(dto, () => new If_OperatorCalculator_VarCondition_ConstThen_VarElse(_stack.Pop(), dto.Then.Const, _stack.Pop()));
            }
            else if (dto.Condition.IsVar && dto.Then.IsVar && dto.Else.IsConst)
            {
                return ProcessOperatorDto(dto, () => new If_OperatorCalculator_VarCondition_VarThen_ConstElse(_stack.Pop(), _stack.Pop(), dto.Else.Const));
            }
            else if (dto.Condition.IsVar && dto.Then.IsVar && dto.Else.IsVar)
            {
                return ProcessOperatorDto(dto, () => new If_OperatorCalculator_VarCondition_VarThen_VarElse(_stack.Pop(), _stack.Pop(), _stack.Pop()));
            }
            else
            {
                throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_InletsToDimension_OperatorDto_Block(InletsToDimension_OperatorDto_Block dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new InletsToDimension_OperatorCalculator_Block(dto.Inputs.Select(x => _stack.Pop()).ToArray(), dimensionStack));
        }

        protected override IOperatorDto Visit_InletsToDimension_OperatorDto_Stripe_LagBehind(InletsToDimension_OperatorDto_Stripe_LagBehind dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new InletsToDimension_OperatorCalculator_Stripe(dto.Inputs.Select(x => _stack.Pop()).ToArray(), dimensionStack));
        }

        protected override IOperatorDto Visit_Interpolate_OperatorDto_Block(Interpolate_OperatorDto_Block dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Interpolate_OperatorCalculator_Block(_stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Interpolate_OperatorDto_CubicAbruptSlope(Interpolate_OperatorDto_CubicAbruptSlope dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Interpolate_OperatorCalculator_CubicAbruptSlope(_stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Interpolate_OperatorDto_CubicEquidistant(Interpolate_OperatorDto_CubicEquidistant dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Interpolate_OperatorCalculator_CubicEquidistant(_stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Interpolate_OperatorDto_CubicSmoothSlope_LagBehind(Interpolate_OperatorDto_CubicSmoothSlope_LagBehind dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Interpolate_OperatorCalculator_CubicSmoothSlope_LagBehind(_stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Interpolate_OperatorDto_Hermite_LagBehind(Interpolate_OperatorDto_Hermite_LagBehind dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Interpolate_OperatorCalculator_Hermite_LagBehind(_stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Interpolate_OperatorDto_Line_LagBehind_ConstSamplingRate(Interpolate_OperatorDto_Line_LagBehind_ConstSamplingRate dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Interpolate_OperatorCalculator_Line_LagBehind_ConstSamplingRate(_stack.Pop(), dto.SamplingRate.Const, dimensionStack));
        }

        protected override IOperatorDto Visit_Interpolate_OperatorDto_Line_LagBehind_VarSamplingRate(Interpolate_OperatorDto_Line_LagBehind_VarSamplingRate dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Interpolate_OperatorCalculator_Line_LagBehind_VarSamplingRate(_stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Interpolate_OperatorDto_Stripe_LagBehind(Interpolate_OperatorDto_Stripe_LagBehind dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Interpolate_OperatorCalculator_Stripe_LagBehind(_stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_LessThanOrEqual_OperatorDto(LessThanOrEqual_OperatorDto dto)
        {
            if (dto.A.IsVar && dto.B.IsConst)
            {
                return ProcessOperatorDto(dto, () => new LessThanOrEqual_OperatorCalculator_VarA_ConstB(_stack.Pop(), dto.B.Const));
            }
            else if (dto.A.IsVar && dto.B.IsVar)
            {
                return ProcessOperatorDto(dto, () => new LessThanOrEqual_OperatorCalculator_VarA_VarB(_stack.Pop(), _stack.Pop()));
            }
            else
            {
                throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_LessThan_OperatorDto(LessThan_OperatorDto dto)
        {
            if (dto.A.IsVar && dto.B.IsConst)
            {
                return ProcessOperatorDto(dto, () => new LessThan_OperatorCalculator_VarA_ConstB(_stack.Pop(), dto.B.Const));
            }
            else if (dto.A.IsVar && dto.B.IsVar)
            {
                return ProcessOperatorDto(dto, () => new LessThan_OperatorCalculator_VarA_VarB(_stack.Pop(), _stack.Pop()));
            }
            else
            {
                throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_Loop_OperatorDto_AllVars(Loop_OperatorDto_AllVars dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Loop_OperatorCalculator_AllVars(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Loop_OperatorDto_ConstSkip_WhichEqualsLoopStartMarker_ConstLoopEndMarker_NoNoteDuration(Loop_OperatorDto_ConstSkip_WhichEqualsLoopStartMarker_ConstLoopEndMarker_NoNoteDuration dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Loop_OperatorCalculator_ConstSkip_WhichEqualsLoopStartMarker_ConstLoopEndMarker_NoNoteDuration(_stack.Pop(), dto.LoopStartMarker.Const, dto.LoopEndMarker.Const, dimensionStack));
        }

        protected override IOperatorDto Visit_Loop_OperatorDto_ConstSkip_WhichEqualsLoopStartMarker_VarLoopEndMarker_NoNoteDuration(Loop_OperatorDto_ConstSkip_WhichEqualsLoopStartMarker_VarLoopEndMarker_NoNoteDuration dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Loop_OperatorCalculator_ConstSkip_WhichEqualsLoopStartMarker_VarLoopEndMarker_NoNoteDuration(_stack.Pop(), dto.LoopStartMarker.Const, _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Loop_OperatorDto_ManyConstants(Loop_OperatorDto_ManyConstants dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Loop_OperatorCalculator_ManyConstants(_stack.Pop(), dto.Skip.Const, dto.LoopStartMarker.Const, dto.LoopEndMarker.Const, dto.ReleaseEndMarker.Const, _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Loop_OperatorDto_NoSkipOrRelease(Loop_OperatorDto_NoSkipOrRelease dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Loop_OperatorCalculator_NoSkipOrRelease(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Loop_OperatorDto_NoSkipOrRelease_ManyConstants(Loop_OperatorDto_NoSkipOrRelease_ManyConstants dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Loop_OperatorCalculator_NoSkipOrRelease_ManyConstants(_stack.Pop(), dto.LoopStartMarker.Const, dto.LoopEndMarker.Const, _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_LowPassFilter_OperatorDto_SoundVarOrConst_OtherInputsVar(LowPassFilter_OperatorDto_SoundVarOrConst_OtherInputsVar dto)
        {
            return ProcessOperatorDto(dto, () => new LowPassFilter_OperatorCalculator_SoundVarOrConst_OtherInputsVar(_stack.Pop(), _stack.Pop(), _stack.Pop(), dto.TargetSamplingRate, _samplesBetweenApplyFilterVariables));
        }

        protected override IOperatorDto Visit_LowPassFilter_OperatorDto_SoundVarOrConst_OtherInputsConst(LowPassFilter_OperatorDto_SoundVarOrConst_OtherInputsConst dto)
        {
            return ProcessOperatorDto(dto, () => new LowPassFilter_OperatorCalculator_SoundVarOrConst_OtherInputsConst(_stack.Pop(), dto.MaxFrequency.Const, dto.BlobVolume.Const, dto.TargetSamplingRate));
        }

        protected override IOperatorDto Visit_LowShelfFilter_OperatorDto_SoundVarOrConst_OtherInputsVar(LowShelfFilter_OperatorDto_SoundVarOrConst_OtherInputsVar dto)
        {
            return ProcessOperatorDto(dto, () => new LowShelfFilter_OperatorCalculator_SoundVarOrConst_OtherInputsVar(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dto.TargetSamplingRate, _samplesBetweenApplyFilterVariables));
        }

        protected override IOperatorDto Visit_LowShelfFilter_OperatorDto_SoundVarOrConst_OtherInputsConst(LowShelfFilter_OperatorDto_SoundVarOrConst_OtherInputsConst dto)
        {
            return ProcessOperatorDto(dto, () => new LowShelfFilter_OperatorCalculator_SoundVarOrConst_OtherInputsConst(_stack.Pop(), dto.TransitionFrequency.Const, dto.TransitionSlope.Const, dto.DBGain.Const, dto.TargetSamplingRate));
        }

        protected override IOperatorDto Visit_MaxFollower_OperatorDto(MaxFollower_OperatorDto dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new MaxFollower_OperatorCalculator(_stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_MaxOverDimension_OperatorDto_CollectionRecalculationContinuous(MaxOverDimension_OperatorDto_CollectionRecalculationContinuous dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new MaxOverDimension_OperatorCalculator_CollectionRecalculationContinuous(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_MaxOverDimension_OperatorDto_CollectionRecalculationUponReset(MaxOverDimension_OperatorDto_CollectionRecalculationUponReset dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new MaxOverDimension_OperatorCalculator_CollectionRecalculationUponReset(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_MaxOverInlets_OperatorDto(MaxOverInlets_OperatorDto dto)
        {
            if (dto.AggregateInfo.Vars.Count == 1 && dto.AggregateInfo.Consts.Count == 1)
            {
                return ProcessOperatorDto(dto, () => new MaxOverInlets_OperatorCalculator_1Var_1Const(_stack.Pop(), dto.AggregateInfo.Const.Const));
            }
            else if (dto.AggregateInfo.OnlyVars && dto.AggregateInfo.Vars.Count == 2)
            {
                return ProcessOperatorDto(dto, () => new MaxOverInlets_OperatorCalculator_2Vars(_stack.Pop(), _stack.Pop()));
            }
            else if (dto.AggregateInfo.HasVars && dto.AggregateInfo.Consts.Count == 1)
            {
                return ProcessOperatorDto(dto, () => new MaxOverInlets_OperatorCalculator_Vars_1Const(dto.AggregateInfo.Vars.Select(x => _stack.Pop()).ToArray(), dto.AggregateInfo.Const.Const));
            }
            else if (dto.AggregateInfo.OnlyVars)
            {
                return ProcessOperatorDto(dto, () => new MaxOverInlets_OperatorCalculator_SignalVarOrConst_OtherInputsVar(dto.AggregateInfo.Vars.Select(x => _stack.Pop()).ToArray()));
            }
            else
            {
                throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_MinFollower_OperatorDto(MinFollower_OperatorDto dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new MinFollower_OperatorCalculator(_stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_MinOverDimension_OperatorDto_CollectionRecalculationContinuous(MinOverDimension_OperatorDto_CollectionRecalculationContinuous dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new MinOverDimension_OperatorCalculator_CollectionRecalculationContinuous(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_MinOverDimension_OperatorDto_CollectionRecalculationUponReset(MinOverDimension_OperatorDto_CollectionRecalculationUponReset dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new MinOverDimension_OperatorCalculator_CollectionRecalculationUponReset(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_MinOverInlets_OperatorDto(MinOverInlets_OperatorDto dto)
        {
            if (dto.AggregateInfo.Vars.Count == 1 && dto.AggregateInfo.Consts.Count == 1)
            {
                return ProcessOperatorDto(dto, () => new MinOverInlets_OperatorCalculator_1Var_1Const(_stack.Pop(), dto.AggregateInfo.Const.Const));
            }
            else if (dto.AggregateInfo.OnlyVars && dto.AggregateInfo.Vars.Count == 2)
            {
                return ProcessOperatorDto(dto, () => new MinOverInlets_OperatorCalculator_2Vars(_stack.Pop(), _stack.Pop()));
            }
            else if (dto.AggregateInfo.HasVars && dto.AggregateInfo.Consts.Count == 1)
            {
                return ProcessOperatorDto(dto, () => new MinOverInlets_OperatorCalculator_Vars_1Const(dto.AggregateInfo.Vars.Select(x => _stack.Pop()).ToArray(), dto.AggregateInfo.Const.Const));
            }
            else if (dto.AggregateInfo.OnlyVars)
            {
                return ProcessOperatorDto(dto, () => new MinOverInlets_OperatorCalculator_SignalVarOrConst_OtherInputsVar(dto.AggregateInfo.Vars.Select(x => _stack.Pop()).ToArray()));
            }
            else
            {
                throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_Multiply_OperatorDto(Multiply_OperatorDto dto)
        {
            switch (dto.AggregateInfo.Consts.Count)
            {
                case 0:
                    return ProcessOperatorDto(dto, () => OperatorCalculatorFactory.CreateMultiplyCalculator_Vars(dto.AggregateInfo.Vars.Select(x => _stack.Pop()).ToArray()));

                case 1:
                    return ProcessOperatorDto(
                        dto,
                        () => OperatorCalculatorFactory.CreateMultiplyCalculator_Vars_1Const(dto.AggregateInfo.Vars.Select(x => _stack.Pop()).ToArray(), dto.AggregateInfo.Const.Const));

                default:
                    throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_Negative_OperatorDto(Negative_OperatorDto dto)
        {
            return ProcessOperatorDto(dto, () => new Negative_OperatorCalculator(_stack.Pop()));
        }

        protected override IOperatorDto Visit_Noise_OperatorDto(Noise_OperatorDto dto)
        {
            base.Visit_Noise_OperatorDto(dto);

            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dto);
            // Cast to concrete calculator type for performance.
            NoiseCalculator noiseCalculator = _calculatorCache.GetNoiseCalculator(dto.OperatorID);

            var calculator = new Noise_OperatorCalculator(noiseCalculator, dimensionStack);
            _stack.Push(calculator);

            return dto;
        }

        protected override IOperatorDto Visit_NotchFilter_OperatorDto_SoundVarOrConst_OtherInputsVar(NotchFilter_OperatorDto_SoundVarOrConst_OtherInputsVar dto)
        {
            return ProcessOperatorDto(dto, () => new NotchFilter_OperatorCalculator_SoundVarOrConst_OtherInputsVar(_stack.Pop(), _stack.Pop(), _stack.Pop(), dto.TargetSamplingRate, _samplesBetweenApplyFilterVariables));
        }

        protected override IOperatorDto Visit_NotchFilter_OperatorDto_SoundVarOrConst_OtherInputsConst(NotchFilter_OperatorDto_SoundVarOrConst_OtherInputsConst dto)
        {
            return ProcessOperatorDto(dto, () => new NotchFilter_OperatorCalculator_SoundVarOrConst_OtherInputsConst(_stack.Pop(), dto.CenterFrequency.Const, dto.Width.Const, dto.TargetSamplingRate));
        }

        protected override IOperatorDto Visit_NotEqual_OperatorDto(NotEqual_OperatorDto dto)
        {
            if (dto.A.IsVar && dto.B.IsConst)
            {
                return ProcessOperatorDto(dto, () => new NotEqual_OperatorCalculator_VarA_ConstB(_stack.Pop(), dto.B.Const));
            }
            else if (dto.A.IsVar && dto.B.IsVar)
            {
                return ProcessOperatorDto(dto, () => new NotEqual_OperatorCalculator_VarA_VarB(_stack.Pop(), _stack.Pop()));
            }
            else
            {
                throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_Not_OperatorDto(Not_OperatorDto dto)
        {
            return ProcessOperatorDto(dto, () => new Not_OperatorCalculator(_stack.Pop()));
        }

        protected override IOperatorDto Visit_Number_OperatorDto(Number_OperatorDto dto)
        {
            return ProcessOperatorDto(dto, () => new Number_OperatorCalculator(dto.Number));
        }

        protected override IOperatorDto Visit_Or_OperatorDto(Or_OperatorDto dto)
        {
            return ProcessOperatorDto(dto, () => new Or_OperatorCalculator_VarA_VarB(_stack.Pop(), _stack.Pop()));
        }

        protected override IOperatorDto Visit_PeakingEQFilter_OperatorDto_SoundVarOrConst_OtherInputsVar(PeakingEQFilter_OperatorDto_SoundVarOrConst_OtherInputsVar dto)
        {
            return ProcessOperatorDto(dto, () => new PeakingEQFilter_OperatorCalculator_SoundVarOrConst_OtherInputsVar(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dto.TargetSamplingRate, _samplesBetweenApplyFilterVariables));
        }

        protected override IOperatorDto Visit_PeakingEQFilter_OperatorDto_SoundVarOrConst_OtherInputsConst(PeakingEQFilter_OperatorDto_SoundVarOrConst_OtherInputsConst dto)
        {
            return ProcessOperatorDto(dto, () => new PeakingEQFilter_OperatorCalculator_SoundVarOrConst_OtherInputsConst(_stack.Pop(), dto.CenterFrequency.Const, dto.Width.Const, dto.DBGain.Const, dto.TargetSamplingRate));
        }

        protected override IOperatorDto Visit_Power_OperatorDto(Power_OperatorDto dto)
        {
            if (dto.Base.IsConst && dto.Exponent.IsVar)
            {
                return ProcessOperatorDto(dto, () => new Power_OperatorCalculator_ConstBase_VarExponent(dto.Base.Const, _stack.Pop()));
            }
            else if (dto.Base.IsVar && dto.Exponent.IsConst)
            {
                return ProcessOperatorDto(dto, () => new Power_OperatorCalculator_VarBase_ConstExponent(_stack.Pop(), dto.Exponent.Const));
            }
            else if (dto.Base.IsVar && dto.Exponent.IsVar)
            {
                return ProcessOperatorDto(dto, () => new Power_OperatorCalculator_VarBase_VarExponent(_stack.Pop(), _stack.Pop()));
            }
            else
            {
                throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_PulseTrigger_OperatorDto(PulseTrigger_OperatorDto dto)
        {
            return ProcessOperatorDto(dto, () => new PulseTrigger_OperatorCalculator(_stack.Pop(), _stack.Pop()));
        }

        protected override IOperatorDto Visit_Random_OperatorDto_Block(Random_OperatorDto_Block dto)
        {
            base.Visit_OperatorDto_Base(dto);

            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dto);
            RandomCalculator_Block randomCalculator = _calculatorCache.GetRandomCalculator_Block(dto.OperatorID);

            var calculator = new Random_OperatorCalculator_Block_VarFrequency(randomCalculator, _stack.Pop(), dimensionStack);
            _stack.Push(calculator);

            return dto;
        }

        protected override IOperatorDto Visit_Random_OperatorDto_Stripe_LagBehind(Random_OperatorDto_Stripe_LagBehind dto)
        {
            base.Visit_OperatorDto_Base(dto);

            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dto);
            RandomCalculator_Stripe randomCalculator = _calculatorCache.GetRandomCalculator_Stripe(dto.OperatorID);

            var calculator = new Random_OperatorCalculator_Stripe_VarFrequency(randomCalculator, _stack.Pop(), dimensionStack);
            _stack.Push(calculator);

            return dto;
        }

        protected override IOperatorDto Visit_RangeOverDimension_OperatorDto_OnlyConsts(RangeOverDimension_OperatorDto_OnlyConsts dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new RangeOverDimension_OperatorCalculator_OnlyConsts(dto.From.Const, dto.Till.Const, dto.Step.Const, dimensionStack));
        }

        protected override IOperatorDto Visit_RangeOverDimension_OperatorDto_OnlyVars(RangeOverDimension_OperatorDto_OnlyVars dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new RangeOverDimension_OperatorCalculator_OnlyVars(_stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_RangeOverDimension_OperatorDto_WithConsts_AndStepOne(RangeOverDimension_OperatorDto_WithConsts_AndStepOne dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new RangeOverDimension_OperatorCalculator_WithConsts_AndStepOne(dto.From.Const, dto.Till.Const, dimensionStack));
        }

        protected override IOperatorDto Visit_Remainder_OperatorDto(Remainder_OperatorDto dto)
        {
            return ProcessOperatorDto(dto, () => new Remainder_OperatorCalculator(_stack.Pop(), _stack.Pop()));
        }

        protected override IOperatorDto Visit_Round_OperatorDto_StepOne_ZeroOffset(Round_OperatorDto_StepOne_ZeroOffset dto)
        {
            return ProcessOperatorDto(dto, () => new Round_OperatorCalculator(_stack.Pop(), _stack.Pop(), _stack.Pop()));
        }

        protected override IOperatorDto Visit_Round_OperatorDto_WithOffset(Round_OperatorDto_WithOffset dto)
        {
            return ProcessOperatorDto(dto, () => new Round_OperatorCalculator(_stack.Pop(), _stack.Pop(), _stack.Pop()));
        }

        protected override IOperatorDto Visit_Round_OperatorDto_ZeroOffset(Round_OperatorDto_ZeroOffset dto)
        {
            return ProcessOperatorDto(dto, () => new Round_OperatorCalculator(_stack.Pop(), _stack.Pop(), _stack.Pop()));
        }

        protected override IOperatorDto Visit_Sample_OperatorDto_ConstFrequency_MonoToStereo_NoOriginShifting(Sample_OperatorDto_ConstFrequency_MonoToStereo_NoOriginShifting dto)
        {
            return Process_Sample_OperatorDto_SingleInputChannel_SingleOutputChannel(dto, (dimensionStack, underlyingCalculator) =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                return new Sample_OperatorCalculator_ConstFrequency_MonoToStereo_NoOriginShifting(dto.Frequency.Const, underlyingCalculator, dimensionStack);
            });
        }

        protected override IOperatorDto Visit_Sample_OperatorDto_ConstFrequency_MonoToStereo_WithOriginShifting(Sample_OperatorDto_ConstFrequency_MonoToStereo_WithOriginShifting dto)
        {
            return Process_Sample_OperatorDto_SingleInputChannel_SingleOutputChannel(dto, (dimensionStack, underlyingCalculator) =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                return new Sample_OperatorCalculator_ConstFrequency_MonoToStereo_WithOriginShifting(dto.Frequency.Const, underlyingCalculator, dimensionStack);
            });
        }

        protected override IOperatorDto Visit_Sample_OperatorDto_ConstFrequency_NoOriginShifting(Sample_OperatorDto_ConstFrequency_NoOriginShifting dto)
        {
            return Process_Sample_OperatorDto_MultipleInputChannel_MultipleOutputChannels(dto, (dimensionStack, channelDimensionStack, underlyingCalculators) =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                return new Sample_OperatorCalculator_ConstFrequency_NoOriginShifting(dto.Frequency.Const, underlyingCalculators, dimensionStack, channelDimensionStack);
            });
        }

        protected override IOperatorDto Visit_Sample_OperatorDto_ConstFrequency_StereoToMono_NoOriginShifting(Sample_OperatorDto_ConstFrequency_StereoToMono_NoOriginShifting dto)
        {
            return Process_Sample_OperatorDto_MultipleInputChannels_SingleOutputChannel(dto, (dimensionStack, underlyingCalculators) =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                return new Sample_OperatorCalculator_ConstFrequency_StereoToMono_NoOriginShifting(dto.Frequency.Const, underlyingCalculators, dimensionStack);
            });
        }

        protected override IOperatorDto Visit_Sample_OperatorDto_ConstFrequency_StereoToMono_WithOriginShifting(Sample_OperatorDto_ConstFrequency_StereoToMono_WithOriginShifting dto)
        {
            return Process_Sample_OperatorDto_MultipleInputChannels_SingleOutputChannel(dto, (dimensionStack, underlyingCalculators) =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                return new Sample_OperatorCalculator_ConstFrequency_StereoToMono_WithOriginShifting(dto.Frequency.Const, underlyingCalculators, dimensionStack);
            });
        }

        protected override IOperatorDto Visit_Sample_OperatorDto_ConstFrequency_WithOriginShifting(Sample_OperatorDto_ConstFrequency_WithOriginShifting dto)
        {
            return Process_Sample_OperatorDto_MultipleInputChannel_MultipleOutputChannels(dto, (dimensionStack, channelDimensionStack, underlyingCalculators) =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                return new Sample_OperatorCalculator_ConstFrequency_WithOriginShifting(dto.Frequency.Const, underlyingCalculators, dimensionStack, channelDimensionStack);
            });
        }

        protected override IOperatorDto Visit_Sample_OperatorDto_VarFrequency_MonoToStereo_NoPhaseTracking(Sample_OperatorDto_VarFrequency_MonoToStereo_NoPhaseTracking dto)
        {
            return Process_Sample_OperatorDto_SingleInputChannel_SingleOutputChannel(dto, (dimensionStack, underlyingCalculator) =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                return new Sample_OperatorCalculator_VarFrequency_MonoToStereo_NoPhaseTracking(_stack.Pop(), underlyingCalculator, dimensionStack);
            });
        }

        protected override IOperatorDto Visit_Sample_OperatorDto_VarFrequency_MonoToStereo_WithPhaseTracking(Sample_OperatorDto_VarFrequency_MonoToStereo_WithPhaseTracking dto)
        {
            return Process_Sample_OperatorDto_SingleInputChannel_SingleOutputChannel(dto, (dimensionStack, underlyingCalculator) =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                return new Sample_OperatorCalculator_VarFrequency_MonoToStereo_WithPhaseTracking(_stack.Pop(), underlyingCalculator, dimensionStack);
            });
        }

        protected override IOperatorDto Visit_Sample_OperatorDto_VarFrequency_NoPhaseTracking(Sample_OperatorDto_VarFrequency_NoPhaseTracking dto)
        {
            return Process_Sample_OperatorDto_MultipleInputChannel_MultipleOutputChannels(dto, (dimensionStack, channelDimensionStack, underlyingCalculators) =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                return new Sample_OperatorCalculator_VarFrequency_NoPhaseTracking(_stack.Pop(), underlyingCalculators, dimensionStack, channelDimensionStack);
            });
        }

        protected override IOperatorDto Visit_Sample_OperatorDto_VarFrequency_StereoToMono_NoPhaseTracking(Sample_OperatorDto_VarFrequency_StereoToMono_NoPhaseTracking dto)
        {
            return Process_Sample_OperatorDto_MultipleInputChannels_SingleOutputChannel(dto, (dimensionStack, underlyingCalculators) =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                return new Sample_OperatorCalculator_VarFrequency_StereoToMono_NoPhaseTracking(_stack.Pop(), underlyingCalculators, dimensionStack);
            });
        }

        protected override IOperatorDto Visit_Sample_OperatorDto_VarFrequency_StereoToMono_WithPhaseTracking(Sample_OperatorDto_VarFrequency_StereoToMono_WithPhaseTracking dto)
        {
            return Process_Sample_OperatorDto_MultipleInputChannels_SingleOutputChannel(dto, (dimensionStack, underlyingCalculators) =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                return new Sample_OperatorCalculator_VarFrequency_StereoToMono_WithPhaseTracking(_stack.Pop(), underlyingCalculators, dimensionStack);
            });
        }

        protected override IOperatorDto Visit_Sample_OperatorDto_VarFrequency_WithPhaseTracking(Sample_OperatorDto_VarFrequency_WithPhaseTracking dto)
        {
            return Process_Sample_OperatorDto_MultipleInputChannel_MultipleOutputChannels(dto, (dimensionStack, channelDimensionStack, underlyingCalculators) =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                return new Sample_OperatorCalculator_VarFrequency_WithPhaseTracking(_stack.Pop(), underlyingCalculators, dimensionStack, channelDimensionStack);
            });
        }

        private IOperatorDto Process_Sample_OperatorDto_SingleInputChannel_SingleOutputChannel(
            Sample_OperatorDto dto,
            Func<DimensionStack, ICalculatorWithPosition, OperatorCalculatorBase> createOperatorCalculatorDelegate)
        {
            base.Visit_OperatorDto_Base(dto);

            ArrayDto arrayDto = dto.ArrayDtos.Single();
            ICalculatorWithPosition underlyingCalculator = ArrayCalculatorFactory.CreateArrayCalculator(arrayDto);
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dto);

            var calculator = createOperatorCalculatorDelegate(dimensionStack, underlyingCalculator);
            _stack.Push(calculator);

            return dto;
        }

        /// <param name="createOperatorCalculatorDelegate"> Second DimensionStack parameter is the channelDimensionStack </param>
        private IOperatorDto Process_Sample_OperatorDto_MultipleInputChannel_MultipleOutputChannels(
            Sample_OperatorDto dto,
            Func<DimensionStack, DimensionStack, IList<ICalculatorWithPosition>, OperatorCalculatorBase> createOperatorCalculatorDelegate)
        {
            base.Visit_OperatorDto_Base(dto);

            IList<ICalculatorWithPosition> underlyingCalculators = dto.ArrayDtos.Select(x => ArrayCalculatorFactory.CreateArrayCalculator(x)).ToArray();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dto);
            DimensionStack channelDimensionStack = _dimensionStackCollection.GetDimensionStack(DimensionEnum.Channel);

            var calculator = createOperatorCalculatorDelegate(dimensionStack, channelDimensionStack, underlyingCalculators);
            _stack.Push(calculator);

            return dto;
        }

        private IOperatorDto Process_Sample_OperatorDto_MultipleInputChannels_SingleOutputChannel(
            Sample_OperatorDto dto,
            Func<DimensionStack, IList<ICalculatorWithPosition>, OperatorCalculatorBase> createOperatorCalculatorDelegate)
        {
            base.Visit_OperatorDto_Base(dto);

            IList<ICalculatorWithPosition> underlyingCalculators = dto.ArrayDtos.Select(x => ArrayCalculatorFactory.CreateArrayCalculator(x)).ToArray();
            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dto);

            var calculator = createOperatorCalculatorDelegate(dimensionStack, underlyingCalculators);
            _stack.Push(calculator);

            return dto;
        }
        
        protected override IOperatorDto Visit_SetDimension_OperatorDto(SetDimension_OperatorDto dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new SetDimension_OperatorCalculator(_stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Sine_OperatorDto_ConstFrequency_NoOriginShifting(Sine_OperatorDto_ConstFrequency_NoOriginShifting dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Sine_OperatorCalculator_ConstFrequency_NoOriginShifting(dto.Frequency.Const, dimensionStack));
        }

        protected override IOperatorDto Visit_Sine_OperatorDto_ConstFrequency_WithOriginShifting(Sine_OperatorDto_ConstFrequency_WithOriginShifting dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Sine_OperatorCalculator_ConstFrequency_WithOriginShifting(dto.Frequency.Const, dimensionStack));
        }

        protected override IOperatorDto Visit_Sine_OperatorDto_VarFrequency_NoPhaseTracking(Sine_OperatorDto_VarFrequency_NoPhaseTracking dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Sine_OperatorCalculator_VarFrequency_NoPhaseTracking(_stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Sine_OperatorDto_VarFrequency_WithPhaseTracking(Sine_OperatorDto_VarFrequency_WithPhaseTracking dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Sine_OperatorCalculator_VarFrequency_WithPhaseTracking(_stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_SortOverDimension_OperatorDto_CollectionRecalculationContinuous(SortOverDimension_OperatorDto_CollectionRecalculationContinuous dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new SortOverDimension_OperatorCalculator_CollectionRecalculationContinuous(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_SortOverDimension_OperatorDto_CollectionRecalculationUponReset(SortOverDimension_OperatorDto_CollectionRecalculationUponReset dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new SortOverDimension_OperatorCalculator_CollectionRecalculationUponReset(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_SortOverInlets_Outlet_OperatorDto(SortOverInlets_Outlet_OperatorDto dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new SortOverInlets_OperatorCalculator(dto.Inputs.Select(x => _stack.Pop()).ToArray(), dimensionStack));
        }

        protected override IOperatorDto Visit_Spectrum_OperatorDto(Spectrum_OperatorDto dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Spectrum_OperatorCalculator(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Squash_OperatorDto_WithOrigin(Squash_OperatorDto_WithOrigin dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Squash_OperatorCalculator_WithOrigin(_stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Squash_OperatorDto_ZeroOrigin(Squash_OperatorDto_ZeroOrigin dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Squash_OperatorCalculator_ZeroOrigin(_stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Squash_OperatorDto_ConstFactor_WithOriginShifting(Squash_OperatorDto_ConstFactor_WithOriginShifting dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Squash_OperatorCalculator_ConstFactor_WithOriginShifting(_stack.Pop(), dto.Factor, dimensionStack));
        }

        protected override IOperatorDto Visit_Squash_OperatorDto_VarFactor_WithPhaseTracking(Squash_OperatorDto_VarFactor_WithPhaseTracking dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Squash_OperatorCalculator_VarFactor_WithPhaseTracking(_stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Stretch_OperatorDto_WithOrigin(Stretch_OperatorDto_WithOrigin dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Stretch_OperatorCalculator_WithOrigin(_stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Stretch_OperatorDto_ZeroOrigin(Stretch_OperatorDto_ZeroOrigin dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Stretch_OperatorCalculator_ZeroOrigin(_stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Stretch_OperatorDto_ConstFactor_WithOriginShifting(Stretch_OperatorDto_ConstFactor_WithOriginShifting dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Stretch_OperatorCalculator_ConstFactor_WithOriginShifting(_stack.Pop(), dto.Factor, dimensionStack));
        }

        protected override IOperatorDto Visit_Stretch_OperatorDto_VarFactor_WithPhaseTracking(Stretch_OperatorDto_VarFactor_WithPhaseTracking dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Stretch_OperatorCalculator_VarFactor_WithPhaseTracking(_stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Subtract_OperatorDto(Subtract_OperatorDto dto)
        {
            if (dto.A.IsConst && dto.B.IsVar)
            {
                return ProcessOperatorDto(dto, () => new Subtract_OperatorCalculator_ConstA_VarB(dto.A.Const, _stack.Pop()));
            }
            else if (dto.A.IsVar && dto.B.IsConst)
            {
                return ProcessOperatorDto(dto, () => new Subtract_OperatorCalculator_VarA_ConstB(_stack.Pop(), dto.B.Const));
            }
            else if (dto.A.IsVar && dto.B.IsVar)
            {
                return ProcessOperatorDto(dto, () => new Subtract_OperatorCalculator_VarA_VarB(_stack.Pop(), _stack.Pop()));
            }
            else
            {
                throw new VisitationCannotBeHandledException();
            }
        }

        protected override IOperatorDto Visit_SumFollower_OperatorDto_SignalVarOrConst_OtherInputsVar(SumFollower_OperatorDto_SignalVarOrConst_OtherInputsVar dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new SumFollower_OperatorCalculator(_stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_SumOverDimension_OperatorDto_CollectionRecalculationContinuous(SumOverDimension_OperatorDto_CollectionRecalculationContinuous dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new SumOverDimension_OperatorCalculator_CollectionRecalculationContinuous(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_SumOverDimension_OperatorDto_CollectionRecalculationUponReset(SumOverDimension_OperatorDto_CollectionRecalculationUponReset dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new SumOverDimension_OperatorCalculator_CollectionRecalculationUponReset(_stack.Pop(), _stack.Pop(), _stack.Pop(), _stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_ToggleTrigger_OperatorDto(ToggleTrigger_OperatorDto dto)
        {
            return ProcessOperatorDto(dto, () => new ToggleTrigger_OperatorCalculator(_stack.Pop(), _stack.Pop()));
       }

        protected override IOperatorDto Visit_Triangle_OperatorDto_ConstFrequency_NoOriginShifting(Triangle_OperatorDto_ConstFrequency_NoOriginShifting dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Triangle_OperatorCalculator_ConstFrequency_NoOriginShifting(dto.Frequency.Const, dimensionStack));
        }

        protected override IOperatorDto Visit_Triangle_OperatorDto_ConstFrequency_WithOriginShifting(Triangle_OperatorDto_ConstFrequency_WithOriginShifting dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Triangle_OperatorCalculator_ConstFrequency_WithOriginShifting(dto.Frequency.Const, dimensionStack));
        }

        protected override IOperatorDto Visit_Triangle_OperatorDto_VarFrequency_NoPhaseTracking(Triangle_OperatorDto_VarFrequency_NoPhaseTracking dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Triangle_OperatorCalculator_VarFrequency_NoPhaseTracking(_stack.Pop(), dimensionStack));
        }

        protected override IOperatorDto Visit_Triangle_OperatorDto_VarFrequency_WithPhaseTracking(Triangle_OperatorDto_VarFrequency_WithPhaseTracking dto)
        {
            return ProcessWithDimension(dto, dimensionStack => new Triangle_OperatorCalculator_VarFrequency_WithPhaseTracking(_stack.Pop(), dimensionStack));
        }

        // Special Visitation

        /// <summary> Instance integrity needs to be maintained over the (top-level) PatchInlets. </summary>
        protected override IOperatorDto Visit_VariableInput_OperatorDto(VariableInput_OperatorDto dto)
        {
            base.Visit_VariableInput_OperatorDto(dto);

            if (!_variableInput_OperatorDto_To_Calculator_Dictionary.TryGetValue(dto, out VariableInput_OperatorCalculator calculator))
            {
                calculator = new VariableInput_OperatorCalculator(dto.DimensionEnum, dto.CanonicalName, dto.Position, dto.DefaultValue);

                _variableInput_OperatorDto_To_Calculator_Dictionary[dto] = calculator;
            }

            _stack.Push(calculator);

            return dto;
        }

        protected override IOperatorDto Visit_Reset_OperatorDto(Reset_OperatorDto dto)
        {
            base.Visit_Reset_OperatorDto(dto);

            OperatorCalculatorBase calculator = _stack.Peek();

            _resettableOperatorTuples.Add(new ResettableOperatorTuple(calculator, dto.Name, dto.Position));

            return dto;
        }

        // Helpers

        private IOperatorDto ProcessOperatorDto(IOperatorDto dto, Func<OperatorCalculatorBase> createOperatorCalculatorDelegate)
        {
            base.Visit_OperatorDto_Base(dto);

            OperatorCalculatorBase calculator = createOperatorCalculatorDelegate();
            _stack.Push(calculator);

            return dto;
        }

        /// <summary> Will get a DimensionStack and pass it to the OperatorCalculator, which is then pushed onto the _stack. </summary>
        private IOperatorDto ProcessWithDimension(IOperatorDto_WithDimension dto, Func<DimensionStack, OperatorCalculatorBase> createOperatorCalculatorDelegate)
        {
            base.Visit_OperatorDto_Base(dto);

            DimensionStack dimensionStack = _dimensionStackCollection.GetDimensionStack(dto);

            OperatorCalculatorBase calculator = createOperatorCalculatorDelegate(dimensionStack);
            _stack.Push(calculator);

            return dto;
        }
    }
}