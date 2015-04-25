using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    public class AnswerValidator : FluentValidator<Answer>
    {
        public AnswerValidator(Answer obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            For(() => Object.Text, PropertyDisplayNames.Text)
                .NotNullOrWhiteSpace();
        }
    }
}
