﻿using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.Calculation.Random;
using JJ.Framework.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    /// <summary> Right now there aren't any other variations than VarFrequency. </summary>
    internal class Random_OperatorCalculator_Block_VarFrequency : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly RandomCalculator_Block _randomCalculator;
        private readonly OperatorCalculatorBase _rateCalculator;
        private readonly DimensionStack _dimensionStack;
        private readonly int _dimensionStackIndex;

        private double _phase;
        private double _previousPosition;

        public Random_OperatorCalculator_Block_VarFrequency(
            RandomCalculator_Block randomCalculator,
            OperatorCalculatorBase rateCalculator,
            DimensionStack dimensionStack)
            : base(new[] { rateCalculator })
        {
            // TODO: Make assertion strict again, once you have more calculator variations.
            //OperatorCalculatorHelper.AssertOperatorCalculatorBase(frequencyCalculator, () => frequencyCalculator);
            OperatorCalculatorHelper.AssertDimensionStack(dimensionStack);

            _randomCalculator = randomCalculator ?? throw new NullException(() => randomCalculator);
            _rateCalculator = rateCalculator;
            _dimensionStack = dimensionStack;
            _dimensionStackIndex = dimensionStack.CurrentIndex;

            ResetNonRecursive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            double rate = _rateCalculator.Calculate();

            double positionChange = position - _previousPosition;
            _phase = _phase + positionChange * rate;

            double value = _randomCalculator.Calculate(_phase);

            _previousPosition = position;

            return value;
        }

        public override void Reset()
        {
            base.Reset();

            ResetNonRecursive();
        }

        private void ResetNonRecursive()
        {
#if !USE_INVAR_INDICES
            _previousPosition = _dimensionStack.Get();
#else
            _previousPosition = _dimensionStack.Get(_dimensionStackIndex);
#endif
#if ASSERT_INVAR_INDICES
            OperatorCalculatorHelper.AssertStackIndex(_dimensionStack, _dimensionStackIndex);
#endif
            _randomCalculator.Reseed();
        }
    }

    /// <summary> Right now there aren't any other variations than VarFrequency. </summary>
    internal class Random_OperatorCalculator_Stripe_VarFrequency : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly RandomCalculator_Stripe _randomCalculator;
        private readonly OperatorCalculatorBase _rateCalculator;
        private readonly DimensionStack _dimensionStack;
        private readonly int _dimensionStackIndex;

        private double _phase;
        private double _previousPosition;

        public Random_OperatorCalculator_Stripe_VarFrequency(
            RandomCalculator_Stripe randomCalculator,
            OperatorCalculatorBase rateCalculator,
            DimensionStack dimensionStack)
            : base(new[] { rateCalculator })
        {
            // TODO: Make assertion strict again, once you have more calculator variations.
            //OperatorCalculatorHelper.AssertOperatorCalculatorBase(frequencyCalculator, () => frequencyCalculator);
            OperatorCalculatorHelper.AssertDimensionStack(dimensionStack);

            _randomCalculator = randomCalculator ?? throw new NullException(() => randomCalculator);
            _rateCalculator = rateCalculator;
            _dimensionStack = dimensionStack;
            _dimensionStackIndex = dimensionStack.CurrentIndex;

            ResetNonRecursive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            double rate = _rateCalculator.Calculate();

            double positionChange = position - _previousPosition;
            _phase = _phase + positionChange * rate;

            double value = _randomCalculator.Calculate(_phase);

            _previousPosition = position;

            return value;
        }

        public override void Reset()
        {
            base.Reset();

            ResetNonRecursive();
        }

        private void ResetNonRecursive()
        {
#if !USE_INVAR_INDICES
            _previousPosition = _dimensionStack.Get();
#else
            _previousPosition = _dimensionStack.Get(_dimensionStackIndex);
#endif
#if ASSERT_INVAR_INDICES
            OperatorCalculatorHelper.AssertStackIndex(_dimensionStack, _dimensionStackIndex);
#endif
            _randomCalculator.Reseed();
        }
    }
}
