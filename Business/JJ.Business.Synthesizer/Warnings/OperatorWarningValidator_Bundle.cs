﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Resources;
using JJ.Business.Synthesizer.Validation;

namespace JJ.Business.Synthesizer.Warnings
{
    internal class OperatorWarningValidator_Bundle : FluentValidator<Operator>
    {
        public OperatorWarningValidator_Bundle(Operator obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            Operator op = Object;

            bool anyInletsFilledIn = op.Inlets.Where(x => x.InputOutlet != null).Any();
            if (!anyInletsFilledIn)
            {
                string operatorIdentifier = ValidationHelper.GetOperatorIdentifier(op);
                ValidationMessages.Add(() => op.Inlets, MessageFormatter.OperatorHasNoInletsFilledIn_WithOperatorName(operatorIdentifier));
            }
        }
    }
}
