using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer;
using JJ.Models.Canonical;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ToViewModel;
using JJ.Framework.Reflection;

namespace JJ.Apps.QuestionAndAnswer.Helpers
{
    internal static class ViewModelHelper
    {
        public static IList<FlagStatusViewModel> CreateFlagStatusListViewModel(IFlagStatusRepository flagStatusRepository)
        {
            if (flagStatusRepository == null) throw new NullException(() => flagStatusRepository);

            var list = new List<FlagStatusViewModel>();

            foreach (FlagStatus flagStatus in flagStatusRepository.GetAll().ToArray())
            {
                FlagStatusViewModel flagStatusViewModel = flagStatus.ToViewModel();
                list.Add(flagStatusViewModel);
            }

            return list;
        }

        /// <summary> Gets a tree of category view models. </summary>
        public static IList<CategoryViewModel> CreateCategoryListViewModelRecursive(ICategoryRepository categoryRepository)
        {
            if (categoryRepository == null) throw new NullException(() => categoryRepository);

            var categoryManager = new CategoryManager(categoryRepository);

            IEnumerable<Category> categories = categoryManager.GetCategoryTree();

            var viewModels = new List<CategoryViewModel>();

            foreach (Category category in categories)
            {
                CategoryViewModel viewModel = category.ToViewModelRecursive();
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public static CategoryViewModel CreateEmptyCategoryViewModel()
        {
            return new CategoryViewModel
            {
                NameParts = new List<string>(),
                SubCategories = new List<CategoryViewModel>(),
                Visible = true
            };
        }
    }
}
