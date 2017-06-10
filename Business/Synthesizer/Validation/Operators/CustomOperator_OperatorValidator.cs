﻿using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer.Entities;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal sealed class CustomOperator_OperatorValidator : OperatorValidator_Base_WithUnderlyingPatch
    {
        public CustomOperator_OperatorValidator(Operator op)
            : base(op)
        { }

        protected override void Execute()
        {
            Operator op = Obj;

            For(() => op.GetOperatorTypeEnum(), ResourceFormatter.OperatorType).Is(OperatorTypeEnum.CustomOperator);

            base.Execute();
        }
    }
}
