using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    public class QuestionCategoryValidator : VersatileValidator
    {
        public QuestionCategoryValidator(QuestionCategory entity)
        {
            if (entity == null) throw new NullException(() => entity);

            For(() => entity.Question, PropertyDisplayNames.Question).NotNull();
            For(() => entity.Category, PropertyDisplayNames.Category).NotNull();
        }
    }
}
