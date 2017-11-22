using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Framework.Exceptions;

namespace JJ.Business.QuestionAndAnswer.Validation
{
	/// <summary>
	/// Performs basic validations for questions in general
	/// and executes different validations depending on the question type (open question, multiple choice, several may apply).
	/// </summary>
	public class VersatileQuestionValidator : VersatileValidator
	{
		/// <summary>
		/// Performs basic validations for questions in general
		/// and executes different validations depending on the question type (open question, multiple choice, several may apply).
		/// </summary>
		public VersatileQuestionValidator(Question question)
		{
			if (question == null) throw new NullException(() => question);

			ExecuteValidator(new BasicQuestionValidator(question));

			if (question.QuestionType != null)
			{
				switch (question.GetQuestionTypeEnum())
				{
					case QuestionTypeEnum.OpenQuestion:
						ExecuteValidator(new OpenQuestionValidator(question));
						break;

					case QuestionTypeEnum.MultipleChoice:
						ExecuteValidator(new MultipleChoiceQuestionValidator(question));
						break;

					case QuestionTypeEnum.SeveralMayApply:
						ExecuteValidator(new SeveralMayApplyQuestionValidator(question));
						break;
				}
			}
		}
	}
}
