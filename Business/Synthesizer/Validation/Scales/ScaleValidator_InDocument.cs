﻿using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.Scales
{
    internal class ScaleValidator_InDocument : VersatileValidator<Scale>
    {
        public ScaleValidator_InDocument(Scale obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            For(() => Obj.Document, PropertyDisplayNames.Document).NotNull();

            ExecuteValidator(new NameValidator(Obj.Name));
        }
    }
}
