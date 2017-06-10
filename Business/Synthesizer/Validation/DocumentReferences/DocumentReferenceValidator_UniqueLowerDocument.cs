﻿using JetBrains.Annotations;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.DocumentReferences
{
    internal class DocumentReferenceValidator_UniqueLowerDocument : VersatileValidator<DocumentReference>
    {
        public DocumentReferenceValidator_UniqueLowerDocument([NotNull] DocumentReference obj) 
            : base(obj)
        {
            bool isUnique = ValidationHelper.DocumentReference_LowerDocument_IsUnique(obj);

            // ReSharper disable once InvertIf
            if (!isUnique)
            {
                string lowerDocumentReferenceIdentifier = ValidationHelper.GetUserFriendlyIdentifier_ForLowerDocumentReference(obj);
                string message = ResourceFormatter.LibraryAlreadyAdded_WithName(lowerDocumentReferenceIdentifier);
                ValidationMessages.Add(nameof(DocumentReference.LowerDocument), message);
            }
        }
    }
}
