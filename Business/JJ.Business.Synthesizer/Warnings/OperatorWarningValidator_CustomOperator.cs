﻿using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer;
using System;

namespace JJ.Business.Synthesizer.Warnings
{
    public class OperatorWarningValidator_CustomOperator : OperatorWarningValidator_Base
    {
        public OperatorWarningValidator_CustomOperator(Operator op)
            : base(op)
        { }

        protected override void Execute()
        {
            For(() => Object.Data, PropertyDisplayNames.UnderlyingDocument)
                .NotNull();
        }
    }
}
