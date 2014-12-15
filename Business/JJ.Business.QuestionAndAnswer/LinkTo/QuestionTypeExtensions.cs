using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.LinkTo
{
    public static class QuestionTypeExtensions
    {
        public static void LinkTo(this QuestionType questionType, Question question)
        {
            if (questionType == null) { throw new ArgumentNullException("questionType"); }
            
            if (question.QuestionType != null)
            {
                if (question.QuestionType.Questions.Contains(question))
                {
                    question.QuestionType.Questions.Remove(question);
                }
            }

            question.QuestionType = questionType;

            if (question.QuestionType != null)
            {
                if (!question.QuestionType.Questions.Contains(question))
                {
                    question.QuestionType.Questions.Add(question);
                }
            }
        }
    }
}