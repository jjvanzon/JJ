﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Business.Synthesizer.Calculation.Arrays
{
    internal class ArrayCalculatorBase_Stripe : ArrayCalculatorBase
    {
        private const int EXTRA_TICKS_BEFORE = 0;
        private const int EXTRA_TICKS_AFTER = 1;

        public ArrayCalculatorBase_Stripe(double[] array, double rate, double minTime)
            : base(array, rate, minTime, EXTRA_TICKS_BEFORE, EXTRA_TICKS_AFTER)
        { }

        public ArrayCalculatorBase_Stripe(
            double[] array, double rate, double minTime, double valueBefore, double valueAfter)
            : base(array, rate, minTime, EXTRA_TICKS_BEFORE, EXTRA_TICKS_AFTER, valueBefore, valueAfter)
        { }

        /// <summary> Base method does not check bounds of time or transform time from seconds to samples. </summary>
        public override double CalculateValue(double t)
        {
            t += 0.5;

            int t0 = (int)t;

            double value = _array[t0];
            return value;
        }
    }
}