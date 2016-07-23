﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Validation.DataProperty;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class ClosestOverDimension_OperatorValidator : OperatorValidator_Base
    {
        public ClosestOverDimension_OperatorValidator(Operator obj)
            : base(
                  obj,
                  OperatorTypeEnum.ClosestOverDimension,
                  expectedInletCount: 5,
                  expectedOutletCount: 1,
                  expectedDataKeys: new string[] { PropertyNames.CollectionRecalculation })
        { }

        protected override void Execute()
        {
            base.Execute();

            ExecuteValidator(new CollectionRecalculation_DataProperty_Validator(Object.Data));
        }
    }
}
