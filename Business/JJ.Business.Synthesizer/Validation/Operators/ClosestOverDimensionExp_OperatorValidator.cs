﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Validation.DataProperty;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class ClosestOverDimensionExp_OperatorValidator : OperatorValidator_Base
    {
        public ClosestOverDimensionExp_OperatorValidator(Operator obj)
            : base(
                  obj,
                OperatorTypeEnum.ClosestOverDimensionExp,
                expectedDataKeys: new string[] { PropertyNames.CollectionRecalculation },
                expectedInletCount: 5,
                expectedOutletCount: 1)
        { }

        protected override void Execute()
        {
            base.Execute();

            ExecuteValidator(new CollectionRecalculation_DataProperty_Validator(Object.Data));
        }
    }
}
