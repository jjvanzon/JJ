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
using JJ.Business.Synthesizer.Calculation.Patches;
using System.Collections.Generic;
using JJ.Framework.Common.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.AudioFileOutputs
{
    internal abstract class AudioFileOutputCalculatorBase : IAudioFileOutputCalculator
    {
        private const int TIME_DIMENSION_INDEX = (int)DimensionEnum.Time;
        private const int CHANNEL_DIMENSION_INDEX = (int)DimensionEnum.Channel;

        private readonly IPatchCalculator _patchCalculator;

        public AudioFileOutputCalculatorBase(IPatchCalculator patchCalculator)
        {
            if (patchCalculator == null) throw new NullException(() => patchCalculator);

            _patchCalculator = patchCalculator;
        }
        
        public void Execute(AudioFileOutput audioFileOutput)
        {
            // Assert
            if (audioFileOutput == null) throw new NullException(() => audioFileOutput);
            if (String.IsNullOrEmpty(audioFileOutput.FilePath)) throw new NullOrEmptyException(() => audioFileOutput.FilePath);
            IValidator validator = new AudioFileOutputValidator(audioFileOutput);
            validator.Assert();

            // Prepare the calculators
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
                                FrameCount = (int)(endTime / dt)
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
                        dimensionStack.Set(TIME_DIMENSION_INDEX, t);
                        for (int i = 0; i < channelCount; i++)
                        {
                            dimensionStack.Set(CHANNEL_DIMENSION_INDEX, i);
                            double value = _patchCalculator.Calculate(dimensionStack);

                            value *= adjustedAmplifier;

                            WriteValue(writer, value);
                        }
                    }
                }
            }
        }

        protected abstract double GetAmplifierAdjustedToSampleDataType(AudioFileOutput audioFileOutput);
        protected abstract void WriteValue(BinaryWriter binaryWriter, double value);
    }
}
