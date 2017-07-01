﻿using JetBrains.Annotations;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Exceptions;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Warnings
{
    internal class InletWarningValidator : VersatileValidator
    {
        public InletWarningValidator([NotNull] Inlet inlet)
        {
            if (inlet == null) throw new NullException(() => inlet);

            bool isPatchInlet = inlet.Operator.GetOperatorTypeEnum() == OperatorTypeEnum.PatchInlet;
            if (isPatchInlet)
            {
                return;
            }

            if (inlet.WarnIfEmpty && inlet.InputOutlet == null)
            {
                ValidationMessages.AddNotFilledInMessage(nameof(Inlet));
            }

            if (inlet.IsObsolete && inlet.InputOutlet != null)
            {
                ValidationMessages.Add(nameof(Inlet.IsObsolete), ResourceFormatter.ObsoleteButStillUsed);
            }
        }
    }
}
