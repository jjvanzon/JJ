﻿using JJ.Framework.Reflection.Exceptions;
using System;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Power_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _baseCalculator;
        private readonly OperatorCalculatorBase _exponentCalculator;

        public Power_OperatorCalculator(OperatorCalculatorBase baseCalculator, OperatorCalculatorBase exponentCalculator)
            : base(new OperatorCalculatorBase[] { baseCalculator, exponentCalculator })
        {
            if (baseCalculator == null) throw new NullException(() => baseCalculator);
            if (baseCalculator is Number_OperatorCalculator) throw new IsTypeException<Number_OperatorCalculator>(() => baseCalculator);
            if (exponentCalculator == null) throw new NullException(() => exponentCalculator);
            if (exponentCalculator is Number_OperatorCalculator) throw new IsTypeException<Number_OperatorCalculator>(() => exponentCalculator);

            _baseCalculator = baseCalculator;
            _exponentCalculator = exponentCalculator;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double @base = _baseCalculator.Calculate(time, channelIndex);
            double exponent = _exponentCalculator.Calculate(time, channelIndex);
            return Math.Pow(@base, exponent);
        }
    }

    internal class Power_WithConstBase_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly double _baseValue;
        private readonly OperatorCalculatorBase _exponentCalculator;

        public Power_WithConstBase_OperatorCalculator(double baseValue, OperatorCalculatorBase exponentCalculator)
            : base(new OperatorCalculatorBase[] { exponentCalculator })
        {
            if (exponentCalculator == null) throw new NullException(() => exponentCalculator);
            if (exponentCalculator is Number_OperatorCalculator) throw new IsTypeException<Number_OperatorCalculator>(() => exponentCalculator);

            _baseValue = baseValue;
            _exponentCalculator = exponentCalculator;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double exponent = _exponentCalculator.Calculate(time, channelIndex);
            return Math.Pow(_baseValue, exponent);
        }
    }

    internal class Power_WithConstExponent_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _baseCalculator;
        private readonly double _exponentValue;

        public Power_WithConstExponent_OperatorCalculator(OperatorCalculatorBase baseCalculator, double exponentValue)
            : base(new OperatorCalculatorBase[] { baseCalculator })
        {
            if (baseCalculator == null) throw new NullException(() => baseCalculator);
            if (baseCalculator is Number_OperatorCalculator) throw new IsTypeException<Number_OperatorCalculator>(() => baseCalculator);

            _baseCalculator = baseCalculator;
            _exponentValue = exponentValue;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double @base = _baseCalculator.Calculate(time, channelIndex);
            return Math.Pow(@base, _exponentValue);
        }
    }
}
