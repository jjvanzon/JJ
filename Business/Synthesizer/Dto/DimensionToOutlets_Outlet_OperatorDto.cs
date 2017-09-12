﻿using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class DimensionToOutlets_Outlet_OperatorDto 
        : OperatorDtoBase_PositionTransformation, IOperatorDto_WithOutletPosition
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.DimensionToOutlets;

        public int OutletPosition { get; set; }

        public override IReadOnlyList<InputDto> Inputs
        {
            get => new[] { Signal, Position };
            set
            {
                Signal = value.ElementAtOrDefault(0);
                Position = value.ElementAtOrDefault(1);
            }
        }
    }
}