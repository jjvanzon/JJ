﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class SawUp_OperatorValidator : OperatorValidator_Base
    {
        public SawUp_OperatorValidator(Operator obj)
            : base(
                  obj,
                OperatorTypeEnum.SawUp,
                expectedDataKeys: new string[0],
                expectedInletCount: 2,
                expectedOutletCount: 1)
        { }
    }
}