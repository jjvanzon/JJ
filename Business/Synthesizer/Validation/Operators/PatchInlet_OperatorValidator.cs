﻿using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Validation.DataProperty;
using System.Linq;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Extensions;
using JJ.Data.Synthesizer.Entities;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class PatchInlet_OperatorValidator : OperatorValidator_Base
    {
        public PatchInlet_OperatorValidator(Operator obj)
            : base(
                obj,
                OperatorTypeEnum.PatchInlet,
                new[] { GetInletDimensionEnum(obj) },
                new[] { DimensionEnum.Undefined },
                expectedDataKeys: new[] { nameof(PatchInlet_OperatorWrapper.ListIndex) })
        { }

        protected override void Execute()
        {
            ExecuteValidator(new ListIndex_DataProperty_Validator(Obj.Data));

            base.Execute();
        }

        // Helpers

        private static DimensionEnum GetInletDimensionEnum(Operator obj)
        {
            return obj?.Inlets.FirstOrDefault()?.GetDimensionEnum() ?? DimensionEnum.Undefined;
        }
    }
}
