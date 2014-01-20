using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class AnswerExtensions_LinkTo
    {
        public static void LinkTo(this Answer answer, Question question)
        {
            if (answer == null) { throw new ArgumentNullException("answer"); }

            if (answer.Question != null)
            {
                if (answer.Question.Answers.Contains(answer))
                {
                    answer.Question.Answers.Remove(answer);
                }
            }

            answer.Question = question;

            if (answer.Question != null)
            {
                if (!answer.Question.Answers.Contains(answer))
                {
                    answer.Question.Answers.Add(answer);
                }
            }
        }
    }
}
