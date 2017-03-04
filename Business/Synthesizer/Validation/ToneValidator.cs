﻿using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation
{
    internal class ToneValidator : VersatileValidator<Tone>
    {
        public ToneValidator(Tone obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            For(() => Obj.Scale, PropertyDisplayNames.Scale).NotNull();
            For(() => Obj.Number, PropertyDisplayNames.Number).NotNaN().NotInfinity();
        }
    }
}
