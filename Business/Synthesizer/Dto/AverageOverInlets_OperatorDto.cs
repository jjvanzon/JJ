﻿using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class AverageOverInlets_OperatorDto : OperatorDtoBase_InputsOnly
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.AverageOverInlets;
    }
}
