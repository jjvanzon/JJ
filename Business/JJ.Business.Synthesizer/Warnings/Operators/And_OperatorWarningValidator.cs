﻿using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Warnings.Operators
{
    internal class And_OperatorWarningValidator : OperatorWarningValidator_Base_AllInletsFilled
    {
        public And_OperatorWarningValidator(Operator obj)
            : base(obj)
        { }
    }
}