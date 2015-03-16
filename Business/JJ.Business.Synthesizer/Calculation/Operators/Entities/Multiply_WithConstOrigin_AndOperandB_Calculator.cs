﻿using JJ.Framework.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.Calculation.Operators.Entities
{
    internal class Multiply_WithConstOrigin_AndOperandB_Calculator : OperatorCalculatorBase
    {
        private OperatorCalculatorBase _operandACalculator;
        private double _operandBValue;
        private double _originValue;

        public Multiply_WithConstOrigin_AndOperandB_Calculator(
            OperatorCalculatorBase operandACalculator, 
            double operandBValue, 
            double originValue)
        {
            if (operandACalculator == null) throw new NullException(() => operandACalculator);
            if (originValue == null) throw new NullException(() => originValue);

            _operandACalculator = operandACalculator;
            _operandBValue = operandBValue;
            _originValue = originValue;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double a = _operandACalculator.Calculate(time, channelIndex);
            return (a - _originValue) * _operandBValue + _originValue;
        }
    }
}