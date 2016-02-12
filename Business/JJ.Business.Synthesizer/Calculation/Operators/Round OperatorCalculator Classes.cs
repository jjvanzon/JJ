﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Mathematics;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Round_VarStep_VarOffSet_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _stepCalculator;
        private readonly OperatorCalculatorBase _offsetCalculator;

        public Round_VarStep_VarOffSet_OperatorCalculator(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase stepCalculator,
            OperatorCalculatorBase offsetCalculator)
            : base(new OperatorCalculatorBase[] { signalCalculator, stepCalculator, offsetCalculator })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(stepCalculator, () => stepCalculator);
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(offsetCalculator, () => offsetCalculator);

            _signalCalculator = signalCalculator;
            _stepCalculator = stepCalculator;
            _offsetCalculator = offsetCalculator;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double signal = _signalCalculator.Calculate(time, channelIndex);
            double step = _stepCalculator.Calculate(time, channelIndex);
            double offset = _offsetCalculator.Calculate(time, channelIndex);

            double result = Maths.RoundWithStep(signal, step, offset);
            return result;
        }
    }

    internal class Round_VarStep_ConstOffSet_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _stepCalculator;
        private readonly double _offset;

        public Round_VarStep_ConstOffSet_OperatorCalculator(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase stepCalculator,
            double offset)
            : base(new OperatorCalculatorBase[] { signalCalculator, stepCalculator})
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(stepCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertRoundOffset(offset);

            _signalCalculator = signalCalculator;
            _stepCalculator = stepCalculator;
            _offset = offset;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double signal = _signalCalculator.Calculate(time, channelIndex);
            double step = _stepCalculator.Calculate(time, channelIndex);

            double result = Maths.RoundWithStep(signal, step, _offset);
            return result;
        }
    }

    internal class Round_VarStep_ZeroOffSet_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _stepCalculator;

        public Round_VarStep_ZeroOffSet_OperatorCalculator(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase stepCalculator)
            : base(new OperatorCalculatorBase[] { signalCalculator, stepCalculator})
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(stepCalculator, () => stepCalculator);

            _signalCalculator = signalCalculator;
            _stepCalculator = stepCalculator;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double signal = _signalCalculator.Calculate(time, channelIndex);
            double step = _stepCalculator.Calculate(time, channelIndex);

            double result = Maths.RoundWithStep(signal, step);
            return result;
        }
    }

    internal class Round_ConstStep_VarOffSet_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _step;
        private readonly OperatorCalculatorBase _offsetCalculator;

        public Round_ConstStep_VarOffSet_OperatorCalculator(
            OperatorCalculatorBase signalCalculator,
            double step,
            OperatorCalculatorBase offsetCalculator)
            : base(new OperatorCalculatorBase[] { signalCalculator, offsetCalculator })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertRoundStep(step);
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(offsetCalculator, () => offsetCalculator);

            _signalCalculator = signalCalculator;
            _step = step;
            _offsetCalculator = offsetCalculator;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double signal = _signalCalculator.Calculate(time, channelIndex);
            double offset = _offsetCalculator.Calculate(time, channelIndex);

            double result = Maths.RoundWithStep(signal, _step, offset);
            return result;
        }
    }

    internal class Round_ConstStep_ConstOffSet_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _step;
        private readonly double _offset;

        public Round_ConstStep_ConstOffSet_OperatorCalculator(
            OperatorCalculatorBase signalCalculator,
            double step,
            double offset)
            : base(new OperatorCalculatorBase[] { signalCalculator })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertRoundStep(step);
            OperatorCalculatorHelper.AssertRoundOffset(offset);

            _signalCalculator = signalCalculator;
            _step = step;
            _offset = offset;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double signal = _signalCalculator.Calculate(time, channelIndex);

            double result = Maths.RoundWithStep(signal, _step, _offset);
            return result;
        }
    }

    internal class Round_ConstStep_ZeroOffSet_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _step;

        public Round_ConstStep_ZeroOffSet_OperatorCalculator(
            OperatorCalculatorBase signalCalculator,
            double step)
            : base(new OperatorCalculatorBase[] { signalCalculator })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertRoundStep(step);

            _signalCalculator = signalCalculator;
            _step = step;
        }

        public override double Calculate(double time, int channelIndex)
        {
            double signal = _signalCalculator.Calculate(time, channelIndex);

            double result = Maths.RoundWithStep(signal, _step);
            return result;
        }
    }

}
