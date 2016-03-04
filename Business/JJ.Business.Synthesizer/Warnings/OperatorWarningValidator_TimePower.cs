﻿using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Warnings
{
    internal class OperatorWarningValidator_TimePower : OperatorWarningValidator_Base_FirstXInletsFilledIn
    {
        public OperatorWarningValidator_TimePower(Operator obj)
            : base(obj, inletCount: 2)
        { }
    }
}
