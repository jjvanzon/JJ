﻿using JJ.Framework.Reflection.Exceptions;
using System;
using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Reverse_WithVarSpeed_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _speedCalculator;
        private readonly DimensionStack _dimensionStack;
        private readonly int _currentDimensionStackIndex;
        private readonly int _previousDimensionStackIndex;

        private double _phase;
        private double _previousPosition;

        public Reverse_WithVarSpeed_OperatorCalculator(
            OperatorCalculatorBase signalCalculator, 
            OperatorCalculatorBase speedCalculator,
            DimensionStack dimensionStack)
            : base(new OperatorCalculatorBase[] 
            {
                signalCalculator,
                speedCalculator
            })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(speedCalculator, () => speedCalculator);
            OperatorCalculatorHelper.AssertDimensionStack_ForWriters(dimensionStack);

            _signalCalculator = signalCalculator;
            _speedCalculator = speedCalculator;
            _dimensionStack = dimensionStack;
            _currentDimensionStackIndex = dimensionStack.CurrentIndex;
            _previousDimensionStackIndex = dimensionStack.CurrentIndex - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double position = _dimensionStack.Get(_previousDimensionStackIndex);

            _phase = GetTransformedPosition(position);

            _dimensionStack.Set(_currentDimensionStackIndex, _phase);

            double result = _signalCalculator.Calculate();

            _previousPosition = position;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double GetTransformedPosition(double position)
        {
            double speed = _speedCalculator.Calculate();

            double positionChange = position - _previousPosition;

            // IMPORTANT: To divide the time in the output, you have to multiply the time of the input.
            double phase = _phase - positionChange * speed;

            return phase;
        }

        public override void Reset()
        {
            double position = _dimensionStack.Get(_previousDimensionStackIndex);

            _previousPosition = position;
            _phase = 0.0;

            double transformedPosition = GetTransformedPosition(position);

            _dimensionStack.Set(_currentDimensionStackIndex, transformedPosition);

            base.Reset();
        }
    }

    internal class Reverse_WithConstSpeed_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _speed;
        private readonly DimensionStack _dimensionStack;
        private readonly int _currentDimensionStackIndex;
        private readonly int _previousDimensionStackIndex;

        public Reverse_WithConstSpeed_OperatorCalculator(
            OperatorCalculatorBase signalCalculator, 
            double speed,
            DimensionStack dimensionStack)
            : base(new OperatorCalculatorBase[] { signalCalculator })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            if (speed == 0) throw new ZeroException(() => speed);
            if (Double.IsNaN(speed)) throw new NaNException(() => speed);
            if (Double.IsInfinity(speed)) throw new InfinityException(() => speed);
            if (dimensionStack == null) throw new NullException(() => dimensionStack);

            _signalCalculator = signalCalculator;
            _speed = -speed;
            _dimensionStack = dimensionStack;
            _currentDimensionStackIndex = dimensionStack.CurrentIndex;
            _previousDimensionStackIndex = dimensionStack.CurrentIndex - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double transformedPosition = GetTransformedPosition();

            _dimensionStack.Set(_currentDimensionStackIndex, transformedPosition);

            double value = _signalCalculator.Calculate();

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double GetTransformedPosition()
        {
            double position = _dimensionStack.Get(_previousDimensionStackIndex);

            // IMPORTANT: To divide the time in the output, you have to multiply the time of the input.
            double transformedPosition = position * _speed;

            return transformedPosition;
        }

        public override void Reset()
        {
            double tranformedPosition = GetTransformedPosition();

            _dimensionStack.Set(_currentDimensionStackIndex, tranformedPosition);

            base.Reset();
        }
    }
}
