using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.Enums;
using System.Collections;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static partial class QuestionExtensions
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

        public static void DeleteRelatedEntities(this Question question, IAnswerRepository answerRepository, IQuestionCategoryRepository questionCategoryRepository, IQuestionLinkRepository questionLinkRepository)
        {
            if (question == null) throw new ArgumentNullException("question");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");

            foreach (Answer answer in question.Answers.ToArray())
            {
                answer.Question = null;

                answerRepository.Delete(answer);
            }

            question.Answers.Clear();

            foreach (QuestionCategory questionCategory in question.QuestionCategories.ToArray())
            {
                questionCategory.Question = null;

                questionCategoryRepository.Delete(questionCategory);
            }

            question.QuestionCategories.Clear();

            foreach (QuestionLink questionLink in question.QuestionLinks.ToArray())
            {
                questionLink.Question = null;

                questionLinkRepository.Delete(questionLink);
            }

            question.QuestionLinks.Clear();
        }
    }
}
