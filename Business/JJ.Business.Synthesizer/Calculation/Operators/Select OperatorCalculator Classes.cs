﻿using JJ.Framework.Reflection.Exceptions;
using System;
using System.Runtime.CompilerServices;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Select_OperatorCalculator_VarPosition : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _positionCalculator;
        private readonly DimensionStack _dimensionStack;
        private readonly int _dimensionStackIndex;

        public Select_OperatorCalculator_VarPosition(
            OperatorCalculatorBase signalCalculator, 
            OperatorCalculatorBase positionCalculator,
            DimensionStack dimensionStack)
            : base(new OperatorCalculatorBase[] 
            {
                signalCalculator,
                positionCalculator
            })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(positionCalculator, () => positionCalculator);
            if (dimensionStack == null) throw new NullException(() => dimensionStack);

            _signalCalculator = signalCalculator;
            _positionCalculator = positionCalculator;
            _dimensionStack = dimensionStack;
            _dimensionStackIndex = dimensionStack.CurrentIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double position = _positionCalculator.Calculate();

            _dimensionStack.Set(_dimensionStackIndex, position);

            double result = _signalCalculator.Calculate();

            return result;
        }

        public override void Reset()
        {
            double position = _positionCalculator.Calculate();

            _dimensionStack.Set(_dimensionStackIndex, position);

            base.Reset();
        }
    }

    internal class Select_OperatorCalculator_ConstPosition : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _position;
        private readonly DimensionStack _dimensionStack;
        private readonly int _dimensionStackIndex;

        public Select_OperatorCalculator_ConstPosition(
            OperatorCalculatorBase signalCalculator, 
            double position,
            DimensionStack dimensionStack)
            : base(new OperatorCalculatorBase[] 
            {
                signalCalculator
            })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertDimensionStack_ForWriters(dimensionStack);

            _signalCalculator = signalCalculator;
            _position = position;
            _dimensionStack = dimensionStack;
            _dimensionStackIndex = dimensionStack.CurrentIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            _dimensionStack.Set(_dimensionStackIndex, _position);

            double result = _signalCalculator.Calculate();

            return result;
        }

        public override void Reset()
        {
            _dimensionStack.Set(_dimensionStackIndex, _position);

            base.Reset();
        }
    }
}
