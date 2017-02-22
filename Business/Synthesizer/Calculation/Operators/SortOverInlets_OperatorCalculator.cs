﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class SortOverInlets_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase[] _itemCalculators;
        private readonly double[] _items;
        private readonly double _maxIndexDouble;
        private readonly int _itemCount;
        private readonly DimensionStack _dimensionStack;
        private readonly int _dimensionStackIndex;

        public SortOverInlets_OperatorCalculator(
            IList<OperatorCalculatorBase> itemCalculators,
            DimensionStack dimensionStack) 
            : base(itemCalculators)
        {
            OperatorCalculatorHelper.AssertDimensionStack(dimensionStack);

            _dimensionStack = dimensionStack;
            _dimensionStackIndex = dimensionStack.CurrentIndex;

            _itemCalculators = itemCalculators.ToArray();
            _itemCount = itemCalculators.Count;
            _maxIndexDouble = _itemCount - 1;
            _items = new double[_itemCount];
        }

        public override double Calculate()
        {
#if !USE_INVAR_INDICES
            double position = _dimensionStack.Get();
#else
            double position = _dimensionStack.Get(_dimensionStackIndex);
#endif
#if ASSERT_INVAR_INDICES
            OperatorCalculatorHelper.AssertStackIndex(_dimensionStack, _dimensionStackIndex);
#endif
            if (!ConversionHelper.CanCastToNonNegativeInt32WithMax(position, _maxIndexDouble))
            {
                return 0.0;
            }

            for (int i = 0; i < _itemCount; i++)
            {
                _items[i] = _itemCalculators[i].Calculate();
            }

            Array.Sort(_items);
            
            double item = _items[(int)position];

            return item;
        }
    }
}
