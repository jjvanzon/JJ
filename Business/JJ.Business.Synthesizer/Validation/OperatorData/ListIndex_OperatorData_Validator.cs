﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.OperatorData
{
    internal class ListIndex_OperatorData_Validator : FluentValidator_WithoutConstructorArgumentNullCheck<string>
    {
        public ListIndex_OperatorData_Validator(string obj) 
            : base(obj)
        { }

        protected override void Execute()
        {
            string data = Object;

            if (DataPropertyParser.DataIsWellFormed(data))
            {
                string listIndexString = DataPropertyParser.TryGetString(Object, PropertyNames.ListIndex);
                For(() => listIndexString, PropertyDisplayNames.ListIndex)
                    .NotNullOrEmpty()
                    .IsInteger()
                    .GreaterThanOrEqual(0);
            }
        }
    }
}
