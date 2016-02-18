﻿using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.Calculation.Arrays;
using JJ.Framework.Mathematics;

namespace JJ.Business.Synthesizer.Calculation.Random
{
    internal class RandomCalculator_StripeInterpolation : RandomCalculatorBase
    {
        private ArrayCalculator_RotateTime_Stripe_NoRate _arrayCalculator;

        public RandomCalculator_StripeInterpolation()
        {
            _arrayCalculator = new ArrayCalculator_RotateTime_Stripe_NoRate(_samples);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double GetValue(double time)
        {
            return _arrayCalculator.CalculateValue(time);
        }
    }
}
