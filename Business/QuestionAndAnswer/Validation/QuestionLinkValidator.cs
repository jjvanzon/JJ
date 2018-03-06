using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions;
using JJ.Framework.Exceptions.Basic;

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
