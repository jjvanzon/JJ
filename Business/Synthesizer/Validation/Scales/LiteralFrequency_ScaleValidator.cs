﻿using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.Scales
{
    internal class LiteralFrequency_ScaleValidator : VersatileValidator<Scale>
    {
        public LiteralFrequency_ScaleValidator(Scale obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            For(() => Obj.GetScaleTypeEnum(), ResourceFormatter.ScaleType).Is(ScaleTypeEnum.LiteralFrequency);
        }
    }
}
