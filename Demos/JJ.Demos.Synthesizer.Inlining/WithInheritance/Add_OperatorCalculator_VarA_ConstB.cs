﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Demos.Synthesizer.Inlining.Shared;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Demos.Synthesizer.Inlining.WithInheritance
{
    internal class Add_OperatorCalculator_VarA_ConstB : OperatorCalculatorBase
    {
        private readonly OperatorCalculatorBase _aCalculator;
        private readonly double _b;

        public Add_OperatorCalculator_VarA_ConstB(
            OperatorCalculatorBase aCalculator,
            double b)
        {
            if (aCalculator == null) throw new NullException(() => aCalculator);

            _aCalculator = aCalculator;
            _b = b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double a = _aCalculator.Calculate();
            double result = OperatorCalculatorHelper.Add(a, _b);
            return result;
        }
    }
}
