using JJ.Framework.Reflection.Exceptions;
using JJ.Persistence.QuestionAndAnswer;
using JJ.Persistence.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Import
{
    public abstract class CategoryDescriptionCorrectorBase
    {
        private ICategoryRepository _categoryRepository;

        public CategoryDescriptionCorrectorBase(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null) throw new NullException(() => categoryRepository);

            _categoryRepository = categoryRepository;
        }

        public abstract void Execute();

        protected void CorrectCategoryDescription(string identifier, string description)
        {
            Category category = _categoryRepository.TryGetByIdentifier(identifier);
            if (category == null)
            {
                return;
            }
            category.Description = description;
        }
    }
}
