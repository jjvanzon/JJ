using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Validation
{
    public class QuestionViewModelValidator : FluentValidator<QuestionViewModel>
    {
        public QuestionViewModelValidator(QuestionViewModel obj)
            : base(obj)
        { }
         
        protected override void Execute()
        {
            QuestionViewModel questionViewModel = Object;

            For(() => questionViewModel.Text, PropertyDisplayNames.Text)
                .NotNullOrWhiteSpace();

            For(() => questionViewModel.Answer, PropertyDisplayNames.Answer)
                .NotNullOrWhiteSpace();

            for (int i = 0; i < questionViewModel.Categories.Count; i++)
            {
                string messagePrefix = String.Format("{0} {1}: ", PropertyDisplayNames.QuestionCategory, i + 1);

                Execute(new QuestionCategoryViewModelValidator(questionViewModel.Categories[i]), () => questionViewModel.Categories[i], messagePrefix);
            }

            for (int i = 0; i < questionViewModel.Links.Count; i++)
            {
                string messagePrefix = String.Format("{0} {1}: ", PropertyDisplayNames.QuestionLink, i + 1);

                Execute(new QuestionLinkViewModelValidator(questionViewModel.Links[i]), () => questionViewModel.Links[i], messagePrefix);
            }
        }
    }
}