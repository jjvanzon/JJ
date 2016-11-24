﻿using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Resources;

namespace JJ.Business.Synthesizer.Validation.Curves
{
    internal class CurveValidator_InDocument : VersatileValidator<Curve>
    {
        public CurveValidator_InDocument(Curve obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            For(() => Object.Document, PropertyDisplayNames.Document).NotNull();

            ExecuteValidator(new NameValidator(Object.Name));

            // TODO: Consider if more additional constraints need to be enforced in a document e.g. reference constraints. 
        }
    }
}