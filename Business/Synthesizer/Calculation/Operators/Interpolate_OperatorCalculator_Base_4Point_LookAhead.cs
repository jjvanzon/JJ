﻿using System;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
	internal abstract class Interpolate_OperatorCalculator_Base_4Point_LookAhead : Interpolate_OperatorCalculator_Base_4Point
	{
		private readonly VariableInput_OperatorCalculator _positionOutputCalculator;

		public Interpolate_OperatorCalculator_Base_4Point_LookAhead(
			OperatorCalculatorBase signalCalculator,
			OperatorCalculatorBase samplingRateCalculator,
			OperatorCalculatorBase positionInputCalculator,
			VariableInput_OperatorCalculator positionOutputCalculator)
			: base(signalCalculator, samplingRateCalculator, positionInputCalculator)
			=> _positionOutputCalculator = positionOutputCalculator ?? throw new ArgumentNullException(nameof(positionOutputCalculator));

		protected override void SetNextSample()
		{
			_x2 += Dx();

			double originalPosition = _positionOutputCalculator._value;
			_positionOutputCalculator._value = _x2;

			_y2 = _signalCalculator.Calculate();

			_positionOutputCalculator._value = originalPosition;
		}

		protected override void SetPreviousSample()
		{
			_xMinus1 -= Dx();

			double originalPosition = _positionOutputCalculator._value;
			_positionOutputCalculator._value = _xMinus1;

			_yMinus1 = _signalCalculator.Calculate();

			_positionOutputCalculator._value = originalPosition;
		}

		// TODO: ResetNonRecursive is still an issue.
	}
}