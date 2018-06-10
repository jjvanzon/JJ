﻿using System.Runtime.CompilerServices;

namespace JJ.Business.SynthesizerPrototype.WithInheritance.Calculation
{
	internal class Number_OperatorCalculator_One : OperatorCalculatorBase
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override double Calculate() => 1.0;
	}
}
