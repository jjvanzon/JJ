﻿using System;
using System.Collections.Generic;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class Random_OperatorDto : OperatorDtoBase_WithDimension, IRandom_OperatorDto
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.Random;

        public OperatorDtoBase RateOperatorDto { get; set; }
        public ResampleInterpolationTypeEnum ResampleInterpolationTypeEnum { get; set; }

        /// <summary> Used as a cache key. </summary>
        public int OperatorID { get; set; }

        public override IList<OperatorDtoBase> InputOperatorDtos
        {
            get { return new[] { RateOperatorDto }; }
            set { RateOperatorDto = value[0]; }
        }
    }

    internal interface IRandom_OperatorDto : IOperatorDto_WithDimension
    {
        ResampleInterpolationTypeEnum ResampleInterpolationTypeEnum { get; set; }
        int OperatorID { get; set; }
    }

    internal class Random_OperatorDto_Block : Random_OperatorDto
    { }

    internal class Random_OperatorDto_CubicAbruptSlope : Random_OperatorDto
    { }

    internal class Random_OperatorDto_CubicEquidistant : Random_OperatorDto
    { }

    internal class Random_OperatorDto_CubicSmoothSlope_LagBehind : Random_OperatorDto
    { }

    internal class Random_OperatorDto_Hermite_LagBehind : Random_OperatorDto
    { }

    internal class Random_OperatorDto_Line_LagBehind_ConstRate : OperatorDtoBase_WithDimension, IRandom_OperatorDto
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.Random;

        public double Rate { get; set; }
        public ResampleInterpolationTypeEnum ResampleInterpolationTypeEnum { get; set; }

        /// <summary> Used as a cache key. </summary>
        public int OperatorID { get; set; }

        public override IList<OperatorDtoBase> InputOperatorDtos { get; set; } = new OperatorDtoBase[0];
    }

    internal class Random_OperatorDto_Line_LagBehind_VarRate : Random_OperatorDto
    { }

    internal class Random_OperatorDto_Stripe_LagBehind : Random_OperatorDto
    { }
}
