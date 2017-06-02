﻿using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Resources;
using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer.Entities;

namespace JJ.Business.Synthesizer.Warnings.Operators
{
    internal class CustomOperator_OperatorWarningValidator : OperatorWarningValidator_Base
    {
        public CustomOperator_OperatorWarningValidator(Operator op)
            : base(op)
        { }

        protected override void Execute()
        {
            // ReSharper disable once InvertIf
            if (DataPropertyParser.DataIsWellFormed(Obj.Data))
            {
                string underlyingPatchIDString = DataPropertyParser.TryGetString(Obj, nameof(CustomOperator_OperatorWrapper.UnderlyingPatchID));

                For(() => underlyingPatchIDString, ResourceFormatter.UnderlyingPatch)
                    .NotNullOrEmpty();
            }
        }
    }
}