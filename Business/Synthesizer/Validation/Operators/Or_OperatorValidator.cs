﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class Or_OperatorValidator : OperatorValidator_Base_WithoutData
    {
        public Or_OperatorValidator(Operator obj)
            : base(
                obj, 
                OperatorTypeEnum.Or,
                new[] { DimensionEnum.Undefined, DimensionEnum.Undefined },
                new[] { DimensionEnum.Undefined })
        { }
    }
}
