using JJ.Framework.Validation;
using JJ.Persistence.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Common;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    /// <summary>
    /// Performs basic validations for questions in general
    /// and executes different validations depending on the question type (open question, multiple choice, several may apply).
    /// </summary>
    public class VersatileQuestionValidator : FluentValidator<Question>
    {
        /// <summary>
        /// Performs basic validations for questions in general
        /// and executes different validations depending on the question type (open question, multiple choice, several may apply).
        /// </summary>
        public VersatileQuestionValidator(Question obj)
            : base(obj)
        { }

        protected override void Execute()
        {
            Question question = Object;

            Execute(new BasicQuestionValidator(question));

            if (question.QuestionType != null)
            {
                switch (question.GetQuestionTypeEnum())
                {
                    case QuestionTypeEnum.OpenQuestion:
                        Execute(new OpenQuestionValidator(question));
                        break;

                    case QuestionTypeEnum.MultipleChoice:
                        Execute(new MultipleChoiceQuestionValidator(question));
                        break;

                    case QuestionTypeEnum.SeveralMayApply:
                        Execute(new SeveralMayApplyQuestionValidator(question));
                        break;
                }
            }
        }
    }
}
