using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Framework.Validation;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Business.QuestionAndAnswer.Resources;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    /// <summary> For full validation, also execute BasicQuestionValidator. </summary>
    public class OpenQuestionValidator : FluentValidator<Question>
    {
        /// <summary> For full validation, also execute BasicQuestionValidator. </summary>
        public OpenQuestionValidator(Question question)
            : base(question)
        { }

        protected override void Execute()
        {
            if (Object.QuestionType != null)
            {
                For(() => Object.GetQuestionTypeEnum(), PropertyDisplayNames.QuestionTypeEnum)
                    .IsValue(QuestionTypeEnum.OpenQuestion);
            }

            For(() => Object.Answers.Count, PropertyDisplayNames.AnswersCount)
                .IsValue(1);

            if (Object.Answers.Count > 0)
            {
                For(() => Object.Answers[0].IsCorrectAnswer, PropertyDisplayNames.IsCorrectAnswer)
                    .IsValue(true);
            }
        }
    }
}
