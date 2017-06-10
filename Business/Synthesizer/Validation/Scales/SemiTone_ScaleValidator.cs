﻿using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.Scales
{
    internal class SemiTone_ScaleValidator : VersatileValidator<Scale>
    {
        public SemiTone_ScaleValidator(Scale obj)
            : base(obj)
        {
            For(() => obj.BaseFrequency, ResourceFormatter.BaseFrequency).NotNull();
            For(() => obj.GetScaleTypeEnum(), ResourceFormatter.ScaleType).Is(ScaleTypeEnum.SemiTone);
        }
    }
}
