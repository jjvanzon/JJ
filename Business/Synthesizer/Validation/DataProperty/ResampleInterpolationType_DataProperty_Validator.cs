﻿using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.DataProperty
{
    internal class ResampleInterpolationType_DataProperty_Validator : VersatileValidator_WithoutConstructorArgumentNullCheck<string>
    {
        public ResampleInterpolationType_DataProperty_Validator(string obj) 
            : base(obj)
        { }

        protected override void Execute()
        {
            string data = Obj;

            // ReSharper disable once InvertIf
            if (DataPropertyParser.DataIsWellFormed(data))
            {
                string interpolationType = DataPropertyParser.TryGetString(Obj, PropertyNames.InterpolationType);

                For(() => interpolationType, PropertyDisplayNames.InterpolationType)
                    .NotNullOrEmpty()
                    .IsEnum<ResampleInterpolationTypeEnum>()
                    .IsNot(ResampleInterpolationTypeEnum.Undefined);
            }
        }
    }
}
