﻿using JJ.Data.Synthesizer;
using System;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Helpers;
using System.Linq;
using JJ.Business.Synthesizer.Validation.DataProperty;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class Bundle_OperatorValidator : OperatorValidator_Base_VariableInletCountOneOutlet
    {
        public Bundle_OperatorValidator(Operator obj)
            : base(obj, OperatorTypeEnum.Bundle, allowedDataKeys: new string[] { PropertyNames.Dimension })
        { }

        protected override void Execute()
        {
            base.Execute();

            ExecuteValidator(new Dimension_DataProperty_Validator(Object.Data));
        }
    }
}