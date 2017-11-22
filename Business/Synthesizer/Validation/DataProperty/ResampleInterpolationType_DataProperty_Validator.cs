﻿using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.DataProperty
{
	internal class ResampleInterpolationType_DataProperty_Validator : VersatileValidator
	{
		public ResampleInterpolationType_DataProperty_Validator(string data)
		{ 
			// ReSharper disable once InvertIf
			if (DataPropertyParser.DataIsWellFormed(data))
			{
				string stringValue = DataPropertyParser.TryGetString(data, nameof(Interpolate_OperatorWrapper.InterpolationType));

				For(stringValue, ResourceFormatter.InterpolationType)
					.NotNullOrEmpty()
					.IsEnum<ResampleInterpolationTypeEnum>()
					.IsNot(ResampleInterpolationTypeEnum.Undefined);
			}
		}
	}
}
