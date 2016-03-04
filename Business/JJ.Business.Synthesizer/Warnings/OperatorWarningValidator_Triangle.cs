﻿using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Warnings
{
    internal class OperatorWarningValidator_Triangle : OperatorWarningValidator_Base_FirstXInletsFilledIn
    {
        public OperatorWarningValidator_Triangle(Operator obj)
            : base(obj, inletCount: 1)
        { }
    }
}
