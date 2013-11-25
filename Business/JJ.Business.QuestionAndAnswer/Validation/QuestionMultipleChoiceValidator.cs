using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Framework.Validation;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Business.QuestionAndAnswer.Resources;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    public class QuestionMultipleChoiceValidator : FluentValidator<Question>
    {
        public QuestionMultipleChoiceValidator(Question question)
            : base(question)
        { }

        protected override void Execute()
        {
            For(Object.GetQuestionTypeEnum(), PropertyDisplayNames.QuestionTypeEnum)
                .IsValue(QuestionTypeEnum.MultipleChoice);

            For(Object.Answers.Count, PropertyDisplayNames.AnswersCount)
                .Above(1);

            For(Object.Answers.Where(x => x.IsCorrectAnswer).Count(), PropertyDisplayNames.CorrectAnswerCount)
                .IsValue(1);
        }
    }
}