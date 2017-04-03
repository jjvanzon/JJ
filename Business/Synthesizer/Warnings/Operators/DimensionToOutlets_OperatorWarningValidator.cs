﻿using System.Linq;
using JetBrains.Annotations;
using JJ.Business.Synthesizer.Helpers;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Validation;
using JJ.Data.Synthesizer.Entities;

namespace JJ.Business.Synthesizer.Warnings.Operators
{
    internal class DimensionToOutlets_OperatorWarningValidator : VersatileValidator<Operator>
    {
        public DimensionToOutlets_OperatorWarningValidator([NotNull] Operator obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            Operator op = Obj;

            // ReSharper disable once InvertIf
            Inlet inlet = op.Inlets.FirstOrDefault();
            if (inlet != null)
            {
                // ReSharper disable once InvertIf
                if (inlet.InputOutlet == null)
                {
                    string inletIdentifier = ValidationHelper.GetUserFriendlyIdentifier(inlet);
                    ValidationMessages.AddNotFilledInMessage(PropertyNames.Inlet, inletIdentifier);
                }
            }
        }
    }
}
