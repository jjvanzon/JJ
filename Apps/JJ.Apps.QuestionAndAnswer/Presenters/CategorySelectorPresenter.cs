using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Business.QuestionAndAnswer;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Helpers;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class CategorySelectorPresenter : IDisposable
    {
        private IContext _context;
        private bool _contextIsOwned;
        private ICategoryRepository _categoryRepository;
        private IQuestionRepository _questionRepository;
        private CategoryManager _categoryManager;
        //private CategorySelectorViewModel _viewModel;

        // Constructors

        public CategorySelectorPresenter()
        {
            Initialize(null, null, null);
        }

        public CategorySelectorPresenter(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            Initialize(context, null, null);
        }

        public CategorySelectorPresenter(ICategoryRepository categoryRepository, IQuestionRepository questionRepository)
        {
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");

            Initialize(null, categoryRepository, questionRepository);
        }

        private void Initialize(IContext context, ICategoryRepository categoryRepository, IQuestionRepository questionRepository)
        {
            bool contextIsOwned = false;

            if (context == null)
            {
                context = ContextHelper.CreateContext();
                contextIsOwned = true;
            }

            if (categoryRepository == null)
            {
                categoryRepository = new CategoryRepository(context);
            }

            if (questionRepository == null)
            {
                questionRepository = new QuestionRepository(context, context.Location);
            }

            _context = context;
            _contextIsOwned = contextIsOwned;
            _categoryRepository = categoryRepository;
            _categoryManager = new CategoryManager(_categoryRepository);
            _questionRepository = questionRepository;
        }

        public void Dispose()
        {
            if (_contextIsOwned && _context != null)
            {
                _context.Dispose();
            }
        }

        // Actions

        public CategorySelectorViewModel Show()
        {
            var viewModel = new CategorySelectorViewModel
            {
                AvailableCategories = GetAvailableCategories(),
                SelectedCategories = new List<CategoryViewModel>()
            };

            viewModel.NoCategoriesAvailable = viewModel.AvailableCategories.Count == 0;

            return viewModel;
        }

        /*public void Add(int categoryID)
        {
            if (_viewModel == null)
            {
                throw new Exception("Call Show before calling other methods.");
            }
            
            bool alreadyPresent = _viewModel.SelectedCategories.Any(x => x.ID == categoryID);
            if (!alreadyPresent)
            {
                Category category = _categoryRepository.Get(categoryID);
                CategoryViewModel categoryModel = category.ToViewModel();
                _viewModel.SelectedCategories.Add(categoryModel);
            }
        }*/

        public CategorySelectorViewModel Add(CategorySelectorViewModel viewModel, int categoryID)
        {
            // TODO: I need to rebuild the complete viewmodel here, because the passed view model is not in tact.
            // But this does not comform to the stateful / stateless nature of the presenters.

            CategorySelectorViewModel viewModel2 = GetCompleteViewModel(viewModel);

            bool alreadyPresent = viewModel2.SelectedCategories.Any(x => x.ID == categoryID);
            if (!alreadyPresent)
            {
                Category category = _categoryRepository.Get(categoryID);
                CategoryViewModel categoryModel = category.ToViewModel();
                viewModel2.SelectedCategories.Add(categoryModel);
            }

            RemoveSelectedCategoriesRecursive(viewModel2.AvailableCategories, viewModel2.SelectedCategories.Select(x => x.ID).ToArray());

            return viewModel2;
        }

        public CategorySelectorViewModel Remove(CategorySelectorViewModel viewModel, int categoryID)
        {
            // TODO: I need to rebuild the complete viewmodel here, because the passed view model is not in tact.
            // But this does not comform to the stateful / stateless nature of the presenters.

            CategorySelectorViewModel viewModel2 = GetCompleteViewModel(viewModel);

            CategoryViewModel categoryModel = viewModel2.SelectedCategories.Where(x => x.ID == categoryID).SingleOrDefault();
            bool alreadyRemoved = categoryModel == null;
            if (!alreadyRemoved)
            {
                viewModel2.SelectedCategories.Remove(categoryModel);
            }

            RemoveSelectedCategoriesRecursive(viewModel2.AvailableCategories, viewModel2.SelectedCategories.Select(x => x.ID).ToArray());

            return viewModel2;
        }

        private CategorySelectorViewModel GetCompleteViewModel(CategorySelectorViewModel viewModel)
        {
            // Get entities.
            var selectedCategories = new List<Category>();
            if (viewModel.SelectedCategories != null)
            {
                foreach (CategoryViewModel selectedCategoryModel in viewModel.SelectedCategories)
                {
                    Category selectedCategory = _categoryRepository.Get(selectedCategoryModel.ID);
                    selectedCategories.Add(selectedCategory);
                }
            }

            // Create new view model.
            var viewModel2 = new CategorySelectorViewModel
            {
                AvailableCategories = GetAvailableCategories(),
                SelectedCategories = selectedCategories.Select(x => x.ToViewModel()).ToList()
            };

            return viewModel2;
        }

        public QuestionDetailViewModel ShowQuestions(CategorySelectorViewModel viewModel)
        {
            int[] categoryIDs = viewModel.SelectedCategories.Select(x => x.ID).ToArray();

            using (var questionPresenter = new QuestionPresenter(_questionRepository, _categoryRepository))
            {
                return questionPresenter.ShowQuestion(categoryIDs);
            }
        }

        // Helpers

        private List<CategoryNodeViewModel> GetAvailableCategories()
        {
            IEnumerable<Category> categories = _categoryManager.GetCategoryTree();

            var viewModels = new List<CategoryNodeViewModel>();

            foreach (Category category in categories)
            {
                CategoryNodeViewModel viewModel = category.ToNodeViewModelRecursive();
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        private void RemoveSelectedCategoriesRecursive(List<CategoryNodeViewModel> availableCategories, int[] selectedCategoryIDs)
        {
            foreach (CategoryNodeViewModel availableCategory in availableCategories.ToArray())
            {
                RemoveSelectedCategoriesRecursive(availableCategory.SubCategories, selectedCategoryIDs);

                bool isSelected = selectedCategoryIDs.Contains(availableCategory.Category.ID);
                bool isLeaf = availableCategory.SubCategories.Count == 0;
                bool mustRemove = isSelected && isLeaf;
                                  
                if (mustRemove) 
                {
                    availableCategories.Remove(availableCategory);
                }
            }
        }
    }
}
