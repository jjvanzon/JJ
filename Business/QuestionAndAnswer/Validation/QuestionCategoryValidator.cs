using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Validation;

namespace JJ.Business.QuestionAndAnswer.Validation
{
	public class QuestionCategoryValidator : VersatileValidator
	{
		public QuestionCategoryValidator(QuestionCategory entity)
		{
			if (entity == null) throw new NullException(() => entity);

			For(entity.Question, PropertyDisplayNames.Question).NotNull();
			For(entity.Category, PropertyDisplayNames.Category).NotNull();
		}
	}
}
