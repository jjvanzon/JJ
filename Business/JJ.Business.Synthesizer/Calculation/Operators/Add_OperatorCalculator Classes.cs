﻿using System;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Add_OperatorCalculator : OperatorCalculatorBase
    {
        private OperatorCalculatorBase _operandACalculator;
        private OperatorCalculatorBase _operandBCalculator;

        public Add_OperatorCalculator(OperatorCalculatorBase operandACalculator, OperatorCalculatorBase operandBCalculator)
        {
            if (operandACalculator == null) throw new NullException(() => operandACalculator);
            if (operandBCalculator == null) throw new NullException(() => operandBCalculator);
            if (operandACalculator is Number_OperatorCalculator) throw new IsTypeException<Number_OperatorCalculator>(() => operandACalculator);
            if (operandBCalculator is Number_OperatorCalculator) throw new IsTypeException<Number_OperatorCalculator>(() => operandBCalculator);

            _operandACalculator = operandACalculator;
            _operandBCalculator = operandBCalculator;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double a = _operandACalculator.Calculate(time, channelIndex);
            double b = _operandBCalculator.Calculate(time, channelIndex);
            return a + b;
        }
    }

    internal class Add_WithConstOperandA_OperatorCalculator : OperatorCalculatorBase
    {
        private double _operandAValue;
        private OperatorCalculatorBase _operandBCalculator;

        public Add_WithConstOperandA_OperatorCalculator(double operandAValue, OperatorCalculatorBase operandBCalculator)
        {
            if (operandBCalculator == null) throw new NullException(() => operandBCalculator);
            if (operandBCalculator is Number_OperatorCalculator) throw new Exception("operandBCalculator cannot be a Value_OperatorCalculator.");

            _operandAValue = operandAValue;
            _operandBCalculator = operandBCalculator;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double b = _operandBCalculator.Calculate(time, channelIndex);
            return _operandAValue + b;
        }
    }

    internal class Add_WithConstOperandB_OperatorCalculator : OperatorCalculatorBase
    {
        private OperatorCalculatorBase _operandACalculator;
        private double _operandBValue;

        public Add_WithConstOperandB_OperatorCalculator(OperatorCalculatorBase operandACalculator, double operandBValue)
        {
            if (operandACalculator == null) throw new NullException(() => operandACalculator);
            if (operandACalculator is Number_OperatorCalculator) throw new Exception("operandACalculator cannot be a Value_OperatorCalculator.");

            _operandACalculator = operandACalculator;
            _operandBValue = operandBValue;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double a = _operandACalculator.Calculate(time, channelIndex);
            return a + _operandBValue;
        }
    }
}