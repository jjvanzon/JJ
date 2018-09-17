﻿using System;
using System.Runtime.CompilerServices;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class ArcTan_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _radiansCalculator;

        public ArcTan_OperatorCalculator(OperatorCalculatorBase radiansCalculator)
            : base(new[] { radiansCalculator })
            => _radiansCalculator = radiansCalculator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double radians = _radiansCalculator.Calculate();

            return Math.Atan(radians);
        }
    }
}