using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;
using System.Linq;
using JJ.Framework.Exceptions;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Business.QuestionAndAnswer.Validation
{
	/// <summary> For full validation, also execute BasicQuestionValidator. </summary>
	public class SeveralMayApplyQuestionValidator : VersatileValidator
	{
		/// <summary> For full validation, also execute BasicQuestionValidator. </summary>
		public SeveralMayApplyQuestionValidator(Question entity)
		{
			if (entity == null) throw new NullException(() => entity);

			if (entity.QuestionType != null)
			{
				For(entity.GetQuestionTypeEnum(), PropertyDisplayNames.QuestionType)
					.Is(QuestionTypeEnum.SeveralMayApply);
			}

			For(entity.Answers.Count, PropertyDisplayNames.AnswersCount)
				.GreaterThan(1);

			For(entity.Answers.Where(x => x.IsCorrectAnswer).Count(), PropertyDisplayNames.CorrectAnswerCount)
				.GreaterThan(1);
		}
	}
}
