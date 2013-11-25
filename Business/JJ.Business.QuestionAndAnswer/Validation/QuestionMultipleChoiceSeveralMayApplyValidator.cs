using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.Enums;
using JJ.Business.QuestionAndAnswer.Resources;
using JJ.Framework.Validation;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Business.QuestionAndAnswer.Validation
{
    public class QuestionMultipleChoiceSeveralMayApplyValidator: FluentValidator<Question>
    {
        public QuestionMultipleChoiceSeveralMayApplyValidator(Question question)
            : base(question)
        { }

        protected override void Execute()
        {
            For(Object.GetQuestionTypeEnum(), PropertyDisplayNames.QuestionTypeEnum)
                .IsValue(QuestionTypeEnum.MultipleChoiceSeveralMayApply);

            For(Object.Answers.Count, PropertyDisplayNames.AnswersCount)
                .Above(1);

            For(Object.Answers.Where(x => x.IsCorrectAnswer).Count(), PropertyDisplayNames.CorrectAnswerCount)
                .Above(1);
        }
    }
}
