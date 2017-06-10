﻿using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Validation;

namespace JJ.Business.Synthesizer.Validation.Documents
{
    internal class DocumentValidator_Basic : VersatileValidator<Document>
    {
        public DocumentValidator_Basic(Document obj)
            : base(obj)
        { 
            For(() => obj.AudioOutput, ResourceFormatter.AudioOutput).NotNull();

            ExecuteValidator(new NameValidator(obj.Name), ValidationHelper.GetMessagePrefix(obj));
        }
    }
}
