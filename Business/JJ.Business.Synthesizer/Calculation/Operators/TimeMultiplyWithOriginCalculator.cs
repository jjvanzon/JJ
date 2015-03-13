﻿using JJ.Framework.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class TimeMultiplyWithOriginCalculator : OperatorCalculatorBase
    {
        private OperatorCalculatorBase _signalCalculator;
        private OperatorCalculatorBase _timeMultiplierCalculator;
        private OperatorCalculatorBase _originOutletCalculator;

        public TimeMultiplyWithOriginCalculator(OperatorCalculatorBase signalCalculator, OperatorCalculatorBase timeMultiplierCalculator, OperatorCalculatorBase originOutletCalculator)
        {
            if (signalCalculator == null) throw new NullException(() => signalCalculator);
            if (timeMultiplierCalculator == null) throw new NullException(() => timeMultiplierCalculator);
            if (originOutletCalculator == null) throw new NullException(() => originOutletCalculator);

            _signalCalculator = signalCalculator;
            _timeMultiplierCalculator = timeMultiplierCalculator;
            _originOutletCalculator = originOutletCalculator;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double timeMultiplier = _timeMultiplierCalculator.Calculate(time, channelIndex);

            // Time multiplier 0? See that as multiplier = 1 or rather: just pass through signal.
            if (timeMultiplier == 0)
            {
                double signal = _signalCalculator.Calculate(time, channelIndex);
                return signal;
            }

            // IMPORTANT: To multiply the time in the output, you have to divide the time of the input.
            double origin = _originOutletCalculator.Calculate(time, channelIndex);
            double transformedTime = (time - origin) / timeMultiplier + origin;
            double result2 = _signalCalculator.Calculate(transformedTime, channelIndex);
            return result2;
        }
    }
}
