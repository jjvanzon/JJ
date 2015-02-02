using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Common;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Repositories;
using JJ.Models.QuestionAndAnswer.Repositories.Interfaces;
using JJ.Business.QuestionAndAnswer;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Entities;
using JJ.Apps.QuestionAndAnswer.ToViewModel;
using JJ.Apps.QuestionAndAnswer.Extensions;
using JJ.Framework.Reflection;
using JJ.Apps.QuestionAndAnswer.Helpers;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class CategorySelectorPresenter
    {
        private ICategoryRepository _categoryRepository;
        private IQuestionRepository _questionRepository;
        private IQuestionFlagRepository _questionFlagRepository;
        private IFlagStatusRepository _flagStatusRepository;
        private IUserRepository _userRepository;

        private CategoryManager _categoryManager;
        private string _authenticatedUserName;

        /// <param name="authenticatedUserName">nullable</param>
        public CategorySelectorPresenter(
            ICategoryRepository categoryRepository, 
            IQuestionRepository questionRepository,
            IQuestionFlagRepository questionFlagRepository,
            IFlagStatusRepository flagStatusRepository,
            IUserRepository userRepository,
            string authenticatedUserName)
        {
            if (categoryRepository == null) throw new NullException(() => categoryRepository);
            if (questionRepository == null) throw new NullException(() => questionRepository);
            if (questionFlagRepository == null) throw new NullException(() => questionFlagRepository);
            if (flagStatusRepository == null) throw new NullException(() => flagStatusRepository);
            if (userRepository == null) throw new NullException(() => userRepository);

            _categoryRepository = categoryRepository;
            _questionRepository = questionRepository;
            _questionFlagRepository = questionFlagRepository;
            _flagStatusRepository = flagStatusRepository;
            _userRepository = userRepository;

            _authenticatedUserName = authenticatedUserName;

            _categoryManager = new CategoryManager(_categoryRepository);
        }

        public CategorySelectorViewModel Show()
        {
            return CreateViewModel();
        }

        /// <summary>
        /// Required to pass along with the view model:
        /// the ID's of the already selected categories and the categoryID of the category to add.
        /// </summary>
        public CategorySelectorViewModel Add(CategorySelectorViewModel viewModel, int categoryID)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            var selectedCategoryIDs = new List<int>();
            AddSelectedCategoryIDsRecursive(selectedCategoryIDs, viewModel.SelectedCategories);
            selectedCategoryIDs.Add(categoryID);

            return CreateViewModel(selectedCategoryIDs);
        }

        /// <summary>
        /// Required to pass along with the view model:
        /// the ID's of the already selected categories and the categoryID of the category to remove.
        /// </summary>
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

        public object StartTraining(CategorySelectorViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            viewModel.NullCoalesce();

            var categoryIDs = new List<int>();
            AddSelectedCategoryIDsRecursive(categoryIDs, viewModel.SelectedCategories);

            var randomQuestionPresenter = new RandomQuestionPresenter(_questionRepository, _categoryRepository, _questionFlagRepository, _flagStatusRepository, _userRepository, _authenticatedUserName);
            return randomQuestionPresenter.Show(categoryIDs.ToArray());
        }

        public CategorySelectorViewModel SetLanguage(CategorySelectorViewModel viewModel, string cultureName)
        {
            if (viewModel == null) throw new NullException(() => viewModel);
            viewModel.NullCoalesce();

            CultureHelper.SetCulture(cultureName);

            // Create view model
            IList<int> selectedCategoryIDs = viewModel.SelectedCategories.UnionRecursive(x => x.SubCategories).Select(x => x.ID).ToArray();
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
