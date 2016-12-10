﻿using JJ.Business.Synthesizer.Dto;

namespace JJ.Business.Synthesizer.Helpers
{
    internal static class DtoHelper
    {
        public static void Clone_AggregateOverDimensionProperties(OperatorDtoBase_AggregateOverDimension_AllVars source, OperatorDtoBase_AggregateOverDimension_AllVars dest)
        {
            dest.SignalOperatorDto = source.SignalOperatorDto;
            dest.FromOperatorDto = source.FromOperatorDto;
            dest.TillOperatorDto = source.TillOperatorDto;
            dest.StepOperatorDto = source.StepOperatorDto;
            dest.CollectionRecalculationEnum = source.CollectionRecalculationEnum;

            Clone_DimensionProperties(source, dest);
        }

        public static void TryClone_AggregateOverDimensionProperties(OperatorDtoBase_AggregateOverDimension_AllVars source, OperatorDtoBase dest)
        {
            var castedDest = dest as OperatorDtoBase_AggregateOverDimension_AllVars;
            if (castedDest != null)
            {
                Clone_AggregateOverDimensionProperties(source, castedDest);
            }
        }

        public static void Clone_AggregateFollowerProperties(OperatorDtoBase_AggregateFollower_AllVars source, OperatorDtoBase_AggregateFollower_AllVars dest)
        {
            dest.SignalOperatorDto = source.SignalOperatorDto;
            dest.SliceLengthOperatorDto = source.SliceLengthOperatorDto;
            dest.SampleCountOperatorDto = source.SampleCountOperatorDto;

            Clone_DimensionProperties(source, dest);
        }

        public static void TryClone_AggregateFollowerProperties(OperatorDtoBase_AggregateFollower_AllVars source, OperatorDtoBase dest)
        {
            var castedDest = dest as OperatorDtoBase_AggregateFollower_AllVars;
            if (castedDest != null)
            {
                Clone_AggregateFollowerProperties(source, castedDest);
            }
        }

        public static void Clone_CacheOperatorProperties(Cache_OperatorDto source, Cache_OperatorDto dest)
        {
            dest.SignalOperatorDto = source.SignalOperatorDto;
            dest.StartOperatorDto = source.StartOperatorDto;
            dest.EndOperatorDto = source.EndOperatorDto;
            dest.SamplingRateOperatorDto = source.SamplingRateOperatorDto;
            dest.InterpolationTypeEnum = source.InterpolationTypeEnum;
            dest.SpeakerSetupEnum = source.SpeakerSetupEnum;
            dest.ChannelCount = source.ChannelCount;
        }

        public static void TryClone_CacheOperatorProperties(Cache_OperatorDto source, OperatorDtoBase dest)
        {
            var castedDest = dest as Cache_OperatorDto;
            if (castedDest != null)
            {
                Clone_CacheOperatorProperties(source, castedDest);
            }
        }

        public static void Clone_ClosestOverDimensionProperties(ClosestOverDimension_OperatorDto source, ClosestOverDimension_OperatorDto dest)
        {
            dest.InputOperatorDto = source.InputOperatorDto;
            dest.CollectionOperatorDto = source.CollectionOperatorDto;
            dest.FromOperatorDto = source.FromOperatorDto;
            dest.TillOperatorDto = source.TillOperatorDto;
            dest.StepOperatorDto = source.StepOperatorDto;
            dest.CollectionRecalculationEnum = source.CollectionRecalculationEnum;

            Clone_DimensionProperties(source, dest);
        }

        public static void Clone_CurveProperties(Curve_OperatorDto source, Curve_OperatorDto dest)
        {
            dest.CurveID = source.CurveID;
            dest.MinX = source.MinX;

            Clone_DimensionProperties(source, dest);
        }

        public static void Clone_DimensionProperties(IOperatorDto_WithDimension source, IOperatorDto_WithDimension dest)
        {
            dest.CustomDimensionName = source.CustomDimensionName;
            dest.StandardDimensionEnum = source.StandardDimensionEnum;
        }

        public static void TryClone_DimensionProperties(IOperatorDto_WithDimension source, OperatorDtoBase dest)
        {
            var asIOperatorDto_WithDimension = dest as IOperatorDto_WithDimension;
            if (asIOperatorDto_WithDimension != null)
            {
                Clone_DimensionProperties(source, asIOperatorDto_WithDimension);
            }
        }

        public static void Clone_InterpolateOperatorProperties(IInterpolate_OperatorDto_VarSignal source, IInterpolate_OperatorDto_VarSignal dest)
        {
            dest.ResampleInterpolationTypeEnum = source.ResampleInterpolationTypeEnum;
            dest.SignalOperatorDto = source.SignalOperatorDto;

            Clone_DimensionProperties(source, dest);
        }

        public static void TryClone_InterpolateOperatorProperties(Interpolate_OperatorDto source, OperatorDtoBase dest)
        {
            var asIInterpolate_OperatorDto_VarSignal = dest as IInterpolate_OperatorDto_VarSignal;
            if (asIInterpolate_OperatorDto_VarSignal != null)
            {
                Clone_InterpolateOperatorProperties(source, asIInterpolate_OperatorDto_VarSignal);
            }
        }

        public static void Clone_RandomOperatorProperties(Random_OperatorDto source, Random_OperatorDto dest)
        {
            dest.RateOperatorDto = source.RateOperatorDto;
            dest.ResampleInterpolationTypeEnum = source.ResampleInterpolationTypeEnum;

            Clone_DimensionProperties(source, dest);
        }

        public static void Clone_SampleOperatorProperties(ISample_OperatorDto dto, ISample_OperatorDto dto2)
        {
            dto2.SampleID = dto.SampleID;
            dto2.ChannelCount = dto.ChannelCount;
            dto2.InterpolationTypeEnum = dto.InterpolationTypeEnum;
        }

        public static void TryClone_SampleOperatorProperties(Sample_OperatorDto source, OperatorDtoBase dest)
        {
            var asISample_OperatorDto = dest as ISample_OperatorDto;
            if (asISample_OperatorDto != null)
            {
                Clone_SampleOperatorProperties(source, asISample_OperatorDto);
            }
        }
    }
}