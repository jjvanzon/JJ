﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;
using JJ.Data.Synthesizer.Entities;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class PulseTrigger_OperatorValidator : OperatorValidator_Base_WithoutData
    {
        public PulseTrigger_OperatorValidator(Operator obj)
            : base(
                obj,
                OperatorTypeEnum.PulseTrigger,
                new[] { DimensionEnum.Undefined, DimensionEnum.Undefined },
                new[] { DimensionEnum.Undefined })
        { }
    }
}
