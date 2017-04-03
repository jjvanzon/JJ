﻿using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.Entities;
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
            For(() => Obj.DesiredBufferDuration, ResourceFormatter.DesiredBufferDuration).LessThan(5);
        }
    }
}