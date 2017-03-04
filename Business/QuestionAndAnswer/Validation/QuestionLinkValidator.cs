using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    public class QuestionLinkValidator : VersatileValidator_WithoutConstructorArgumentNullCheck<QuestionLink>
    {
        public QuestionLinkValidator(QuestionLink obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            For(() => Obj.Description, PropertyDisplayNames.Description)
                .NotNullOrWhiteSpace();

            For(() => Obj.Url, PropertyDisplayNames.Url)
                .NotNullOrWhiteSpace();
        }
    }
}
