﻿using JJ.Business.Synthesizer.Enums;
using JJ.Data.Synthesizer.Entities;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class Divide_OperatorValidator : OperatorValidator_Base_WithoutData
    {
        public Divide_OperatorValidator(Operator obj)
            : base(
                  obj, 
                  OperatorTypeEnum.Divide,
                  new[] { DimensionEnum.A, DimensionEnum.B, DimensionEnum.Origin },
                  new[] { DimensionEnum.Result })
        { }
    }
}
