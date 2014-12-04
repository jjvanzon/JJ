﻿using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Models.QuestionAndAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.Extensions
{
    internal static class CategoryExtensions_ToViewModel
    {
        // TODO: Use GetRecursive method.

        public static CategoryViewModel ToViewModelRecursive(this Category category)
        {
            if (category == null) throw new ArgumentNullException("category");

            CategoryViewModel viewModel = category.ToViewModel();

            foreach (Category subCategory in category.SubCategories)
            {
                CategoryViewModel subCategoryViewModel = subCategory.ToViewModelRecursive();
                viewModel.SubCategories.Add(subCategoryViewModel);
            }

            // Sort by alphabet
            viewModel.SubCategories = viewModel.SubCategories.OrderBy(x => x.NameParts.Last()).ToList();

            return viewModel;
        }

        public static CategoryViewModel ToViewModel(this Category category)
        {
            if (category == null) throw new ArgumentNullException("category");

            var categoryViewModel = new CategoryViewModel
            {
                ID = category.ID,
                Visible = true,
                NameParts = GetNameParts(category),
                SubCategories = new List<CategoryViewModel>()
            };

            return categoryViewModel;
        }

        private static List<string> GetNameParts(Category category)
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
