using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Reflection;
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

            For(() => question.QuestionType, PropertyDisplayNames.QuestionType)
                .NotNull();

            For(() => question.Text, PropertyDisplayNames.Text)
                .NotNullOrWhiteSpace();

            for (int i = 0; i < question.Answers.Count ; i++)
            {
                string messagePrefix = String.Format("{0} {1}: ", PropertyDisplayNames.Answer, i + 1);

                Execute(new AnswerValidator(question.Answers[i]), messagePrefix);
            }

            for (int i = 0; i < question.QuestionCategories.Count; i++)
            {
                string messagePrefix = String.Format("{0} {1}: ", PropertyDisplayNames.QuestionCategory, i + 1);
                Execute(new QuestionCategoryValidator(question.QuestionCategories[i]), messagePrefix);
            }

            for (int i = 0; i < question.QuestionLinks.Count; i++)
            {
                string messagePrefix = String.Format("{0} {1}: ", PropertyDisplayNames.QuestionLink, i + 1);

                Execute(new QuestionLinkValidator(question.QuestionLinks[i]), messagePrefix);
            }
        }
    }
}
