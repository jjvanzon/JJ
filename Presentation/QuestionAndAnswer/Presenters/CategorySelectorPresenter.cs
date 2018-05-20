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

namespace JJ.Presentation.QuestionAndAnswer.Presenters
{
	public class CategorySelectorPresenter
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IQuestionRepository _questionRepository;
		private readonly IQuestionFlagRepository _questionFlagRepository;
		private readonly IFlagStatusRepository _flagStatusRepository;
		private readonly IUserRepository _userRepository;

		private readonly CategoryManager _categoryManager;
		private readonly string _authenticatedUserName;

		/// <param name="authenticatedUserName">nullable</param>
		public CategorySelectorPresenter(
			ICategoryRepository categoryRepository, 
			IQuestionRepository questionRepository,
			IQuestionFlagRepository questionFlagRepository,
			IFlagStatusRepository flagStatusRepository,
			IUserRepository userRepository,
			string authenticatedUserName)
		{
			_categoryRepository = categoryRepository ?? throw new NullException(() => categoryRepository);
			_questionRepository = questionRepository ?? throw new NullException(() => questionRepository);
			_questionFlagRepository = questionFlagRepository ?? throw new NullException(() => questionFlagRepository);
			_flagStatusRepository = flagStatusRepository ?? throw new NullException(() => flagStatusRepository);
			_userRepository = userRepository ?? throw new NullException(() => userRepository);

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
