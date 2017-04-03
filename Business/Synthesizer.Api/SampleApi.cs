﻿using System.Collections.Generic;
using JJ.Business.Synthesizer.Api.Helpers;
using JJ.Business.Synthesizer.Calculation;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.Entities;

namespace JJ.Business.Synthesizer.Api
{
    public static class SampleApi
    {
        private static readonly SampleManager _sampleManager = CreateSampleManager();

        private static SampleManager CreateSampleManager()
        {
            return new SampleManager(RepositoryHelper.SampleRepositories);
        }

        public static IList<ICalculatorWithPosition> CreateCalculators(Sample sample, byte[] bytes)
        {
            return _sampleManager.CreateCalculators(sample, bytes);
        }
    }
}
