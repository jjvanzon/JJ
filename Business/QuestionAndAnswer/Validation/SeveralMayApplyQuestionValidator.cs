using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;
using System.Linq;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    /// <summary> For full validation, also execute BasicQuestionValidator. </summary>
    public class SeveralMayApplyQuestionValidator : VersatileValidator<Question>
    {
        /// <summary> For full validation, also execute BasicQuestionValidator. </summary>
        public SeveralMayApplyQuestionValidator(Question entity)
            : base(entity)
        { 
            if (entity.QuestionType != null)
            {
                For(() => entity.GetQuestionTypeEnum(), PropertyDisplayNames.QuestionType)
                    .Is(QuestionTypeEnum.SeveralMayApply);
            }

            For(() => entity.Answers.Count, PropertyDisplayNames.AnswersCount)
                .GreaterThan(1);

            For(() => entity.Answers.Where(x => x.IsCorrectAnswer).Count(), PropertyDisplayNames.CorrectAnswerCount)
                .GreaterThan(1);
        }
    }
}
