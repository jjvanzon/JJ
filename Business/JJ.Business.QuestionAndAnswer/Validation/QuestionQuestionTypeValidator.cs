using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Validation;
using JJ.Models.QuestionAndAnswer;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Business.QuestionAndAnswer.Enums;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    public class QuestionQuestionTypeValidator : FluentValidator<Question>
    {
        public QuestionQuestionTypeValidator(Question entity)
            : base(entity)
        { }

        protected override void Execute()
        {
            For(X.QuestionType, PropertyDisplayNames.QuestionType)
                .NotNull();

            if (X.QuestionType != null)
            {
                switch (X.GetQuestionTypeEnum())
                {
                    case QuestionTypeEnum.OpenQuestion:
                        Execute(new QuestionOpenQuestionValidator(X));
                        break;

                    case QuestionTypeEnum.MultipleChoice:
                        Execute(new QuestionMultipleChoiceValidator(X));
                        break;

                    case QuestionTypeEnum.MultipleChoiceSeveralMayApply:
                        Execute(new QuestionMultipleChoiceSeveralMayApplyValidator(X));
                        break;
                }
            }
        }
    }
}
