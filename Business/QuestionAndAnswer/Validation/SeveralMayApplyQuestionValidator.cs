using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;
using System.Linq;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    /// <summary> For full validation, also execute BasicQuestionValidator. </summary>
    public class SeveralMayApplyQuestionValidator : VersatileValidator_WithoutConstructorArgumentNullCheck<Question>
    {
        /// <summary> For full validation, also execute BasicQuestionValidator. </summary>
        public SeveralMayApplyQuestionValidator(Question question)
            : base(question)
        { }

        protected override void Execute()
        {
            if (Obj.QuestionType != null)
            {
                For(() => Obj.GetQuestionTypeEnum(), PropertyDisplayNames.QuestionType)
                    .Is(QuestionTypeEnum.SeveralMayApply);
            }

            For(() => Obj.Answers.Count, PropertyDisplayNames.AnswersCount)
                .GreaterThan(1);

            For(() => Obj.Answers.Where(x => x.IsCorrectAnswer).Count(), PropertyDisplayNames.CorrectAnswerCount)
                .GreaterThan(1);
        }
    }
}
