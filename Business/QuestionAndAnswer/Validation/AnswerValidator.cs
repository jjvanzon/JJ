using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    public class AnswerValidator : VersatileValidator<Answer>
    {
        public AnswerValidator(Answer entity)
            : base(entity)
        { 
            For(() => entity.Text, PropertyDisplayNames.Text).NotNullOrWhiteSpace();
        }
    }
}
