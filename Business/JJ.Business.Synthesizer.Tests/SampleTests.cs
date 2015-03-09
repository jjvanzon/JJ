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
using JJ.Persistence.Synthesizer;
using JJ.Persistence.Synthesizer.DefaultRepositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.IO;
using JJ.Business.Synthesizer.Calculation.AudioFileOutputs;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.Tests
{
    [TestClass]
    public class SampleTests
    {
        private const string VIOLIN_16BIT_MONO_RAW_FILE_NAME = "violin_16bit_mono.raw";
        private const string VIOLIN_16BIT_MONO_44100_WAV_FILE_NAME = "violin_16bit_mono_44100.wav";
        private const string OUTPUT_FILE_NAME = "AudioFileOutput.wav";

        [TestMethod]
        public void Test_Synthesizer_Sample()
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                Stream stream = GetViolin16BitMonoRawStream();
                byte[] bytes = StreamHelper.StreamToBytes(stream);

                SampleManager sampleManager = TestHelper.CreateSampleManager(context);
                Sample sample = sampleManager.CreateSample();
                sample.Bytes = bytes;

                IValidator sampleValidator = sampleManager.ValidateSample(sample);
                sampleValidator.Verify();

                double timeMultiplier = 1;
                double duration = sample.GetDuration();

                OperatorFactory x = TestHelper.CreateOperatorFactory(context);
                Outlet outlet = x.TimeMultiply(x.Sample(sample), x.Value(timeMultiplier));

                AudioFileOutputManager audioFileOutputManager = TestHelper.CreateAudioFileOutputManager(context);
                AudioFileOutput audioFileOutput = audioFileOutputManager.CreateAudioFileOutput();
                audioFileOutput.Duration = duration;
                audioFileOutput.FilePath = OUTPUT_FILE_NAME;
                audioFileOutput.AudioFileOutputChannels[0].LinkTo(outlet);

                IValidator audioFileOutputValidator = audioFileOutputManager.ValidateAudioFileOutput(audioFileOutput);
                audioFileOutputValidator.Verify();

                IAudioFileOutputCalculator calculator = AudioFileOutputCalculatorFactory.CreateAudioFileOutputCalculator(audioFileOutput);
                calculator.Execute();
            }
        }

        [TestMethod]
        public void Test_Synthesizer_Sample_WithWavHeader()
        {
            using (IContext context = PersistenceHelper.CreateContext())
            {
                IAudioFileFormatRepository audioFileFormatRepository = PersistenceHelper.CreateRepository<IAudioFileFormatRepository>(context);

                Stream stream = GetViolin16BitMono44100WavStream();
                byte[] bytes = StreamHelper.StreamToBytes(stream);

                SampleManager sampleManager = TestHelper.CreateSampleManager(context);
                Sample sample = sampleManager.CreateSample();
                sample.Bytes = bytes;
                sample.SetAudioFileFormatEnum(AudioFileFormatEnum.Wav, audioFileFormatRepository);

                OperatorFactory x = TestHelper.CreateOperatorFactory(context);
                Outlet outlet = x.Sample(sample);

                // Trigger SampleCalculation
                var calculator = new OperatorCalculator(0);
                double value = calculator.CalculateValue(outlet, 0);
            }
        }

        private Stream GetViolin16BitMonoRawStream()
        {
            Stream stream = EmbeddedResourceHelper.GetEmbeddedResourceStream(this.GetType().Assembly, "TestResources", VIOLIN_16BIT_MONO_RAW_FILE_NAME);
            return stream;
        }

        private Stream GetViolin16BitMono44100WavStream()
        {
            Stream stream = EmbeddedResourceHelper.GetEmbeddedResourceStream(this.GetType().Assembly, "TestResources", VIOLIN_16BIT_MONO_44100_WAV_FILE_NAME);
            return stream;
        }
    }
}
