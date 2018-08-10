﻿using JJ.Framework.Mathematics;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
	internal sealed class Interpolate_OperatorCalculator_Cubic_LookAhead : OperatorCalculatorBase_FollowingSampler_4Point_LookAhead
	{
		private double _a;
		private double _b;
		private double _c;

		public Interpolate_OperatorCalculator_Cubic_LookAhead(
			OperatorCalculatorBase signalCalculator,
			OperatorCalculatorBase samplingRateCalculator,
			OperatorCalculatorBase positionCalculator,
			VariableInput_OperatorCalculator positionOutputCalculator)
			: base(signalCalculator, samplingRateCalculator, positionCalculator, positionOutputCalculator)
		    => ResetNonRecursive();

	    protected override void Precalculate() => (_a, _b, _c) = Interpolator.CubicSmoothSlope_PrecalculateVariables(
	                                                  _xMinus1, _x0, _x1, _x2,
	                                                  _yMinus1, _y0, _y1, _y2);

	    protected override double Calculate(double x)
		{
			double y = Interpolator.CubicSmoothSlope_FromPrecalculatedVariables(_x0, _y0, _a, _b, _c, x);
			return y;
		}
	}
}