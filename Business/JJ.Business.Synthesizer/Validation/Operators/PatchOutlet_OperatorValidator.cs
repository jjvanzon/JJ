﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Resources;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Validation.DataProperty;
using System.Linq;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class PatchOutlet_OperatorValidator : OperatorValidator_Base
    {
        public PatchOutlet_OperatorValidator(Operator obj)
            : base(
                  obj, 
                  OperatorTypeEnum.PatchOutlet, 
                  expectedInletCount: 1, 
                  expectedOutletCount: 1,
                  expectedDataKeys: new string[] { PropertyNames.ListIndex })
        { }

        protected override void Execute()
        {
            base.Execute();

            Execute(new ListIndex_DataProperty_Validator(Object.Data));
        }
    }
}
