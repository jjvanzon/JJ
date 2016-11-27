﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JJ.Business.Synthesizer.Dto;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Helpers;
using JJ.Framework.Common;
using JJ.Framework.Common.Exceptions;

namespace JJ.Business.Synthesizer.Visitors
{
    internal class ClassSpecialization_OperatorDtoVisitor : OperatorDtoVisitorBase
    {
        private class ClosestOverInlets_MathPropertiesDto
        {
            public MathPropertiesDto InputMathPropertiesDto { get; set; }
            public IList<MathPropertiesDto> ItemMathPropertiesDtos { get; set; }
            public bool AllItemsAreConst { get; set; }
            public IList<double> ItemsValues { get; set; }
        }

        public OperatorDtoBase Execute(OperatorDtoBase dto)
        {
            return Visit_OperatorDto_Polymorphic(dto);
        }

        protected override OperatorDtoBase Visit_Absolute_OperatorDto(Absolute_OperatorDto dto)
        {
            base.Visit_Absolute_OperatorDto(dto);

            OperatorDtoBase xOperatorDto = dto.XOperatorDto;

            MathPropertiesDto xMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(xOperatorDto);

            if (xMathPropertiesDto.IsConst)
            {
                return new Absolute_OperatorDto_ConstX { X = xMathPropertiesDto.ConstValue };
            }
            else
            {
                return new Absolute_OperatorDto_VarX { XOperatorDto = xOperatorDto };
            }
        }

        protected override OperatorDtoBase Visit_Add_OperatorDto(Add_OperatorDto dto)
        {
            base.Visit_Add_OperatorDto(dto);

            IList<OperatorDtoBase> operatorDtos = dto.InputOperatorDtos;

            IList<OperatorDtoBase> constOperatorDtos = operatorDtos.Where(x => MathPropertiesHelper.GetMathPropertiesDto(x).IsConst).ToArray();
            IList<OperatorDtoBase> varOperatorDtos = operatorDtos.Except(constOperatorDtos).ToArray();
            IList<double> consts = constOperatorDtos.Select(x => MathPropertiesHelper.GetMathPropertiesDto(x).ConstValue).ToArray();

            bool hasVars = varOperatorDtos.Any();
            bool hasConsts = constOperatorDtos.Any();

            if (hasVars && hasConsts)
            {
                return new Add_OperatorDto_Vars_Consts { Vars = varOperatorDtos, Consts = consts };
            }
            else if (hasVars && !hasConsts)
            {
                return new Add_OperatorDto_Vars_NoConsts { Vars = varOperatorDtos };
            }
            else if (!hasVars && hasConsts)
            {
                return new Add_OperatorDto_NoVars_Consts { Consts = consts };
            }
            else if (!hasVars && !hasConsts)
            {
                return new Add_OperatorDto_NoVars_NoConsts();
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_AllPassFilter_OperatorDto(AllPassFilter_OperatorDto dto)
        {
            base.Visit_AllPassFilter_OperatorDto(dto);

            OperatorDtoBase signalOperatorDto = dto.SignalOperatorDto;
            OperatorDtoBase centerFrequencyOperatorDto = dto.CenterFrequencyOperatorDto;
            OperatorDtoBase bandWidthOperatorDto = dto.BandWidthOperatorDto;

            MathPropertiesDto signalMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(signalOperatorDto);
            MathPropertiesDto centerFrequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(centerFrequencyOperatorDto);
            MathPropertiesDto bandWidthMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bandWidthOperatorDto);

            if (signalMathPropertiesDto.IsConst)
            {
                return signalOperatorDto;
            }
            else if (centerFrequencyMathPropertiesDto.IsConst && bandWidthMathPropertiesDto.IsConst)
            {
                return new AllPassFilter_OperatorDto_ManyConsts { SignalOperatorDto = signalOperatorDto, CenterFrequency = centerFrequencyMathPropertiesDto.ConstValue, BandWidth = bandWidthMathPropertiesDto.ConstValue };
            }
            else
            {
                return new AllPassFilter_OperatorDto_AllVars { SignalOperatorDto = signalOperatorDto, CenterFrequencyOperatorDto = centerFrequencyOperatorDto, BandWidthOperatorDto = bandWidthOperatorDto };
            }
        }

        protected override OperatorDtoBase Visit_And_OperatorDto(And_OperatorDto dto)
        {
            base.Visit_And_OperatorDto(dto);

            OperatorDtoBase aOperatorDto = dto.AOperatorDto;
            OperatorDtoBase bOperatorDto = dto.BOperatorDto;

            MathPropertiesDto aMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(aOperatorDto);
            MathPropertiesDto bMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bOperatorDto);

            if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsConst)
            {
                return new And_OperatorDto_ConstA_ConstB { A = aMathPropertiesDto.ConstValue, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsConst)
            {
                return new And_OperatorDto_VarA_ConstB { AOperatorDto = aOperatorDto, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsVar)
            {
                return new And_OperatorDto_ConstA_VarB { A = aMathPropertiesDto.ConstValue, BOperatorDto = bOperatorDto };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsVar)
            {
                return new And_OperatorDto_VarA_VarB { AOperatorDto = aOperatorDto, BOperatorDto = bOperatorDto };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_AverageOverDimension_OperatorDto(AverageOverDimension_OperatorDto dto)
        {
            base.Visit_AverageOverDimension_OperatorDto(dto);

            AverageOverDimension_OperatorDto dto2;

            switch (dto.CollectionRecalculationEnum)
            {
                case CollectionRecalculationEnum.Continuous:
                    dto2 = new AverageOverDimension_OperatorDto_CollectionRecalculationContinuous();
                    break;

                case CollectionRecalculationEnum.UponReset:
                    dto2 = new AverageOverDimension_OperatorDto_CollectionRecalculationUponReset();
                    break;

                default:
                    throw new ValueNotSupportedException(dto.CollectionRecalculationEnum);
            }

            Clone_AggregateOverDimensionProperties(dto, dto2);

            return dto2;
        }

        protected override OperatorDtoBase Visit_BandPassFilterConstantPeakGain_OperatorDto(BandPassFilterConstantPeakGain_OperatorDto dto)
        {
            base.Visit_BandPassFilterConstantPeakGain_OperatorDto(dto);

            OperatorDtoBase signalOperatorDto = dto.SignalOperatorDto;
            OperatorDtoBase centerFrequencyOperatorDto = dto.CenterFrequencyOperatorDto;
            OperatorDtoBase bandWidthOperatorDto = dto.BandWidthOperatorDto;

            MathPropertiesDto signalMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(signalOperatorDto);
            MathPropertiesDto centerFrequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(centerFrequencyOperatorDto);
            MathPropertiesDto bandWidthMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bandWidthOperatorDto);

            if (signalMathPropertiesDto.IsConst)
            {
                return signalOperatorDto;
            }
            else if (centerFrequencyMathPropertiesDto.IsConst && bandWidthMathPropertiesDto.IsConst)
            {
                return new BandPassFilterConstantPeakGain_OperatorDto_ConstCenterFrequency_ConstBandWidth { SignalOperatorDto = signalOperatorDto, CenterFrequency = centerFrequencyMathPropertiesDto.ConstValue, BandWidth = bandWidthMathPropertiesDto.ConstValue };
            }
            else
            {
                return new BandPassFilterConstantPeakGain_OperatorDto_VarCenterFrequency_VarBandWidth { SignalOperatorDto = signalOperatorDto, CenterFrequencyOperatorDto = centerFrequencyOperatorDto, BandWidthOperatorDto = bandWidthOperatorDto };
            }
        }

        protected override OperatorDtoBase Visit_BandPassFilterConstantTransitionGain_OperatorDto(BandPassFilterConstantTransitionGain_OperatorDto dto)
        {
            base.Visit_BandPassFilterConstantTransitionGain_OperatorDto(dto);

            OperatorDtoBase signalOperatorDto = dto.SignalOperatorDto;
            OperatorDtoBase centerFrequencyOperatorDto = dto.CenterFrequencyOperatorDto;
            OperatorDtoBase bandWidthOperatorDto = dto.BandWidthOperatorDto;

            MathPropertiesDto signalMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(signalOperatorDto);
            MathPropertiesDto centerFrequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(centerFrequencyOperatorDto);
            MathPropertiesDto bandWidthMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bandWidthOperatorDto);

            if (signalMathPropertiesDto.IsConst)
            {
                return signalOperatorDto;
            }
            else if (centerFrequencyMathPropertiesDto.IsConst && bandWidthMathPropertiesDto.IsConst)
            {
                return new BandPassFilterConstantTransitionGain_OperatorDto_ConstCenterFrequency_ConstBandWidth { SignalOperatorDto = signalOperatorDto, CenterFrequency = centerFrequencyMathPropertiesDto.ConstValue, BandWidth = bandWidthMathPropertiesDto.ConstValue };
            }
            else
            {
                return new BandPassFilterConstantTransitionGain_OperatorDto_VarCenterFrequency_VarBandWidth { SignalOperatorDto = signalOperatorDto, CenterFrequencyOperatorDto = centerFrequencyOperatorDto, BandWidthOperatorDto = bandWidthOperatorDto };
            }
        }

        protected override OperatorDtoBase Visit_Cache_OperatorDto(Cache_OperatorDto dto)
        {
            base.Visit_Cache_OperatorDto(dto);

            Cache_OperatorDto dto2;

            switch (dto.ChannelCount)
            {
                case 1:
                    dto2 = new Cache_OperatorDto_SingleChannel();
                    break;

                default:
                    dto2 = new Cache_OperatorDto_MultiChannel();
                    break;
            }

            Clone_CacheOperatorProperties(dto, dto2);

            return dto2;
        }

        protected override OperatorDtoBase Visit_ClosestOverDimension_OperatorDto(ClosestOverDimension_OperatorDto dto)
        {
            base.Visit_ClosestOverDimension_OperatorDto(dto);

            ClosestOverDimension_OperatorDto dto2;

            switch (dto.CollectionRecalculationEnum)
            {
                case CollectionRecalculationEnum.Continuous:
                    dto2 = new ClosestOverDimension_OperatorDto_CollectionRecalculationContinuous();
                    break;

                case CollectionRecalculationEnum.UponReset:
                    dto2 = new ClosestOverDimension_OperatorDto_CollectionRecalculationUponReset();
                    break;

                default:
                    throw new ValueNotSupportedException(dto.CollectionRecalculationEnum);
            }

            Clone_ClosestOverDimensionProperties(dto, dto2);

            return dto2;
        }

        protected override OperatorDtoBase Visit_ClosestOverDimensionExp_OperatorDto(ClosestOverDimensionExp_OperatorDto dto)
        {
            base.Visit_ClosestOverDimensionExp_OperatorDto(dto);

            ClosestOverDimensionExp_OperatorDto dto2;

            switch (dto.CollectionRecalculationEnum)
            {
                case CollectionRecalculationEnum.Continuous:
                    dto2 = new ClosestOverDimensionExp_OperatorDto_CollectionRecalculationContinuous();
                    break;

                case CollectionRecalculationEnum.UponReset:
                    dto2 = new ClosestOverDimensionExp_OperatorDto_CollectionRecalculationUponReset();
                    break;

                default:
                    throw new ValueNotSupportedException(dto.CollectionRecalculationEnum);
            }

            Clone_ClosestOverDimensionProperties(dto, dto2);

            return dto2;
        }

        protected override OperatorDtoBase Visit_ClosestOverInlets_OperatorDto(ClosestOverInlets_OperatorDto dto)
        {
            base.Visit_ClosestOverInlets_OperatorDto(dto);

            var mathPropertiesDto = Get_ClosestOverInlets_MathPropertiesDto(dto);

            if (mathPropertiesDto.InputMathPropertiesDto.IsConst)
            {
                return dto.InputOperatorDto;
            }
            if (dto.ItemOperatorDtos.Count == 2 && mathPropertiesDto.AllItemsAreConst)
            {
                double item1 = mathPropertiesDto.ItemsValues[0];
                double item2 = mathPropertiesDto.ItemsValues[1];

                return new ClosestOverInlets_OperatorDto_VarInput_2ConstItems { InputOperatorDto = dto.InputOperatorDto, Item1 = item1, Item2 = item2 };
            }
            else if (mathPropertiesDto.AllItemsAreConst)
            {
                return new ClosestOverInlets_OperatorDto_VarInput_ConstItems { InputOperatorDto = dto.InputOperatorDto, Items = mathPropertiesDto.ItemsValues };
            }
            else
            {
                return new ClosestOverInlets_OperatorDto_AllVars { InputOperatorDto = dto.InputOperatorDto, ItemOperatorDtos = dto.ItemOperatorDtos };
            }
        }

        protected override OperatorDtoBase Visit_ClosestOverInletsExp_OperatorDto(ClosestOverInletsExp_OperatorDto dto)
        {
            base.Visit_ClosestOverInlets_OperatorDto(dto);

            var mathPropertiesDto = Get_ClosestOverInlets_MathPropertiesDto(dto);

            if (mathPropertiesDto.InputMathPropertiesDto.IsConst)
            {
                return dto.InputOperatorDto;
            }
            if (dto.ItemOperatorDtos.Count == 2 && mathPropertiesDto.AllItemsAreConst)
            {
                double item1 = mathPropertiesDto.ItemsValues[0];
                double item2 = mathPropertiesDto.ItemsValues[1];

                return new ClosestOverInletsExp_OperatorDto_VarInput_2ConstItems { InputOperatorDto = dto.InputOperatorDto, Item1 = item1, Item2 = item2 };
            }
            else if (mathPropertiesDto.AllItemsAreConst)
            {
                return new ClosestOverInletsExp_OperatorDto_VarInput_ConstItems { InputOperatorDto = dto.InputOperatorDto, Items = mathPropertiesDto.ItemsValues };
            }
            else
            {
                return new ClosestOverInletsExp_OperatorDto_AllVars { InputOperatorDto = dto.InputOperatorDto, ItemOperatorDtos = dto.ItemOperatorDtos };
            }
        }

        protected override OperatorDtoBase Visit_Curve_OperatorDto(Curve_OperatorDto dto)
        {
            base.Visit_Curve_OperatorDto(dto);

            if (dto.Curve == null)
            {
                return new Number_OperatorDto_Zero();
            }

            Curve_OperatorDto dto2;

            if (dto.MinX == 0.0)
            {
                if (dto.StandardDimensionEnum == DimensionEnum.Time)
                {
                    dto2 = new Curve_OperatorDto_MinXZero_WithOriginShifting();

                }
                else
                {
                    dto2 = new Curve_OperatorDto_MinXZero_NoOriginShifting();
                }
            }
            else
            {
                if (dto.StandardDimensionEnum == DimensionEnum.Time)
                {
                    dto2 = new Curve_OperatorDto_MinX_WithOriginShifting();
                }
                else
                {
                    dto2 = new Curve_OperatorDto_MinX_NoOriginShifting();
                }
            }

            Clone_CurveProperties(dto, dto2);

            return dto2;
        }

        protected override OperatorDtoBase Visit_Divide_OperatorDto(Divide_OperatorDto dto)
        {
            base.Visit_Divide_OperatorDto(dto);

            OperatorDtoBase numeratorOperatorDto = dto.NumeratorOperatorDto;
            OperatorDtoBase denominatorOperatorDto = dto.DenominatorOperatorDto;
            OperatorDtoBase originOperatorDto = dto.OriginOperatorDto;

            MathPropertiesDto numeratorMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(numeratorOperatorDto);
            MathPropertiesDto denominatorMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(denominatorOperatorDto);
            MathPropertiesDto originMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(originOperatorDto);

            if (numeratorMathPropertiesDto.IsVar && denominatorMathPropertiesDto.IsVar && originMathPropertiesDto.IsVar)
            {
                return new Divide_OperatorDto_VarNumerator_VarDenominator_VarOrigin { NumeratorOperatorDto = numeratorOperatorDto, DenominatorOperatorDto = denominatorOperatorDto, OriginOperatorDto = originOperatorDto };
            }
            else if (numeratorMathPropertiesDto.IsVar && denominatorMathPropertiesDto.IsVar && originMathPropertiesDto.IsConstZero)
            {
                return new Divide_OperatorDto_VarNumerator_VarDenominator_ZeroOrigin { NumeratorOperatorDto = numeratorOperatorDto, DenominatorOperatorDto = denominatorOperatorDto };
            }
            else if (numeratorMathPropertiesDto.IsVar && denominatorMathPropertiesDto.IsVar && originMathPropertiesDto.IsConst)
            {
                return new Divide_OperatorDto_VarNumerator_VarDenominator_ConstOrigin { NumeratorOperatorDto = numeratorOperatorDto, DenominatorOperatorDto = denominatorOperatorDto, Origin = originMathPropertiesDto.ConstValue };
            }
            else if (numeratorMathPropertiesDto.IsVar && denominatorMathPropertiesDto.IsConst && originMathPropertiesDto.IsVar)
            {
                return new Divide_OperatorDto_VarNumerator_ConstDenominator_VarOrigin { NumeratorOperatorDto = numeratorOperatorDto, Denominator = denominatorMathPropertiesDto.ConstValue, OriginOperatorDto = originOperatorDto };
            }
            else if (numeratorMathPropertiesDto.IsVar && denominatorMathPropertiesDto.IsConst && originMathPropertiesDto.IsConstZero)
            {
                return new Divide_OperatorDto_VarNumerator_ConstDenominator_ZeroOrigin { NumeratorOperatorDto = numeratorOperatorDto, Denominator = denominatorMathPropertiesDto.ConstValue };
            }
            else if (numeratorMathPropertiesDto.IsVar && denominatorMathPropertiesDto.IsConst && originMathPropertiesDto.IsConst)
            {
                return new Divide_OperatorDto_VarNumerator_ConstDenominator_ConstOrigin { NumeratorOperatorDto = numeratorOperatorDto, Denominator = denominatorMathPropertiesDto.ConstValue, Origin = originMathPropertiesDto.ConstValue };
            }
            else if (numeratorMathPropertiesDto.IsConst && denominatorMathPropertiesDto.IsVar && originMathPropertiesDto.IsVar)
            {
                return new Divide_OperatorDto_ConstNumerator_VarDenominator_VarOrigin { Numerator = numeratorMathPropertiesDto.ConstValue, DenominatorOperatorDto = denominatorOperatorDto, OriginOperatorDto = originOperatorDto };
            }
            else if (numeratorMathPropertiesDto.IsConst && denominatorMathPropertiesDto.IsVar && originMathPropertiesDto.IsConstZero)
            {
                return new Divide_OperatorDto_ConstNumerator_VarDenominator_ZeroOrigin { Numerator = numeratorMathPropertiesDto.ConstValue, DenominatorOperatorDto = denominatorOperatorDto };
            }
            else if (numeratorMathPropertiesDto.IsConst && denominatorMathPropertiesDto.IsVar && originMathPropertiesDto.IsConst)
            {
                return new Divide_OperatorDto_ConstNumerator_VarDenominator_ConstOrigin { Numerator = numeratorMathPropertiesDto.ConstValue, DenominatorOperatorDto = denominatorOperatorDto, Origin = originMathPropertiesDto.ConstValue };
            }
            else if (numeratorMathPropertiesDto.IsConst && denominatorMathPropertiesDto.IsConst && originMathPropertiesDto.IsVar)
            {
                return new Divide_OperatorDto_ConstNumerator_ConstDenominator_VarOrigin { Numerator = numeratorMathPropertiesDto.ConstValue, Denominator = denominatorMathPropertiesDto.ConstValue, OriginOperatorDto = originOperatorDto };
            }
            else if (numeratorMathPropertiesDto.IsConst && denominatorMathPropertiesDto.IsConst && originMathPropertiesDto.IsConstZero)
            {
                return new Divide_OperatorDto_ConstNumerator_ConstDenominator_ZeroOrigin { Numerator = numeratorMathPropertiesDto.ConstValue, Denominator = denominatorMathPropertiesDto.ConstValue };
            }
            else if (numeratorMathPropertiesDto.IsConst && denominatorMathPropertiesDto.IsConst && originMathPropertiesDto.IsConst)
            {
                return new Divide_OperatorDto_ConstNumerator_ConstDenominator_ConstOrigin { Numerator = numeratorMathPropertiesDto.ConstValue, Denominator = denominatorMathPropertiesDto.ConstValue, Origin = originMathPropertiesDto.ConstValue };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_Equal_OperatorDto(Equal_OperatorDto dto)
        {
            base.Visit_Equal_OperatorDto(dto);

            OperatorDtoBase aOperatorDto = dto.AOperatorDto;
            OperatorDtoBase bOperatorDto = dto.BOperatorDto;

            MathPropertiesDto aMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(aOperatorDto);
            MathPropertiesDto bMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bOperatorDto);

            if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsConst)
            {
                return new Equal_OperatorDto_ConstA_ConstB { A = aMathPropertiesDto.ConstValue, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsConst)
            {
                return new Equal_OperatorDto_VarA_ConstB { AOperatorDto = aOperatorDto, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsVar)
            {
                return new Equal_OperatorDto_ConstA_VarB { A = aMathPropertiesDto.ConstValue, BOperatorDto = bOperatorDto };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsVar)
            {
                return new Equal_OperatorDto_VarA_VarB { AOperatorDto = aOperatorDto, BOperatorDto = bOperatorDto };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_Exponent_OperatorDto(Exponent_OperatorDto dto)
        {
            base.Visit_Exponent_OperatorDto(dto);

            OperatorDtoBase lowOperatorDto = dto.LowOperatorDto;
            OperatorDtoBase highOperatorDto = dto.HighOperatorDto;
            OperatorDtoBase ratioOperatorDto = dto.RatioOperatorDto;

            MathPropertiesDto lowMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(lowOperatorDto);
            MathPropertiesDto highMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(highOperatorDto);
            MathPropertiesDto ratioMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(ratioOperatorDto);

            if (lowMathPropertiesDto.IsVar && highMathPropertiesDto.IsVar && ratioMathPropertiesDto.IsVar)
            {
                return new Exponent_OperatorDto_VarLow_VarHigh_VarRatio { LowOperatorDto = lowOperatorDto, HighOperatorDto = highOperatorDto, RatioOperatorDto = ratioOperatorDto };
            }
            else if (lowMathPropertiesDto.IsVar && highMathPropertiesDto.IsVar && ratioMathPropertiesDto.IsConst)
            {
                return new Exponent_OperatorDto_VarLow_VarHigh_ConstRatio { LowOperatorDto = lowOperatorDto, HighOperatorDto = highOperatorDto, Ratio = ratioMathPropertiesDto.ConstValue };
            }
            else if (lowMathPropertiesDto.IsVar && highMathPropertiesDto.IsConst && ratioMathPropertiesDto.IsVar)
            {
                return new Exponent_OperatorDto_VarLow_ConstHigh_VarRatio { LowOperatorDto = lowOperatorDto, High = highMathPropertiesDto.ConstValue, RatioOperatorDto = ratioOperatorDto };
            }
            else if (lowMathPropertiesDto.IsVar && highMathPropertiesDto.IsConst && ratioMathPropertiesDto.IsConst)
            {
                return new Exponent_OperatorDto_VarLow_ConstHigh_ConstRatio { LowOperatorDto = lowOperatorDto, High = highMathPropertiesDto.ConstValue, Ratio = ratioMathPropertiesDto.ConstValue };
            }
            else if (lowMathPropertiesDto.IsConst && highMathPropertiesDto.IsVar && ratioMathPropertiesDto.IsVar)
            {
                return new Exponent_OperatorDto_ConstLow_VarHigh_VarRatio { Low = lowMathPropertiesDto.ConstValue, HighOperatorDto = highOperatorDto, RatioOperatorDto = ratioOperatorDto };
            }
            else if (lowMathPropertiesDto.IsConst && highMathPropertiesDto.IsVar && ratioMathPropertiesDto.IsConst)
            {
                return new Exponent_OperatorDto_ConstLow_VarHigh_ConstRatio { Low = lowMathPropertiesDto.ConstValue, HighOperatorDto = highOperatorDto, Ratio = ratioMathPropertiesDto.ConstValue };
            }
            else if (lowMathPropertiesDto.IsConst && highMathPropertiesDto.IsConst && ratioMathPropertiesDto.IsVar)
            {
                return new Exponent_OperatorDto_ConstLow_ConstHigh_VarRatio { Low = lowMathPropertiesDto.ConstValue, High = highMathPropertiesDto.ConstValue, RatioOperatorDto = ratioOperatorDto };
            }
            else if (lowMathPropertiesDto.IsConst && highMathPropertiesDto.IsConst && ratioMathPropertiesDto.IsConst)
            {
                return new Exponent_OperatorDto_ConstLow_ConstHigh_ConstRatio { Low = lowMathPropertiesDto.ConstValue, High = highMathPropertiesDto.ConstValue, Ratio = ratioMathPropertiesDto.ConstValue };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_GreaterThan_OperatorDto(GreaterThan_OperatorDto dto)
        {
            base.Visit_GreaterThan_OperatorDto(dto);

            OperatorDtoBase aOperatorDto = dto.AOperatorDto;
            OperatorDtoBase bOperatorDto = dto.BOperatorDto;

            MathPropertiesDto aMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(aOperatorDto);
            MathPropertiesDto bMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bOperatorDto);

            if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsConst)
            {
                return new GreaterThan_OperatorDto_ConstA_ConstB { A = aMathPropertiesDto.ConstValue, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsConst)
            {
                return new GreaterThan_OperatorDto_VarA_ConstB { AOperatorDto = aOperatorDto, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsVar)
            {
                return new GreaterThan_OperatorDto_ConstA_VarB { A = aMathPropertiesDto.ConstValue, BOperatorDto = bOperatorDto };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsVar)
            {
                return new GreaterThan_OperatorDto_VarA_VarB { AOperatorDto = aOperatorDto, BOperatorDto = bOperatorDto };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_GreaterThanOrEqual_OperatorDto(GreaterThanOrEqual_OperatorDto dto)
        {
            base.Visit_GreaterThanOrEqual_OperatorDto(dto);

            OperatorDtoBase aOperatorDto = dto.AOperatorDto;
            OperatorDtoBase bOperatorDto = dto.BOperatorDto;

            MathPropertiesDto aMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(aOperatorDto);
            MathPropertiesDto bMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bOperatorDto);

            if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsConst)
            {
                return new GreaterThanOrEqual_OperatorDto_ConstA_ConstB { A = aMathPropertiesDto.ConstValue, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsConst)
            {
                return new GreaterThanOrEqual_OperatorDto_VarA_ConstB { AOperatorDto = aOperatorDto, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsVar)
            {
                return new GreaterThanOrEqual_OperatorDto_ConstA_VarB { A = aMathPropertiesDto.ConstValue, BOperatorDto = bOperatorDto };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsVar)
            {
                return new GreaterThanOrEqual_OperatorDto_VarA_VarB { AOperatorDto = aOperatorDto, BOperatorDto = bOperatorDto };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_HighPassFilter_OperatorDto(HighPassFilter_OperatorDto dto)
        {
            base.Visit_HighPassFilter_OperatorDto(dto);

            OperatorDtoBase signalOperatorDto = dto.SignalOperatorDto;
            OperatorDtoBase minFrequencyOperatorDto = dto.MinFrequencyOperatorDto;
            OperatorDtoBase bandWidthOperatorDto = dto.BandWidthOperatorDto;

            MathPropertiesDto signalMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(signalOperatorDto);
            MathPropertiesDto minFrequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(minFrequencyOperatorDto);
            MathPropertiesDto bandWidthMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bandWidthOperatorDto);

            if (signalMathPropertiesDto.IsConst)
            {
                return signalOperatorDto;
            }
            else if (minFrequencyMathPropertiesDto.IsConst && bandWidthMathPropertiesDto.IsConst)
            {
                return new HighPassFilter_OperatorDto_ManyConsts { SignalOperatorDto = signalOperatorDto, MinFrequency = minFrequencyMathPropertiesDto.ConstValue, BandWidth = bandWidthMathPropertiesDto.ConstValue };
            }
            else
            {
                return new HighPassFilter_OperatorDto_AllVars { SignalOperatorDto = signalOperatorDto, MinFrequencyOperatorDto = minFrequencyOperatorDto, BandWidthOperatorDto = bandWidthOperatorDto };
            }
        }

        protected override OperatorDtoBase Visit_HighShelfFilter_OperatorDto(HighShelfFilter_OperatorDto dto)
        {
            base.Visit_HighShelfFilter_OperatorDto(dto);

            OperatorDtoBase signalOperatorDto = dto.SignalOperatorDto;
            OperatorDtoBase transitionFrequencyOperatorDto = dto.TransitionFrequencyOperatorDto;
            OperatorDtoBase transitionSlopeOperatorDto = dto.TransitionSlopeOperatorDto;
            OperatorDtoBase dbGainOperatorDto = dto.DBGainOperatorDto;

            MathPropertiesDto signalMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(signalOperatorDto);
            MathPropertiesDto transitionFrequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(transitionFrequencyOperatorDto);
            MathPropertiesDto transitionSlopeMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(transitionSlopeOperatorDto);
            MathPropertiesDto dbGainMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(dbGainOperatorDto);

            if (signalMathPropertiesDto.IsConst)
            {
                return signalOperatorDto;
            }
            else if (transitionFrequencyMathPropertiesDto.IsConst && transitionSlopeMathPropertiesDto.IsConst & dbGainMathPropertiesDto.IsConst)
            {
                return new HighShelfFilter_OperatorDto_ManyConsts { SignalOperatorDto = signalOperatorDto, TransitionFrequency = transitionFrequencyMathPropertiesDto.ConstValue, TransitionSlope = transitionSlopeMathPropertiesDto.ConstValue, DBGain = dbGainMathPropertiesDto.ConstValue };
            }
            else
            {
                return new HighShelfFilter_OperatorDto_AllVars { SignalOperatorDto = signalOperatorDto, TransitionFrequencyOperatorDto = transitionFrequencyOperatorDto, TransitionSlopeOperatorDto = transitionSlopeOperatorDto, DBGainOperatorDto = dbGainOperatorDto };
            }
        }

        protected override OperatorDtoBase Visit_If_OperatorDto(If_OperatorDto dto)
        {
            base.Visit_If_OperatorDto(dto);

            OperatorDtoBase conditionOperatorDto = dto.ConditionOperatorDto;
            OperatorDtoBase thenOperatorDto = dto.ThenOperatorDto;
            OperatorDtoBase elseOperatorDto = dto.ElseOperatorDto;

            MathPropertiesDto conditionMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(conditionOperatorDto);
            MathPropertiesDto thenMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(thenOperatorDto);
            MathPropertiesDto elseMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(elseOperatorDto);

            if (conditionMathPropertiesDto.IsVar && thenMathPropertiesDto.IsVar && elseMathPropertiesDto.IsVar)
            {
                return new If_OperatorDto_VarCondition_VarThen_VarElse { ConditionOperatorDto = conditionOperatorDto, ThenOperatorDto = thenOperatorDto, ElseOperatorDto = elseOperatorDto };
            }
            else if (conditionMathPropertiesDto.IsVar && thenMathPropertiesDto.IsVar && elseMathPropertiesDto.IsConst)
            {
                return new If_OperatorDto_VarCondition_VarThen_ConstElse { ConditionOperatorDto = conditionOperatorDto, ThenOperatorDto = thenOperatorDto, Else = elseMathPropertiesDto.ConstValue };
            }
            else if (conditionMathPropertiesDto.IsVar && thenMathPropertiesDto.IsConst && elseMathPropertiesDto.IsVar)
            {
                return new If_OperatorDto_VarCondition_ConstThen_VarElse { ConditionOperatorDto = conditionOperatorDto, Then = thenMathPropertiesDto.ConstValue, ElseOperatorDto = elseOperatorDto };
            }
            else if (conditionMathPropertiesDto.IsVar && thenMathPropertiesDto.IsConst && elseMathPropertiesDto.IsConst)
            {
                return new If_OperatorDto_VarCondition_ConstThen_ConstElse { ConditionOperatorDto = conditionOperatorDto, Then = thenMathPropertiesDto.ConstValue, Else = elseMathPropertiesDto.ConstValue };
            }
            else if (conditionMathPropertiesDto.IsConst && thenMathPropertiesDto.IsVar && elseMathPropertiesDto.IsVar)
            {
                return new If_OperatorDto_ConstCondition_VarThen_VarElse { Condition = conditionMathPropertiesDto.ConstValue, ThenOperatorDto = thenOperatorDto, ElseOperatorDto = elseOperatorDto };
            }
            else if (conditionMathPropertiesDto.IsConst && thenMathPropertiesDto.IsVar && elseMathPropertiesDto.IsConst)
            {
                return new If_OperatorDto_ConstCondition_VarThen_ConstElse { Condition = conditionMathPropertiesDto.ConstValue, ThenOperatorDto = thenOperatorDto, Else = elseMathPropertiesDto.ConstValue };
            }
            else if (conditionMathPropertiesDto.IsConst && thenMathPropertiesDto.IsConst && elseMathPropertiesDto.IsVar)
            {
                return new If_OperatorDto_ConstCondition_ConstThen_VarElse { Condition = conditionMathPropertiesDto.ConstValue, Then = thenMathPropertiesDto.ConstValue, ElseOperatorDto = elseOperatorDto };
            }
            else if (conditionMathPropertiesDto.IsConst && thenMathPropertiesDto.IsConst && elseMathPropertiesDto.IsConst)
            {
                return new If_OperatorDto_ConstCondition_ConstThen_ConstElse { Condition = conditionMathPropertiesDto.ConstValue, Then = thenMathPropertiesDto.ConstValue, Else = elseMathPropertiesDto.ConstValue };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_Interpolate_OperatorDto(Interpolate_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_LessThan_OperatorDto(LessThan_OperatorDto dto)
        {
            base.Visit_LessThan_OperatorDto(dto);

            OperatorDtoBase aOperatorDto = dto.AOperatorDto;
            OperatorDtoBase bOperatorDto = dto.BOperatorDto;

            MathPropertiesDto aMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(aOperatorDto);
            MathPropertiesDto bMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bOperatorDto);

            if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsConst)
            {
                return new LessThan_OperatorDto_ConstA_ConstB { A = aMathPropertiesDto.ConstValue, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsConst)
            {
                return new LessThan_OperatorDto_VarA_ConstB { AOperatorDto = aOperatorDto, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsVar)
            {
                return new LessThan_OperatorDto_ConstA_VarB { A = aMathPropertiesDto.ConstValue, BOperatorDto = bOperatorDto };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsVar)
            {
                return new LessThan_OperatorDto_VarA_VarB { AOperatorDto = aOperatorDto, BOperatorDto = bOperatorDto };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_LessThanOrEqual_OperatorDto(LessThanOrEqual_OperatorDto dto)
        {
            base.Visit_LessThanOrEqual_OperatorDto(dto);

            OperatorDtoBase aOperatorDto = dto.AOperatorDto;
            OperatorDtoBase bOperatorDto = dto.BOperatorDto;

            MathPropertiesDto aMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(aOperatorDto);
            MathPropertiesDto bMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bOperatorDto);

            if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsConst)
            {
                return new LessThanOrEqual_OperatorDto_ConstA_ConstB { A = aMathPropertiesDto.ConstValue, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsConst)
            {
                return new LessThanOrEqual_OperatorDto_VarA_ConstB { AOperatorDto = aOperatorDto, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsVar)
            {
                return new LessThanOrEqual_OperatorDto_ConstA_VarB { A = aMathPropertiesDto.ConstValue, BOperatorDto = bOperatorDto };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsVar)
            {
                return new LessThanOrEqual_OperatorDto_VarA_VarB { AOperatorDto = aOperatorDto, BOperatorDto = bOperatorDto };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_Loop_OperatorDto(Loop_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_LowPassFilter_OperatorDto(LowPassFilter_OperatorDto dto)
        {
            base.Visit_LowPassFilter_OperatorDto(dto);

            OperatorDtoBase signalOperatorDto = dto.SignalOperatorDto;
            OperatorDtoBase maxFrequencyOperatorDto = dto.MaxFrequencyOperatorDto;
            OperatorDtoBase bandWidthOperatorDto = dto.BandWidthOperatorDto;

            MathPropertiesDto signalMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(signalOperatorDto);
            MathPropertiesDto maxFrequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(maxFrequencyOperatorDto);
            MathPropertiesDto bandWidthMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bandWidthOperatorDto);

            if (signalMathPropertiesDto.IsConst)
            {
                return signalOperatorDto;
            }
            else if (maxFrequencyMathPropertiesDto.IsConst && bandWidthMathPropertiesDto.IsConst)
            {
                return new LowPassFilter_OperatorDto_ManyConsts { SignalOperatorDto = signalOperatorDto, MaxFrequency = maxFrequencyMathPropertiesDto.ConstValue, BandWidth = bandWidthMathPropertiesDto.ConstValue };
            }
            else
            {
                return new LowPassFilter_OperatorDto_AllVars { SignalOperatorDto = signalOperatorDto, MaxFrequencyOperatorDto = maxFrequencyOperatorDto, BandWidthOperatorDto = bandWidthOperatorDto };
            }
        }

        protected override OperatorDtoBase Visit_LowShelfFilter_OperatorDto(LowShelfFilter_OperatorDto dto)
        {
            base.Visit_LowShelfFilter_OperatorDto(dto);

            OperatorDtoBase signalOperatorDto = dto.SignalOperatorDto;
            OperatorDtoBase transitionFrequencyOperatorDto = dto.TransitionFrequencyOperatorDto;
            OperatorDtoBase transitionSlopeOperatorDto = dto.TransitionSlopeOperatorDto;
            OperatorDtoBase dbGainOperatorDto = dto.DBGainOperatorDto;

            MathPropertiesDto signalMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(signalOperatorDto);
            MathPropertiesDto transitionFrequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(transitionFrequencyOperatorDto);
            MathPropertiesDto transitionSlopeMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(transitionSlopeOperatorDto);
            MathPropertiesDto dbGainMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(dbGainOperatorDto);

            if (signalMathPropertiesDto.IsConst)
            {
                return signalOperatorDto;
            }
            else if (transitionFrequencyMathPropertiesDto.IsConst && transitionSlopeMathPropertiesDto.IsConst & dbGainMathPropertiesDto.IsConst)
            {
                return new LowShelfFilter_OperatorDto_ManyConsts { SignalOperatorDto = signalOperatorDto, TransitionFrequency = transitionFrequencyMathPropertiesDto.ConstValue, TransitionSlope = transitionSlopeMathPropertiesDto.ConstValue, DBGain = dbGainMathPropertiesDto.ConstValue };
            }
            else
            {
                return new LowShelfFilter_OperatorDto_AllVars { SignalOperatorDto = signalOperatorDto, TransitionFrequencyOperatorDto = transitionFrequencyOperatorDto, TransitionSlopeOperatorDto = transitionSlopeOperatorDto, DBGainOperatorDto = dbGainOperatorDto };
            }
        }

        protected override OperatorDtoBase Visit_MaxOverDimension_OperatorDto(MaxOverDimension_OperatorDto dto)
        {
            base.Visit_MaxOverDimension_OperatorDto(dto);

            MaxOverDimension_OperatorDto dto2;

            switch (dto.CollectionRecalculationEnum)
            {
                case CollectionRecalculationEnum.Continuous:
                    dto2 = new MaxOverDimension_OperatorDto_CollectionRecalculationContinuous();
                    break;

                case CollectionRecalculationEnum.UponReset:
                    dto2 = new MaxOverDimension_OperatorDto_CollectionRecalculationUponReset();
                    break;

                default:
                    throw new ValueNotSupportedException(dto.CollectionRecalculationEnum);
            }

            Clone_AggregateOverDimensionProperties(dto, dto2);

            return dto2;
        }

        protected override OperatorDtoBase Visit_MaxOverInlets_OperatorDto(MaxOverInlets_OperatorDto dto)
        {
            base.Visit_MaxOverInlets_OperatorDto(dto);

            IList<OperatorDtoBase> operatorDtos = dto.InputOperatorDtos;

            IList<OperatorDtoBase> constOperatorDtos = operatorDtos.Where(x => MathPropertiesHelper.GetMathPropertiesDto(x).IsConst).ToArray();
            IList<OperatorDtoBase> varOperatorDtos = operatorDtos.Except(constOperatorDtos).ToArray();
            IList<double> consts = constOperatorDtos.Select(x => MathPropertiesHelper.GetMathPropertiesDto(x).ConstValue).ToArray();

            bool hasVars = varOperatorDtos.Any();
            bool hasConsts = constOperatorDtos.Any();

            if (hasVars && hasConsts)
            {
                return new MaxOverInlets_OperatorDto_Vars_Consts { Vars = varOperatorDtos, Consts = consts };
            }
            else if (hasVars && !hasConsts)
            {
                return new MaxOverInlets_OperatorDto_Vars_NoConsts { Vars = varOperatorDtos };
            }
            else if (!hasVars && hasConsts)
            {
                return new MaxOverInlets_OperatorDto_NoVars_Consts { Consts = consts };
            }
            else if (!hasVars && !hasConsts)
            {
                return new MaxOverInlets_OperatorDto_NoVars_NoConsts();
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_MinOverDimension_OperatorDto(MinOverDimension_OperatorDto dto)
        {
            base.Visit_MinOverDimension_OperatorDto(dto);

            MinOverDimension_OperatorDto dto2;

            switch (dto.CollectionRecalculationEnum)
            {
                case CollectionRecalculationEnum.Continuous:
                    dto2 = new MinOverDimension_OperatorDto_CollectionRecalculationContinuous();
                    break;

                case CollectionRecalculationEnum.UponReset:
                    dto2 = new MinOverDimension_OperatorDto_CollectionRecalculationUponReset();
                    break;

                default:
                    throw new ValueNotSupportedException(dto.CollectionRecalculationEnum);
            }

            Clone_AggregateOverDimensionProperties(dto, dto2);

            return dto2;
        }

        protected override OperatorDtoBase Visit_MinOverInlets_OperatorDto(MinOverInlets_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_Multiply_OperatorDto(Multiply_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_MultiplyWithOrigin_OperatorDto(MultiplyWithOrigin_OperatorDto dto)
        {
            base.Visit_MultiplyWithOrigin_OperatorDto(dto);

            OperatorDtoBase numeratorOperatorDto = dto.AOperatorDto;
            OperatorDtoBase denominatorOperatorDto = dto.BOperatorDto;
            OperatorDtoBase originOperatorDto = dto.OriginOperatorDto;

            MathPropertiesDto numeratorMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(numeratorOperatorDto);
            MathPropertiesDto denominatorMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(denominatorOperatorDto);
            MathPropertiesDto originMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(originOperatorDto);

            if (numeratorMathPropertiesDto.IsVar && denominatorMathPropertiesDto.IsVar && originMathPropertiesDto.IsVar)
            {
                return new MultiplyWithOrigin_OperatorDto_VarA_VarB_VarOrigin { AOperatorDto = numeratorOperatorDto, BOperatorDto = denominatorOperatorDto, OriginOperatorDto = originOperatorDto };
            }
            else if (numeratorMathPropertiesDto.IsVar && denominatorMathPropertiesDto.IsVar && originMathPropertiesDto.IsConstZero)
            {
                return new MultiplyWithOrigin_OperatorDto_VarA_VarB_ZeroOrigin { AOperatorDto = numeratorOperatorDto, BOperatorDto = denominatorOperatorDto };
            }
            else if (numeratorMathPropertiesDto.IsVar && denominatorMathPropertiesDto.IsVar && originMathPropertiesDto.IsConst)
            {
                return new MultiplyWithOrigin_OperatorDto_VarA_VarB_ConstOrigin { AOperatorDto = numeratorOperatorDto, BOperatorDto = denominatorOperatorDto, Origin = originMathPropertiesDto.ConstValue };
            }
            else if (numeratorMathPropertiesDto.IsVar && denominatorMathPropertiesDto.IsConst && originMathPropertiesDto.IsVar)
            {
                return new MultiplyWithOrigin_OperatorDto_VarA_ConstB_VarOrigin { AOperatorDto = numeratorOperatorDto, B = denominatorMathPropertiesDto.ConstValue, OriginOperatorDto = originOperatorDto };
            }
            else if (numeratorMathPropertiesDto.IsVar && denominatorMathPropertiesDto.IsConst && originMathPropertiesDto.IsConstZero)
            {
                return new MultiplyWithOrigin_OperatorDto_VarA_ConstB_ZeroOrigin { AOperatorDto = numeratorOperatorDto, B = denominatorMathPropertiesDto.ConstValue };
            }
            else if (numeratorMathPropertiesDto.IsVar && denominatorMathPropertiesDto.IsConst && originMathPropertiesDto.IsConst)
            {
                return new MultiplyWithOrigin_OperatorDto_VarA_ConstB_ConstOrigin { AOperatorDto = numeratorOperatorDto, B = denominatorMathPropertiesDto.ConstValue, Origin = originMathPropertiesDto.ConstValue };
            }
            else if (numeratorMathPropertiesDto.IsConst && denominatorMathPropertiesDto.IsVar && originMathPropertiesDto.IsVar)
            {
                return new MultiplyWithOrigin_OperatorDto_ConstA_VarB_VarOrigin { A = numeratorMathPropertiesDto.ConstValue, BOperatorDto = denominatorOperatorDto, OriginOperatorDto = originOperatorDto };
            }
            else if (numeratorMathPropertiesDto.IsConst && denominatorMathPropertiesDto.IsVar && originMathPropertiesDto.IsConstZero)
            {
                return new MultiplyWithOrigin_OperatorDto_ConstA_VarB_ZeroOrigin { A = numeratorMathPropertiesDto.ConstValue, BOperatorDto = denominatorOperatorDto };
            }
            else if (numeratorMathPropertiesDto.IsConst && denominatorMathPropertiesDto.IsVar && originMathPropertiesDto.IsConst)
            {
                return new MultiplyWithOrigin_OperatorDto_ConstA_VarB_ConstOrigin { A = numeratorMathPropertiesDto.ConstValue, BOperatorDto = denominatorOperatorDto, Origin = originMathPropertiesDto.ConstValue };
            }
            else if (numeratorMathPropertiesDto.IsConst && denominatorMathPropertiesDto.IsConst && originMathPropertiesDto.IsVar)
            {
                return new MultiplyWithOrigin_OperatorDto_ConstA_ConstB_VarOrigin { A = numeratorMathPropertiesDto.ConstValue, B = denominatorMathPropertiesDto.ConstValue, OriginOperatorDto = originOperatorDto };
            }
            else if (numeratorMathPropertiesDto.IsConst && denominatorMathPropertiesDto.IsConst && originMathPropertiesDto.IsConstZero)
            {
                return new MultiplyWithOrigin_OperatorDto_ConstA_ConstB_ZeroOrigin { A = numeratorMathPropertiesDto.ConstValue, B = denominatorMathPropertiesDto.ConstValue };
            }
            else if (numeratorMathPropertiesDto.IsConst && denominatorMathPropertiesDto.IsConst && originMathPropertiesDto.IsConst)
            {
                return new MultiplyWithOrigin_OperatorDto_ConstA_ConstB_ConstOrigin { A = numeratorMathPropertiesDto.ConstValue, B = denominatorMathPropertiesDto.ConstValue, Origin = originMathPropertiesDto.ConstValue };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_Negative_OperatorDto(Negative_OperatorDto dto)
        {
            base.Visit_Negative_OperatorDto(dto);

            OperatorDtoBase xOperatorDto = dto.XOperatorDto;

            MathPropertiesDto xMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(xOperatorDto);

            if (xMathPropertiesDto.IsConst)
            {
                return new Negative_OperatorDto_ConstX { X = xMathPropertiesDto.ConstValue };
            }
            else
            {
                return new Negative_OperatorDto_VarX { XOperatorDto = xOperatorDto };
            }
        }

        protected override OperatorDtoBase Visit_Not_OperatorDto(Not_OperatorDto dto)
        {
            base.Visit_Not_OperatorDto(dto);

            OperatorDtoBase xOperatorDto = dto.XOperatorDto;

            MathPropertiesDto xMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(xOperatorDto);

            if (xMathPropertiesDto.IsConst)
            {
                return new Not_OperatorDto_ConstX { X = xMathPropertiesDto.ConstValue };
            }
            else
            {
                return new Not_OperatorDto_VarX { XOperatorDto = xOperatorDto };
            }
        }

        protected override OperatorDtoBase Visit_NotchFilter_OperatorDto(NotchFilter_OperatorDto dto)
        {
            base.Visit_NotchFilter_OperatorDto(dto);

            OperatorDtoBase signalOperatorDto = dto.SignalOperatorDto;
            OperatorDtoBase centerFrequencyOperatorDto = dto.CenterFrequencyOperatorDto;
            OperatorDtoBase bandWidthOperatorDto = dto.BandWidthOperatorDto;

            MathPropertiesDto signalMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(signalOperatorDto);
            MathPropertiesDto centerFrequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(centerFrequencyOperatorDto);
            MathPropertiesDto bandWidthMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bandWidthOperatorDto);

            if (signalMathPropertiesDto.IsConst)
            {
                return signalOperatorDto;
            }
            else if (centerFrequencyMathPropertiesDto.IsConst && bandWidthMathPropertiesDto.IsConst)
            {
                return new NotchFilter_OperatorDto_ManyConsts { SignalOperatorDto = signalOperatorDto, CenterFrequency = centerFrequencyMathPropertiesDto.ConstValue, BandWidth = bandWidthMathPropertiesDto.ConstValue };
            }
            else
            {
                return new NotchFilter_OperatorDto_AllVars { SignalOperatorDto = signalOperatorDto, CenterFrequencyOperatorDto = centerFrequencyOperatorDto, BandWidthOperatorDto = bandWidthOperatorDto };
            }
        }

        protected override OperatorDtoBase Visit_NotEqual_OperatorDto(NotEqual_OperatorDto dto)
        {
            base.Visit_NotEqual_OperatorDto(dto);

            OperatorDtoBase aOperatorDto = dto.AOperatorDto;
            OperatorDtoBase bOperatorDto = dto.BOperatorDto;

            MathPropertiesDto aMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(aOperatorDto);
            MathPropertiesDto bMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bOperatorDto);

            if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsConst)
            {
                return new NotEqual_OperatorDto_ConstA_ConstB { A = aMathPropertiesDto.ConstValue, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsConst)
            {
                return new NotEqual_OperatorDto_VarA_ConstB { AOperatorDto = aOperatorDto, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsVar)
            {
                return new NotEqual_OperatorDto_ConstA_VarB { A = aMathPropertiesDto.ConstValue, BOperatorDto = bOperatorDto };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsVar)
            {
                return new NotEqual_OperatorDto_VarA_VarB { AOperatorDto = aOperatorDto, BOperatorDto = bOperatorDto };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_OneOverX_OperatorDto(OneOverX_OperatorDto dto)
        {
            base.Visit_OneOverX_OperatorDto(dto);

            OperatorDtoBase xOperatorDto = dto.XOperatorDto;

            MathPropertiesDto xMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(xOperatorDto);

            if (xMathPropertiesDto.IsConst)
            {
                return new OneOverX_OperatorDto_ConstX { X = xMathPropertiesDto.ConstValue };
            }
            else
            {
                return new OneOverX_OperatorDto_VarX { XOperatorDto = xOperatorDto };
            }
        }

        protected override OperatorDtoBase Visit_Or_OperatorDto(Or_OperatorDto dto)
        {
            base.Visit_Or_OperatorDto(dto);

            OperatorDtoBase aOperatorDto = dto.AOperatorDto;
            OperatorDtoBase bOperatorDto = dto.BOperatorDto;

            MathPropertiesDto aMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(aOperatorDto);
            MathPropertiesDto bMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bOperatorDto);

            if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsConst)
            {
                return new Or_OperatorDto_ConstA_ConstB { A = aMathPropertiesDto.ConstValue, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsConst)
            {
                return new Or_OperatorDto_VarA_ConstB { AOperatorDto = aOperatorDto, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsVar)
            {
                return new Or_OperatorDto_ConstA_VarB { A = aMathPropertiesDto.ConstValue, BOperatorDto = bOperatorDto };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsVar)
            {
                return new Or_OperatorDto_VarA_VarB { AOperatorDto = aOperatorDto, BOperatorDto = bOperatorDto };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_PeakingEQFilter_OperatorDto(PeakingEQFilter_OperatorDto dto)
        {
            base.Visit_PeakingEQFilter_OperatorDto(dto);

            OperatorDtoBase signalOperatorDto = dto.SignalOperatorDto;
            OperatorDtoBase centerFrequencyOperatorDto = dto.CenterFrequencyOperatorDto;
            OperatorDtoBase bandWidthOperatorDto = dto.BandWidthOperatorDto;
            OperatorDtoBase dbGainOperatorDto = dto.DBGainOperatorDto;

            MathPropertiesDto signalMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(signalOperatorDto);
            MathPropertiesDto centerFrequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(centerFrequencyOperatorDto);
            MathPropertiesDto bandWidthMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bandWidthOperatorDto);
            MathPropertiesDto dbGainMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(dbGainOperatorDto);

            if (signalMathPropertiesDto.IsConst)
            {
                return signalOperatorDto;
            }
            else if (centerFrequencyMathPropertiesDto.IsConst && bandWidthMathPropertiesDto.IsConst & dbGainMathPropertiesDto.IsConst)
            {
                return new PeakingEQFilter_OperatorDto_ManyConsts { SignalOperatorDto = signalOperatorDto, CenterFrequency = centerFrequencyMathPropertiesDto.ConstValue, BandWidth = bandWidthMathPropertiesDto.ConstValue, DBGain = dbGainMathPropertiesDto.ConstValue };
            }
            else
            {
                return new PeakingEQFilter_OperatorDto_AllVars { SignalOperatorDto = signalOperatorDto, CenterFrequencyOperatorDto = centerFrequencyOperatorDto, BandWidthOperatorDto = bandWidthOperatorDto, DBGainOperatorDto = dbGainOperatorDto };
            }
        }

        protected override OperatorDtoBase Visit_Power_OperatorDto(Power_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_Pulse_OperatorDto(Pulse_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_Random_OperatorDto(Random_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_RangeOverDimension_OperatorCalculator_OnlyConsts(RangeOverDimension_OperatorCalculator_OnlyConsts dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_RangeOverOutlets_OperatorDto(RangeOverOutlets_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_Reverse_OperatorDto(Reverse_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_Round_OperatorDto(Round_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_Sample_OperatorDto(Sample_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_SawDown_OperatorDto(SawDown_OperatorDto dto)
        {
            base.Visit_SawDown_OperatorDto(dto);

            OperatorDtoBase frequencyOperatorDto = dto.FrequencyOperatorDto;
            MathPropertiesDto frequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(dto);

            if (frequencyMathPropertiesDto.IsVar && dto.StandardDimensionEnum == DimensionEnum.Time)
            {
                return new SawDown_OperatorDto_VarFrequency_WithPhaseTracking { FrequencyOperatorDto = frequencyOperatorDto };
            }
            else if (frequencyMathPropertiesDto.IsVar && dto.StandardDimensionEnum != DimensionEnum.Time)
            {
                return new SawDown_OperatorDto_VarFrequency_NoPhaseTracking { FrequencyOperatorDto = frequencyOperatorDto };
            }
            else if (frequencyMathPropertiesDto.IsConst && dto.StandardDimensionEnum == DimensionEnum.Time)
            {
                return new SawDown_OperatorDto_ConstFrequency_WithOriginShifting { Frequency = frequencyMathPropertiesDto.ConstValue };
            }
            else if (frequencyMathPropertiesDto.IsConst && dto.StandardDimensionEnum != DimensionEnum.Time)
            {
                return new SawDown_OperatorDto_ConstFrequency_NoOriginShifting { Frequency = frequencyMathPropertiesDto.ConstValue };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_SawUp_OperatorDto(SawUp_OperatorDto dto)
        {
            base.Visit_SawUp_OperatorDto(dto);

            OperatorDtoBase frequencyOperatorDto = dto.FrequencyOperatorDto;
            MathPropertiesDto frequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(dto);

            if (frequencyMathPropertiesDto.IsVar && dto.StandardDimensionEnum == DimensionEnum.Time)
            {
                return new SawUp_OperatorDto_VarFrequency_WithPhaseTracking { FrequencyOperatorDto = frequencyOperatorDto };
            }
            else if (frequencyMathPropertiesDto.IsVar && dto.StandardDimensionEnum != DimensionEnum.Time)
            {
                return new SawUp_OperatorDto_VarFrequency_NoPhaseTracking { FrequencyOperatorDto = frequencyOperatorDto };
            }
            else if (frequencyMathPropertiesDto.IsConst && dto.StandardDimensionEnum == DimensionEnum.Time)
            {
                return new SawUp_OperatorDto_ConstFrequency_WithOriginShifting { Frequency = frequencyMathPropertiesDto.ConstValue };
            }
            else if (frequencyMathPropertiesDto.IsConst && dto.StandardDimensionEnum != DimensionEnum.Time)
            {
                return new SawUp_OperatorDto_ConstFrequency_NoOriginShifting { Frequency = frequencyMathPropertiesDto.ConstValue };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_Scaler_OperatorDto(Scaler_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_Select_OperatorDto(Select_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_SetDimension_OperatorDto(SetDimension_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_Shift_OperatorDto(Shift_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_Sine_OperatorDto(Sine_OperatorDto dto)
        {
            base.Visit_Sine_OperatorDto(dto);

            OperatorDtoBase frequencyOperatorDto = dto.FrequencyOperatorDto;
            MathPropertiesDto frequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(dto);

            if (frequencyMathPropertiesDto.IsVar && dto.StandardDimensionEnum == DimensionEnum.Time)
            {
                return new Sine_OperatorDto_VarFrequency_WithPhaseTracking { FrequencyOperatorDto = frequencyOperatorDto };
            }
            else if (frequencyMathPropertiesDto.IsVar && dto.StandardDimensionEnum != DimensionEnum.Time)
            {
                return new Sine_OperatorDto_VarFrequency_NoPhaseTracking { FrequencyOperatorDto = frequencyOperatorDto };
            }
            else if (frequencyMathPropertiesDto.IsConst && dto.StandardDimensionEnum == DimensionEnum.Time)
            {
                return new Sine_OperatorDto_ConstFrequency_WithOriginShifting { Frequency = frequencyMathPropertiesDto.ConstValue };
            }
            else if (frequencyMathPropertiesDto.IsConst && dto.StandardDimensionEnum != DimensionEnum.Time)
            {
                return new Sine_OperatorDto_ConstFrequency_NoOriginShifting { Frequency = frequencyMathPropertiesDto.ConstValue };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_SortOverDimension_OperatorDto(SortOverDimension_OperatorDto dto)
        {
            base.Visit_SortOverDimension_OperatorDto(dto);

            SortOverDimension_OperatorDto dto2;

            switch (dto.CollectionRecalculationEnum)
            {
                case CollectionRecalculationEnum.Continuous:
                    dto2 = new SortOverDimension_OperatorDto_CollectionRecalculationContinuous();
                    break;

                case CollectionRecalculationEnum.UponReset:
                    dto2 = new SortOverDimension_OperatorDto_CollectionRecalculationUponReset();
                    break;

                default:
                    throw new ValueNotSupportedException(dto.CollectionRecalculationEnum);
            }

            Clone_AggregateOverDimensionProperties(dto, dto2);

            return dto2;
        }

        protected override OperatorDtoBase Visit_Square_OperatorDto(Square_OperatorDto dto)
        {
            base.Visit_Square_OperatorDto(dto);

            OperatorDtoBase frequencyOperatorDto = dto.FrequencyOperatorDto;
            MathPropertiesDto frequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(dto);

            if (frequencyMathPropertiesDto.IsVar && dto.StandardDimensionEnum == DimensionEnum.Time)
            {
                return new Square_OperatorDto_VarFrequency_WithPhaseTracking { FrequencyOperatorDto = frequencyOperatorDto };
            }
            else if (frequencyMathPropertiesDto.IsVar && dto.StandardDimensionEnum != DimensionEnum.Time)
            {
                return new Square_OperatorDto_VarFrequency_NoPhaseTracking { FrequencyOperatorDto = frequencyOperatorDto };
            }
            else if (frequencyMathPropertiesDto.IsConst && dto.StandardDimensionEnum == DimensionEnum.Time)
            {
                return new Square_OperatorDto_ConstFrequency_WithOriginShifting { Frequency = frequencyMathPropertiesDto.ConstValue };
            }
            else if (frequencyMathPropertiesDto.IsConst && dto.StandardDimensionEnum != DimensionEnum.Time)
            {
                return new Square_OperatorDto_ConstFrequency_NoOriginShifting { Frequency = frequencyMathPropertiesDto.ConstValue };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_Squash_OperatorDto(Squash_OperatorDto dto)
        {
            base.Visit_Squash_OperatorDto(dto);

            OperatorDtoBase signalOperatorDto = dto.SignalOperatorDto;
            OperatorDtoBase factorOperatorDto = dto.FactorOperatorDto;
            OperatorDtoBase originOperatorDto = dto.OriginOperatorDto;

            MathPropertiesDto signalMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(signalOperatorDto);
            MathPropertiesDto factorMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(factorOperatorDto);
            MathPropertiesDto originMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(originOperatorDto);

            if (dto.StandardDimensionEnum == DimensionEnum.Time)
            {
                if (signalMathPropertiesDto.IsVar && factorMathPropertiesDto.IsVar)
                {
                    return new Squash_OperatorDto_VarSignal_VarFactor_WithPhaseTracking { SignalOperatorDto = signalOperatorDto, FactorOperatorDto = factorOperatorDto };
                }
                else if (signalMathPropertiesDto.IsVar && factorMathPropertiesDto.IsConst)
                {
                    return new Squash_OperatorDto_VarSignal_ConstFactor_WithOriginShifting { SignalOperatorDto = signalOperatorDto, Factor = factorMathPropertiesDto.ConstValue };
                }
                else if (signalMathPropertiesDto.IsConst && factorMathPropertiesDto.IsVar)
                {
                    return new Squash_OperatorDto_ConstSignal_VarFactor_WithPhaseTracking { Signal = signalMathPropertiesDto.ConstValue, FactorOperatorDto = factorOperatorDto };
                }
                else if (signalMathPropertiesDto.IsConst && factorMathPropertiesDto.IsConst)
                {
                    return new Squash_OperatorDto_ConstSignal_ConstFactor_WithOriginShifting { Signal = signalMathPropertiesDto.ConstValue, Factor = factorMathPropertiesDto.ConstValue };
                }
                else
                {
                    throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
                }
            }
            else
            {
                OperatorDtoBase_WithDimension dto2;

                if (signalMathPropertiesDto.IsVar && factorMathPropertiesDto.IsVar && originMathPropertiesDto.IsVar)
                {
                    dto2 = new Squash_OperatorDto_VarSignal_VarFactor_VarOrigin { SignalOperatorDto = signalOperatorDto, FactorOperatorDto = factorOperatorDto, OriginOperatorDto = originOperatorDto };
                }
                else if (signalMathPropertiesDto.IsVar && factorMathPropertiesDto.IsVar && originMathPropertiesDto.IsConstZero)
                {
                    dto2 = new Squash_OperatorDto_VarSignal_VarFactor_ZeroOrigin { SignalOperatorDto = signalOperatorDto, FactorOperatorDto = factorOperatorDto };
                }
                else if (signalMathPropertiesDto.IsVar && factorMathPropertiesDto.IsVar && originMathPropertiesDto.IsConst)
                {
                    dto2 = new Squash_OperatorDto_VarSignal_VarFactor_ConstOrigin { SignalOperatorDto = signalOperatorDto, FactorOperatorDto = factorOperatorDto, Origin = originMathPropertiesDto.ConstValue };
                }
                else if (signalMathPropertiesDto.IsVar && factorMathPropertiesDto.IsConst && originMathPropertiesDto.IsVar)
                {
                    dto2 = new Squash_OperatorDto_VarSignal_ConstFactor_VarOrigin { SignalOperatorDto = signalOperatorDto, Factor = factorMathPropertiesDto.ConstValue, OriginOperatorDto = originOperatorDto };
                }
                else if (signalMathPropertiesDto.IsVar && factorMathPropertiesDto.IsConst && originMathPropertiesDto.IsConstZero)
                {
                    dto2 = new Squash_OperatorDto_VarSignal_ConstFactor_ZeroOrigin { SignalOperatorDto = signalOperatorDto, Factor = factorMathPropertiesDto.ConstValue };
                }
                else if (signalMathPropertiesDto.IsVar && factorMathPropertiesDto.IsConst && originMathPropertiesDto.IsConst)
                {
                    dto2 = new Squash_OperatorDto_VarSignal_ConstFactor_ConstOrigin { SignalOperatorDto = signalOperatorDto, Factor = factorMathPropertiesDto.ConstValue, Origin = originMathPropertiesDto.ConstValue };
                }
                else if (signalMathPropertiesDto.IsConst && factorMathPropertiesDto.IsVar && originMathPropertiesDto.IsVar)
                {
                    dto2 = new Squash_OperatorDto_ConstSignal_VarFactor_VarOrigin { Signal = signalMathPropertiesDto.ConstValue, FactorOperatorDto = factorOperatorDto, OriginOperatorDto = originOperatorDto };
                }
                else if (signalMathPropertiesDto.IsConst && factorMathPropertiesDto.IsVar && originMathPropertiesDto.IsConstZero)
                {
                    dto2 = new Squash_OperatorDto_ConstSignal_VarFactor_ZeroOrigin { Signal = signalMathPropertiesDto.ConstValue, FactorOperatorDto = factorOperatorDto };
                }
                else if (signalMathPropertiesDto.IsConst && factorMathPropertiesDto.IsVar && originMathPropertiesDto.IsConst)
                {
                    dto2 = new Squash_OperatorDto_ConstSignal_VarFactor_ConstOrigin { Signal = signalMathPropertiesDto.ConstValue, FactorOperatorDto = factorOperatorDto, Origin = originMathPropertiesDto.ConstValue };
                }
                else if (signalMathPropertiesDto.IsConst && factorMathPropertiesDto.IsConst && originMathPropertiesDto.IsVar)
                {
                    dto2 = new Squash_OperatorDto_ConstSignal_ConstFactor_VarOrigin { Signal = signalMathPropertiesDto.ConstValue, Factor = factorMathPropertiesDto.ConstValue, OriginOperatorDto = originOperatorDto };
                }
                else if (signalMathPropertiesDto.IsConst && factorMathPropertiesDto.IsConst && originMathPropertiesDto.IsConstZero)
                {
                    dto2 = new Squash_OperatorDto_ConstSignal_ConstFactor_ZeroOrigin { Signal = signalMathPropertiesDto.ConstValue, Factor = factorMathPropertiesDto.ConstValue };
                }
                else if (signalMathPropertiesDto.IsConst && factorMathPropertiesDto.IsConst && originMathPropertiesDto.IsConst)
                {
                    dto2 = new Squash_OperatorDto_ConstSignal_ConstFactor_ConstOrigin { Signal = signalMathPropertiesDto.ConstValue, Factor = factorMathPropertiesDto.ConstValue, Origin = originMathPropertiesDto.ConstValue };
                }
                else
                {
                    throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
                }

                Clone_DimensionProperties(dto, dto2);

                return dto2;
            }
        }

        protected override OperatorDtoBase Visit_Stretch_OperatorDto(Stretch_OperatorDto dto)
        {
            throw new NotImplementedException();
        }

        protected override OperatorDtoBase Visit_Subtract_OperatorDto(Subtract_OperatorDto dto)
        {
            base.Visit_Subtract_OperatorDto(dto);

            OperatorDtoBase aOperatorDto = dto.AOperatorDto;
            OperatorDtoBase bOperatorDto = dto.BOperatorDto;

            MathPropertiesDto aMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(aOperatorDto);
            MathPropertiesDto bMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bOperatorDto);

            if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsConst)
            {
                return new Subtract_OperatorDto_ConstA_ConstB { A = aMathPropertiesDto.ConstValue, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsConst)
            {
                return new Subtract_OperatorDto_VarA_ConstB { AOperatorDto = aOperatorDto, B = bMathPropertiesDto.ConstValue };
            }
            else if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsVar)
            {
                return new Subtract_OperatorDto_ConstA_VarB { A = aMathPropertiesDto.ConstValue, BOperatorDto = bOperatorDto };
            }
            else if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsVar)
            {
                return new Subtract_OperatorDto_VarA_VarB { AOperatorDto = aOperatorDto, BOperatorDto = bOperatorDto };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_SumOverDimension_OperatorDto(SumOverDimension_OperatorDto dto)
        {
            base.Visit_SumOverDimension_OperatorDto(dto);

            SumOverDimension_OperatorDto dto2;

            switch (dto.CollectionRecalculationEnum)
            {
                case CollectionRecalculationEnum.Continuous:
                    dto2 = new SumOverDimension_OperatorDto_CollectionRecalculationContinuous();
                    break;

                case CollectionRecalculationEnum.UponReset:
                    dto2 = new SumOverDimension_OperatorDto_CollectionRecalculationUponReset();
                    break;

                default:
                    throw new ValueNotSupportedException(dto.CollectionRecalculationEnum);
            }

            Clone_AggregateOverDimensionProperties(dto, dto2);

            return dto2;
        }

        protected override OperatorDtoBase Visit_TimePower_OperatorDto(TimePower_OperatorDto dto)
        {
            base.Visit_TimePower_OperatorDto(dto);

            OperatorDtoBase originOperatorDto = dto.OriginOperatorDto;
            MathPropertiesDto originMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(originOperatorDto);

            OperatorDtoBase_WithDimension dto2;

            if (originMathPropertiesDto.IsVar)
            {
                dto2 = new TimePower_OperatorDto_VarOrigin { SignalOperatorDto = dto.SignalOperatorDto, ExponentOperatorDto = dto.ExponentOperatorDto, OriginOperatorDto = dto.OriginOperatorDto };
            }
            else if (originMathPropertiesDto.IsConst)
            {
                dto2 = new TimePower_OperatorDto_ConstOrigin { SignalOperatorDto = dto.SignalOperatorDto, ExponentOperatorDto = dto.ExponentOperatorDto, Origin = originMathPropertiesDto.ConstValue };
            }
            else
            {
                throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
            }

            Clone_DimensionProperties(dto, dto2);

            return dto2;
        }

        protected override OperatorDtoBase Visit_Triangle_OperatorDto(Triangle_OperatorDto dto)
        {
            base.Visit_Triangle_OperatorDto(dto);

            OperatorDtoBase frequencyOperatorDto = dto.FrequencyOperatorDto;
            MathPropertiesDto frequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(dto);

            if (frequencyMathPropertiesDto.IsVar && dto.StandardDimensionEnum == DimensionEnum.Time)
            {
                return new Triangle_OperatorDto_VarFrequency_WithPhaseTracking { FrequencyOperatorDto = frequencyOperatorDto };
            }
            else if (frequencyMathPropertiesDto.IsVar && dto.StandardDimensionEnum != DimensionEnum.Time)
            {
                return new Triangle_OperatorDto_VarFrequency_NoPhaseTracking { FrequencyOperatorDto = frequencyOperatorDto };
            }
            else if (frequencyMathPropertiesDto.IsConst && dto.StandardDimensionEnum == DimensionEnum.Time)
            {
                return new Triangle_OperatorDto_ConstFrequency_WithOriginShifting { Frequency = frequencyMathPropertiesDto.ConstValue };
            }
            else if (frequencyMathPropertiesDto.IsConst && dto.StandardDimensionEnum != DimensionEnum.Time)
            {
                return new Triangle_OperatorDto_ConstFrequency_NoOriginShifting { Frequency = frequencyMathPropertiesDto.ConstValue };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        // Clone

        private void Clone_AggregateOverDimensionProperties(OperatorDtoBase_AggregateOverDimension source, OperatorDtoBase_AggregateOverDimension dest)
        {
            dest.SignalOperatorDto = source.SignalOperatorDto;
            dest.FromOperatorDto = source.FromOperatorDto;
            dest.TillOperatorDto = source.TillOperatorDto;
            dest.StepOperatorDto = source.StepOperatorDto;
            dest.CollectionRecalculationEnum = source.CollectionRecalculationEnum;

            Clone_DimensionProperties(source, dest);
        }

        private void Clone_CacheOperatorProperties(Cache_OperatorDto source, Cache_OperatorDto dest)
        {
            dest.SignalOperatorDto = source.SignalOperatorDto;
            dest.StartOperatorDto = source.StartOperatorDto;
            dest.EndOperatorDto = source.EndOperatorDto;
            dest.SamplingRateOperatorDto = source.SamplingRateOperatorDto;
            dest.InterpolationTypeEnum = source.InterpolationTypeEnum;
            dest.SpeakerSetupEnum = source.SpeakerSetupEnum;
            dest.ChannelCount = source.ChannelCount;
        }

        private void Clone_ClosestOverDimensionProperties(ClosestOverDimension_OperatorDto source, ClosestOverDimension_OperatorDto dest)
        {
            dest.InputOperatorDto = source.InputOperatorDto;
            dest.CollectionOperatorDto = source.CollectionOperatorDto;
            dest.FromOperatorDto = source.FromOperatorDto;
            dest.TillOperatorDto = source.TillOperatorDto;
            dest.StepOperatorDto = source.StepOperatorDto;
            dest.CollectionRecalculationEnum = source.CollectionRecalculationEnum;

            Clone_DimensionProperties(source, dest);
        }

        private void Clone_CurveProperties(Curve_OperatorDto source, Curve_OperatorDto dest)
        {
            dest.Curve = source.Curve;
            dest.MinX = source.MinX;
            Clone_DimensionProperties(source, dest);
        }

        private void Clone_DimensionProperties(IOperatorDto_WithDimension source, IOperatorDto_WithDimension dest)
        {
            dest.CustomDimensionName = source.CustomDimensionName;
            dest.StandardDimensionEnum = source.StandardDimensionEnum;
        }

        // Helpers

        private ClosestOverInlets_MathPropertiesDto Get_ClosestOverInlets_MathPropertiesDto(ClosestOverInlets_OperatorDto dto)
        {
            OperatorDtoBase inputOperatorDto = dto.InputOperatorDto;
            IList<OperatorDtoBase> itemOperatorDtos = dto.ItemOperatorDtos;

            MathPropertiesDto inputMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(inputOperatorDto);

            int count = itemOperatorDtos.Count;
            IList<MathPropertiesDto> itemMathPropertiesDtos = new MathPropertiesDto[count];
            for (int i = 0; i < count; i++)
            {
                OperatorDtoBase itemOperatorDto = itemOperatorDtos[i];
                MathPropertiesDto itemMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(itemOperatorDto);
                itemMathPropertiesDtos[i] = itemMathPropertiesDto;
            }

            bool allItemsAreConst = itemMathPropertiesDtos.All(x => x.IsConst);
            IList<double> itemsValues = itemMathPropertiesDtos.Select(x => x.ConstValue).ToArray();

            var mathPropertiesDto = new ClosestOverInlets_MathPropertiesDto
            {
                InputMathPropertiesDto = inputMathPropertiesDto,
                ItemMathPropertiesDtos = itemMathPropertiesDtos,
                AllItemsAreConst = allItemsAreConst,
                ItemsValues = itemsValues
            };

            return mathPropertiesDto;
        }
    }
}
