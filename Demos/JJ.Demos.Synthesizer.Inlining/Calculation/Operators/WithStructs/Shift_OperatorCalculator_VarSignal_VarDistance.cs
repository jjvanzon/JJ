﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Demos.Synthesizer.Inlining.Helpers;

namespace JJ.Demos.Synthesizer.Inlining.Calculation.Operators.WithStructs
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    internal struct Shift_OperatorCalculator_VarSignal_VarDistance<TSignalCalculator, TDistanceCalculator> 
        : IShift_OperatorCalculator_VarSignal_VarDistance
        where TSignalCalculator : IOperatorCalculator
        where TDistanceCalculator : IOperatorCalculator
    {
        public TSignalCalculator _signalCalculator;
        public TDistanceCalculator _distanceCalculator;
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

            double distance = _distanceCalculator.Calculate();

            // IMPORTANT: To shift to the right in the output, you have shift to the left in the input.
            double transformedPosition = position - distance;

            return transformedPosition;
        }

        IOperatorCalculator IShift_OperatorCalculator_VarSignal_VarDistance.SignalCalculator
        {
            get { return _signalCalculator; }
            set { _signalCalculator = (TSignalCalculator)value; }
        }

        IOperatorCalculator IShift_OperatorCalculator_VarSignal_VarDistance.DistanceCalculator
        {
            get { return _distanceCalculator; }
            set { _distanceCalculator = (TDistanceCalculator)value; }
        }

        DimensionStack IShift_OperatorCalculator_VarSignal_VarDistance.DimensionStack
        {
            get { return _dimensionStack; }
            set { _dimensionStack = value; }
        }

        private string DebuggerDisplay => DebugHelper.GetDebuggerDisplay(this);
    }
}