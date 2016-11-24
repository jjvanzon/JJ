using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    public class QuestionLinkValidator : VersatileValidator_WithoutConstructorArgumentNullCheck<QuestionLink>
    {
        public QuestionLinkValidator(QuestionLink obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            For(() => Object.Description, PropertyDisplayNames.Description)
                .NotNullOrWhiteSpace();

            For(() => Object.Url, PropertyDisplayNames.Url)
                .NotNullOrWhiteSpace();
        }
    }
}
