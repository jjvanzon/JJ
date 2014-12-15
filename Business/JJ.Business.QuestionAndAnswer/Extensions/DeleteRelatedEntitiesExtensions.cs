using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.LinkTo;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class DeleteRelatedEntitiesExtensions
    {
        public static void DeleteRelatedEntities(this Question question, IAnswerRepository answerRepository, IQuestionCategoryRepository questionCategoryRepository, IQuestionLinkRepository questionLinkRepository, IQuestionFlagRepository questionFlagRepository)
        {
            if (question == null) throw new ArgumentNullException("question");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");
            if (questionCategoryRepository == null) throw new ArgumentNullException("questionCategoryRepository");
            if (questionLinkRepository == null) throw new ArgumentNullException("questionLinkRepository");
            if (questionFlagRepository == null) throw new ArgumentNullException("questionFlagRepository");

            foreach (Answer answer in question.Answers.ToArray())
            {
                answer.UnlinkQuestion();
                answerRepository.Delete(answer);
            }
            question.Answers.Clear();

            foreach (QuestionCategory questionCategory in question.QuestionCategories.ToArray())
            {
                questionCategory.UnlinkQuestion();
                questionCategoryRepository.Delete(questionCategory);
            }
            question.QuestionCategories.Clear();

            foreach (QuestionLink questionLink in question.QuestionLinks.ToArray())
            {
                questionLink.UnlinkQuestion();
                questionLinkRepository.Delete(questionLink);
            }
            question.QuestionLinks.Clear();

            foreach (QuestionFlag questionFlag in question.QuestionFlags.ToArray())
            {
                questionFlag.UnlinkQuestion();
                questionFlagRepository.Delete(questionFlag);
            }
            question.QuestionFlags.Clear();
        }
    }
}
