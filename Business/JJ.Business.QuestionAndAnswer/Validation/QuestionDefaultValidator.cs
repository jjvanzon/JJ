using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    public class QuestionDefaultValidator : FluentValidator<Question>
    {
        public QuestionDefaultValidator(Question question)
            : base (question)
        { }

        protected override void Execute()
        {
            Execute(new QuestionQuestionTypeValidator(X));

            For(X.Text, PropertyDisplayNames.Text)
                .NotNullOrWhiteSpace();
        }
    }
}
