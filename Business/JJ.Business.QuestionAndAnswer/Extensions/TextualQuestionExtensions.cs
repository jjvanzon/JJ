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
            Category category = categoryRepository.Get((int)value);
            entity.Category = category;
        }

        public static void SetSourceValue(this TextualQuestion entity, ISourceRepository sourceRepository, SourceEnum value)
        {
            Source source = sourceRepository.Get((int)value);
            entity.Source = source;
        }
    }
}
