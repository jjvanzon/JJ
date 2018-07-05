using System.Collections.Generic;
using System.Linq;
using JJ.Business.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Exceptions.Basic;
using JJ.Presentation.QuestionAndAnswer.Extensions;
using JJ.Presentation.QuestionAndAnswer.ToViewModel;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.ViewModels.Entities;

// ReSharper disable PossibleMultipleEnumeration

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
    public class CategorySelectorPresenter
    {
        private readonly IUserRepository _userRepository;
        private readonly CategoryManager _categoryManager;
        private readonly string _authenticatedUserName;

        /// <param name="authenticatedUserName">nullable</param>
        public CategorySelectorPresenter(ICategoryRepository categoryRepository, IUserRepository userRepository, string authenticatedUserName)
        {
            _userRepository = userRepository ?? throw new NullException(() => userRepository);
            _authenticatedUserName = authenticatedUserName;
            _categoryManager = new CategoryManager(categoryRepository);
        }

        public CategorySelectorViewModel Show() => CreateViewModel();

        public CategorySelectorViewModel Add(CategorySelectorViewModel viewModel, int categoryID)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            var selectedCategoryIDs = new List<int>();
            AddSelectedCategoryIDsRecursive(selectedCategoryIDs, viewModel.SelectedCategories);
            selectedCategoryIDs.Add(categoryID);

            return CreateViewModel(selectedCategoryIDs);
        }

        public CategorySelectorViewModel Remove(CategorySelectorViewModel viewModel, int categoryID)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            viewModel.NullCoalesce();

            var selectedCategoryIDs = new List<int>();
            AddSelectedCategoryIDsRecursive(selectedCategoryIDs, viewModel.SelectedCategories);

            while (selectedCategoryIDs.Contains(categoryID)) // while is for when there are duplicates
            {
                selectedCategoryIDs.Remove(categoryID);
            }

            return CreateViewModel(selectedCategoryIDs);
        }

        // Private Methods

        private CategorySelectorViewModel CreateViewModel()
        {
            IList<CategoryViewModel> availableCategories = CreateCategoriesViewModelRecursive();
            IList<CategoryViewModel> selectedCategories = CreateCategoriesViewModelRecursive();

            HideAllNodesRecursive(selectedCategories);

            var viewModel = new CategorySelectorViewModel
            {
                AvailableCategories = availableCategories,
                SelectedCategories = selectedCategories
            };

            viewModel.NoCategoriesAvailable = viewModel.AvailableCategories.Count == 0;

            viewModel.Login = ViewModelHelper.CreateLoginPartialViewModel(_authenticatedUserName, _userRepository);

            return viewModel;
        }

        private CategorySelectorViewModel CreateViewModel(IEnumerable<int> selectedCategoryIDs)
        {
            CategorySelectorViewModel viewModel = CreateViewModel();
            HideSelectedLeafNodesRecursive(viewModel.AvailableCategories, selectedCategoryIDs);
            ShowSelectedNodesRecursive(viewModel.SelectedCategories, selectedCategoryIDs);
            return viewModel;
        }

        private IList<CategoryViewModel> CreateCategoriesViewModelRecursive()
        {
            IEnumerable<Category> categories = _categoryManager.GetCategoryTree();

            var viewModels = new List<CategoryViewModel>();

            foreach (Category category in categories)
            {
                CategoryViewModel viewModel = category.ToViewModelRecursive();
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        private void HideAllNodesRecursive(IList<CategoryViewModel> categoryViewModels)
        {
            foreach (CategoryViewModel categoryViewModel in categoryViewModels)
            {
                categoryViewModel.Visible = false;

                HideAllNodesRecursive(categoryViewModel.SubCategories);
            }
        }

        private void AddSelectedCategoryIDsRecursive(IList<int> outputList, IList<CategoryViewModel> categoryViewModels)
        {
            if (categoryViewModels == null)
            {
                return;
            }

            foreach (CategoryViewModel categoryViewModel in categoryViewModels)
            {
                if (categoryViewModel.Visible)
                {
                    outputList.Add(categoryViewModel.ID);
                }

                AddSelectedCategoryIDsRecursive(outputList, categoryViewModel.SubCategories);
            }
        }

        private void ShowSelectedNodesRecursive(IList<CategoryViewModel> categoryViewModels, IEnumerable<int> selectedCategoryIDs)
        {
            foreach (CategoryViewModel categoryViewModel in categoryViewModels)
            {
                bool isSelected = selectedCategoryIDs.Contains(categoryViewModel.ID);

                if (isSelected)
                {
                    categoryViewModel.Visible = true;
                }

                ShowSelectedNodesRecursive(categoryViewModel.SubCategories, selectedCategoryIDs);
            }
        }

        private void HideSelectedLeafNodesRecursive(IList<CategoryViewModel> categoryViewModels, IEnumerable<int> selectedCategoryIDs)
        {
            foreach (CategoryViewModel categoryViewModel in categoryViewModels)
            {
                bool isSelected = selectedCategoryIDs.Contains(categoryViewModel.ID);
                bool isLeaf = categoryViewModel.SubCategories.Count == 0;

                if (isSelected && isLeaf)
                {
                    categoryViewModel.Visible = false;
                }

                HideSelectedLeafNodesRecursive(categoryViewModel.SubCategories, selectedCategoryIDs);
            }
        }
    }
}