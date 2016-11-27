﻿using System.Collections.Generic;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class SetDimension_OperatorDto : SetDimension_OperatorDto_VarValue
    { }

    internal class SetDimension_OperatorDto_VarValue : OperatorDtoBase_WithDimension
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.SetDimension);

        public OperatorDtoBase PassThroughInputOperatorDto { get; set; }
        public OperatorDtoBase ValueOperatorDto { get; set; }

        public override IList<OperatorDtoBase> InputOperatorDtos
        {
            get { return new OperatorDtoBase[] { PassThroughInputOperatorDto, ValueOperatorDto }; }
            set { PassThroughInputOperatorDto = value[0]; ValueOperatorDto = value[1]; }
        }
    }

    internal class SetDimension_OperatorDto_ConstValue : OperatorDtoBase_WithDimension
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.SetDimension);

        public OperatorDtoBase PassThroughInputOperatorDto { get; set; }
        public double Value { get; set; }

        public override IList<OperatorDtoBase> InputOperatorDtos
        {
            get { return new OperatorDtoBase[] { PassThroughInputOperatorDto }; }
            set { PassThroughInputOperatorDto = value[0]; }
        }
    }
}