﻿using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Warnings.Operators
{
    internal class Sine_OperatorWarningValidator : OperatorWarningValidator_Base_FirstXInletsFilledIn
    {
        public Sine_OperatorWarningValidator(Operator obj)
            : base(obj, inletCount: 1)
        { }
    }
}