using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    public class QuestionLinkValidator : VersatileValidator<QuestionLink>
    {
        public QuestionLinkValidator(QuestionLink entity)
            : base(entity)
        { 
            For(() => entity.Description, PropertyDisplayNames.Description).NotNullOrWhiteSpace();
            For(() => entity.Url, PropertyDisplayNames.Url).NotNullOrWhiteSpace();
        }
    }
}
