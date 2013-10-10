using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    internal static class CategoryExtensions
    {
        public static CategoryNodeViewModel ToNodeViewModelRecursive(this Category category)
        {
            if (category == null) throw new ArgumentNullException("category");

            CategoryNodeViewModel viewModel = category.ToNodeViewModel();

            foreach (Category subCategory in category.SubCategories)
            {
                CategoryNodeViewModel subCategoryViewModel = subCategory.ToNodeViewModelRecursive();
                viewModel.SubCategories.Add(subCategoryViewModel);
            }

            // Sort by alphabet
            viewModel.SubCategories = viewModel.SubCategories.OrderBy(x => x.Category.NameParts.Last()).ToList();

            return viewModel;
        }

        public static CategoryNodeViewModel ToNodeViewModel(this Category category)
        {
            if (category == null) throw new ArgumentNullException("category");

            return new CategoryNodeViewModel
            {
                Category = category.ToViewModel(),
                SubCategories = new List<CategoryNodeViewModel>()
            };
        }

        public static CategoryViewModel ToViewModel(this Category category)
        {
            if (category == null) throw new ArgumentNullException("category");

            var categoryViewModel = new CategoryViewModel();
            categoryViewModel.ID = category.ID;
            categoryViewModel.NameParts = GetCategoryParts(category);

            return categoryViewModel;
        }

        private static List<string> GetCategoryParts(Category category)
        {
            List<string> parts = new List<string>();

            parts.Add(category.Description);
            category = category.ParentCategory;

            int counter = 0;
            int maxRecursion = 100;

            while (category != null && counter < maxRecursion)
            {
                parts.Add(category.Description);
                category = category.ParentCategory;

                counter++;
            }

            parts.Reverse();

            return parts;
        }
    }
}
