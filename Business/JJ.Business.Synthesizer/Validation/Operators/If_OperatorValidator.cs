﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class If_OperatorValidator : OperatorValidator_Base_WithoutData
    {
        public If_OperatorValidator(Operator obj)
            : base(obj, OperatorTypeEnum.If, expectedInletCount: 3, expectedOutletCount: 1)
        { }
    }
}