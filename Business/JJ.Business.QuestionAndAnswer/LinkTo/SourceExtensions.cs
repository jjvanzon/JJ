using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.LinkTo
{
    public static class SourceExtensions
    {
        public static void LinkTo(this Source source, Question question)
        {
            if (source == null) { throw new ArgumentNullException("source"); }

            if (question.Source != null)
            {
                if (question.Source.Questions.Contains(question))
                {
                    question.Source.Questions.Remove(question);
                }
            }
            
            question.Source = source;
            
            if (question.Source != null)
            {
                if (!question.Source.Questions.Contains(question))
                {
                    question.Source.Questions.Add(question);
                }
            }
        }
    }
}