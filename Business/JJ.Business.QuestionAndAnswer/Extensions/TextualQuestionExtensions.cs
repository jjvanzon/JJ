using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.Enums;

namespace JJ.Business.QuestionAndAnswer.Extensions
{
    public static class TextualQuestionExtensions
    {
        public static void SetCategoryValue(this TextualQuestion entity, ICategoryRepository categoryRepository, CategoryEnum value)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");

            Category category = categoryRepository.Get((int)value);
            entity.Category = category;
        }

        public static void SetSourceValue(this TextualQuestion entity, ISourceRepository sourceRepository, SourceEnum value)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (sourceRepository == null) throw new ArgumentNullException("sourceRepository");

            Source source = sourceRepository.Get((int)value);
            entity.Source = source;
        }

        public static void AutoCreateRelatedEntities(this TextualQuestion entity, ITextualAnswerRepository textualAnswerRepository)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (textualAnswerRepository == null) throw new ArgumentNullException("textualAnswerRepository");

            if (entity.TextualAnswer() == null)
            {
                TextualAnswer textualAnswer = textualAnswerRepository.Create();
                entity.SetTextualAnswer(textualAnswer);
                textualAnswer.TextualQuestion = entity;
            }
        }

        public static void DeleteRelatedEntities(this TextualQuestion entity, ITextualAnswerRepository textualAnswerRepository)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (textualAnswerRepository == null) throw new ArgumentNullException("textualAnswerRepository");

            if (entity.TextualAnswer() != null)
            {
                textualAnswerRepository.Delete(entity.TextualAnswer());
            }
        }

        public static TextualAnswer TextualAnswer(this TextualQuestion entity)
        {
            switch (entity.TextualAnswers.Count)
            {
                case 0:
                    return null;

                case 1:
                    return entity.TextualAnswers[0];

                default:
                    throw new Exception(String.Format("TextualQuestion with ID '{0}' has multiple answers.", entity.ID));
            }
        }

        public static void SetTextualAnswer(this TextualQuestion entity, TextualAnswer textualAnswer)
        {
            switch (entity.TextualAnswers.Count)
            {
                case 0:
                    entity.TextualAnswers.Add(textualAnswer);
                    break;

                case 1:
                    entity.TextualAnswers[0] = textualAnswer;
                    break;

                default:
                    throw new Exception(String.Format("TextualQuestion with ID '{0}' has multiple answers.", entity.ID));
            }
        }
    }
}
