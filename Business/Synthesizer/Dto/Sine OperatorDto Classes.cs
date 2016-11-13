﻿using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class Sine_OperatorDto : OperatorDtoBase_VarFrequency
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Sine);

        public Sine_OperatorDto(OperatorDtoBase frequencyOperatorDto)
            : base(frequencyOperatorDto)
        { }
    }

    internal class Sine_OperatorDto_ConstFrequency_NoOriginShifting : OperatorDtoBase_ConstFrequency
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Sine);

        public Sine_OperatorDto_ConstFrequency_NoOriginShifting(double frequency)
            : base(frequency)
        { }
    }

    internal class Sine_OperatorDto_ConstFrequency_WithOriginShifting : OperatorDtoBase_ConstFrequency
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Sine);

        public Sine_OperatorDto_ConstFrequency_WithOriginShifting(double frequency)
            : base(frequency)
        { }
    }

    internal class Sine_OperatorDto_VarFrequency_NoPhaseTracking : OperatorDtoBase_VarFrequency
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Sine);

        public Sine_OperatorDto_VarFrequency_NoPhaseTracking(OperatorDtoBase frequencyOperatorDto)
            : base(frequencyOperatorDto)
        { }
    }

    internal class Sine_OperatorDto_VarFrequency_WithPhaseTracking : OperatorDtoBase_VarFrequency
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Sine);

        public Sine_OperatorDto_VarFrequency_WithPhaseTracking(OperatorDtoBase frequencyOperatorDto)
            : base(frequencyOperatorDto)
        { }
    }
}
