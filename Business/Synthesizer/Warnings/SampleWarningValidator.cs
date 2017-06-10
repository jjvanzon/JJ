﻿using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Validation;
using System.Collections.Generic;
using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Presentation.Resources;
using JJ.Framework.Validation.Resources;

namespace JJ.Business.Synthesizer.Warnings
{
    internal class SampleWarningValidator : VersatileValidator<Sample>
    {
        /// <param name="bytes">nullable</param>
        public SampleWarningValidator(Sample sample, byte[] bytes, HashSet<object> alreadyDone)
            : base(sample)
        {
            if (alreadyDone == null) throw new AlreadyDoneIsNullException();


            if (alreadyDone.Contains(sample))
            {
                return;
            }
            alreadyDone.Add(sample);

            For(() => sample.Amplifier, ResourceFormatter.Amplifier).IsNot(0.0);

            if (!sample.IsActive)
            {
                ValidationMessages.Add(() => sample.Amplifier, ResourceFormatter.NotActive);
            }

            if (bytes == null)
            {
                ValidationMessages.Add(() => bytes, ResourceFormatter.NotLoaded);
            }
            else if (bytes.Length == 0)
            {
                ValidationMessages.Add(() => bytes.Length, ValidationResourceFormatter.IsZero(CommonResourceFormatter.Count_WithNamePlural(ResourceFormatter.Samples)));
            }
        }
    }
}
