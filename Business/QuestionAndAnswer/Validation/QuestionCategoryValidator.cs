using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    public class QuestionCategoryValidator : VersatileValidator<QuestionCategory>
    {
        public QuestionCategoryValidator(QuestionCategory entity)
            : base(entity)
        { 
            For(() => entity.Question, PropertyDisplayNames.Question).NotNull();
            For(() => entity.Category, PropertyDisplayNames.Category).NotNull();
        }
    }
}
