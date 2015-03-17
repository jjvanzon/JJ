﻿using JJ.Framework.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.Calculation.Operators.Entities
{
    internal class Divide_WithoutOrigin_WithConstNumerator_Calculator : OperatorCalculatorBase
    {
        private double _numeratorValue;
        private OperatorCalculatorBase _denominatorCalculator;

        public Divide_WithoutOrigin_WithConstNumerator_Calculator(double numeratorValue, OperatorCalculatorBase denominatorCalculator)
        {
            if (denominatorCalculator == null) throw new NullException(() => denominatorCalculator);
            if (denominatorCalculator is Value_Calculator) throw new Exception("denominatorCalculator cannot be a Value_Calculator.");

            _numeratorValue = numeratorValue;
            _denominatorCalculator = denominatorCalculator;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double denominator = _denominatorCalculator.Calculate(time, channelIndex);

            if (denominator == 0)
            {
                return _numeratorValue;
            }

            return _numeratorValue / denominator;
        }
    }
}
