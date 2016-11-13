﻿using JJ.Business.Synthesizer.Enums;
using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Dto
{
    internal class Sample_OperatorDto : OperatorDtoBase_VarFrequency
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Sample);

        public Sample Sample { get; }

        public Sample_OperatorDto(Sample sample, OperatorDtoBase frequencyOperatorDto)
            : base(frequencyOperatorDto)
        {
            Sample = sample;
        }
    }
}