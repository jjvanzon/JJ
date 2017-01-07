﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class GreaterThan_OperatorValidator : OperatorValidator_Base_WithoutData
    {
        public GreaterThan_OperatorValidator(Operator obj)
            : base(
                  obj, 
                  OperatorTypeEnum.GreaterThan,
                  new DimensionEnum[] { DimensionEnum.Undefined, DimensionEnum.Undefined },
                  new DimensionEnum[] { DimensionEnum.Undefined })
        { }
    }
}
