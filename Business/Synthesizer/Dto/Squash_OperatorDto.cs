﻿using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class Squash_OperatorDto : StretchOrSquash_OperatorDto
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Squash);

        public Squash_OperatorDto(OperatorDtoBase signalOperatorDto, OperatorDtoBase factorOperatorDto, OperatorDtoBase originOperatorDto)
            : base(signalOperatorDto, factorOperatorDto, originOperatorDto)
        { }
    }
}