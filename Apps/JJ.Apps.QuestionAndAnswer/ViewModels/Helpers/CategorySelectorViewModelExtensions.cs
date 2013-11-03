using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Apps.QuestionAndAnswer.ViewModels.Helpers
{
    public static class CategorySelectorViewModelExtensions
    {
        public static IEnumerable<CategoryViewModel> GetSelectedCategoriesRecursive(this CategorySelectorViewModel categorySelectorViewModel)
        {
            return GetSelectedCategoriesRecursive(categorySelectorViewModel.SelectedCategories);
        }

        private static IEnumerable<CategoryViewModel> GetSelectedCategoriesRecursive(this List<CategoryViewModel> categoryViewModels)
        {
            if (categoryViewModels == null)
            {
                yield break;
            }

            foreach (CategoryViewModel categoryViewModel in categoryViewModels)
            {
                yield return categoryViewModel;

                foreach (CategoryViewModel categoryViewModel2 in categoryViewModel.SubCategories.GetSelectedCategoriesRecursive())
                {
                    yield return categoryViewModel2;
                }
            }
        }
    }
}
