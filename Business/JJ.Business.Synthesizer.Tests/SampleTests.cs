﻿using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Calculation;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Factories;
using JJ.Business.Synthesizer.Managers;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Tests.Helpers;
using JJ.Framework.Common;
using JJ.Framework.Persistence;
using JJ.Framework.Validation;
using JJ.Infrastructure.Synthesizer;
using JJ.Persistence.Synthesizer;
using JJ.Persistence.Synthesizer.DefaultRepositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.Tests
{
    [TestClass]
    public class SampleTests
    {
        private const string VIOLIN_16BIT_MONO_RAW_FILE_NAME = "violin_16bit_mono.raw";

        [TestMethod]
        public void Test_Synthesizer_Sample()
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                IInterpolationTypeRepository interpolationTypeRepository = PersistenceHelper.CreateRepository<IInterpolationTypeRepository>(context);
                IChannelRepository channelRepository = PersistenceHelper.CreateRepository<IChannelRepository>(context);

                // Create
                SampleManager sampleManager = TestHelper.CreateSampleManager(context);
                Sample sample = sampleManager.CreateSample();

                // Load
                Stream stream = GetViolinSampleStream();
                SampleLoadHelper.LoadSample(sample, stream);

                // Validate
                IValidator sampleValidator = sampleManager.ValidateSample(sample);
                sampleValidator.Verify();

                // Create Patch
                OperatorFactory f = TestHelper.CreateOperatorFactory(context);
                var wrapper = f.TimeMultiply(f.Sample(sample), f.Value(10));

                // Calculate and write to file
                OperatorCalculator calculator = new OperatorCalculator(sample.SampleChannels[0].Channel);

                using (Stream destStream = new FileStream("SampleOperatorOutput.raw", FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    using (BinaryWriter writer = new BinaryWriter(destStream))
                    {
                        int destSampleCount = 44100 * 6;

                        double t = 0;
                        double dt = 1.0 / 44100.0;

                        for (int i = 0; i < destSampleCount; i++)
			            {
			                double value = calculator.CalculateValue(wrapper, t);
                            short convertedValue = (short)value;

                            writer.Write(convertedValue);

                            t += dt;
			            }
                    }
                }
            }
        }

        private Stream GetViolinSampleStream()
        {
            Stream stream = EmbeddedResourceHelper.GetEmbeddedResourceStream(this.GetType().Assembly, "TestResources", VIOLIN_16BIT_MONO_RAW_FILE_NAME);
            return stream;
        }
    }
}
