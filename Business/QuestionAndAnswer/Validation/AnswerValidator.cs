using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions;

namespace JJ.Business.QuestionAndAnswer.Validation
{
	public class AnswerValidator : VersatileValidator
	{
		public AnswerValidator(Answer entity)
		{
			if (entity == null) throw new NullException(() => entity);

			For(entity.Text, PropertyDisplayNames.Text).NotNullOrWhiteSpace();
		}
	}
}
