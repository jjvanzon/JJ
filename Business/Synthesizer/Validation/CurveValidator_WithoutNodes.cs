﻿using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Exceptions;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation
{
	internal class CurveValidator_WithoutNodes : VersatileValidator
	{
		public CurveValidator_WithoutNodes(Curve curve)
		{
			if (curve == null) throw new NullException(() => curve);

			ExecuteValidator(new NameValidator(curve.Name, required: false));
		}
	}
}
