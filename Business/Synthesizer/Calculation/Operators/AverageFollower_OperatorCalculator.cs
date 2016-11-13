﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class AverageFollower_OperatorCalculator : SumFollower_OperatorCalculator
    {
        public AverageFollower_OperatorCalculator(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase sliceLengthCalculator,
            OperatorCalculatorBase sampleCountCalculator,
            DimensionStack dimensionStack)
            : base(signalCalculator, sliceLengthCalculator, sampleCountCalculator, dimensionStack)
        { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override double PostProcessAggregate()
        {
            return _sum / _sampleCountDouble;
        }
    }
}