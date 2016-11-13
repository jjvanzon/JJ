﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Framework.Reflection.Exceptions;
using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Extensions
{
    public static class SampleExtensions
    {
        public static int GetChannelCount(this Sample sample)
        {
            if (sample == null) throw new NullException(() => sample);
            return sample.SpeakerSetup.SpeakerSetupChannels.Count;
        }

        /// <param name="bytes">nullable</param>
        public static double GetDuration(this Sample sample, byte[] bytes)
        {
            // Bytes are nullable, so we choose here not to make GetDuration crash on that.
            if (bytes == null) return 0.0;
            if (bytes.Length == 0) return 0.0;

            return sample.GetDuration(bytes.Length);
        }

        public static double GetDuration(this Sample sample, long byteCount)
        {
            if (sample == null) throw new NullException(() => sample);
            if (sample.SamplingRate == 0) throw new ZeroException(() => sample.SamplingRate);

            double duration = (double)(byteCount - sample.BytesToSkip)
                              / (double)sample.GetChannelCount()
                              / (double)sample.SamplingRate
                              / (double)SampleDataTypeHelper.SizeOf(sample.SampleDataType)
                              * sample.TimeMultiplier;
            return duration;
        }
    }
}