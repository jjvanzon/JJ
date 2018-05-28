using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Validation;

namespace JJ.Business.QuestionAndAnswer.Validation
{
	/// <summary> For full validation, also execute BasicQuestionValidator. </summary>
	public class OpenQuestionValidator : VersatileValidator
	{
		/// <summary> For full validation, also execute BasicQuestionValidator. </summary>
		public OpenQuestionValidator(Question entity)
		{
			if (entity == null) throw new NullException(() => entity);

			if (entity.QuestionType != null)
			{
				For(entity.GetQuestionTypeEnum(), PropertyDisplayNames.QuestionType).Is(QuestionTypeEnum.OpenQuestion);
			}

			For(entity.Answers.Count, PropertyDisplayNames.AnswersCount).Is(1);

			if (entity.Answers.Count > 0)
			{
				For(entity.Answers[0].IsCorrectAnswer, PropertyDisplayNames.IsCorrectAnswer).Is(true);
			}
		}
	}
}
