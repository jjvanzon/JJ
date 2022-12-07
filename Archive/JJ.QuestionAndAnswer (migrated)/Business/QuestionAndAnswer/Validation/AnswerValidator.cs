using JJ.Data.QuestionAndAnswer;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.ResourceStrings;
using JJ.Framework.Validation;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    public class AnswerValidator : VersatileValidator
    {
        public AnswerValidator(Answer entity)
        {
            if (entity == null) throw new NullException(() => entity);

            For(entity.Text, CommonResourceFormatter.Text).NotNullOrWhiteSpace();
        }
    }
}