﻿using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Validation
{
    /// <summary>
    /// Base class for operator validators that do not have additional data.
    /// Verifies that the Data property is null.
    /// </summary>
    public abstract class OperatorValidator_Base_WithoutData : OperatorValidator_Base
    {
        public OperatorValidator_Base_WithoutData(
            Operator obj,
            OperatorTypeEnum expectedOperatorTypeEnum,
            int expectedInletCount,
            params string[] expectedInletAndOutletNames)
            : base(obj, expectedOperatorTypeEnum, expectedInletCount, expectedInletAndOutletNames)
        { }

        protected override void Execute()
        {
            base.Execute();

            For(() => Object.Data, PropertyDisplayNames.Data).IsNull();
        }
    }
}
