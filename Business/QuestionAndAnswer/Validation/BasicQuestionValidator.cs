using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Validation;
// ReSharper disable JoinDeclarationAndInitializer

namespace JJ.Business.QuestionAndAnswer.Validation
{
	/// <summary> Performs basic validations for questions in general </summary>
	public class BasicQuestionValidator : VersatileValidator
	{
		/// <summary> Performs basic validations for questions in general </summary>
		public BasicQuestionValidator(Question entity)
		{
			if (entity == null) throw new NullException(() => entity);

			For(entity.QuestionType, ResourceFormatter.QuestionType).NotNull();
			For(entity.Text, ResourceFormatter.Text).NotNullOrWhiteSpace();

			int i;

			i = 1;
			foreach (Answer answer in entity.Answers)
			{
				string messagePrefix = $"{ResourceFormatter.Answer} {i++}: ";

				ExecuteValidator(new AnswerValidator(answer), messagePrefix);
			}
			
			i = 1;
			foreach (QuestionCategory questionCategory in entity.QuestionCategories)
			{
				string messagePrefix = $"{ResourceFormatter.QuestionCategory} {i++}: ";
				ExecuteValidator(new QuestionCategoryValidator(questionCategory), messagePrefix);
			}

			i = 1;
			foreach (QuestionLink questionLink in entity.QuestionLinks)
			{
				string messagePrefix = $"{ResourceFormatter.QuestionLink} {i++}: ";

				ExecuteValidator(new QuestionLinkValidator(questionLink), messagePrefix);
			}
		}
	}
}
