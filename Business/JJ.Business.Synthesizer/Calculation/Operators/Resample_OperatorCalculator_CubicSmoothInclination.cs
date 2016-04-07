﻿using JJ.Framework.Mathematics;
using JJ.Framework.Reflection.Exceptions;
using System;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Resample_OperatorCalculator_CubicSmoothInclination : OperatorCalculatorBase_WithChildCalculators
    {
        private const double MINIMUM_SAMPLING_RATE = 0.01666666666666667; // Once a minute

        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _samplingRateCalculator;

        private double _xMinus1;
        private double _x0;
        private double _x1;
        private double _x2;
        private double _dx1;
        private double _yMinus1;
        private double _y0;
        private double _y1;
        private double _y2;

        public Resample_OperatorCalculator_CubicSmoothInclination(
            OperatorCalculatorBase signalCalculator, 
            OperatorCalculatorBase samplingRateCalculator)
            : base(new OperatorCalculatorBase[] { signalCalculator, samplingRateCalculator })
        {
            if (signalCalculator == null) throw new NullException(() => signalCalculator);
            if (signalCalculator is Number_OperatorCalculator) throw new IsNotTypeException<Number_OperatorCalculator>(() => signalCalculator);
            if (samplingRateCalculator == null) throw new NullException(() => samplingRateCalculator);
            // TODO: Resample with constant sampling rate does not have specialized calculators yet. Reactivate code line after those specialized calculators have been programmed.
            //if (samplingRateCalculator is Number_OperatorCalculator) throw new IsNotTypeException<Number_OperatorCalculator>(() => samplingRateCalculator);

            _signalCalculator = signalCalculator;
            _samplingRateCalculator = samplingRateCalculator;

            ResetNonRecursive(OperatorCalculatorHelper.DEFAULT_TIME, OperatorCalculatorHelper.DEFAULT_CHANNEL_INDEX);
        }

        public override double Calculate(double time, int channelIndex)
        {
            // TODO: What if times goes in reverse?
            // TODO: What if _x0 or _x1 are way off? How will it correct itself?
            double x = time;

            // When x goes past _x1 you must shift things.
            if (x >= _x1)
            {
                // Shift the samples to the left.
                _xMinus1 = _x0;
                _x0 = _x1;
                _x1 = _x2;
                _yMinus1 = _y0;
                _y0 = _y1;
                _y1 = _y2;

                // Determine next sample
                double samplingRate = GetSamplingRate(_x1, channelIndex);
                _dx1 = 1.0 / samplingRate;
                _x2 = _x1 + _dx1;
                _y2 = _signalCalculator.Calculate(_x2, channelIndex);
            }

            double y = Interpolator.Interpolate_Cubic_SmoothInclination(
                _xMinus1, _x0, _x1, _x2,
                _yMinus1, _y0, _y1, _y2,
                x);

            return y;
        }

        /// <summary> Gets the sampling rate, converts it to an absolute number and ensures a minimum value. </summary>
        private double GetSamplingRate(double x, int channelIndex)
        {
            double samplingRate = _samplingRateCalculator.Calculate(x, channelIndex);

            samplingRate = Math.Abs(samplingRate);

            if (samplingRate < MINIMUM_SAMPLING_RATE)
            {
                samplingRate = MINIMUM_SAMPLING_RATE;
            }

            return samplingRate;
        }

        public override void Reset(double time, int channelIndex)
        {
            ResetNonRecursive(time, channelIndex);

            base.Reset(time, channelIndex);
        }

        private void ResetNonRecursive(double time, int channelIndex)
        {
            _xMinus1 = CalculationHelper.VERY_LOW_VALUE;
            _x0 = time - Double.Epsilon;
            _x1 = time;
            _x2 = time + Double.Epsilon;
            _dx1 = Double.Epsilon;

            // Assume values begin at 0
            _yMinus1 = 0;
            _y0 = 0;
            _y1 = 0;
            _y2 = 0;
        }
    }
}
