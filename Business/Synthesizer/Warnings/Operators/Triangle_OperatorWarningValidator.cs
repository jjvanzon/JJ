﻿using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Warnings.Operators
{
    internal class Triangle_OperatorWarningValidator : OperatorWarningValidator_Base_FirstXInletsFilledInOrHaveDefaults
    {
        public Triangle_OperatorWarningValidator(Operator obj)
            : base(obj, inletCount: 1)
        { }
    }
}