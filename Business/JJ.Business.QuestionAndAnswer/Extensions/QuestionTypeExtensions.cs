using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class QuestionTypeExtensions
    {
        public static void LinkTo(this QuestionType questionType, Question question)
        {
            if (questionType == null)
            {
                throw new ArgumentNullException("questionType");
            }
            if (question == null)
            {
                throw new ArgumentNullException("question");
            }

            questionType.Questions.Add(question);
            question.QuestionType = questionType;
        }
    }
}
