﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;
using JJ.Data.Synthesizer.Entities;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class GreaterThanOrEqual_OperatorValidator : OperatorValidator_Base_WithoutData
    {
        public GreaterThanOrEqual_OperatorValidator(Operator obj)
            : base(
                  obj,
                  OperatorTypeEnum.GreaterThanOrEqual,
                  new[] { DimensionEnum.Undefined, DimensionEnum.Undefined },
                  new[] { DimensionEnum.Undefined })
        { }
    }
}
