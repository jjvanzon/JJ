using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class SourceExtensions
    {
        public static void LinkTo(this Source source, Question question)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (question == null)
            {
                throw new ArgumentNullException("question");
            }

            source.Questions.Add(question);
            question.Source = source;
        }
    }
}
