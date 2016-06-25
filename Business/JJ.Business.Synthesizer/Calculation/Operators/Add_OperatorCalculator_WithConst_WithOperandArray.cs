﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Add_OperatorCalculator_WithConst_WithOperandArray : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly double _constValue;
        private readonly OperatorCalculatorBase[] _varOperandCalculators;

        public Add_OperatorCalculator_WithConst_WithOperandArray(double constValue, IList<OperatorCalculatorBase> varOperandCalculators)
            : base(varOperandCalculators)
        {
            if (varOperandCalculators == null) throw new NullException(() => varOperandCalculators);

            _constValue = constValue;

            _varOperandCalculators = varOperandCalculators.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double sum = _constValue;

            for (int i = 0; i < _varOperandCalculators.Length; i++)
            {
                double value = _varOperandCalculators[i].Calculate();

                sum += value;
            }

            return sum;
        }
    }
}