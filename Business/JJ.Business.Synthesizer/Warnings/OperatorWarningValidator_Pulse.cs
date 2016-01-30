﻿using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Warnings
{
    public class OperatorWarningValidator_Pulse : OperatorWarningValidator_Base_FirstXInletsFilledIn
    {
        public OperatorWarningValidator_Pulse(Operator obj)
            : base(obj, inletCount: 2)
        { }
    }
}
