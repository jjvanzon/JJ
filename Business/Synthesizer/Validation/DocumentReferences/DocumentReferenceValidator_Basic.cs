﻿using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.DocumentReferences
{
    internal class DocumentReferenceValidator_Basic : VersatileValidator<DocumentReference>
    {
        public DocumentReferenceValidator_Basic(DocumentReference documentReference) 
            : base(documentReference)
        { 
            ExecuteValidator(new NameValidator(documentReference.Alias, ResourceFormatter.Alias, required: false));

            For(() => documentReference.HigherDocument, ResourceFormatter.HigherDocument).NotNull();
            For(() => documentReference.LowerDocument, ResourceFormatter.Library).NotNull();
        }
    }
}
