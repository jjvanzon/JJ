using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class QuestionLinkExtensions_LinkTo
    {
        public static void LinkTo(this QuestionLink questionLink, Question question)
        {
            if (questionLink == null) throw new ArgumentNullException("questionLink");
            if (question == null) throw new ArgumentNullException("question");

            questionLink.Question = question;
            question.QuestionLinks.Add(questionLink);
        }

        public static void Unlink(this QuestionLink questionLink, Question question)
        {
            if (questionLink == null) throw new ArgumentNullException("questionLink");
            if (question == null) throw new ArgumentNullException("question");

            questionLink.Question = null;
            if (question.QuestionLinks.Contains(questionLink))
            {
                question.QuestionLinks.Remove(questionLink);
            }
        }
    }
}