﻿using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Warnings.Operators
{
    internal class Equal_OperatorWarningValidator : OperatorWarningValidator_Base_AllInletsFilledInOrHaveDefaults
    {
        public Equal_OperatorWarningValidator(Operator obj)
            : base(obj)
        { }
    }
}
