﻿using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class Shift_OperatorDto_VarSignal_VarDistance : OperatorDtoBase
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Shift);

        public OperatorDtoBase SignalOperatorDto => InputOperatorDtos[0];
        public OperatorDtoBase DifferenceOperatorDto => InputOperatorDtos[1];

        public Shift_OperatorDto_VarSignal_VarDistance(OperatorDtoBase signalOperatorDto, OperatorDtoBase differenceOperatorDto)
            : base(new OperatorDtoBase[] { signalOperatorDto, differenceOperatorDto })
        { }
    }
}
