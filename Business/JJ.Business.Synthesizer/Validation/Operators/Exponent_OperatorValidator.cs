﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Resources;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class Exponent_OperatorValidator : OperatorValidator_Base_WithoutData
    {
        public Exponent_OperatorValidator(Operator obj)
            : base(obj, OperatorTypeEnum.Exponent, expectedInletCount: 3, expectedOutletCount: 1)
        { }

        protected override void Execute()
        {
            For(() => Object.Dimension, PropertyDisplayNames.Dimension).IsNull();

            base.Execute();
        }
    }
}
