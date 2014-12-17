using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Framework.Reflection;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class AutoCreateRelatedEntitiesExtensions
    {
        public static void AutoCreateRelatedEntities(this Question question, IAnswerRepository answerRepository)
        {
            if (question == null) throw new NullException(() => question);
            if (answerRepository == null) throw new NullException(() => answerRepository);

            if (question.Answers.Count == 0)
            {
                Answer answer = answerRepository.Create();
                answer.LinkTo(question);
            }
        }
    }
}
