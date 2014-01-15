using JJ.Framework.Validation;
using JJ.Models.QuestionAndAnswer;
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
    public class QuestionValidator : FluentValidator<Question>
    {
        /// <summary>
        /// Performs basic validations for questions in general
        /// and executes different validations depending on the question type (open question, multiple choice, several may apply).
        /// </summary>
        public QuestionValidator(Question obj)
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

            // HACK: Replace stuff in the property keys to at least see the invalid field highlighting work in the MVC view.
            ValidationMessages.ForEach(x => x.PropertyKey = "Question." + x.PropertyKey);
            ValidationMessages.ForEach(x => x.PropertyKey = x.PropertyKey.Replace("QuestionLinks", "Links"));
            ValidationMessages.ForEach(x => x.PropertyKey = x.PropertyKey.Replace("QuestionCategories", "Categories"));
            ValidationMessages.ForEach(x => x.PropertyKey = x.PropertyKey.Replace("].Category", "]"));
            ValidationMessages.ForEach(x => x.PropertyKey = x.PropertyKey.Replace("QuestionFlags", "Flags"));
            ValidationMessages.ForEach(x => x.PropertyKey = x.PropertyKey.Replace("Answers[0].Text", "Answer"));
        }
    }
}
