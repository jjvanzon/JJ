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
        public static void AutoCreateRelatedEntities(this Question entity, IAnswerRepository answerRepository)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");

            if (entity.Answers.Count == 0)
            {
                Answer answer = answerRepository.Create();
                answer.Question = entity;
                entity.Answers.Add(answer);
            }
        }

        public static void DeleteRelatedEntities(this Question question, IAnswerRepository answerRepository, IQuestionCategoryRepository questionCategoryRepository)
        {
            if (question == null) throw new ArgumentNullException("question");
            if (answerRepository == null) throw new ArgumentNullException("answerRepository");

            foreach (Answer answer in question.Answers.ToArray())
            {
                answerRepository.Delete(answer);
            }

            foreach (QuestionCategory questionCategory in question.QuestionCategories.ToArray())
            {
                questionCategoryRepository.Delete(questionCategory);
            }
        }

        public static Answer Answer(this Question entity)
        {
            switch (entity.Answers.Count)
            {
                case 0:
                    return null;

                case 1:
                    return entity.Answers[0];

                default:
                    throw new Exception(String.Format("Question with ID '{0}' has multiple answers.", entity.ID));
            }
        }

        /*public static void SetAnswer(this Question entity, Answer answer)
        {
            switch (entity.Answers.Count)
            {
                case 0:
                    entity.Answers.Add(answer);
                    break;

                case 1:
                    entity.Answers[0] = answer;
                    break;

                default:
                    throw new Exception(String.Format("Question with ID '{0}' has multiple answers.", entity.ID));
            }
        }*/

        // Temporary

        public static Category Category(this Question entity)
        {
            switch (entity.QuestionCategories.Count)
            {
                case 0:
                    return null;

                case 1:
                    return entity.QuestionCategories[0].Category;

                default:
                    throw new Exception(String.Format("Question with ID '{0}' has multiple categories.", entity.ID));
            }
        }

        // Temporary

        /*public static void SetCategory(this Question entity, Category category)
        {
            switch (entity.QuestionCategories.Count)
            {
                case 0:
                    QuestionCategory questionCategorie = new QuestionCategory() { Question = entity, Category = category };
                    entity.QuestionCategories.Add(questionCategorie);
                    break;

                case 1:
                    entity.QuestionCategories[0].Category = category;
                    break;

                default:
                    throw new Exception(String.Format("Question with ID '{0}' has multiple categories.", entity.ID));
            }
        }*/
    }
}
