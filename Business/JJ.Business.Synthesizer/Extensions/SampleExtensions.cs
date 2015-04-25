﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Framework.Reflection.Exceptions;
using JJ.Data.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.Extensions
{
    public static class SampleExtensions
    {
        public static int GetChannelCount(this Sample sample)
        {
            if (sample == null) throw new NullException(() => sample);
            return sample.SpeakerSetup.SpeakerSetupChannels.Count;
        }

        public static double GetDuration(this Sample sample)
        {
            if (sample == null) throw new NullException(() => sample);
            if (sample.Bytes.Length == 0) throw new Exception("sample.Bytes.Length cannot be 0.");
            if (sample.SamplingRate == 0) throw new Exception("sample.SamplingRate cannot be null.");

            double duration = (double)(sample.Bytes.Length - sample.BytesToSkip)
                              / (double)sample.GetChannelCount()
                              / (double)sample.SamplingRate
                              / (double)SampleDataTypeHelper.SizeOf(sample.SampleDataType)
                              * sample.TimeMultiplier;
            return duration;
        }
    }
}
