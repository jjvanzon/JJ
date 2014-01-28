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
    public static class QuestionExtensions_Other
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

        public static void DeleteRelatedEntities(this Question question, IAnswerRepository answerRepository, IQuestionCategoryRepository questionCategoryRepository, IQuestionLinkRepository questionLinkRepository, IQuestionFlagRepository questionFlagRepository)
        {
            if (question == null) throw new ArgumentNullException("question");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");

            foreach (Answer answer in question.Answers.ToArray())
            {
                answer.LinkTo((Question)null);
                answerRepository.Delete(answer);
            }
            question.Answers.Clear();

            foreach (QuestionCategory questionCategory in question.QuestionCategories.ToArray())
            {
                questionCategory.LinkTo((Question)null);
                questionCategoryRepository.Delete(questionCategory);
            }
            question.QuestionCategories.Clear();

            foreach (QuestionLink questionLink in question.QuestionLinks.ToArray())
            {
                questionLink.LinkTo((Question)null);
                questionLinkRepository.Delete(questionLink);
            }
            question.QuestionLinks.Clear();

            foreach (QuestionFlag questionFlag in question.QuestionFlags.ToArray())
            {
                questionFlag.LinkTo((Question)null);
                questionFlagRepository.Delete(questionFlag);
            }
            question.QuestionFlags.Clear();
        }

        /// <summary> Unlinks only the non-owned related entities. </summary>
        public static void UnlinkRelatedEntities(this Question question)
        {
            question.LinkTo((Source)null);
            question.LinkTo((QuestionType)null);
        }
    }
}

