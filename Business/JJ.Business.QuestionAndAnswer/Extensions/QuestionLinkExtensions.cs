using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static partial class QuestionLinkExtensions
    {
        public static void LinkTo(this QuestionLink questionLink, Question question)
        {
            if (questionLink == null)
            {
                throw new ArgumentNullException("questionLink");
            }
            if (question == null)
            {
                throw new ArgumentNullException("question");
            }

            questionLink.Question = question;
            question.QuestionLinks.Add(questionLink);
        }
    }
}
