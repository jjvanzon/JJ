﻿using JJ.Framework.Reflection.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Business.Synthesizer.Calculation.Operators.Entities
{
    internal class TimeMultiply_WithoutOrigin_WithConstTimeMultiplier_Calculator : OperatorCalculatorBase
    {
        private OperatorCalculatorBase _signalCalculator;
        private double _timeMultiplierValue;

        public TimeMultiply_WithoutOrigin_WithConstTimeMultiplier_Calculator(
            OperatorCalculatorBase signalCalculator,
            double timeMultiplierValue)
        {
            if (signalCalculator == null) throw new NullException(() => signalCalculator);
            if (signalCalculator is Value_Calculator) throw new Exception("signalCalculator cannot be a ValueCalculator.");
            if (timeMultiplierValue == 0) throw new Exception("timeMultiplierValue cannot be 0.");

            _signalCalculator = signalCalculator;
            _timeMultiplierValue = timeMultiplierValue;
        }

        public override double Calculate(double time, int channelIndex)
        {
            // IMPORTANT: To divide the time in the output, you have to multiply the time of the input.
            double transformedTime = time / _timeMultiplierValue;
            double result = _signalCalculator.Calculate(transformedTime, channelIndex);
            return result;
        }
    }
}
