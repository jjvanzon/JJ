﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class Shift_OperatorValidator : OperatorValidator_Base
    {
        public Shift_OperatorValidator(Operator obj)
            : base(obj, OperatorTypeEnum.Shift, expectedInletCount: 2, expectedOutletCount: 1, expectedDataKeys: new string[0])
        { }
    }
}