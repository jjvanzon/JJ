using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Business.QuestionAndAnswer;
using JJ.Models.Canonical;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;

namespace JJ.Apps.QuestionAndAnswer.Helpers
{
    internal static class ViewModelHelper
    {
        public static QuestionEditViewModel CreateEmptyQuestionEditViewModel(ICategoryRepository categoryRepository, IFlagStatusRepository flagStatusRepository)
        {
            var viewModel = new QuestionEditViewModel
            {
                Question = ViewModelHelper.CreateEmptyQuestionViewModel(),
                FlagStatuses = ViewModelHelper.CreateFlagStatusListViewModel(flagStatusRepository),
                Categories = ViewModelHelper.CreateCategoryListViewModelRecursive(categoryRepository),
                ValidationMessages = new List<ValidationMessage>()
            };

            return viewModel;
        }

        private static QuestionViewModel CreateEmptyQuestionViewModel()
        {
            return new QuestionViewModel
            {
                ID = 0,
                Text = "",
                Answer = "",
                IsActive = true,
                IsFlagged = false,
                Source = new SourceViewModel { ID = 0, Description = "" },
                Type = new QuestionTypeViewModel { ID = 0, Name = "" },
                Categories = new ListViewModel<QuestionCategoryViewModel>(),
                Links = new ListViewModel<QuestionLinkViewModel>(),
                Flags = new ListViewModel<QuestionFlagViewModel>()
            };
        }

        public static IList<FlagStatusViewModel> CreateFlagStatusListViewModel(IFlagStatusRepository flagStatusRepository)
        {
            if (flagStatusRepository == null) throw new ArgumentNullException("flagStatusRepository");

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
            if (categoryRepository == null) { throw new ArgumentNullException("categoryRepository"); }

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
                ID = 0,
                NameParts = new List<string>(),
                SubCategories = new List<CategoryViewModel>(),
                Visible = true
            };
        }
    }
}
