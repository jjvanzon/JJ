﻿using JJ.Framework.IO;
using JJ.Business.Synthesizer.Validation;
using JJ.Business.Synthesizer.Extensions;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using System;
using System.Linq;
using JJ.Business.Synthesizer.Enums;
using System.IO;
using JJ.Business.Synthesizer.Helpers;
using JJ.Framework.Common;
using JJ.Business.Synthesizer.Calculation.Patches;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Business.Synthesizer.Configuration;
using System.Collections.Generic;
using JJ.Framework.Common.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.AudioFileOutputs
{
    internal abstract class AudioFileOutputCalculatorBase : IAudioFileOutputCalculator
    {
        private readonly ICurveRepository _curveRepository;
        private readonly ISampleRepository _sampleRepository;
        private readonly IPatchRepository _patchRepository;

        private readonly IPatchCalculator _patchCalculator;

        /// <param name="patchRepository">
        /// Nullable. 
        /// If provided this saves you the overhead of re-initializing the patch calculation every time you write a file.
        /// </param>
        public AudioFileOutputCalculatorBase(
            IPatchCalculator patchCalculator,
            ICurveRepository curveRepository, 
            ISampleRepository sampleRepository,
            IPatchRepository patchRepository)
        {
            if (patchCalculator == null) throw new NullException(() => patchCalculator);
            if (curveRepository == null) throw new NullException(() => curveRepository);
            if (sampleRepository == null) throw new NullException(() => sampleRepository);
            if (patchRepository == null) throw new NullException(() => patchRepository);

            _patchCalculator = patchCalculator;
            _curveRepository = curveRepository;
            _sampleRepository = sampleRepository;
            _patchRepository = patchRepository;
        }
        
        public void Execute(AudioFileOutput audioFileOutput)
        {
            // Assert
            if (audioFileOutput == null) throw new NullException(() => audioFileOutput);
            if (String.IsNullOrEmpty(audioFileOutput.FilePath)) throw new NullOrEmptyException(() => audioFileOutput.FilePath);
            IValidator validator = new AudioFileOutputValidator(audioFileOutput);
            validator.Assert();

            // Prepare the calculators
            IList<Outlet> outlets = audioFileOutput.AudioFileOutputChannels
                                                    .OrderBy(x => x.IndexNumber)
                                                    .Select(x => x.Outlet)
                                                    .ToArray();

            var noiseCalculator = new NoiseCalculator(audioFileOutput.SamplingRate);
            
            // Calculate output and write file
            int channelCount = audioFileOutput.GetChannelCount();

            double dt = 1.0 / audioFileOutput.SamplingRate / audioFileOutput.TimeMultiplier;
            double endTime = audioFileOutput.GetEndTime();

            using (Stream stream = new FileStream(audioFileOutput.FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    // Write header
                    AudioFileFormatEnum audioFileFormatEnum = audioFileOutput.GetAudioFileFormatEnum();
                    switch (audioFileFormatEnum)
                    {
                        case AudioFileFormatEnum.Wav:
                            var audioFileInfo = new AudioFileInfo
                            {
                                SamplingRate = audioFileOutput.SamplingRate,
                                BytesPerValue = SampleDataTypeHelper.SizeOf(audioFileOutput.SampleDataType),
                                ChannelCount = channelCount,
                                SampleCount = (int)(endTime / dt)
                            };

                            WavHeaderStruct wavHeaderStruct = WavHeaderManager.CreateWavHeaderStruct(audioFileInfo);
                            writer.WriteStruct(wavHeaderStruct);
                            break;

                        case AudioFileFormatEnum.Raw:
                            // Do nothing
                            break;

                        default:
                            throw new ValueNotSupportedException(audioFileFormatEnum);
                    }

                    double adjustedAmplifier = GetAmplifierAdjustedToSampleDataType(audioFileOutput);

                    // Write Samples
                    var dimensionStack = new DimensionStack();
                    for (double t = 0; t <= endTime; t += dt)
                    {
                        dimensionStack.Push(DimensionEnum.Time, t);
                        for (int i = 0; i < channelCount; i++)
                        {
                            dimensionStack.Push(DimensionEnum.Channel, i);
                            double value = _patchCalculator.Calculate(dimensionStack);
                            dimensionStack.Pop(DimensionEnum.Channel);

                            value *= adjustedAmplifier;

                            WriteValue(writer, value);
                        }
                        dimensionStack.Pop(DimensionEnum.Time);
                    }
                }
            }
        }

        protected abstract double GetAmplifierAdjustedToSampleDataType(AudioFileOutput audioFileOutput);
        protected abstract void WriteValue(BinaryWriter binaryWriter, double value);
    }
}
