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
    /// <summary> Performs basic validations for questions in general </summary>
    public class BasicQuestionValidator : FluentValidator<Question>
    {
        /// <summary> Performs basic validations for questions in general </summary>
        public BasicQuestionValidator(Question question)
            : base (question)
        { }

        protected override void Execute()
        {
            Question question = Object;

            For(question.QuestionType, PropertyDisplayNames.QuestionType)
                .NotNull();

            For(question.Text, PropertyDisplayNames.Text)
                .NotNullOrWhiteSpace();

            foreach (Answer answer in question.Answers)
            {
                string messageHeading = String.Format("{0}: ", PropertyDisplayNames.Answer);

                Execute(new AnswerValidator(answer), messageHeading);
            }

            foreach (QuestionLink questionLink in question.QuestionLinks)
            {
                string messageHeading = String.Format("{0}: ", PropertyDisplayNames.QuestionLink);

                Execute(new QuestionLinkValidator(questionLink), messageHeading);
            }
        }
    }
}
