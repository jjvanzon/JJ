﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Business.Synthesizer.Calculation.Arrays
{
    internal class ArrayCalculator_Block : ArrayCalculatorBase
    {
        public ArrayCalculator_Block(double[] array, double rate) 
            : base(array, rate)
        { }

        public ArrayCalculator_Block(
            double[] array, 
            double valueBefore, 
            double valueAfter,
            double rate) 
            : base(array, valueBefore, valueAfter, rate)
        { }

        public override double CalculateValue(double time)
        {
            // Return if sample not in range.
            // Execute it on the doubles, to prevent integer overflow.
            if (time < 0) return _valueBefore;
            if (time > _maxTime) return _valueAfter;

            double t = time * _rate;
            int t0 = (int)t;

            double value = _array[t0];
            return value;
        }
    }
}
