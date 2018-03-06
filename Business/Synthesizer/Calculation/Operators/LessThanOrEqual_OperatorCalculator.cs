﻿using System.Runtime.CompilerServices;
using JJ.Framework.Exceptions;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
	internal class LessThanOrEqual_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
	{
		private readonly OperatorCalculatorBase _calculatorA;
		private readonly OperatorCalculatorBase _calculatorB;

		public LessThanOrEqual_OperatorCalculator(
			OperatorCalculatorBase calculatorA,
			OperatorCalculatorBase calculatorB)
			: base(new[] { calculatorA, calculatorB })
		{
			_calculatorA = calculatorA ?? throw new NullException(() => calculatorA);
			_calculatorB = calculatorB ?? throw new NullException(() => calculatorB);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override double Calculate()
		{
			double a = _calculatorA.Calculate();
			double b = _calculatorB.Calculate();

			if (a <= b) return 1.0;
			else return 0.0;
		}
	}
}