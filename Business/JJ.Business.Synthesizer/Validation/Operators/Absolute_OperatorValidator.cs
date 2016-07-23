﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Resources;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class Absolute_OperatorValidator : OperatorValidator_Base_WithoutData
    {
        public Absolute_OperatorValidator(Operator obj)
            : base(obj, OperatorTypeEnum.Absolute, expectedInletCount: 1, expectedOutletCount: 1)
        { }

        protected override void Execute()
        {
            For(() => Object.Dimension, PropertyDisplayNames.Dimension).IsNull();

            base.Execute();
        }
    }
}
