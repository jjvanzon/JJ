﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.Calculation.Arrays;
using JJ.Business.Synthesizer.Helpers;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Cache_OperatorCalculator_SingleChannel<TArrayCalculator> : OperatorCalculatorBase
        where TArrayCalculator : ArrayCalculatorBase
    {
        private readonly TArrayCalculator _arrayCalculator;
        private readonly DimensionStack _dimensionStack;
        private readonly int _dimensionStackIndex;

        public Cache_OperatorCalculator_SingleChannel(
            TArrayCalculatomr arrayCalculator, 
            DimensionStack dimensionStack)
        {
            if (arrayCalculator == null) throw new NullException(() => arrayCalculator);
            OperatorCalculatorHelper.AssertDimensionStack_ForReaders(dimensionStack);

            _arrayCalculator = arrayCalculator;
            _dimensionStack = dimensionStack;
            _dimensionStackIndex = dimensionStack.CurrentIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double time = _dimensionStack.Get(_dimensionStackIndex);

            return _arrayCalculator.CalculateValue(time);
        }
    }

    internal class Cache_OperatorCalculator_MultiChannel<TArrayCalculator> : OperatorCalculatorBase
        where TArrayCalculator : ArrayCalculatorBase
    {
        private readonly TArrayCalculator[] _arrayCalculators;
        private readonly int _arrayCalculatorsLength;
        private readonly DimensionStack _dimensionStack;
        private readonly DimensionStack _channelDimensionStack;
        private readonly int _dimensionStackIndex;
        private readonly int _channelDimensionStackIndex;

        public Cache_OperatorCalculator_MultiChannel(
            IList<TArrayCalculator> arrayCalculators,
            DimensionStack dimensionStack,
            DimensionStack channelDimensionStack)
        {
            if (arrayCalculators == null) throw new NullException(() => arrayCalculators);
            OperatorCalculatorHelper.AssertDimensionStack_ForReaders(dimensionStack);
            OperatorCalculatorHelper.AssertDimensionStack_ForReaders(channelDimensionStack);

            _arrayCalculators = arrayCalculators.ToArray();
            _arrayCalculatorsLength = _arrayCalculators.Length;
            _dimensionStack = dimensionStack;
            _channelDimensionStack = channelDimensionStack;
            _dimensionStackIndex = dimensionStack.CurrentIndex;
            _channelDimensionStackIndex = channelDimensionStack.CurrentIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double channelDouble = _channelDimensionStack.Get(_channelDimensionStackIndex);
            if (!ConversionHelper.CanCastToNonNegativeInt32WithMax(channelDouble, _arrayCalculatorsLength))
            {
                return 0.0;
            }
            int channelInt = (int)channelDouble;

            double position = _dimensionStack.Get(_dimensionStackIndex);

            return _arrayCalculators[channelInt].CalculateValue(position);
        }
    }
}