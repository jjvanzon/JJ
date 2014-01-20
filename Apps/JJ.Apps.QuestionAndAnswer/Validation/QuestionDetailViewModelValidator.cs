using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Validation
{
    public class QuestionDetailViewModelValidator : FluentValidator<QuestionDetailViewModel>
    {
        public QuestionDetailViewModelValidator(QuestionDetailViewModel obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            Execute(new QuestionViewModelValidator(Object.Question), () => Object.Question);
        }
    }
}
