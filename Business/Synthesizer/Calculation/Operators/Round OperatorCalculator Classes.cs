﻿using System;
using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.CopiedCode.FromFramework;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Round_OperatorCalculator_VarSignal_VarStep_VarOffset : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _stepCalculator;
        private readonly OperatorCalculatorBase _offsetCalculator;

        public Round_OperatorCalculator_VarSignal_VarStep_VarOffset(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase stepCalculator,
            OperatorCalculatorBase offsetCalculator)
            : base(new[] { signalCalculator, stepCalculator, offsetCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertChildOperatorCalculator(stepCalculator, () => stepCalculator);
            OperatorCalculatorHelper.AssertChildOperatorCalculator(offsetCalculator, () => offsetCalculator);

            _signalCalculator = signalCalculator;
            _stepCalculator = stepCalculator;
            _offsetCalculator = offsetCalculator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double signal = _signalCalculator.Calculate();
            double step = _stepCalculator.Calculate();
            double offset = _offsetCalculator.Calculate();

            double result = MathHelper.RoundWithStep(signal, step, offset);
            return result;
        }
    }

    internal class Round_OperatorCalculator_VarSignal_VarStep_ConstOffset : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _stepCalculator;
        private readonly double _offset;

        public Round_OperatorCalculator_VarSignal_VarStep_ConstOffset(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase stepCalculator,
            double offset)
            : base(new[] { signalCalculator, stepCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertChildOperatorCalculator(stepCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertRoundOffset(offset);

            _signalCalculator = signalCalculator;
            _stepCalculator = stepCalculator;
            _offset = offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double signal = _signalCalculator.Calculate();
            double step = _stepCalculator.Calculate();

            double result = MathHelper.RoundWithStep(signal, step, _offset);
            return result;
        }
    }

    internal class Round_OperatorCalculator_VarSignal_VarStep_ZeroOffset : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _stepCalculator;

        public Round_OperatorCalculator_VarSignal_VarStep_ZeroOffset(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase stepCalculator)
            : base(new[] { signalCalculator, stepCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertChildOperatorCalculator(stepCalculator, () => stepCalculator);

            _signalCalculator = signalCalculator;
            _stepCalculator = stepCalculator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double signal = _signalCalculator.Calculate();
            double step = _stepCalculator.Calculate();

            double result = MathHelper.RoundWithStep(signal, step);
            return result;
        }
    }

    internal class Round_OperatorCalculator_VarSignal_ConstStep_VarOffset : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _step;
        private readonly OperatorCalculatorBase _offsetCalculator;

        public Round_OperatorCalculator_VarSignal_ConstStep_VarOffset(
            OperatorCalculatorBase signalCalculator,
            double step,
            OperatorCalculatorBase offsetCalculator)
            : base(new[] { signalCalculator, offsetCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertRoundStep(step);
            OperatorCalculatorHelper.AssertChildOperatorCalculator(offsetCalculator, () => offsetCalculator);

            _signalCalculator = signalCalculator;
            _step = step;
            _offsetCalculator = offsetCalculator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double signal = _signalCalculator.Calculate();
            double offset = _offsetCalculator.Calculate();

            double result = MathHelper.RoundWithStep(signal, _step, offset);
            return result;
        }
    }

    internal class Round_OperatorCalculator_VarSignal_ConstStep_ConstOffset : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _step;
        private readonly double _offset;

        public Round_OperatorCalculator_VarSignal_ConstStep_ConstOffset(
            OperatorCalculatorBase signalCalculator,
            double step,
            double offset)
            : base(new[] { signalCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertRoundStep(step);
            OperatorCalculatorHelper.AssertRoundOffset(offset);

            _signalCalculator = signalCalculator;
            _step = step;
            _offset = offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double signal = _signalCalculator.Calculate();

            double result = MathHelper.RoundWithStep(signal, _step, _offset);
            return result;
        }
    }

    internal class Round_OperatorCalculator_VarSignal_ConstStep_ZeroOffset : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _step;

        public Round_OperatorCalculator_VarSignal_ConstStep_ZeroOffset(
            OperatorCalculatorBase signalCalculator,
            double step)
            : base(new[] { signalCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertRoundStep(step);

            _signalCalculator = signalCalculator;
            _step = step;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double signal = _signalCalculator.Calculate();

            double result = MathHelper.RoundWithStep(signal, _step);
            return result;
        }
    }

    // Special cases

    internal class Round_OperatorCalculator_ConstSignal : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly double _signal;
        private readonly OperatorCalculatorBase _stepCalculator;
        private readonly OperatorCalculatorBase _offsetCalculator;

        public Round_OperatorCalculator_ConstSignal(
            double signal,
            OperatorCalculatorBase stepCalculator,
            OperatorCalculatorBase offsetCalculator)
            : base(new[] { offsetCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(stepCalculator, () => stepCalculator);
            OperatorCalculatorHelper.AssertChildOperatorCalculator(offsetCalculator, () => offsetCalculator);

            _signal = signal;
            _stepCalculator = stepCalculator;
            _offsetCalculator = offsetCalculator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double step = _stepCalculator.Calculate();
            double offset = _offsetCalculator.Calculate();

            double result = MathHelper.RoundWithStep(_signal, step, offset);
            return result;
        }
    }

    internal class Round_OperatorCalculator_VarSignal_StepOne_OffsetZero : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;

        public Round_OperatorCalculator_VarSignal_StepOne_OffsetZero(OperatorCalculatorBase signalCalculator)
            : base(new[] { signalCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);

            _signalCalculator = signalCalculator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double signal = _signalCalculator.Calculate();

            double result = Math.Round(signal, MidpointRounding.AwayFromZero);
            return result;
        }
    }
}