using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    /// <summary> For full validation, also execute BasicQuestionValidator. </summary>
    public class SeveralMayApplyQuestionValidator : FluentValidator<Question>
    {
        /// <summary> For full validation, also execute BasicQuestionValidator. </summary>
        public SeveralMayApplyQuestionValidator(Question question)
            : base(question)
        { }

        protected override void Execute()
        {
            if (Object.QuestionType != null)
            {
                For(() => Object.GetQuestionTypeEnum(), PropertyDisplayNames.QuestionTypeEnum)
                    .IsValue(QuestionTypeEnum.SeveralMayApply);
            }

            For(() => Object.Answers.Count, PropertyDisplayNames.AnswersCount)
                .Above(1);

            For(() => Object.Answers.Where(x => x.IsCorrectAnswer).Count(), PropertyDisplayNames.CorrectAnswerCount)
                .Above(1);
        }
    }
}
