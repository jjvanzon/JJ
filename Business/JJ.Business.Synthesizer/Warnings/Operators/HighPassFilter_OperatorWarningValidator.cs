﻿using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Warnings.Operators
{
    internal class HighPassFilter_OperatorWarningValidator : OperatorWarningValidator_Base_AllInletsFilled
    {
        public HighPassFilter_OperatorWarningValidator(Operator obj)
            : base(obj)
        { }
    }
}
