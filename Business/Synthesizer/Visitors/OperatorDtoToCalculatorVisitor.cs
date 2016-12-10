﻿using System.Collections.Generic;
using JJ.Business.Synthesizer.Calculation.Operators;
using JJ.Business.Synthesizer.Dto;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Visitors
{
    internal class OperatorDtoToCalculatorVisitor : OperatorDtoVisitorBase_AfterMathSimplification
    {
        private readonly int _targetChannelCount;
        private readonly Stack<OperatorCalculatorBase> _stack = new Stack<OperatorCalculatorBase>();

        public OperatorDtoToCalculatorVisitor(int targetChannelCount)
        {
            _targetChannelCount = targetChannelCount;
        }

        public OperatorCalculatorBase Execute(OperatorDtoBase dto)
        {
            var preProcessing = new PreProcessing_OperatorDtoVisitor(_targetChannelCount);

            dto = preProcessing.Execute(dto);

            Visit_OperatorDto_Polymorphic(dto);

            if (_stack.Count != 1)
            {
                throw new NotEqualException(() => _stack.Count, 1);
            }

            return _stack.Pop();
        }

        protected override OperatorDtoBase Visit_Absolute_OperatorDto_VarX(Absolute_OperatorDto_VarX dto)
        {
            return base.Visit_Absolute_OperatorDto_VarX(dto);
        }

        protected override OperatorDtoBase Visit_Add_OperatorDto_Vars_1Const(Add_OperatorDto_Vars_1Const dto)
        {
            return base.Visit_Add_OperatorDto_Vars_1Const(dto);
        }

        protected override OperatorDtoBase Visit_Add_OperatorDto_Vars_NoConsts(Add_OperatorDto_Vars_NoConsts dto)
        {
            return base.Visit_Add_OperatorDto_Vars_NoConsts(dto);
        }

        protected override OperatorDtoBase Visit_AllPassFilter_OperatorDto_AllVars(AllPassFilter_OperatorDto_AllVars dto)
        {
            return base.Visit_AllPassFilter_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_AllPassFilter_OperatorDto_ManyConsts(AllPassFilter_OperatorDto_ManyConsts dto)
        {
            return base.Visit_AllPassFilter_OperatorDto_ManyConsts(dto);
        }

        protected override OperatorDtoBase Visit_And_OperatorDto_VarA_VarB(And_OperatorDto_VarA_VarB dto)
        {
            return base.Visit_And_OperatorDto_VarA_VarB(dto);
        }

        protected override OperatorDtoBase Visit_AverageFollower_OperatorDto_AllVars(AverageFollower_OperatorDto_AllVars dto)
        {
            return base.Visit_AverageFollower_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_AverageOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous(AverageOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous dto)
        {
            return base.Visit_AverageOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous(dto);
        }

        protected override OperatorDtoBase Visit_AverageOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset(AverageOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset dto)
        {
            return base.Visit_AverageOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset(dto);
        }

        protected override OperatorDtoBase Visit_AverageOverInlets_OperatorDto_Vars(AverageOverInlets_OperatorDto_Vars dto)
        {
            return base.Visit_AverageOverInlets_OperatorDto_Vars(dto);
        }

        protected override OperatorDtoBase Visit_BandPassFilterConstantPeakGain_OperatorDto_ConstCenterFrequency_ConstBandWidth(BandPassFilterConstantPeakGain_OperatorDto_ConstCenterFrequency_ConstBandWidth dto)
        {
            return base.Visit_BandPassFilterConstantPeakGain_OperatorDto_ConstCenterFrequency_ConstBandWidth(dto);
        }

        protected override OperatorDtoBase Visit_BandPassFilterConstantPeakGain_OperatorDto_VarCenterFrequency_VarBandWidth(BandPassFilterConstantPeakGain_OperatorDto_VarCenterFrequency_VarBandWidth dto)
        {
            return base.Visit_BandPassFilterConstantPeakGain_OperatorDto_VarCenterFrequency_VarBandWidth(dto);
        }

        protected override OperatorDtoBase Visit_BandPassFilterConstantTransitionGain_OperatorDto_ConstCenterFrequency_ConstBandWidth(BandPassFilterConstantTransitionGain_OperatorDto_ConstCenterFrequency_ConstBandWidth dto)
        {
            return base.Visit_BandPassFilterConstantTransitionGain_OperatorDto_ConstCenterFrequency_ConstBandWidth(dto);
        }

        protected override OperatorDtoBase Visit_BandPassFilterConstantTransitionGain_OperatorDto_VarCenterFrequency_VarBandWidth(BandPassFilterConstantTransitionGain_OperatorDto_VarCenterFrequency_VarBandWidth dto)
        {
            return base.Visit_BandPassFilterConstantTransitionGain_OperatorDto_VarCenterFrequency_VarBandWidth(dto);
        }

        protected override OperatorDtoBase Visit_Bundle_OperatorDto(Bundle_OperatorDto dto)
        {
            return base.Visit_Bundle_OperatorDto(dto);
        }

        protected override OperatorDtoBase Visit_Cache_OperatorDto_MultiChannel(Cache_OperatorDto_MultiChannel dto)
        {
            return base.Visit_Cache_OperatorDto_MultiChannel(dto);
        }

        protected override OperatorDtoBase Visit_Cache_OperatorDto_SingleChannel(Cache_OperatorDto_SingleChannel dto)
        {
            return base.Visit_Cache_OperatorDto_SingleChannel(dto);
        }

        protected override OperatorDtoBase Visit_ChangeTrigger_OperatorDto_VarPassThrough_VarReset(ChangeTrigger_OperatorDto_VarPassThrough_VarReset dto)
        {
            return base.Visit_ChangeTrigger_OperatorDto_VarPassThrough_VarReset(dto);
        }

        protected override OperatorDtoBase Visit_ClosestOverDimensionExp_OperatorDto_CollectionRecalculationContinuous(ClosestOverDimensionExp_OperatorDto_CollectionRecalculationContinuous dto)
        {
            return base.Visit_ClosestOverDimensionExp_OperatorDto_CollectionRecalculationContinuous(dto);
        }

        protected override OperatorDtoBase Visit_ClosestOverDimensionExp_OperatorDto_CollectionRecalculationUponReset(ClosestOverDimensionExp_OperatorDto_CollectionRecalculationUponReset dto)
        {
            return base.Visit_ClosestOverDimensionExp_OperatorDto_CollectionRecalculationUponReset(dto);
        }

        protected override OperatorDtoBase Visit_ClosestOverDimension_OperatorDto_CollectionRecalculationContinuous(ClosestOverDimension_OperatorDto_CollectionRecalculationContinuous dto)
        {
            return base.Visit_ClosestOverDimension_OperatorDto_CollectionRecalculationContinuous(dto);
        }

        protected override OperatorDtoBase Visit_ClosestOverDimension_OperatorDto_CollectionRecalculationUponReset(ClosestOverDimension_OperatorDto_CollectionRecalculationUponReset dto)
        {
            return base.Visit_ClosestOverDimension_OperatorDto_CollectionRecalculationUponReset(dto);
        }

        protected override OperatorDtoBase Visit_ClosestOverInletsExp_OperatorDto_VarInput_2ConstItems(ClosestOverInletsExp_OperatorDto_VarInput_2ConstItems dto)
        {
            return base.Visit_ClosestOverInletsExp_OperatorDto_VarInput_2ConstItems(dto);
        }

        protected override OperatorDtoBase Visit_ClosestOverInletsExp_OperatorDto_VarInput_ConstItems(ClosestOverInletsExp_OperatorDto_VarInput_ConstItems dto)
        {
            return base.Visit_ClosestOverInletsExp_OperatorDto_VarInput_ConstItems(dto);
        }

        protected override OperatorDtoBase Visit_ClosestOverInletsExp_OperatorDto_VarInput_VarItems(ClosestOverInletsExp_OperatorDto_VarInput_VarItems dto)
        {
            return base.Visit_ClosestOverInletsExp_OperatorDto_VarInput_VarItems(dto);
        }

        protected override OperatorDtoBase Visit_ClosestOverInlets_OperatorDto_VarInput_2ConstItems(ClosestOverInlets_OperatorDto_VarInput_2ConstItems dto)
        {
            return base.Visit_ClosestOverInlets_OperatorDto_VarInput_2ConstItems(dto);
        }

        protected override OperatorDtoBase Visit_ClosestOverInlets_OperatorDto_VarInput_ConstItems(ClosestOverInlets_OperatorDto_VarInput_ConstItems dto)
        {
            return base.Visit_ClosestOverInlets_OperatorDto_VarInput_ConstItems(dto);
        }

        protected override OperatorDtoBase Visit_ClosestOverInlets_OperatorDto_VarInput_VarItems(ClosestOverInlets_OperatorDto_VarInput_VarItems dto)
        {
            return base.Visit_ClosestOverInlets_OperatorDto_VarInput_VarItems(dto);
        }

        protected override OperatorDtoBase Visit_Curve_OperatorDto_MinXZero_NoOriginShifting(Curve_OperatorDto_MinXZero_NoOriginShifting dto)
        {
            return base.Visit_Curve_OperatorDto_MinXZero_NoOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Curve_OperatorDto_MinXZero_WithOriginShifting(Curve_OperatorDto_MinXZero_WithOriginShifting dto)
        {
            return base.Visit_Curve_OperatorDto_MinXZero_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Curve_OperatorDto_MinX_NoOriginShifting(Curve_OperatorDto_MinX_NoOriginShifting dto)
        {
            return base.Visit_Curve_OperatorDto_MinX_NoOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Curve_OperatorDto_MinX_WithOriginShifting(Curve_OperatorDto_MinX_WithOriginShifting dto)
        {
            return base.Visit_Curve_OperatorDto_MinX_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_CustomOperator_OperatorDto(CustomOperator_OperatorDto dto)
        {
            return base.Visit_CustomOperator_OperatorDto(dto);
        }

        protected override OperatorDtoBase Visit_DimensionToOutlets_OperatorDto(DimensionToOutlets_OperatorDto dto)
        {
            return base.Visit_DimensionToOutlets_OperatorDto(dto);
        }

        protected override OperatorDtoBase Visit_Divide_OperatorDto_ConstA_ConstB_VarOrigin(Divide_OperatorDto_ConstA_ConstB_VarOrigin dto)
        {
            return base.Visit_Divide_OperatorDto_ConstA_ConstB_VarOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Divide_OperatorDto_ConstA_VarB_ConstOrigin(Divide_OperatorDto_ConstA_VarB_ConstOrigin dto)
        {
            return base.Visit_Divide_OperatorDto_ConstA_VarB_ConstOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Divide_OperatorDto_ConstA_VarB_VarOrigin(Divide_OperatorDto_ConstA_VarB_VarOrigin dto)
        {
            return base.Visit_Divide_OperatorDto_ConstA_VarB_VarOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Divide_OperatorDto_ConstA_VarB_ZeroOrigin(Divide_OperatorDto_ConstA_VarB_ZeroOrigin dto)
        {
            return base.Visit_Divide_OperatorDto_ConstA_VarB_ZeroOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Divide_OperatorDto_VarA_ConstB_ConstOrigin(Divide_OperatorDto_VarA_ConstB_ConstOrigin dto)
        {
            return base.Visit_Divide_OperatorDto_VarA_ConstB_ConstOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Divide_OperatorDto_VarA_ConstB_VarOrigin(Divide_OperatorDto_VarA_ConstB_VarOrigin dto)
        {
            return base.Visit_Divide_OperatorDto_VarA_ConstB_VarOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Divide_OperatorDto_VarA_ConstB_ZeroOrigin(Divide_OperatorDto_VarA_ConstB_ZeroOrigin dto)
        {
            return base.Visit_Divide_OperatorDto_VarA_ConstB_ZeroOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Divide_OperatorDto_VarA_VarB_ConstOrigin(Divide_OperatorDto_VarA_VarB_ConstOrigin dto)
        {
            return base.Visit_Divide_OperatorDto_VarA_VarB_ConstOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Divide_OperatorDto_VarA_VarB_VarOrigin(Divide_OperatorDto_VarA_VarB_VarOrigin dto)
        {
            return base.Visit_Divide_OperatorDto_VarA_VarB_VarOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Divide_OperatorDto_VarA_VarB_ZeroOrigin(Divide_OperatorDto_VarA_VarB_ZeroOrigin dto)
        {
            return base.Visit_Divide_OperatorDto_VarA_VarB_ZeroOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Equal_OperatorDto_VarA_ConstB(Equal_OperatorDto_VarA_ConstB dto)
        {
            return base.Visit_Equal_OperatorDto_VarA_ConstB(dto);
        }

        protected override OperatorDtoBase Visit_Equal_OperatorDto_VarA_VarB(Equal_OperatorDto_VarA_VarB dto)
        {
            return base.Visit_Equal_OperatorDto_VarA_VarB(dto);
        }

        protected override OperatorDtoBase Visit_Exponent_OperatorDto_ConstLow_ConstHigh_VarRatio(Exponent_OperatorDto_ConstLow_ConstHigh_VarRatio dto)
        {
            return base.Visit_Exponent_OperatorDto_ConstLow_ConstHigh_VarRatio(dto);
        }

        protected override OperatorDtoBase Visit_Exponent_OperatorDto_ConstLow_VarHigh_ConstRatio(Exponent_OperatorDto_ConstLow_VarHigh_ConstRatio dto)
        {
            return base.Visit_Exponent_OperatorDto_ConstLow_VarHigh_ConstRatio(dto);
        }

        protected override OperatorDtoBase Visit_Exponent_OperatorDto_ConstLow_VarHigh_VarRatio(Exponent_OperatorDto_ConstLow_VarHigh_VarRatio dto)
        {
            return base.Visit_Exponent_OperatorDto_ConstLow_VarHigh_VarRatio(dto);
        }

        protected override OperatorDtoBase Visit_Exponent_OperatorDto_VarLow_ConstHigh_ConstRatio(Exponent_OperatorDto_VarLow_ConstHigh_ConstRatio dto)
        {
            return base.Visit_Exponent_OperatorDto_VarLow_ConstHigh_ConstRatio(dto);
        }

        protected override OperatorDtoBase Visit_Exponent_OperatorDto_VarLow_ConstHigh_VarRatio(Exponent_OperatorDto_VarLow_ConstHigh_VarRatio dto)
        {
            return base.Visit_Exponent_OperatorDto_VarLow_ConstHigh_VarRatio(dto);
        }

        protected override OperatorDtoBase Visit_Exponent_OperatorDto_VarLow_VarHigh_ConstRatio(Exponent_OperatorDto_VarLow_VarHigh_ConstRatio dto)
        {
            return base.Visit_Exponent_OperatorDto_VarLow_VarHigh_ConstRatio(dto);
        }

        protected override OperatorDtoBase Visit_Exponent_OperatorDto_VarLow_VarHigh_VarRatio(Exponent_OperatorDto_VarLow_VarHigh_VarRatio dto)
        {
            return base.Visit_Exponent_OperatorDto_VarLow_VarHigh_VarRatio(dto);
        }

        protected override OperatorDtoBase Visit_GetDimension_OperatorDto(GetDimension_OperatorDto dto)
        {
            return base.Visit_GetDimension_OperatorDto(dto);
        }

        protected override OperatorDtoBase Visit_GreaterThanOrEqual_OperatorDto_VarA_ConstB(GreaterThanOrEqual_OperatorDto_VarA_ConstB dto)
        {
            return base.Visit_GreaterThanOrEqual_OperatorDto_VarA_ConstB(dto);
        }

        protected override OperatorDtoBase Visit_GreaterThanOrEqual_OperatorDto_VarA_VarB(GreaterThanOrEqual_OperatorDto_VarA_VarB dto)
        {
            return base.Visit_GreaterThanOrEqual_OperatorDto_VarA_VarB(dto);
        }

        protected override OperatorDtoBase Visit_GreaterThan_OperatorDto_VarA_ConstB(GreaterThan_OperatorDto_VarA_ConstB dto)
        {
            return base.Visit_GreaterThan_OperatorDto_VarA_ConstB(dto);
        }

        protected override OperatorDtoBase Visit_GreaterThan_OperatorDto_VarA_VarB(GreaterThan_OperatorDto_VarA_VarB dto)
        {
            return base.Visit_GreaterThan_OperatorDto_VarA_VarB(dto);
        }

        protected override OperatorDtoBase Visit_HighPassFilter_OperatorDto_AllVars(HighPassFilter_OperatorDto_AllVars dto)
        {
            return base.Visit_HighPassFilter_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_HighPassFilter_OperatorDto_ManyConsts(HighPassFilter_OperatorDto_ManyConsts dto)
        {
            return base.Visit_HighPassFilter_OperatorDto_ManyConsts(dto);
        }

        protected override OperatorDtoBase Visit_HighShelfFilter_OperatorDto_AllVars(HighShelfFilter_OperatorDto_AllVars dto)
        {
            return base.Visit_HighShelfFilter_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_HighShelfFilter_OperatorDto_ManyConsts(HighShelfFilter_OperatorDto_ManyConsts dto)
        {
            return base.Visit_HighShelfFilter_OperatorDto_ManyConsts(dto);
        }

        protected override OperatorDtoBase Visit_Hold_OperatorDto_VarSignal(Hold_OperatorDto_VarSignal dto)
        {
            return base.Visit_Hold_OperatorDto_VarSignal(dto);
        }

        protected override OperatorDtoBase Visit_If_OperatorDto_VarCondition_ConstThen_ConstElse(If_OperatorDto_VarCondition_ConstThen_ConstElse dto)
        {
            return base.Visit_If_OperatorDto_VarCondition_ConstThen_ConstElse(dto);
        }

        protected override OperatorDtoBase Visit_If_OperatorDto_VarCondition_ConstThen_VarElse(If_OperatorDto_VarCondition_ConstThen_VarElse dto)
        {
            return base.Visit_If_OperatorDto_VarCondition_ConstThen_VarElse(dto);
        }

        protected override OperatorDtoBase Visit_If_OperatorDto_VarCondition_VarThen_ConstElse(If_OperatorDto_VarCondition_VarThen_ConstElse dto)
        {
            return base.Visit_If_OperatorDto_VarCondition_VarThen_ConstElse(dto);
        }

        protected override OperatorDtoBase Visit_If_OperatorDto_VarCondition_VarThen_VarElse(If_OperatorDto_VarCondition_VarThen_VarElse dto)
        {
            return base.Visit_If_OperatorDto_VarCondition_VarThen_VarElse(dto);
        }

        protected override OperatorDtoBase Visit_InletsToDimension_OperatorDto(InletsToDimension_OperatorDto dto)
        {
            return base.Visit_InletsToDimension_OperatorDto(dto);
        }

        protected override OperatorDtoBase Visit_Interpolate_OperatorDto_Block(Interpolate_OperatorDto_Block dto)
        {
            return base.Visit_Interpolate_OperatorDto_Block(dto);
        }

        protected override OperatorDtoBase Visit_Interpolate_OperatorDto_CubicAbruptSlope(Interpolate_OperatorDto_CubicAbruptSlope dto)
        {
            return base.Visit_Interpolate_OperatorDto_CubicAbruptSlope(dto);
        }

        protected override OperatorDtoBase Visit_Interpolate_OperatorDto_CubicEquidistant(Interpolate_OperatorDto_CubicEquidistant dto)
        {
            return base.Visit_Interpolate_OperatorDto_CubicEquidistant(dto);
        }

        protected override OperatorDtoBase Visit_Interpolate_OperatorDto_CubicSmoothSlope_LagBehind(Interpolate_OperatorDto_CubicSmoothSlope_LagBehind dto)
        {
            return base.Visit_Interpolate_OperatorDto_CubicSmoothSlope_LagBehind(dto);
        }

        protected override OperatorDtoBase Visit_Interpolate_OperatorDto_Hermite_LagBehind(Interpolate_OperatorDto_Hermite_LagBehind dto)
        {
            return base.Visit_Interpolate_OperatorDto_Hermite_LagBehind(dto);
        }

        protected override OperatorDtoBase Visit_Interpolate_OperatorDto_Line_LagBehind_ConstSamplingRate(Interpolate_OperatorDto_Line_LagBehind_ConstSamplingRate dto)
        {
            return base.Visit_Interpolate_OperatorDto_Line_LagBehind_ConstSamplingRate(dto);
        }

        protected override OperatorDtoBase Visit_Interpolate_OperatorDto_Line_LagBehind_VarSamplingRate(Interpolate_OperatorDto_Line_LagBehind_VarSamplingRate dto)
        {
            return base.Visit_Interpolate_OperatorDto_Line_LagBehind_VarSamplingRate(dto);
        }

        protected override OperatorDtoBase Visit_Interpolate_OperatorDto_Stripe_LagBehind(Interpolate_OperatorDto_Stripe_LagBehind dto)
        {
            return base.Visit_Interpolate_OperatorDto_Stripe_LagBehind(dto);
        }

        protected override OperatorDtoBase Visit_Loop_OperatorDto_ConstSkip_WhichEqualsLoopStartMarker_ConstLoopEndMarker_NoNoteDuration(Loop_OperatorDto_ConstSkip_WhichEqualsLoopStartMarker_ConstLoopEndMarker_NoNoteDuration dto)
        {
            return base.Visit_Loop_OperatorDto_ConstSkip_WhichEqualsLoopStartMarker_ConstLoopEndMarker_NoNoteDuration(dto);
        }

        protected override OperatorDtoBase Visit_LessThanOrEqual_OperatorDto_VarA_ConstB(LessThanOrEqual_OperatorDto_VarA_ConstB dto)
        {
            return base.Visit_LessThanOrEqual_OperatorDto_VarA_ConstB(dto);
        }

        protected override OperatorDtoBase Visit_LessThanOrEqual_OperatorDto_VarA_VarB(LessThanOrEqual_OperatorDto_VarA_VarB dto)
        {
            return base.Visit_LessThanOrEqual_OperatorDto_VarA_VarB(dto);
        }

        protected override OperatorDtoBase Visit_LessThan_OperatorDto_VarA_ConstB(LessThan_OperatorDto_VarA_ConstB dto)
        {
            return base.Visit_LessThan_OperatorDto_VarA_ConstB(dto);
        }

        protected override OperatorDtoBase Visit_LessThan_OperatorDto_VarA_VarB(LessThan_OperatorDto_VarA_VarB dto)
        {
            return base.Visit_LessThan_OperatorDto_VarA_VarB(dto);
        }

        protected override OperatorDtoBase Visit_Loop_OperatorDto_AllVars(Loop_OperatorDto_AllVars dto)
        {
            return base.Visit_Loop_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_Loop_OperatorDto_ConstSkip_WhichEqualsLoopStartMarker_VarLoopEndMarker_NoNoteDuration(Loop_OperatorDto_ConstSkip_WhichEqualsLoopStartMarker_VarLoopEndMarker_NoNoteDuration dto)
        {
            return base.Visit_Loop_OperatorDto_ConstSkip_WhichEqualsLoopStartMarker_VarLoopEndMarker_NoNoteDuration(dto);
        }

        protected override OperatorDtoBase Visit_Loop_OperatorDto_ManyConstants(Loop_OperatorDto_ManyConstants dto)
        {
            return base.Visit_Loop_OperatorDto_ManyConstants(dto);
        }

        protected override OperatorDtoBase Visit_Loop_OperatorDto_NoSkipOrRelease(Loop_OperatorDto_NoSkipOrRelease dto)
        {
            return base.Visit_Loop_OperatorDto_NoSkipOrRelease(dto);
        }

        protected override OperatorDtoBase Visit_Loop_OperatorDto_NoSkipOrRelease_ManyConstants(Loop_OperatorDto_NoSkipOrRelease_ManyConstants dto)
        {
            return base.Visit_Loop_OperatorDto_NoSkipOrRelease_ManyConstants(dto);
        }

        protected override OperatorDtoBase Visit_LowPassFilter_OperatorDto_AllVars(LowPassFilter_OperatorDto_AllVars dto)
        {
            return base.Visit_LowPassFilter_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_LowPassFilter_OperatorDto_ManyConsts(LowPassFilter_OperatorDto_ManyConsts dto)
        {
            return base.Visit_LowPassFilter_OperatorDto_ManyConsts(dto);
        }

        protected override OperatorDtoBase Visit_LowShelfFilter_OperatorDto_AllVars(LowShelfFilter_OperatorDto_AllVars dto)
        {
            return base.Visit_LowShelfFilter_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_LowShelfFilter_OperatorDto_ManyConsts(LowShelfFilter_OperatorDto_ManyConsts dto)
        {
            return base.Visit_LowShelfFilter_OperatorDto_ManyConsts(dto);
        }

        protected override OperatorDtoBase Visit_MaxFollower_OperatorDto_AllVars(MaxFollower_OperatorDto_AllVars dto)
        {
            return base.Visit_MaxFollower_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_MaxOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous(MaxOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous dto)
        {
            return base.Visit_MaxOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous(dto);
        }

        protected override OperatorDtoBase Visit_MaxOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset(MaxOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset dto)
        {
            return base.Visit_MaxOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset(dto);
        }

        protected override OperatorDtoBase Visit_MaxOverInlets_OperatorDto_1Var_1Const(MaxOverInlets_OperatorDto_1Var_1Const dto)
        {
            return base.Visit_MaxOverInlets_OperatorDto_1Var_1Const(dto);
        }

        protected override OperatorDtoBase Visit_MaxOverInlets_OperatorDto_2Vars(MaxOverInlets_OperatorDto_2Vars dto)
        {
            return base.Visit_MaxOverInlets_OperatorDto_2Vars(dto);
        }

        protected override OperatorDtoBase Visit_MaxOverInlets_OperatorDto_Vars_1Const(MaxOverInlets_OperatorDto_Vars_1Const dto)
        {
            return base.Visit_MaxOverInlets_OperatorDto_Vars_1Const(dto);
        }

        protected override OperatorDtoBase Visit_MaxOverInlets_OperatorDto_Vars_NoConsts(MaxOverInlets_OperatorDto_Vars_NoConsts dto)
        {
            return base.Visit_MaxOverInlets_OperatorDto_Vars_NoConsts(dto);
        }

        protected override OperatorDtoBase Visit_MinFollower_OperatorDto_AllVars(MinFollower_OperatorDto_AllVars dto)
        {
            return base.Visit_MinFollower_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_MinOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous(MinOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous dto)
        {
            return base.Visit_MinOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous(dto);
        }

        protected override OperatorDtoBase Visit_MinOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset(MinOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset dto)
        {
            return base.Visit_MinOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset(dto);
        }

        protected override OperatorDtoBase Visit_MinOverInlets_OperatorDto_1Var_1Const(MinOverInlets_OperatorDto_1Var_1Const dto)
        {
            return base.Visit_MinOverInlets_OperatorDto_1Var_1Const(dto);
        }

        protected override OperatorDtoBase Visit_MinOverInlets_OperatorDto_2Vars(MinOverInlets_OperatorDto_2Vars dto)
        {
            return base.Visit_MinOverInlets_OperatorDto_2Vars(dto);
        }

        protected override OperatorDtoBase Visit_MinOverInlets_OperatorDto_Vars_1Const(MinOverInlets_OperatorDto_Vars_1Const dto)
        {
            return base.Visit_MinOverInlets_OperatorDto_Vars_1Const(dto);
        }

        protected override OperatorDtoBase Visit_MinOverInlets_OperatorDto_Vars_NoConsts(MinOverInlets_OperatorDto_Vars_NoConsts dto)
        {
            return base.Visit_MinOverInlets_OperatorDto_Vars_NoConsts(dto);
        }

        protected override OperatorDtoBase Visit_MultiplyWithOrigin_OperatorDto_ConstA_ConstB_VarOrigin(MultiplyWithOrigin_OperatorDto_ConstA_ConstB_VarOrigin dto)
        {
            return base.Visit_MultiplyWithOrigin_OperatorDto_ConstA_ConstB_VarOrigin(dto);
        }

        protected override OperatorDtoBase Visit_MultiplyWithOrigin_OperatorDto_ConstA_VarB_ConstOrigin(MultiplyWithOrigin_OperatorDto_ConstA_VarB_ConstOrigin dto)
        {
            return base.Visit_MultiplyWithOrigin_OperatorDto_ConstA_VarB_ConstOrigin(dto);
        }

        protected override OperatorDtoBase Visit_MultiplyWithOrigin_OperatorDto_ConstA_VarB_VarOrigin(MultiplyWithOrigin_OperatorDto_ConstA_VarB_VarOrigin dto)
        {
            return base.Visit_MultiplyWithOrigin_OperatorDto_ConstA_VarB_VarOrigin(dto);
        }

        protected override OperatorDtoBase Visit_MultiplyWithOrigin_OperatorDto_VarA_ConstB_ConstOrigin(MultiplyWithOrigin_OperatorDto_VarA_ConstB_ConstOrigin dto)
        {
            return base.Visit_MultiplyWithOrigin_OperatorDto_VarA_ConstB_ConstOrigin(dto);
        }

        protected override OperatorDtoBase Visit_MultiplyWithOrigin_OperatorDto_VarA_ConstB_VarOrigin(MultiplyWithOrigin_OperatorDto_VarA_ConstB_VarOrigin dto)
        {
            return base.Visit_MultiplyWithOrigin_OperatorDto_VarA_ConstB_VarOrigin(dto);
        }

        protected override OperatorDtoBase Visit_MultiplyWithOrigin_OperatorDto_VarA_VarB_ConstOrigin(MultiplyWithOrigin_OperatorDto_VarA_VarB_ConstOrigin dto)
        {
            return base.Visit_MultiplyWithOrigin_OperatorDto_VarA_VarB_ConstOrigin(dto);
        }

        protected override OperatorDtoBase Visit_MultiplyWithOrigin_OperatorDto_VarA_VarB_VarOrigin(MultiplyWithOrigin_OperatorDto_VarA_VarB_VarOrigin dto)
        {
            return base.Visit_MultiplyWithOrigin_OperatorDto_VarA_VarB_VarOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Multiply_OperatorDto_Vars_1Const(Multiply_OperatorDto_Vars_1Const dto)
        {
            return base.Visit_Multiply_OperatorDto_Vars_1Const(dto);
        }

        protected override OperatorDtoBase Visit_Multiply_OperatorDto_Vars_NoConsts(Multiply_OperatorDto_Vars_NoConsts dto)
        {
            return base.Visit_Multiply_OperatorDto_Vars_NoConsts(dto);
        }

        protected override OperatorDtoBase Visit_Negative_OperatorDto_VarX(Negative_OperatorDto_VarX dto)
        {
            return base.Visit_Negative_OperatorDto_VarX(dto);
        }

        protected override OperatorDtoBase Visit_Noise_OperatorDto(Noise_OperatorDto dto)
        {
            return base.Visit_Noise_OperatorDto(dto);
        }

        protected override OperatorDtoBase Visit_NotchFilter_OperatorDto_AllVars(NotchFilter_OperatorDto_AllVars dto)
        {
            return base.Visit_NotchFilter_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_NotchFilter_OperatorDto_ManyConsts(NotchFilter_OperatorDto_ManyConsts dto)
        {
            return base.Visit_NotchFilter_OperatorDto_ManyConsts(dto);
        }

        protected override OperatorDtoBase Visit_NotEqual_OperatorDto_VarA_ConstB(NotEqual_OperatorDto_VarA_ConstB dto)
        {
            return base.Visit_NotEqual_OperatorDto_VarA_ConstB(dto);
        }

        protected override OperatorDtoBase Visit_NotEqual_OperatorDto_VarA_VarB(NotEqual_OperatorDto_VarA_VarB dto)
        {
            return base.Visit_NotEqual_OperatorDto_VarA_VarB(dto);
        }

        protected override OperatorDtoBase Visit_Not_OperatorDto_VarX(Not_OperatorDto_VarX dto)
        {
            return base.Visit_Not_OperatorDto_VarX(dto);
        }

        protected override OperatorDtoBase Visit_Number_OperatorDto(Number_OperatorDto dto)
        {
            return base.Visit_Number_OperatorDto(dto);
        }

        protected override OperatorDtoBase Visit_Number_OperatorDto_NaN(Number_OperatorDto_NaN dto)
        {
            return base.Visit_Number_OperatorDto_NaN(dto);
        }

        protected override OperatorDtoBase Visit_Number_OperatorDto_One(Number_OperatorDto_One dto)
        {
            return base.Visit_Number_OperatorDto_One(dto);
        }

        protected override OperatorDtoBase Visit_Number_OperatorDto_Zero(Number_OperatorDto_Zero dto)
        {
            return base.Visit_Number_OperatorDto_Zero(dto);
        }

        protected override OperatorDtoBase Visit_OneOverX_OperatorDto_VarX(OneOverX_OperatorDto_VarX dto)
        {
            return base.Visit_OneOverX_OperatorDto_VarX(dto);
        }

        protected override OperatorDtoBase Visit_Or_OperatorDto_VarA_VarB(Or_OperatorDto_VarA_VarB dto)
        {
            return base.Visit_Or_OperatorDto_VarA_VarB(dto);
        }

        protected override OperatorDtoBase Visit_PatchInlet_OperatorDto(PatchInlet_OperatorDto dto)
        {
            return base.Visit_PatchInlet_OperatorDto(dto);
        }

        protected override OperatorDtoBase Visit_PatchOutlet_OperatorDto(PatchOutlet_OperatorDto dto)
        {
            return base.Visit_PatchOutlet_OperatorDto(dto);
        }

        protected override OperatorDtoBase Visit_PeakingEQFilter_OperatorDto_AllVars(PeakingEQFilter_OperatorDto_AllVars dto)
        {
            return base.Visit_PeakingEQFilter_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_PeakingEQFilter_OperatorDto_ManyConsts(PeakingEQFilter_OperatorDto_ManyConsts dto)
        {
            return base.Visit_PeakingEQFilter_OperatorDto_ManyConsts(dto);
        }

        protected override OperatorDtoBase Visit_Power_OperatorDto_ConstBase_VarExponent(Power_OperatorDto_ConstBase_VarExponent dto)
        {
            return base.Visit_Power_OperatorDto_ConstBase_VarExponent(dto);
        }

        protected override OperatorDtoBase Visit_Power_OperatorDto_VarBase_ConstExponent(Power_OperatorDto_VarBase_ConstExponent dto)
        {
            return base.Visit_Power_OperatorDto_VarBase_ConstExponent(dto);
        }

        protected override OperatorDtoBase Visit_Power_OperatorDto_VarBase_VarExponent(Power_OperatorDto_VarBase_VarExponent dto)
        {
            return base.Visit_Power_OperatorDto_VarBase_VarExponent(dto);
        }

        protected override OperatorDtoBase Visit_PulseTrigger_OperatorDto_VarPassThrough_VarReset(PulseTrigger_OperatorDto_VarPassThrough_VarReset dto)
        {
            return base.Visit_PulseTrigger_OperatorDto_VarPassThrough_VarReset(dto);
        }

        protected override OperatorDtoBase Visit_Pulse_OperatorDto_ConstFrequency_ConstWidth_NoOriginShifting(Pulse_OperatorDto_ConstFrequency_ConstWidth_NoOriginShifting dto)
        {
            return base.Visit_Pulse_OperatorDto_ConstFrequency_ConstWidth_NoOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Pulse_OperatorDto_ConstFrequency_ConstWidth_WithOriginShifting(Pulse_OperatorDto_ConstFrequency_ConstWidth_WithOriginShifting dto)
        {
            return base.Visit_Pulse_OperatorDto_ConstFrequency_ConstWidth_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Pulse_OperatorDto_ConstFrequency_VarWidth_NoOriginShifting(Pulse_OperatorDto_ConstFrequency_VarWidth_NoOriginShifting dto)
        {
            return base.Visit_Pulse_OperatorDto_ConstFrequency_VarWidth_NoOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Pulse_OperatorDto_ConstFrequency_VarWidth_WithOriginShifting(Pulse_OperatorDto_ConstFrequency_VarWidth_WithOriginShifting dto)
        {
            return base.Visit_Pulse_OperatorDto_ConstFrequency_VarWidth_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Pulse_OperatorDto_VarFrequency_ConstWidth_NoPhaseTracking(Pulse_OperatorDto_VarFrequency_ConstWidth_NoPhaseTracking dto)
        {
            return base.Visit_Pulse_OperatorDto_VarFrequency_ConstWidth_NoPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Pulse_OperatorDto_VarFrequency_ConstWidth_WithPhaseTracking(Pulse_OperatorDto_VarFrequency_ConstWidth_WithPhaseTracking dto)
        {
            return base.Visit_Pulse_OperatorDto_VarFrequency_ConstWidth_WithPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Pulse_OperatorDto_VarFrequency_VarWidth_NoPhaseTracking(Pulse_OperatorDto_VarFrequency_VarWidth_NoPhaseTracking dto)
        {
            return base.Visit_Pulse_OperatorDto_VarFrequency_VarWidth_NoPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Pulse_OperatorDto_VarFrequency_VarWidth_WithPhaseTracking(Pulse_OperatorDto_VarFrequency_VarWidth_WithPhaseTracking dto)
        {
            return base.Visit_Pulse_OperatorDto_VarFrequency_VarWidth_WithPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Random_OperatorDto_Block(Random_OperatorDto_Block dto)
        {
            return base.Visit_Random_OperatorDto_Block(dto);
        }

        protected override OperatorDtoBase Visit_Random_OperatorDto_CubicAbruptSlope(Random_OperatorDto_CubicAbruptSlope dto)
        {
            return base.Visit_Random_OperatorDto_CubicAbruptSlope(dto);
        }

        protected override OperatorDtoBase Visit_Random_OperatorDto_CubicEquidistant(Random_OperatorDto_CubicEquidistant dto)
        {
            return base.Visit_Random_OperatorDto_CubicEquidistant(dto);
        }

        protected override OperatorDtoBase Visit_Random_OperatorDto_CubicSmoothSlope(Random_OperatorDto_CubicSmoothSlope dto)
        {
            return base.Visit_Random_OperatorDto_CubicSmoothSlope(dto);
        }

        protected override OperatorDtoBase Visit_Random_OperatorDto_Hermite(Random_OperatorDto_Hermite dto)
        {
            return base.Visit_Random_OperatorDto_Hermite(dto);
        }

        protected override OperatorDtoBase Visit_Random_OperatorDto_Line(Random_OperatorDto_Line dto)
        {
            return base.Visit_Random_OperatorDto_Line(dto);
        }

        protected override OperatorDtoBase Visit_Random_OperatorDto_Stripe(Random_OperatorDto_Stripe dto)
        {
            return base.Visit_Random_OperatorDto_Stripe(dto);
        }

        protected override OperatorDtoBase Visit_RangeOverDimension_OperatorCalculator_OnlyConsts(Dto.RangeOverDimension_OperatorCalculator_OnlyConsts dto)
        {
            return base.Visit_RangeOverDimension_OperatorCalculator_OnlyConsts(dto);
        }

        protected override OperatorDtoBase Visit_RangeOverDimension_OperatorCalculator_OnlyVars(Dto.RangeOverDimension_OperatorCalculator_OnlyVars dto)
        {
            return base.Visit_RangeOverDimension_OperatorCalculator_OnlyVars(dto);
        }

        protected override OperatorDtoBase Visit_RangeOverDimension_OperatorCalculator_WithConsts_AndStepOne(Dto.RangeOverDimension_OperatorCalculator_WithConsts_AndStepOne dto)
        {
            return base.Visit_RangeOverDimension_OperatorCalculator_WithConsts_AndStepOne(dto);
        }

        protected override OperatorDtoBase Visit_RangeOverOutlets_OperatorDto_ConstFrom_ConstStep(RangeOverOutlets_OperatorDto_ConstFrom_ConstStep dto)
        {
            return base.Visit_RangeOverOutlets_OperatorDto_ConstFrom_ConstStep(dto);
        }

        protected override OperatorDtoBase Visit_RangeOverOutlets_OperatorDto_ConstFrom_VarStep(RangeOverOutlets_OperatorDto_ConstFrom_VarStep dto)
        {
            return base.Visit_RangeOverOutlets_OperatorDto_ConstFrom_VarStep(dto);
        }

        protected override OperatorDtoBase Visit_RangeOverOutlets_OperatorDto_VarFrom_ConstStep(RangeOverOutlets_OperatorDto_VarFrom_ConstStep dto)
        {
            return base.Visit_RangeOverOutlets_OperatorDto_VarFrom_ConstStep(dto);
        }

        protected override OperatorDtoBase Visit_RangeOverOutlets_OperatorDto_VarFrom_VarStep(RangeOverOutlets_OperatorDto_VarFrom_VarStep dto)
        {
            return base.Visit_RangeOverOutlets_OperatorDto_VarFrom_VarStep(dto);
        }

        protected override OperatorDtoBase Visit_Reset_OperatorDto(Reset_OperatorDto dto)
        {
            return base.Visit_Reset_OperatorDto(dto);
        }

        protected override OperatorDtoBase Visit_Reverse_OperatorDto_ConstSpeed_NoOriginShifting(Reverse_OperatorDto_ConstSpeed_NoOriginShifting dto)
        {
            return base.Visit_Reverse_OperatorDto_ConstSpeed_NoOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Reverse_OperatorDto_ConstSpeed_WithOriginShifting(Reverse_OperatorDto_ConstSpeed_WithOriginShifting dto)
        {
            return base.Visit_Reverse_OperatorDto_ConstSpeed_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Reverse_OperatorDto_VarSpeed_NoPhaseTracking(Reverse_OperatorDto_VarSpeed_NoPhaseTracking dto)
        {
            return base.Visit_Reverse_OperatorDto_VarSpeed_NoPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Reverse_OperatorDto_VarSpeed_WithPhaseTracking(Reverse_OperatorDto_VarSpeed_WithPhaseTracking dto)
        {
            return base.Visit_Reverse_OperatorDto_VarSpeed_WithPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Round_OperatorDto_ConstSignal(Round_OperatorDto_ConstSignal dto)
        {
            return base.Visit_Round_OperatorDto_ConstSignal(dto);
        }

        protected override OperatorDtoBase Visit_Round_OperatorDto_VarSignal_ConstStep_ConstOffset(Round_OperatorDto_VarSignal_ConstStep_ConstOffset dto)
        {
            return base.Visit_Round_OperatorDto_VarSignal_ConstStep_ConstOffset(dto);
        }

        protected override OperatorDtoBase Visit_Round_OperatorDto_VarSignal_ConstStep_VarOffset(Round_OperatorDto_VarSignal_ConstStep_VarOffset dto)
        {
            return base.Visit_Round_OperatorDto_VarSignal_ConstStep_VarOffset(dto);
        }

        protected override OperatorDtoBase Visit_Round_OperatorDto_VarSignal_ConstStep_ZeroOffset(Round_OperatorDto_VarSignal_ConstStep_ZeroOffset dto)
        {
            return base.Visit_Round_OperatorDto_VarSignal_ConstStep_ZeroOffset(dto);
        }

        protected override OperatorDtoBase Visit_Round_OperatorDto_VarSignal_StepOne_OffsetZero(Round_OperatorDto_VarSignal_StepOne_OffsetZero dto)
        {
            return base.Visit_Round_OperatorDto_VarSignal_StepOne_OffsetZero(dto);
        }

        protected override OperatorDtoBase Visit_Round_OperatorDto_VarSignal_VarStep_ConstOffset(Round_OperatorDto_VarSignal_VarStep_ConstOffset dto)
        {
            return base.Visit_Round_OperatorDto_VarSignal_VarStep_ConstOffset(dto);
        }

        protected override OperatorDtoBase Visit_Round_OperatorDto_VarSignal_VarStep_VarOffset(Round_OperatorDto_VarSignal_VarStep_VarOffset dto)
        {
            return base.Visit_Round_OperatorDto_VarSignal_VarStep_VarOffset(dto);
        }

        protected override OperatorDtoBase Visit_Round_OperatorDto_VarSignal_VarStep_ZeroOffset(Round_OperatorDto_VarSignal_VarStep_ZeroOffset dto)
        {
            return base.Visit_Round_OperatorDto_VarSignal_VarStep_ZeroOffset(dto);
        }

        protected override OperatorDtoBase Visit_Sample_OperatorDto_ConstFrequency_MonoToStereo_NoOriginShifting(Sample_OperatorDto_ConstFrequency_MonoToStereo_NoOriginShifting dto)
        {
            return base.Visit_Sample_OperatorDto_ConstFrequency_MonoToStereo_NoOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Sample_OperatorDto_ConstFrequency_MonoToStereo_WithOriginShifting(Sample_OperatorDto_ConstFrequency_MonoToStereo_WithOriginShifting dto)
        {
            return base.Visit_Sample_OperatorDto_ConstFrequency_MonoToStereo_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Sample_OperatorDto_ConstFrequency_NoOriginShifting(Sample_OperatorDto_ConstFrequency_NoOriginShifting dto)
        {
            return base.Visit_Sample_OperatorDto_ConstFrequency_NoOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Sample_OperatorDto_ConstFrequency_StereoToMono_NoOriginShifting(Sample_OperatorDto_ConstFrequency_StereoToMono_NoOriginShifting dto)
        {
            return base.Visit_Sample_OperatorDto_ConstFrequency_StereoToMono_NoOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Sample_OperatorDto_ConstFrequency_StereoToMono_WithOriginShifting(Sample_OperatorDto_ConstFrequency_StereoToMono_WithOriginShifting dto)
        {
            return base.Visit_Sample_OperatorDto_ConstFrequency_StereoToMono_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Sample_OperatorDto_ConstFrequency_WithOriginShifting(Sample_OperatorDto_ConstFrequency_WithOriginShifting dto)
        {
            return base.Visit_Sample_OperatorDto_ConstFrequency_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Sample_OperatorDto_VarFrequency_MonoToStereo_NoPhaseTracking(Sample_OperatorDto_VarFrequency_MonoToStereo_NoPhaseTracking dto)
        {
            return base.Visit_Sample_OperatorDto_VarFrequency_MonoToStereo_NoPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Sample_OperatorDto_VarFrequency_MonoToStereo_WithPhaseTracking(Sample_OperatorDto_VarFrequency_MonoToStereo_WithPhaseTracking dto)
        {
            return base.Visit_Sample_OperatorDto_VarFrequency_MonoToStereo_WithPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Sample_OperatorDto_VarFrequency_NoPhaseTracking(Sample_OperatorDto_VarFrequency_NoPhaseTracking dto)
        {
            return base.Visit_Sample_OperatorDto_VarFrequency_NoPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Sample_OperatorDto_VarFrequency_StereoToMono_NoPhaseTracking(Sample_OperatorDto_VarFrequency_StereoToMono_NoPhaseTracking dto)
        {
            return base.Visit_Sample_OperatorDto_VarFrequency_StereoToMono_NoPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Sample_OperatorDto_VarFrequency_StereoToMono_WithPhaseTracking(Sample_OperatorDto_VarFrequency_StereoToMono_WithPhaseTracking dto)
        {
            return base.Visit_Sample_OperatorDto_VarFrequency_StereoToMono_WithPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Sample_OperatorDto_VarFrequency_WithPhaseTracking(Sample_OperatorDto_VarFrequency_WithPhaseTracking dto)
        {
            return base.Visit_Sample_OperatorDto_VarFrequency_WithPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_SawDown_OperatorDto_ConstFrequency_NoOriginShifting(SawDown_OperatorDto_ConstFrequency_NoOriginShifting dto)
        {
            return base.Visit_SawDown_OperatorDto_ConstFrequency_NoOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_SawDown_OperatorDto_ConstFrequency_WithOriginShifting(SawDown_OperatorDto_ConstFrequency_WithOriginShifting dto)
        {
            return base.Visit_SawDown_OperatorDto_ConstFrequency_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_SawDown_OperatorDto_VarFrequency_NoPhaseTracking(SawDown_OperatorDto_VarFrequency_NoPhaseTracking dto)
        {
            return base.Visit_SawDown_OperatorDto_VarFrequency_NoPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_SawDown_OperatorDto_VarFrequency_WithPhaseTracking(SawDown_OperatorDto_VarFrequency_WithPhaseTracking dto)
        {
            return base.Visit_SawDown_OperatorDto_VarFrequency_WithPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_SawUp_OperatorDto_ConstFrequency_NoOriginShifting(SawUp_OperatorDto_ConstFrequency_NoOriginShifting dto)
        {
            return base.Visit_SawUp_OperatorDto_ConstFrequency_NoOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_SawUp_OperatorDto_ConstFrequency_WithOriginShifting(SawUp_OperatorDto_ConstFrequency_WithOriginShifting dto)
        {
            return base.Visit_SawUp_OperatorDto_ConstFrequency_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_SawUp_OperatorDto_VarFrequency_NoPhaseTracking(SawUp_OperatorDto_VarFrequency_NoPhaseTracking dto)
        {
            return base.Visit_SawUp_OperatorDto_VarFrequency_NoPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_SawUp_OperatorDto_VarFrequency_WithPhaseTracking(SawUp_OperatorDto_VarFrequency_WithPhaseTracking dto)
        {
            return base.Visit_SawUp_OperatorDto_VarFrequency_WithPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Scaler_OperatorDto_AllVars(Scaler_OperatorDto_AllVars dto)
        {
            return base.Visit_Scaler_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_Scaler_OperatorDto_ManyConsts(Scaler_OperatorDto_ManyConsts dto)
        {
            return base.Visit_Scaler_OperatorDto_ManyConsts(dto);
        }

        protected override OperatorDtoBase Visit_Select_OperatorDto_VarSignal_ConstPosition(Select_OperatorDto_VarSignal_ConstPosition dto)
        {
            return base.Visit_Select_OperatorDto_VarSignal_ConstPosition(dto);
        }

        protected override OperatorDtoBase Visit_Select_OperatorDto_VarSignal_VarPosition(Select_OperatorDto_VarSignal_VarPosition dto)
        {
            return base.Visit_Select_OperatorDto_VarSignal_VarPosition(dto);
        }

        protected override OperatorDtoBase Visit_SetDimension_OperatorDto_ConstValue(SetDimension_OperatorDto_ConstValue dto)
        {
            return base.Visit_SetDimension_OperatorDto_ConstValue(dto);
        }

        protected override OperatorDtoBase Visit_SetDimension_OperatorDto_VarValue(SetDimension_OperatorDto_VarValue dto)
        {
            return base.Visit_SetDimension_OperatorDto_VarValue(dto);
        }

        protected override OperatorDtoBase Visit_Shift_OperatorDto_VarSignal_ConstDistance(Shift_OperatorDto_VarSignal_ConstDistance dto)
        {
            return base.Visit_Shift_OperatorDto_VarSignal_ConstDistance(dto);
        }

        protected override OperatorDtoBase Visit_Shift_OperatorDto_VarSignal_VarDistance(Shift_OperatorDto_VarSignal_VarDistance dto)
        {
            return base.Visit_Shift_OperatorDto_VarSignal_VarDistance(dto);
        }

        protected override OperatorDtoBase Visit_Sine_OperatorDto_ConstFrequency_NoOriginShifting(Sine_OperatorDto_ConstFrequency_NoOriginShifting dto)
        {
            return base.Visit_Sine_OperatorDto_ConstFrequency_NoOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Sine_OperatorDto_ConstFrequency_WithOriginShifting(Sine_OperatorDto_ConstFrequency_WithOriginShifting dto)
        {
            return base.Visit_Sine_OperatorDto_ConstFrequency_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Sine_OperatorDto_VarFrequency_NoPhaseTracking(Sine_OperatorDto_VarFrequency_NoPhaseTracking dto)
        {
            return base.Visit_Sine_OperatorDto_VarFrequency_NoPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Sine_OperatorDto_VarFrequency_WithPhaseTracking(Sine_OperatorDto_VarFrequency_WithPhaseTracking dto)
        {
            return base.Visit_Sine_OperatorDto_VarFrequency_WithPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_SortOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous(SortOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous dto)
        {
            return base.Visit_SortOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous(dto);
        }

        protected override OperatorDtoBase Visit_SortOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset(SortOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset dto)
        {
            return base.Visit_SortOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset(dto);
        }

        protected override OperatorDtoBase Visit_SortOverInlets_OperatorDto(SortOverInlets_OperatorDto dto)
        {
            return base.Visit_SortOverInlets_OperatorDto(dto);
        }

        protected override OperatorDtoBase Visit_Spectrum_OperatorDto_AllVars(Spectrum_OperatorDto_AllVars dto)
        {
            return base.Visit_Spectrum_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_Square_OperatorDto_ConstFrequency_NoOriginShifting(Square_OperatorDto_ConstFrequency_NoOriginShifting dto)
        {
            return base.Visit_Square_OperatorDto_ConstFrequency_NoOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Square_OperatorDto_ConstFrequency_WithOriginShifting(Square_OperatorDto_ConstFrequency_WithOriginShifting dto)
        {
            return base.Visit_Square_OperatorDto_ConstFrequency_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Square_OperatorDto_VarFrequency_NoPhaseTracking(Square_OperatorDto_VarFrequency_NoPhaseTracking dto)
        {
            return base.Visit_Square_OperatorDto_VarFrequency_NoPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Square_OperatorDto_VarFrequency_WithPhaseTracking(Square_OperatorDto_VarFrequency_WithPhaseTracking dto)
        {
            return base.Visit_Square_OperatorDto_VarFrequency_WithPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Squash_OperatorDto_VarSignal_ConstFactor_ConstOrigin(Squash_OperatorDto_VarSignal_ConstFactor_ConstOrigin dto)
        {
            return base.Visit_Squash_OperatorDto_VarSignal_ConstFactor_ConstOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Squash_OperatorDto_VarSignal_ConstFactor_VarOrigin(Squash_OperatorDto_VarSignal_ConstFactor_VarOrigin dto)
        {
            return base.Visit_Squash_OperatorDto_VarSignal_ConstFactor_VarOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Squash_OperatorDto_VarSignal_ConstFactor_WithOriginShifting(Squash_OperatorDto_VarSignal_ConstFactor_WithOriginShifting dto)
        {
            return base.Visit_Squash_OperatorDto_VarSignal_ConstFactor_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Squash_OperatorDto_VarSignal_ConstFactor_ZeroOrigin(Squash_OperatorDto_VarSignal_ConstFactor_ZeroOrigin dto)
        {
            return base.Visit_Squash_OperatorDto_VarSignal_ConstFactor_ZeroOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Squash_OperatorDto_VarSignal_VarFactor_ConstOrigin(Squash_OperatorDto_VarSignal_VarFactor_ConstOrigin dto)
        {
            return base.Visit_Squash_OperatorDto_VarSignal_VarFactor_ConstOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Squash_OperatorDto_VarSignal_VarFactor_VarOrigin(Squash_OperatorDto_VarSignal_VarFactor_VarOrigin dto)
        {
            return base.Visit_Squash_OperatorDto_VarSignal_VarFactor_VarOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Squash_OperatorDto_VarSignal_VarFactor_WithPhaseTracking(Squash_OperatorDto_VarSignal_VarFactor_WithPhaseTracking dto)
        {
            return base.Visit_Squash_OperatorDto_VarSignal_VarFactor_WithPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Squash_OperatorDto_VarSignal_VarFactor_ZeroOrigin(Squash_OperatorDto_VarSignal_VarFactor_ZeroOrigin dto)
        {
            return base.Visit_Squash_OperatorDto_VarSignal_VarFactor_ZeroOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Stretch_OperatorDto_VarSignal_ConstFactor_ConstOrigin(Stretch_OperatorDto_VarSignal_ConstFactor_ConstOrigin dto)
        {
            return base.Visit_Stretch_OperatorDto_VarSignal_ConstFactor_ConstOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Stretch_OperatorDto_VarSignal_ConstFactor_VarOrigin(Stretch_OperatorDto_VarSignal_ConstFactor_VarOrigin dto)
        {
            return base.Visit_Stretch_OperatorDto_VarSignal_ConstFactor_VarOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Stretch_OperatorDto_VarSignal_ConstFactor_WithOriginShifting(Stretch_OperatorDto_VarSignal_ConstFactor_WithOriginShifting dto)
        {
            return base.Visit_Stretch_OperatorDto_VarSignal_ConstFactor_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Stretch_OperatorDto_VarSignal_ConstFactor_ZeroOrigin(Stretch_OperatorDto_VarSignal_ConstFactor_ZeroOrigin dto)
        {
            return base.Visit_Stretch_OperatorDto_VarSignal_ConstFactor_ZeroOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Stretch_OperatorDto_VarSignal_VarFactor_ConstOrigin(Stretch_OperatorDto_VarSignal_VarFactor_ConstOrigin dto)
        {
            return base.Visit_Stretch_OperatorDto_VarSignal_VarFactor_ConstOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Stretch_OperatorDto_VarSignal_VarFactor_VarOrigin(Stretch_OperatorDto_VarSignal_VarFactor_VarOrigin dto)
        {
            return base.Visit_Stretch_OperatorDto_VarSignal_VarFactor_VarOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Stretch_OperatorDto_VarSignal_VarFactor_WithPhaseTracking(Stretch_OperatorDto_VarSignal_VarFactor_WithPhaseTracking dto)
        {
            return base.Visit_Stretch_OperatorDto_VarSignal_VarFactor_WithPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Stretch_OperatorDto_VarSignal_VarFactor_ZeroOrigin(Stretch_OperatorDto_VarSignal_VarFactor_ZeroOrigin dto)
        {
            return base.Visit_Stretch_OperatorDto_VarSignal_VarFactor_ZeroOrigin(dto);
        }

        protected override OperatorDtoBase Visit_Subtract_OperatorDto_ConstA_VarB(Subtract_OperatorDto_ConstA_VarB dto)
        {
            return base.Visit_Subtract_OperatorDto_ConstA_VarB(dto);
        }

        protected override OperatorDtoBase Visit_Subtract_OperatorDto_VarA_ConstB(Subtract_OperatorDto_VarA_ConstB dto)
        {
            return base.Visit_Subtract_OperatorDto_VarA_ConstB(dto);
        }

        protected override OperatorDtoBase Visit_Subtract_OperatorDto_VarA_VarB(Subtract_OperatorDto_VarA_VarB dto)
        {
            return base.Visit_Subtract_OperatorDto_VarA_VarB(dto);
        }

        protected override OperatorDtoBase Visit_SumFollower_OperatorDto_AllVars(SumFollower_OperatorDto_AllVars dto)
        {
            return base.Visit_SumFollower_OperatorDto_AllVars(dto);
        }

        protected override OperatorDtoBase Visit_SumFollower_OperatorDto_ConstSignal_VarSampleCount(SumFollower_OperatorDto_ConstSignal_VarSampleCount dto)
        {
            return base.Visit_SumFollower_OperatorDto_ConstSignal_VarSampleCount(dto);
        }

        protected override OperatorDtoBase Visit_SumOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous(SumOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous dto)
        {
            return base.Visit_SumOverDimension_OperatorDto_AllVars_CollectionRecalculationContinuous(dto);
        }

        protected override OperatorDtoBase Visit_SumOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset(SumOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset dto)
        {
            return base.Visit_SumOverDimension_OperatorDto_AllVars_CollectionRecalculationUponReset(dto);
        }

        protected override OperatorDtoBase Visit_TimePower_OperatorDto_VarSignal_VarExponent_VarOrigin(TimePower_OperatorDto_VarSignal_VarExponent_VarOrigin dto)
        {
            return base.Visit_TimePower_OperatorDto_VarSignal_VarExponent_VarOrigin(dto);
        }

        protected override OperatorDtoBase Visit_TimePower_OperatorDto_VarSignal_VarExponent_ZeroOrigin(TimePower_OperatorDto_VarSignal_VarExponent_ZeroOrigin dto)
        {
            return base.Visit_TimePower_OperatorDto_VarSignal_VarExponent_ZeroOrigin(dto);
        }

        protected override OperatorDtoBase Visit_ToggleTrigger_OperatorDto_VarPassThrough_VarReset(ToggleTrigger_OperatorDto_VarPassThrough_VarReset dto)
        {
            return base.Visit_ToggleTrigger_OperatorDto_VarPassThrough_VarReset(dto);
        }

        protected override OperatorDtoBase Visit_Triangle_OperatorDto_ConstFrequency_NoOriginShifting(Triangle_OperatorDto_ConstFrequency_NoOriginShifting dto)
        {
            return base.Visit_Triangle_OperatorDto_ConstFrequency_NoOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Triangle_OperatorDto_ConstFrequency_WithOriginShifting(Triangle_OperatorDto_ConstFrequency_WithOriginShifting dto)
        {
            return base.Visit_Triangle_OperatorDto_ConstFrequency_WithOriginShifting(dto);
        }

        protected override OperatorDtoBase Visit_Triangle_OperatorDto_VarFrequency_NoPhaseTracking(Triangle_OperatorDto_VarFrequency_NoPhaseTracking dto)
        {
            return base.Visit_Triangle_OperatorDto_VarFrequency_NoPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Triangle_OperatorDto_VarFrequency_WithPhaseTracking(Triangle_OperatorDto_VarFrequency_WithPhaseTracking dto)
        {
            return base.Visit_Triangle_OperatorDto_VarFrequency_WithPhaseTracking(dto);
        }

        protected override OperatorDtoBase Visit_Unbundle_OperatorDto(Unbundle_OperatorDto dto)
        {
            return base.Visit_Unbundle_OperatorDto(dto);
        }
    }
}
