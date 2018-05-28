using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Validation;

namespace JJ.Business.QuestionAndAnswer.Validation
{
	public class QuestionLinkValidator : VersatileValidator
	{
		public QuestionLinkValidator(QuestionLink entity)
		{
			if (entity == null) throw new NullException(() => entity);

			For(entity.Description, PropertyDisplayNames.Description).NotNullOrWhiteSpace();
			For(entity.Url, PropertyDisplayNames.Url).NotNullOrWhiteSpace();
		}
	}
}
