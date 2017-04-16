﻿using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.Scales
{
    internal class Exponent_ScaleValidator : VersatileValidator<Scale>
    {
        public Exponent_ScaleValidator(Scale obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            For(() => Obj.BaseFrequency, ResourceFormatter.BaseFrequency).NotNull();
            For(() => Obj.GetScaleTypeEnum(), ResourceFormatter.ScaleType).Is(ScaleTypeEnum.Exponent);
        }
    }
}
