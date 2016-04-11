﻿using System;
using JJ.Business.Synthesizer.Calculation.Samples;
using JJ.Business.Synthesizer.Enums;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal static class Sample_OperatorCalculator_Helper
    {
        public const double BASE_FREQUENCY = 440.0;
    }

    internal class Sample_WithVarFrequency_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _frequencyCalculator;
        private readonly ISampleCalculator _sampleCalculator;

        private double _phase;
        private double _previousTime;

        public Sample_WithVarFrequency_OperatorCalculator(OperatorCalculatorBase frequencyCalculator, ISampleCalculator sampleCalculator)
            : base(new OperatorCalculatorBase[] { frequencyCalculator })
        {
            if (frequencyCalculator == null) throw new NullException(() => frequencyCalculator);
            if (sampleCalculator == null) throw new NullException(() => sampleCalculator);

            _frequencyCalculator = frequencyCalculator;
            _sampleCalculator = sampleCalculator;
        }

        public override double Calculate(DimensionStack dimensionStack)
        {
            double time = dimensionStack.Get(DimensionEnum.Time);
            // TODO: Cast to int can fail.
            int channelIndex = (int)dimensionStack.Get(DimensionEnum.Channel);

            double frequency = _frequencyCalculator.Calculate(dimensionStack);
            double rate = frequency / Sample_OperatorCalculator_Helper.BASE_FREQUENCY;
            
            double dt = time - _previousTime;
            double phase = _phase + dt * rate;

            // Prevent phase from becoming a special number, rendering it unusable forever.
            if (Double.IsNaN(phase) || Double.IsInfinity(phase))
            {
                return Double.NaN;
            }
            _phase = phase;

            double value = _sampleCalculator.CalculateValue(_phase, channelIndex);

            _previousTime = time;

            return value;
        }

        public override void Reset(DimensionStack dimensionStack)
        {
            double time = dimensionStack.Get(DimensionEnum.Time);

            _previousTime = time;
            _phase = 0.0;

            base.Reset(dimensionStack);
        }
    }

    internal class Sample_WithConstFrequency_OperatorCalculator : OperatorCalculatorBase
    {
        private readonly ISampleCalculator _sampleCalculator;
        private readonly double _rate;

        private double _phase;
        private double _previousTime;

        public Sample_WithConstFrequency_OperatorCalculator(double frequency, ISampleCalculator sampleCalculator)
        {
            if (sampleCalculator == null) throw new NullException(() => sampleCalculator);
            _sampleCalculator = sampleCalculator;

            _rate = frequency / Sample_OperatorCalculator_Helper.BASE_FREQUENCY;
        }

        public override double Calculate(DimensionStack dimensionStack)
        {
            double time = dimensionStack.Get(DimensionEnum.Time);
            // TODO: Cast to int can fail.
            int channelIndex = (int)dimensionStack.Get(DimensionEnum.Channel);

            double dt = time - _previousTime;
            double phase = _phase + dt * _rate;

            // Prevent phase from becoming a special number, rendering it unusable forever.
            if (Double.IsNaN(phase) || Double.IsInfinity(phase))
            {
                return Double.NaN;
            }
            _phase = phase;

            double value = _sampleCalculator.CalculateValue(_phase, channelIndex);

            _previousTime = time;

            return value;
        }

        public override void Reset(DimensionStack dimensionStack)
        {
            double time = dimensionStack.Get(DimensionEnum.Time);

            _previousTime = time;
            _phase = 0.0;

            base.Reset(dimensionStack);
        }
    }

    internal class Sample_WithVarFrequency_MonoToStereo_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _frequencyCalculator;
        private readonly ISampleCalculator _sampleCalculator;

        private double _phase;
        private double _previousTime;

        public Sample_WithVarFrequency_MonoToStereo_OperatorCalculator(OperatorCalculatorBase frequencyCalculator, ISampleCalculator sampleCalculator)
            : base(new OperatorCalculatorBase[] { frequencyCalculator })
        {
            if (frequencyCalculator == null) throw new NullException(() => frequencyCalculator);
            if (sampleCalculator == null) throw new NullException(() => sampleCalculator);

            _frequencyCalculator = frequencyCalculator;
            _sampleCalculator = sampleCalculator;
        }

        public override double Calculate(DimensionStack dimensionStack)
        {
            double time = dimensionStack.Get(DimensionEnum.Time);

            double frequency = _frequencyCalculator.Calculate(dimensionStack);
            double rate = frequency / Sample_OperatorCalculator_Helper.BASE_FREQUENCY;

            double dt = time - _previousTime;
            double phase = _phase + dt * rate;

            // Prevent phase from becoming a special number, rendering it unusable forever.
            if (Double.IsNaN(phase) || Double.IsInfinity(phase))
            {
                return Double.NaN;
            }
            _phase = phase;

            // Return the single channel for both channels.
            double value = _sampleCalculator.CalculateValue(_phase, 0);

            _previousTime = time;

            return value;
        }

        public override void Reset(DimensionStack dimensionStack)
        {
            double time = dimensionStack.Get(DimensionEnum.Time);

            _previousTime = time;
            _phase = 0.0;

            base.Reset(dimensionStack);
        }
    }

    internal class Sample_WithConstFrequency_MonoToStereo_OperatorCalculator : OperatorCalculatorBase
    {
        private readonly ISampleCalculator _sampleCalculator;
        private readonly double _rate;

        private double _phase;
        private double _previousTime;

        public Sample_WithConstFrequency_MonoToStereo_OperatorCalculator(double frequency, ISampleCalculator sampleCalculator)
        {
            if (sampleCalculator == null) throw new NullException(() => sampleCalculator);
            _sampleCalculator = sampleCalculator;

            _rate = frequency / Sample_OperatorCalculator_Helper.BASE_FREQUENCY;
        }

        public override double Calculate(DimensionStack dimensionStack)
        {
            double time = dimensionStack.Get(DimensionEnum.Time);

            double dt = time - _previousTime;
            double phase = _phase + dt * _rate;

            // Prevent phase from becoming a special number, rendering it unusable forever.
            if (Double.IsNaN(phase) || Double.IsInfinity(phase))
            {
                return Double.NaN;
            }
            _phase = phase;

            // Return the single channel for both channels.
            double value = _sampleCalculator.CalculateValue(_phase, 0);

            _previousTime = time;

            return value;
        }

        public override void Reset(DimensionStack dimensionStack)
        {
            double time = dimensionStack.Get(DimensionEnum.Time);

            _previousTime = time;
            _phase = 0.0;

            base.Reset(dimensionStack);
        }
    }

    internal class Sample_WithVarFrequency_StereoToMono_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _frequencyCalculator;
        private readonly ISampleCalculator _sampleCalculator;

        private double _phase;
        private double _previousTime;

        public Sample_WithVarFrequency_StereoToMono_OperatorCalculator(OperatorCalculatorBase frequencyCalculator, ISampleCalculator sampleCalculator)
            : base(new OperatorCalculatorBase[] { frequencyCalculator })
        {
            if (frequencyCalculator == null) throw new NullException(() => frequencyCalculator);
            if (sampleCalculator == null) throw new NullException(() => sampleCalculator);

            _frequencyCalculator = frequencyCalculator;
            _sampleCalculator = sampleCalculator;
        }

        public override double Calculate(DimensionStack dimensionStack)
        {
            double time = dimensionStack.Get(DimensionEnum.Time);

            double frequency = _frequencyCalculator.Calculate(dimensionStack);
            double rate = frequency / Sample_OperatorCalculator_Helper.BASE_FREQUENCY;

            double dt = time - _previousTime;
            double phase = _phase + dt * rate;

            // Prevent phase from becoming a special number, rendering it unusable forever.
            if (Double.IsNaN(phase) || Double.IsInfinity(phase))
            {
                return Double.NaN;
            }
            _phase = phase;

            double value0 = _sampleCalculator.CalculateValue(_phase, 0);
            double value1 = _sampleCalculator.CalculateValue(_phase, 1);

            _previousTime = time;

            return value0 + value1;
        }

        public override void Reset(DimensionStack dimensionStack)
        {
            double time = dimensionStack.Get(DimensionEnum.Time);

            _previousTime = time;
            _phase = 0.0;

            base.Reset(dimensionStack);
        }
    }

    internal class Sample_WithConstFrequency_StereoToMono_OperatorCalculator : OperatorCalculatorBase
    {
        private readonly ISampleCalculator _sampleCalculator;
        private readonly double _rate;

        private double _phase;
        private double _previousTime;

        public Sample_WithConstFrequency_StereoToMono_OperatorCalculator(double frequency, ISampleCalculator sampleCalculator)
        {
            if (sampleCalculator == null) throw new NullException(() => sampleCalculator);
            _sampleCalculator = sampleCalculator;

            _rate = frequency / Sample_OperatorCalculator_Helper.BASE_FREQUENCY;
        }

        public override double Calculate(DimensionStack dimensionStack)
        {
            double time = dimensionStack.Get(DimensionEnum.Time);

            double dt = time - _previousTime;
            double phase = _phase + dt * _rate;

            // Prevent phase from becoming a special number, rendering it unusable forever.
            if (Double.IsNaN(phase) || Double.IsInfinity(phase))
            {
                return Double.NaN;
            }
            _phase = phase;

            double value0 = _sampleCalculator.CalculateValue(_phase, 0);
            double value1 = _sampleCalculator.CalculateValue(_phase, 1);

            _previousTime = time;

            return value0 + value1;
        }

        public override void Reset(DimensionStack dimensionStack)
        {
            double time = dimensionStack.Get(DimensionEnum.Time);

            _previousTime = time;
            _phase = 0.0;

            base.Reset(dimensionStack);
        }
    }
}
