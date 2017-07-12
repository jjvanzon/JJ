﻿using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Exceptions;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.Samples
{
    internal class SampleValidator : VersatileValidator
    {
        public SampleValidator(Sample sample)
        {
            if (sample == null) throw new NullException(() => sample);

            ExecuteValidator(new NameValidator(sample.Name, required: false));

            For(sample.SamplingRate, ResourceFormatter.SamplingRate).GreaterThan(0);
            For(sample.Amplifier, ResourceFormatter.Amplifier).NotNaN().NotInfinity();
            For(sample.TimeMultiplier, ResourceFormatter.TimeMultiplier).NotNaN().NotInfinity().IsNot(0);
            For(sample.AudioFileFormat, ResourceFormatter.AudioFileFormat).NotNull();
            For(sample.SampleDataType, ResourceFormatter.SampleDataType).NotNull();
            For(sample.InterpolationType, ResourceFormatter.InterpolationType).NotNull();
            For(sample.SpeakerSetup, ResourceFormatter.SpeakerSetup).NotNull();
        }
    }
}
