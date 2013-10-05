using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer.Extensions;

namespace JJ.Business.QuestionAndAnswer
{
    public class CategoryManager
    {
        private ICategoryRepository _categoryRepository;

        public CategoryManager(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");
            _categoryRepository = categoryRepository;
        }

        public Category FindOrCreateCategory(params string[] identifiers)
        {
            if (identifiers == null) throw new ArgumentNullException("identifiers");
            if (identifiers.Length == 0) throw new Exception("identifiers collection cannot be empty.");

            string rootIdentifier = identifiers[0];
            Category category = _categoryRepository.TryGetByIdentifier(rootIdentifier);
            if (category == null)
            {
                category = _categoryRepository.Create();
                category.Identifier = rootIdentifier;
                category.Description = rootIdentifier; // Default description is the identifier. Somebody could edit it later.
            }

            Category parentCategory = category;

            foreach (string identifier in identifiers.Skip(1))
            {
                category = _categoryRepository.TryGetCategoryByParentAndIdentifier(parentCategory, identifier);
                if (category == null)
                {
                    category = _categoryRepository.Create();
                    category.Identifier = identifier;
                    category.Description = identifier; // Default description is the identifier. Somebody could edit it later.

                    category.LinkToParentCategory(parentCategory);
                }

                parentCategory = category;
            }

            return category;
        }

        public Category TryGetCategory(params string[] identifiers)
        {
            if (identifiers == null) throw new ArgumentNullException("identifiers");
            if (identifiers.Length == 0) throw new Exception("identifiers collection cannot be empty.");

            string rootIdentifier = identifiers[0];
            Category category = _categoryRepository.TryGetByIdentifier(rootIdentifier);
            if (category == null)
            {
                return null;
            }

            Category parentCategory = category;

            foreach (string identifier in identifiers.Skip(1))
            {
                category = _categoryRepository.TryGetCategoryByParentAndIdentifier(parentCategory, identifier);
                if (category == null)
                {
                    return null;
                }

                parentCategory = category;
            }

            return category;
        }

        public List<Category> GetCategoryTree()
        {
            List<Category> allCategories = _categoryRepository.GetAll();
            List<Category> rootCategories = allCategories.Where(x => x.ParentCategory == null).ToList();
            return rootCategories;
        }
    }
}
