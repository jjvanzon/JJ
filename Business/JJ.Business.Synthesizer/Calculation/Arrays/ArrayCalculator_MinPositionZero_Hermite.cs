﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JJ.Business.Synthesizer.Calculation.Arrays
{
    internal class ArrayCalculator_MinPositionZero_Hermite : ArrayCalculatorBase_Hermite
    {
        private const double MIN_POSITION = 0.0;

        public ArrayCalculator_MinPositionZero_Hermite(
            double[] array, double rate)
            : base(array, rate, MIN_POSITION)
        { }

        public ArrayCalculator_MinPositionZero_Hermite(
            double[] array, double rate, double valueBefore, double valueAfter)
            : base(array, rate, MIN_POSITION, valueBefore, valueAfter)
        { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double CalculateValue(double position)
        {
            // Return if sample not in range.
            // Execute it on the doubles, to prevent integer overflow later.
            if (position < 0) return _valueBefore;
            if (position > _maxPosition) return _valueAfter;

            double t = position * _rate;

            return base.CalculateValue(t);
        }
    }
}