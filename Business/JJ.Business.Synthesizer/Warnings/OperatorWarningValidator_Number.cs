﻿using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using System;
using JJ.Framework.Common;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.Warnings
{
    internal class OperatorWarningValidator_Number : OperatorWarningValidator_Base
    {
        public OperatorWarningValidator_Number(Operator obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            double number;
            if (Doubles.TryParse(Object.Data, OperatorDataParser.FormattingCulture, out number))
            {
                if (number == 0.0)
                {
                    ValidationMessages.Add(() => Object.Data, MessageFormatter.NumberIs0WithName(Object.Name));
                }
            }
        }
    }
}
