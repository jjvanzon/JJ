﻿using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class Or_OperatorDto : OperatorDtoBase_VarA_VarB
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Or);

        public Or_OperatorDto(OperatorDtoBase aOperatorDto, OperatorDtoBase bOperatorDto)
            : base(aOperatorDto, bOperatorDto)
        { }
    }
}