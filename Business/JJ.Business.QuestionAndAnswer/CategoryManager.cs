using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Business.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer.LinkTo;
using JJ.Framework.Reflection;

namespace JJ.Business.QuestionAndAnswer
{
    public class CategoryManager
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryManager(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null) throw new NullException(() => categoryRepository);
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Retrieves all categories from the data store and returns the root nodes.
        /// </summary>
        public Category[] GetCategoryTree()
        {
            IEnumerable<Category> allCategories = _categoryRepository.GetAll().ToArray(); // For performance, make sure all POCO's are loaded.
            IEnumerable<Category> rootCategories = allCategories.Where(x => x.ParentCategory == null);
            return rootCategories.ToArray();
        }

        // Get By Path

        public Category TryGetCategoryByIdentifierPath(params string[] identifiers)
        {
            if (identifiers == null) throw new NullException(() => identifiers);
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

        public Category FindOrCreateCategoryByIdentifierPath(params string[] identifiers)
        {
            if (identifiers == null) throw new NullException(() => identifiers);
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

        /// <summary>
        /// Builds up a flat list of category nodes, based on the selected categories.
        /// This is not as straightforeward as just taking the selected categories with their descendants.
        /// When a parent category is selected, but no child categories, you have to take all the nodes in the parent category.
        /// When a parent and some child categories are selected, you have to take those child categories.
        /// Effectively this means first excluding the parent nodes from the selected branches and then flattening the hierarchy of the nodes that remain.
        /// </summary>
        public IList<Category> SelectNodesRecursive(IEnumerable<Category> selectedCategories)
        {
            IList<Category> ancestors = GetAncestorsRecursive(selectedCategories);
            Category[] selectedBranches = selectedCategories.Except(ancestors).ToArray();

            var list = new List<Category>();

            foreach(Category selectedBranch in selectedBranches)
            {
                AddNodesRecursive(list, selectedBranch);
            }

            return list;
        }

        private IList<Category> GetAncestorsRecursive(IEnumerable<Category> categories)
        {
            var list = new List<Category>();
            foreach (Category category in categories)
            {
                AddAncestorsRecursive(list, category);
            }
            return list;
        }

        private void AddAncestorsRecursive(List<Category> outputList, Category category)
        {
            // TODO: Handle circularities.
            while (category.ParentCategory != null)
            {
                outputList.Add(category.ParentCategory);

                category = category.ParentCategory;
            }
        }

        private void AddNodesRecursive(List<Category> outputList, Category node)
        {
            // TODO: Handle circularities.
            outputList.Add(node);

            foreach (Category subNode in node.SubCategories)
            {
                AddNodesRecursive(outputList, subNode);
            }
        }
    }
}
