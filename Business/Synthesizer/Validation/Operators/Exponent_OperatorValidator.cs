﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class Exponent_OperatorValidator : OperatorValidator_Base_WithoutData
    {
        public Exponent_OperatorValidator(Operator obj)
            : base(
                  obj, 
                  OperatorTypeEnum.Exponent,
                  new[] { DimensionEnum.Undefined, DimensionEnum.Undefined, DimensionEnum.Undefined },
                  new[] { DimensionEnum.Undefined })
        { }
    }
}
