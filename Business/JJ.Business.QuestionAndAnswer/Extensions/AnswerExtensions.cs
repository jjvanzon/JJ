using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class AnswerExtensions
    {
        public static void LinkTo(this Answer answer, Question question)
        {
            if (answer == null)
            {
                throw new ArgumentNullException("answer");
            }
            if (question == null)
            {
                throw new ArgumentNullException("question");
            }

            answer.Question = question;
            question.Answers.Add(answer);
        }
    }
}
