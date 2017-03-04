using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Resources;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    /// <summary> For full validation, also execute BasicQuestionValidator. </summary>
    public class OpenQuestionValidator : VersatileValidator_WithoutConstructorArgumentNullCheck<Question>
    {
        /// <summary> For full validation, also execute BasicQuestionValidator. </summary>
        public OpenQuestionValidator(Question question)
            : base(question)
        { }

        protected override void Execute()
        {
            if (Obj.QuestionType != null)
            {
                For(() => Obj.GetQuestionTypeEnum(), PropertyDisplayNames.QuestionType)
                    .Is(QuestionTypeEnum.OpenQuestion);
            }

            For(() => Obj.Answers.Count, PropertyDisplayNames.AnswersCount)
                .Is(1);

            if (Obj.Answers.Count > 0)
            {
                For(() => Obj.Answers[0].IsCorrectAnswer, PropertyDisplayNames.IsCorrectAnswer)
                    .Is(true);
            }
        }
    }
}
