using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Business.QuestionAndAnswer.LinkTo;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class AutoCreateRelatedEntitiesExtensions
    {
        public static void AutoCreateRelatedEntities(this Question question, IAnswerRepository answerRepository)
        {
            if (question == null) throw new ArgumentNullException("question");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");

            if (question.Answers.Count == 0)
            {
                Answer answer = answerRepository.Create();
                answer.LinkTo(question);
            }
        }
    }
}
