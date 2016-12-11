﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.CopiedCode.FromFramework;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class ClosestOverInletsExp_OperatorCalculator_VarInput_VarItems : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _inputCalculator;
        private readonly OperatorCalculatorBase _firstItemCalculators;
        private readonly OperatorCalculatorBase[] _remainingItemCalculators;
        private readonly int _remainingItemCalculatorsCount;

        public ClosestOverInletsExp_OperatorCalculator_VarInput_VarItems(
            OperatorCalculatorBase inputCalculator,
            IList<OperatorCalculatorBase> itemCalculators)
            : base(new List<OperatorCalculatorBase>(itemCalculators) { inputCalculator })
        {
            if (inputCalculator == null) throw new NullException(() => inputCalculator);
            if (itemCalculators == null) throw new NullException(() => itemCalculators);
            if (itemCalculators.Count < 1) throw new LessThanException(() => itemCalculators.Count, 1);

            _inputCalculator = inputCalculator;
            _firstItemCalculators = itemCalculators.First();
            _remainingItemCalculators = itemCalculators.Skip(1).ToArray();
            _remainingItemCalculatorsCount = _remainingItemCalculators.Length;
        }

        public override double Calculate()
        {
            double input = _inputCalculator.Calculate();
            double firstItem = _firstItemCalculators.Calculate();

            double logInput = Math.Log(input);

            double smallestDistance = Geometry.AbsoluteDistance(logInput, Math.Log(firstItem));
            double closestItem = firstItem;

            for (int i = 0; i < _remainingItemCalculatorsCount; i++)
            {
                OperatorCalculatorBase itemCalculator = _remainingItemCalculators[i];
                double item = itemCalculator.Calculate();

                double distance = Geometry.AbsoluteDistance(logInput, Math.Log(item));
                if (smallestDistance > distance)
                {
                    smallestDistance = distance;
                    closestItem = item;
                }
            }

            return closestItem;
        }
    }

    internal class ClosestOverInletsExp_OperatorCalculator_VarInput_ConstItems : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _inputCalculator;
        private readonly double _firstItem;
        private readonly double[] _remainingItems;
        private readonly int _remainingItemsCount;

        public ClosestOverInletsExp_OperatorCalculator_VarInput_ConstItems(
            OperatorCalculatorBase inputCalculator,
            IList<double> items)
            : base(new OperatorCalculatorBase[] { inputCalculator })
        {
            if (inputCalculator == null) throw new NullException(() => inputCalculator);
            if (items == null) throw new NullException(() => items);
            if (items.Count < 1) throw new LessThanException(() => items.Count, 1);

            _inputCalculator = inputCalculator;
            _firstItem = items[0];
            _remainingItems = items.Skip(1).ToArray();
            _remainingItemsCount = _remainingItems.Length;
        }

        public override double Calculate()
        {
            double input = _inputCalculator.Calculate();

            double result = AggregateCalculator.ClosestExpUnsafe(input, _firstItem, _remainingItems, _remainingItemsCount);

            return result;
        }
    }

    internal class ClosestOverInletsExp_OperatorCalculator_VarInput_2ConstItems : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _inputCalculator;
        private readonly double _item1;
        private readonly double _item2;

        public ClosestOverInletsExp_OperatorCalculator_VarInput_2ConstItems(
            OperatorCalculatorBase inputCalculator,
            double item1,
            double item2)
            : base(new OperatorCalculatorBase[] { inputCalculator })
        {
            if (inputCalculator == null) throw new NullException(() => inputCalculator);

            _inputCalculator = inputCalculator;
            _item1 = item1;
            _item2 = item2;
        }

        public override double Calculate()
        {
            double input = _inputCalculator.Calculate();

            double result = AggregateCalculator.ClosestExp(input, _item1, _item2);

            return result;
        }
    }
}