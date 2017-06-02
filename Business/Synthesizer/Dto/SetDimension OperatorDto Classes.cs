﻿using System.Collections.Generic;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class SetDimension_OperatorDto : SetDimension_OperatorDto_VarPassThrough_VarX
    { }

    internal class SetDimension_OperatorDto_VarPassThrough_VarX : OperatorDtoBase_WithDimension, IOperatorDto_VarSignal_WithDimension
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.SetDimension;

        public IOperatorDto PassThroughInputOperatorDto { get; set; }
        public IOperatorDto XOperatorDto { get; set; }

        public override IList<IOperatorDto> InputOperatorDtos
        {
            get => new[] { PassThroughInputOperatorDto, XOperatorDto };
            set { PassThroughInputOperatorDto = value[0]; XOperatorDto = value[1]; }
        }

        public IOperatorDto SignalOperatorDto
        {
            get => PassThroughInputOperatorDto;
            set => PassThroughInputOperatorDto = value;
        }
    }

    internal class SetDimension_OperatorDto_VarPassThrough_ConstX : OperatorDtoBase_WithDimension, IOperatorDto_VarSignal_WithDimension
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.SetDimension;

        public IOperatorDto PassThroughInputOperatorDto { get; set; }
        public double X { get; set; }

        public override IList<IOperatorDto> InputOperatorDtos
        {
            get => new[] { PassThroughInputOperatorDto };
            set => PassThroughInputOperatorDto = value[0];
        }

        public IOperatorDto SignalOperatorDto
        {
            get => PassThroughInputOperatorDto;
            set => PassThroughInputOperatorDto = value;
        }
    }

    internal class SetDimension_OperatorDto_ConstPassThrough_VarX : OperatorDtoBase_WithDimension
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.SetDimension;

        public double PassThrough { get; set; }
        public IOperatorDto XOperatorDto { get; set; }

        public override IList<IOperatorDto> InputOperatorDtos
        {
            get => new[] { XOperatorDto };
            set => XOperatorDto = value[0];
        }
    }

    internal class SetDimension_OperatorDto_ConstPassThrough_ConstX : OperatorDtoBase_WithDimension
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.SetDimension;

        public double PassThrough { get; set; }
        public double X { get; set; }

        public override IList<IOperatorDto> InputOperatorDtos { get; set; } = new IOperatorDto[0];
    }
}
