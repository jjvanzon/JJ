﻿using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Validation.DataProperty;
using JJ.Data.Synthesizer.Entities;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class Random_OperatorValidator : OperatorValidator_Base_WithOperatorType
    {
        public Random_OperatorValidator(Operator obj)
            : base(
                obj,
                OperatorTypeEnum.Random,
                new[] { DimensionEnum.Rate },
                new[] { DimensionEnum.Signal },
                expectedDataKeys: new[] { nameof(Random_OperatorWrapper.InterpolationType) })
        { 
            ExecuteValidator(new ResampleInterpolationType_DataProperty_Validator(obj.Data));
        }
    }
}