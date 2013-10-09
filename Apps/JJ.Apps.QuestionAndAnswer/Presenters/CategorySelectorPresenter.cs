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

        public CategorySelectorViewModel AddCategory(CategorySelectorViewModel viewModel, CategoryViewModel category)
        {
            // TODO: Remove from AvailableCategories.
            viewModel.SelectedCategories.Add(category);
            return viewModel;
        }

        public CategorySelectorViewModel RemoveCategory(CategorySelectorViewModel viewModel, CategoryViewModel category)
        {
            // TODO: There is no identity integrity here.
            // TODO: Add to AvailableCategories.
            viewModel.SelectedCategories.Remove(category);
            return viewModel;
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
    }
}
