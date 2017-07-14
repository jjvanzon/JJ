﻿using System.Runtime.CompilerServices;
using JJ.Framework.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class DimensionToOutlets_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _operandCalculator;
        private readonly double _position;
        private readonly DimensionStack _dimensionStack;
        private readonly int _dimensionStackIndex;

        public DimensionToOutlets_OperatorCalculator(
            OperatorCalculatorBase operandCalculator, 
            double position,
            DimensionStack dimensionStack)
            : base(new[] { operandCalculator })
        {
            OperatorCalculatorHelper.AssertDimensionStack(dimensionStack);

            _operandCalculator = operandCalculator ?? throw new NullException(() => operandCalculator);
            _position = position;
            _dimensionStack = dimensionStack;
            _dimensionStackIndex = dimensionStack.CurrentIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
#if !USE_INVAR_INDICES
            _dimensionStack.Push(_position);
#else
            _dimensionStack.Set(_dimensionStackIndex, _position);
#endif
#if ASSERT_INVAR_INDICES
            OperatorCalculatorHelper.AssertStackIndex(_dimensionStack, _dimensionStackIndex);
#endif
            double result = _operandCalculator.Calculate();

#if !USE_INVAR_INDICES
            _dimensionStack.Pop();
#endif
            return result;
        }
    }
}
