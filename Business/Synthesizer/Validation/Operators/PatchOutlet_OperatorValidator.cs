﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Validation.DataProperty;
using System.Linq;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Extensions;
using JJ.Data.Synthesizer.Entities;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class PatchOutlet_OperatorValidator : OperatorValidator_Base
    {
        public PatchOutlet_OperatorValidator(Operator obj)
            : base(
                obj,
                OperatorTypeEnum.PatchOutlet,
                new[] { DimensionEnum.Undefined },
                new[] { GetOutletDimensionEnum(obj) },
                expectedDataKeys: new[] { nameof(PatchOutlet_OperatorWrapper.ListIndex) })
        { }

        protected override void Execute()
        {
            ExecuteValidator(new ListIndex_DataProperty_Validator(Obj.Data));

            base.Execute();
        }

        // Helpers

        private static DimensionEnum GetOutletDimensionEnum(Operator obj)
        {
            return obj?.Outlets.FirstOrDefault()?.GetDimensionEnum() ?? DimensionEnum.Undefined;
        }
    }
}
