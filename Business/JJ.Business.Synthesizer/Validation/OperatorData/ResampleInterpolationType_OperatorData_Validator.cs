﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.OperatorData
{
    internal class ResampleInterpolationType_OperatorData_Validator : FluentValidator_WithoutConstructorArgumentNullCheck<string>
    {
        public ResampleInterpolationType_OperatorData_Validator(string obj) 
            : base(obj)
        { }

        protected override void Execute()
        {
            string data = Object;

            if (DataPropertyParser.DataIsWellFormed(data))
            {
                string interpolationType = DataPropertyParser.TryGetString(Object, PropertyNames.InterpolationType);

                For(() => interpolationType, PropertyDisplayNames.InterpolationType)
                    .NotNullOrEmpty()
                    .IsEnum<ResampleInterpolationTypeEnum>()
                    .IsNot(ResampleInterpolationTypeEnum.Undefined);
            }
        }
    }
}
