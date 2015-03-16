using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Persistence.QuestionAndAnswer;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Framework.Reflection;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class DeleteRelatedEntitiesExtensions
    {
        public static void DeleteRelatedEntities(this Question question, IAnswerRepository answerRepository, IQuestionCategoryRepository questionCategoryRepository, IQuestionLinkRepository questionLinkRepository, IQuestionFlagRepository questionFlagRepository)
        {
            if (question == null) throw new NullException(() => question);
            if (answerRepository == null) throw new NullException(() => answerRepository);
            if (questionCategoryRepository == null) throw new NullException(() => questionCategoryRepository);
            if (questionLinkRepository == null) throw new NullException(() => questionLinkRepository);
            if (questionFlagRepository == null) throw new NullException(() => questionFlagRepository);

            foreach (Answer answer in question.Answers.ToArray())
            {
                answer.UnlinkQuestion();
                answerRepository.Delete(answer);
            }
            // TODO: Are these calls to Clear() not redundant? Unlink should have removed all the items already.
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
