﻿using JJ.Framework.Reflection.Exceptions;
using System;
using JJ.Business.Synthesizer.Enums;
using System.Runtime.CompilerServices;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Select_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _dimensionValueCalculator;
        private readonly int _dimensionIndex;
        private readonly DimensionStack _dimensionStack;

        public Select_OperatorCalculator(
            OperatorCalculatorBase signalCalculator, 
            OperatorCalculatorBase dimensionValueCalculator,
            DimensionEnum dimensionEnum,
            DimensionStack dimensionStack)
            : base(new OperatorCalculatorBase[] 
            {
                signalCalculator,
                dimensionValueCalculator
            })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(dimensionValueCalculator, () => dimensionValueCalculator);
            OperatorCalculatorHelper.AssertDimensionEnum(dimensionEnum);
            if (dimensionStack == null) throw new NullException(() => dimensionStack);

            _signalCalculator = signalCalculator;
            _dimensionValueCalculator = dimensionValueCalculator;
            _dimensionIndex = (int)dimensionEnum;
            _dimensionStack = dimensionStack;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double dimensionValue = _dimensionValueCalculator.Calculate();

            _dimensionStack.Push(_dimensionIndex, dimensionValue);

            double result = _signalCalculator.Calculate();

            _dimensionStack.Pop(_dimensionIndex);

            return result;
        }
    }

    internal class Select_WithConstDimensionValue_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _dimensionValue;
        private readonly int _dimensionIndex;
        private readonly DimensionStack _dimensionStack;

        public Select_WithConstDimensionValue_OperatorCalculator(
            OperatorCalculatorBase signalCalculator, 
            double dimensionValue,
            DimensionEnum dimensionEnum,
            DimensionStack dimensionStack)
            : base(new OperatorCalculatorBase[] 
            {
                signalCalculator
            })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertDimensionEnum(dimensionEnum);
            if (dimensionStack == null) throw new NullException(() => dimensionStack);

            _signalCalculator = signalCalculator;
            _dimensionValue = dimensionValue;
            _dimensionIndex = (int)dimensionEnum;
            _dimensionStack = dimensionStack;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            _dimensionStack.Push(_dimensionIndex, _dimensionValue);

            double result = _signalCalculator.Calculate();

            _dimensionStack.Pop(_dimensionIndex);

            return result;
        }
    }
}
