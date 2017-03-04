using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Data.QuestionAndAnswer;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    public class AnswerValidator : VersatileValidator_WithoutConstructorArgumentNullCheck<Answer>
    {
        public AnswerValidator(Answer obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            For(() => Obj.Text, PropertyDisplayNames.Text)
                .NotNullOrWhiteSpace();
        }
    }
}
