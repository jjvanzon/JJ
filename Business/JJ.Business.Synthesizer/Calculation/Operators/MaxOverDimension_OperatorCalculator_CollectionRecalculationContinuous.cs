﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class MaxOverDimension_OperatorCalculator_CollectionRecalculationContinuous : MinOrMaxOverDimension_OperatorCalculatorBase
    {
        public MaxOverDimension_OperatorCalculator_CollectionRecalculationContinuous(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase fromCalculator,
            OperatorCalculatorBase tillCalculator,
            OperatorCalculatorBase stepCalculator,
            DimensionStack dimensionStack)
            : base(signalCalculator, fromCalculator, tillCalculator, stepCalculator, dimensionStack)
        { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            RecalculateAggregate();

            return base.Calculate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool MustOverwrite(double currentValue, double newValue)
        {
            return newValue > currentValue;
        }
    }
}