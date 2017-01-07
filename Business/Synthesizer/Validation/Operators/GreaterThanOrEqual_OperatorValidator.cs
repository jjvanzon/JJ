﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class GreaterThanOrEqual_OperatorValidator : OperatorValidator_Base_WithoutData
    {
        public GreaterThanOrEqual_OperatorValidator(Operator obj)
            : base(
                  obj,
                  OperatorTypeEnum.GreaterThanOrEqual,
                  new DimensionEnum[] { DimensionEnum.Undefined, DimensionEnum.Undefined },
                  new DimensionEnum[] { DimensionEnum.Undefined })
        { }
    }
}
