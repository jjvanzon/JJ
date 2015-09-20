﻿using JJ.Framework.Reflection.Exceptions;
using System;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Divide_WithoutOrigin_OperatorCalculator : OperatorCalculatorBase
    {
        private OperatorCalculatorBase _numeratorCalculator;
        private OperatorCalculatorBase _denominatorCalculator;

        public Divide_WithoutOrigin_OperatorCalculator(OperatorCalculatorBase numeratorCalculator, OperatorCalculatorBase denominatorCalculator)
        {
            if (numeratorCalculator == null) throw new NullException(() => numeratorCalculator);
            if (numeratorCalculator is Number_OperatorCalculator) throw new Exception("numeratorCalculator cannot be a Value_OperatorCalculator.");
            if (denominatorCalculator == null) throw new NullException(() => denominatorCalculator);
            if (denominatorCalculator is Number_OperatorCalculator) throw new Exception("denominatorCalculator cannot be a Value_OperatorCalculator.");

            _numeratorCalculator = numeratorCalculator;
            _denominatorCalculator = denominatorCalculator;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double numerator = _numeratorCalculator.Calculate(time, channelIndex);
            double denominator = _denominatorCalculator.Calculate(time, channelIndex);

            if (denominator == 0)
            {
                return numerator;
            }

            return numerator / denominator;
        }
    }
}
