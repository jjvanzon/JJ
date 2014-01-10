using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Framework.Validation;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Business.QuestionAndAnswer.Resources;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    /// <summary> For full validation, also execute BasicQuestionValidator. </summary>
    public class MultipleChoiceQuestionValidator : FluentValidator<Question>
    {
        /// <summary> For full validation, also execute BasicQuestionValidator. </summary>
        public MultipleChoiceQuestionValidator(Question question)
            : base(question)
        { }

        protected override void Execute()
        {
            if (Object.QuestionType != null)
            {
                For(() => Object.QuestionType.ID, PropertyDisplayNames.QuestionTypeEnum)
                    .IsValue((int)QuestionTypeEnum.MultipleChoice);
            }

            For(() => Object.Answers.Count, PropertyDisplayNames.AnswersCount)
                .Above(1);

            For(() => Object.Answers.Where(x => x.IsCorrectAnswer).Count(), PropertyDisplayNames.CorrectAnswerCount)
                .IsValue(1);
        }
    }
}