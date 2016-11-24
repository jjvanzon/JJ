﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Warnings
{
    internal class AudioOutputWarningValidator : VersatileValidator<AudioOutput>
    {
        public AudioOutputWarningValidator(AudioOutput obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            For(() => Object.DesiredBufferDuration, PropertyDisplayNames.DesiredBufferDuration).LessThan(5);
        }
    }
}