﻿using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using JJ.Business.Synthesizer.Calculation.Patches;
using System.Threading;
using JJ.Framework.Reflection.Exceptions;
using JJ.Business.Synthesizer.Calculation;
using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;

namespace JJ.Presentation.Synthesizer.NAudio
{
    internal class AudioOutputSampleProvider : ISampleProvider
    {
        private readonly WaveFormat _waveFormat;
        private readonly IPatchCalculatorContainer _patchCalculatorContainer;
        private readonly double _sampleDuration;
        private readonly DimensionStack _dimensionStack;
        private readonly int _channelCount;

        /// <summary> Public field for performance. </summary>
        public double _time;
        public volatile bool _isRunning;

        public AudioOutputSampleProvider(IPatchCalculatorContainer patchCalculatorContainer, AudioOutput audioOutput)
        {
            if (patchCalculatorContainer == null) throw new NullException(() => patchCalculatorContainer);
            if (audioOutput == null) throw new NullException(() => audioOutput);
            if (audioOutput.SamplingRate == 0) throw new ZeroException(() => audioOutput.SamplingRate);

            _patchCalculatorContainer = patchCalculatorContainer;

            _sampleDuration = audioOutput.GetSampleDuration();
            _channelCount = audioOutput.SpeakerSetup.SpeakerSetupChannels.Count;

            _waveFormat = CreateWaveFormat(audioOutput);

            _dimensionStack = new DimensionStack();
        }

        private WaveFormat CreateWaveFormat(AudioOutput audioOutput)
        {
            WaveFormat waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(
                audioOutput.SamplingRate, 
                audioOutput.SpeakerSetup.SpeakerSetupChannels.Count);

            return waveFormat;
        }

        WaveFormat ISampleProvider.WaveFormat
        {
            get { return _waveFormat; }
        }

        int ISampleProvider.Read(float[] buffer, int offset, int count)
        {
            ReaderWriterLockSlim lck = _patchCalculatorContainer.Lock;

            lck.EnterReadLock();
            try
            {
                IPatchCalculator patchCalculator = _patchCalculatorContainer.Calculator;

                if (!_isRunning || patchCalculator == null)
                {
                    Array.Clear(buffer, 0, buffer.Length);
                    return count;
                }

                int frameOffset = offset / _channelCount;
                int frameCount = count / _channelCount;

                // First index is channel, second index is frame.
                double[][] values = new double[_channelCount][];
                for (int channelIndex = 0; channelIndex < _channelCount; channelIndex++)
                {
                    _dimensionStack.Set(DimensionEnum.Channel, channelIndex);

                    values[channelIndex] = patchCalculator.Calculate(_time, _sampleDuration, frameCount, _dimensionStack);
                }

                int i = 0;
                for (int frameIndex = frameOffset; frameIndex < frameCount; frameIndex++)
                {
                    for (int channelIndex = 0; channelIndex < _channelCount; channelIndex++)
                    {
                        double value;

                        value = values[channelIndex][frameIndex];
                            
                        // winmm will trip over NaN.
                        if (Double.IsNaN(value))
                        {
                            value = 0;
                        }

                        // TODO: This seems unsafe. What happens if the cast is invalid?
                        buffer[i] = (float)value;

                        i++;
                    }
                }

                _time += _sampleDuration * count;

                return count;
            }
            finally
            {
                lck.ExitReadLock();
            }
        }
    }
}