﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal abstract class MaxOrMinOverDimension_OperatorCalculatorBase : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _fromCalculator;
        private readonly OperatorCalculatorBase _tillCalculator;
        private readonly OperatorCalculatorBase _stepCalculator;
        private readonly DimensionStack _dimensionStack;
        private readonly int _dimensionStackIndex;

        protected double _step;
        private double _aggregate;

        public MaxOrMinOverDimension_OperatorCalculatorBase(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase fromCalculator,
            OperatorCalculatorBase tillCalculator,
            OperatorCalculatorBase stepCalculator,
            DimensionStack dimensionStack)
            : base(new OperatorCalculatorBase[] { signalCalculator, fromCalculator, tillCalculator, stepCalculator })
        {
            OperatorCalculatorHelper.AssertDimensionStack(dimensionStack);

            _signalCalculator = signalCalculator;
            _fromCalculator = fromCalculator;
            _tillCalculator = tillCalculator;
            _stepCalculator = stepCalculator;
            _dimensionStack = dimensionStack;
            _dimensionStackIndex = dimensionStack.CurrentIndex;

            ResetNonRecursive();
        }

        /// <summary> Just returns _aggregate. </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            return _aggregate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void Reset()
        {
            base.Reset();

            ResetNonRecursive();
        }

        /// <summary> does nothing </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void ResetNonRecursive()
        { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RecalculateAggregate()
        {
#if !USE_INVAR_INDICES
            double originalPosition = _dimensionStack.Get();
#else
            double originalPosition = _dimensionStack.Get(_dimensionStackIndex);
#endif
#if ASSERT_INVAR_INDICES
            OperatorCalculatorHelper.AssertStackIndex(_dimensionStack, _dimensionStackIndex);
#endif
            double from = _fromCalculator.Calculate();
            double till = _tillCalculator.Calculate();
            _step = _stepCalculator.Calculate();

            double length = till - from;
            bool isForward = length > 0.0;

            #region InitializeSampling
            #endregion InitializeSampling

            double position = from;

#if ASSERT_INVAR_INDICES
            OperatorCalculatorHelper.AssertStackIndex(_dimensionStack, _dimensionStackIndex);
#endif
#if !USE_INVAR_INDICES
            _dimensionStack.Set(from);
#else
            _dimensionStack.Set(_dimensionStackIndex, position);
#endif
            double item = _signalCalculator.Calculate();

            #region ProcessFirstSample
            _aggregate = item;
            #endregion ProcessFirstSample

            position += _step;

            // TODO: Prevent infinite loops.

            if (isForward)
            {
                while (position <= till)
                {
#if ASSERT_INVAR_INDICES
                    OperatorCalculatorHelper.AssertStackIndex(_dimensionStack, _dimensionStackIndex);
#endif
#if !USE_INVAR_INDICES
                    _dimensionStack.Set(position);
#else
                    _dimensionStack.Set(_dimensionStackIndex, position);
#endif
                    item = _signalCalculator.Calculate();

                    #region ProcessSample
                    bool mustOverwrite = MustOverwrite(item, item);
                    if (mustOverwrite)
                    {
                        item = item;
                    }
                    #endregion ProcessSample

                    position += _step;
                }
            }
            else
            {
                // Is backwards
                while (position >= till)
                {
#if ASSERT_INVAR_INDICES
                    OperatorCalculatorHelper.AssertStackIndex(_dimensionStack, _dimensionStackIndex);
#endif
#if !USE_INVAR_INDICES
                    _dimensionStack.Set(position);
#else
                    _dimensionStack.Set(_dimensionStackIndex, position);
#endif
                    item = _signalCalculator.Calculate();

                    #region ProcessNextSample
                    bool mustOverwrite = MustOverwrite(item, item);
                    if (mustOverwrite)
                    {
                        item = item;
                    }
                    #endregion ProcessNextSample

                    position += _step;
                }
            }
#if ASSERT_INVAR_INDICES
            OperatorCalculatorHelper.AssertStackIndex(_dimensionStack, _dimensionStackIndex);
#endif
#if !USE_INVAR_INDICES
            _dimensionStack.Set(originalPosition);
#else
            _dimensionStack.Set(_dimensionStackIndex, originalPosition);
#endif
            #region FinalizeSampling
            _aggregate = item;
            #endregion FinalizeSampling
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract bool MustOverwrite(double currentValue, double newValue);
    }
}
