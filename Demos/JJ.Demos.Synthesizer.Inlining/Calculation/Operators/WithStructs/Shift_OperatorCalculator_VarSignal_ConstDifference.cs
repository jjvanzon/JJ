﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JJ.Demos.Synthesizer.Inlining.Calculation.Operators.WithStructs
{
    internal struct Shift_OperatorCalculator_VarSignal_ConstDifference<TSignalCalculator> 
        : IShift_OperatorCalculator_VarSignal_ConstDifference
        where TSignalCalculator : struct, IOperatorCalculator
    {
        public TSignalCalculator _signalCalculator;
        public double _distance;
        public DimensionStack _dimensionStack;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Calculate()
        {
            double transformedPosition = GetTransformedPosition();

            _dimensionStack.Push(transformedPosition);

            double result = _signalCalculator.Calculate();

            _dimensionStack.Pop();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double GetTransformedPosition()
        {
            double position = _dimensionStack.Get();

            // IMPORTANT: To shift to the right in the output, you have shift to the left in the input.
            double transformedPosition = position - _distance;

            return transformedPosition;
        }

        DimensionStack IShift_OperatorCalculator_VarSignal_ConstDifference.DimensionStack
        {
            get { return _dimensionStack; }
            set { _dimensionStack = value; }
        }

        IOperatorCalculator IShift_OperatorCalculator_VarSignal_ConstDifference.SignalCalculator
        {
            get { return _signalCalculator; }
            set { _signalCalculator = (TSignalCalculator) value; }
        }

        double IShift_OperatorCalculator_VarSignal_ConstDifference.Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }
    }
}
