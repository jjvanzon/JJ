﻿using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.Entities;

namespace JJ.Business.Synthesizer.Warnings.Operators
{
    internal class LowPassFilter_OperatorWarningValidator : OperatorWarningValidator_Base_AllInletsFilledInOrHaveDefaults
    {
        public LowPassFilter_OperatorWarningValidator(Operator obj)
            : base(obj)
        { }
    }
}
