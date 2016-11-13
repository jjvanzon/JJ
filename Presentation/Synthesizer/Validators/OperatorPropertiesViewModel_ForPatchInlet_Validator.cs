﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Validation;
using JJ.Presentation.Synthesizer.Resources;
using JJ.Presentation.Synthesizer.ViewModels;

namespace JJ.Presentation.Synthesizer.Validators
{
    /// <summary>
    /// Validate the Number property of the view model, which is 1-based,
    /// to not confuse the user with a validation message talking about the 0-based ListIndex property,
    /// </summary>
    internal class OperatorPropertiesViewModel_ForPatchInlet_Validator : FluentValidator<OperatorPropertiesViewModel_ForPatchInlet>
    {
        public OperatorPropertiesViewModel_ForPatchInlet_Validator(OperatorPropertiesViewModel_ForPatchInlet obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            For(() => Object.Number, Titles.Number).GreaterThanOrEqual(1);
            For(() => Object.DefaultValue, PropertyDisplayNames.DefaultValue).IsDouble();
        }
    }
}