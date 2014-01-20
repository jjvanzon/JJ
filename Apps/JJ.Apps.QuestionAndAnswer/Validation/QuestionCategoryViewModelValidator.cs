using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Apps.QuestionAndAnswer.Validation
{
    public class QuestionCategoryViewModelValidator : FluentValidator<QuestionCategoryViewModel>
    {
        public QuestionCategoryViewModelValidator(QuestionCategoryViewModel obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            For(() => Object.Category.ID, PropertyDisplayNames.Category)
                .NotZero();
        }
    }
}
