﻿using System.Linq;
using JetBrains.Annotations;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Validation;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Exceptions;

namespace JJ.Business.Synthesizer.Warnings.Operators
{
    internal class OperatorWarningValidator_Basic : VersatileValidator
    {
        public OperatorWarningValidator_Basic([NotNull] Operator op)
        {
            if (op == null) throw new NullException(() => op);

            foreach (Inlet inlet in op.Inlets.OrderBy(x => x.Position))
            {
                bool isPatchInlet = inlet.Operator.GetOperatorTypeEnum() == OperatorTypeEnum.PatchInlet;
                if (isPatchInlet)
                {
                    continue;
                }

                if (inlet.WarnIfEmpty && inlet.InputOutlet == null)
                {
                    string identifier = ValidationHelper.GetUserFriendlyIdentifier(inlet);
                    ValidationMessages.AddNotFilledInMessage(nameof(Inlet), identifier);
                }
            }
        }
    }
}
