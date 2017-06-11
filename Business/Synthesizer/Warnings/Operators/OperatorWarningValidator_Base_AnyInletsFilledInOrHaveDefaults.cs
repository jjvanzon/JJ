﻿using System.Linq;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Warnings.Operators
{
    internal abstract class OperatorWarningValidator_Base_AnyInletsFilledInOrHaveDefaults : VersatileValidator<Operator>
    {
        public OperatorWarningValidator_Base_AnyInletsFilledInOrHaveDefaults(Operator op)
            : base(op)
        { 
            bool anyInletsFilledIn = op.Inlets.Where(x => x.InputOutlet != null &&
                                                         !x.DefaultValue.HasValue)
                                              .Any();
            // ReSharper disable once InvertIf
            if (!anyInletsFilledIn)
            {
                ValidationMessages.AddIsEmptyMessage(nameof(op.Inlets), ResourceFormatter.Inlets);
            }
        }
    }
}
