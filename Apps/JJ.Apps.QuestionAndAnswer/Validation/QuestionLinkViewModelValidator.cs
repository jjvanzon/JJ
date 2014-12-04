using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.Validation
{
    public class QuestionLinkViewModelValidator : FluentValidator<QuestionLinkViewModel>
    {
        public QuestionLinkViewModelValidator(QuestionLinkViewModel obj)
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