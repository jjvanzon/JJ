﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Framework.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Add_OperatorCalculator_VarArray_1Const : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase[] _varOperandCalculators;
        private readonly int _varOperandCalculatorsCount;

        private readonly double _constValue;

        public Add_OperatorCalculator_VarArray_1Const(IList<OperatorCalculatorBase> varOperandCalculators, double constValue)
            : base(varOperandCalculators)
        {
            if (varOperandCalculators == null) throw new NullException(() => varOperandCalculators);

            _varOperandCalculators = varOperandCalculators.ToArray();
            _varOperandCalculatorsCount = _varOperandCalculators.Length;
            _constValue = constValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double sum = _constValue;

            for (int i = 0; i < _varOperandCalculatorsCount; i++)
            {
                double value = _varOperandCalculators[i].Calculate();

                sum += value;
            }

            return sum;
        }
    }
}