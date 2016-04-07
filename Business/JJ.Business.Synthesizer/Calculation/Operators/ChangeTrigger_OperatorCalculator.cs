﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class ChangeTrigger_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _calculationCalculator;
        private readonly OperatorCalculatorBase _resetCalculator;

        private double _previousTriggerValue;

        public ChangeTrigger_OperatorCalculator(
            OperatorCalculatorBase calculationCalculator,
            OperatorCalculatorBase resetCalculator)
            : base(new OperatorCalculatorBase[] { calculationCalculator, resetCalculator })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(calculationCalculator, () => calculationCalculator);
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(resetCalculator, () => resetCalculator);

            _calculationCalculator = calculationCalculator;
            _resetCalculator = resetCalculator;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double newTriggerValue = _resetCalculator.Calculate(time, channelIndex);

            if (_previousTriggerValue != newTriggerValue)
            {
                _calculationCalculator.Reset(time, channelIndex);

                _previousTriggerValue = newTriggerValue;
            }

            return _calculationCalculator.Calculate(time, channelIndex);
        }
    }
}