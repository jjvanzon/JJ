using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    /// <summary> Performs basic validations for questions in general </summary>
    public class BasicQuestionValidator : VersatileValidator_WithoutConstructorArgumentNullCheck<Question>
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

            int i;

            i = 1;
            foreach (Answer answer in question.Answers)
            {
                string messagePrefix = String.Format("{0} {1}: ", PropertyDisplayNames.Answer, i++);

                ExecuteValidator(new AnswerValidator(answer), messagePrefix);
            }
            
            i = 1;
            foreach (QuestionCategory questionCategory in question.QuestionCategories)
            {
                string messagePrefix = String.Format("{0} {1}: ", PropertyDisplayNames.QuestionCategory, i++);
                ExecuteValidator(new QuestionCategoryValidator(questionCategory), messagePrefix);
            }

            i = 1;
            foreach (QuestionLink questionLink in question.QuestionLinks)
            {
                string messagePrefix = String.Format("{0} {1}: ", PropertyDisplayNames.QuestionLink, i++);

                ExecuteValidator(new QuestionLinkValidator(questionLink), messagePrefix);
            }
        }
    }
}
