﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Reflection.Exceptions;
using NAudio.Dsp;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class LowPass_VarMaxFrequency_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private const float ASSUMED_SAMPLE_RATE = 44100;
        private const float DEFAULT_MAX_FREQUENCY = 22050;
        private const float DEFAULT_BAND_WIDTH = 1;

        private readonly BiQuadFilter _biQuadFilter;

        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _maxFrequencyCalculator;

        public LowPass_VarMaxFrequency_OperatorCalculator(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase maxFrequencyCalculator)
            : base(new OperatorCalculatorBase[] { signalCalculator, maxFrequencyCalculator })
        {
            if (signalCalculator == null) throw new NullException(() => signalCalculator);
            if (signalCalculator is Number_OperatorCalculator) throw new IsTypeException<Number_OperatorCalculator>(() => signalCalculator);
            if (maxFrequencyCalculator == null) throw new NullException(() => maxFrequencyCalculator);
            if (maxFrequencyCalculator is Number_OperatorCalculator) throw new IsTypeException<Number_OperatorCalculator>(() => maxFrequencyCalculator);

            _signalCalculator = signalCalculator;
            _maxFrequencyCalculator = maxFrequencyCalculator;

            _biQuadFilter = BiQuadFilter.LowPassFilter(ASSUMED_SAMPLE_RATE, DEFAULT_MAX_FREQUENCY, DEFAULT_BAND_WIDTH);
        }

        public override double Calculate(double time, int channelIndex)
        {
            double maxFrequency = _maxFrequencyCalculator.Calculate(time, channelIndex);
            double signal = _signalCalculator.Calculate(time, channelIndex);

            _biQuadFilter.SetLowPassFilter(ASSUMED_SAMPLE_RATE, (float)maxFrequency, DEFAULT_BAND_WIDTH);

            float value = _biQuadFilter.Transform((float)signal);

            return value;
        }
    }

    internal class LowPass_ConstMaxFrequency_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private const float ASSUMED_SAMPLE_RATE = 44100;
        private const float DEFAULT_BAND_WIDTH = 1;

        private readonly BiQuadFilter _biQuadFilter;

        private readonly OperatorCalculatorBase _signalCalculator;

        public LowPass_ConstMaxFrequency_OperatorCalculator(
            OperatorCalculatorBase signalCalculator,
            double maxFrequency)
            : base(new OperatorCalculatorBase[] { signalCalculator })
        {
            if (signalCalculator == null) throw new NullException(() => signalCalculator);
            if (signalCalculator is Number_OperatorCalculator) throw new IsTypeException<Number_OperatorCalculator>(() => signalCalculator);

            _signalCalculator = signalCalculator;

            _biQuadFilter = BiQuadFilter.LowPassFilter(ASSUMED_SAMPLE_RATE, (float)maxFrequency, DEFAULT_BAND_WIDTH);
        }

        public override double Calculate(double time, int channelIndex)
        {
            double signal = _signalCalculator.Calculate(time, channelIndex);

            float value = _biQuadFilter.Transform((float)signal);

            return value;
        }
    }

}
