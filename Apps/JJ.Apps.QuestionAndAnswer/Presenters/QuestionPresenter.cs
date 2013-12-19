using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Helpers;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;
using JJ.Business.QuestionAndAnswer;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    /// <summary>
    /// A presenter has action methods that communicate by means of view models and ID's.
    /// A presenter can retrieve data from a context or repositories, but it will never communicate the entities themselves to the outside.
    /// </summary>
    public class QuestionPresenter : IDisposable
    {
        private IContext _context;
        private bool _contextIsOwned;
        private IQuestionRepository _questionRepository;
        private ICategoryRepository _categoryRepository;
        private CategoryManager _categoryManager;

        // Constructors

        #region Constructors

        public QuestionPresenter()
        {
            Initialize(null, null, null);
        }

        public QuestionPresenter(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            Initialize(context, null, null);
        }

        public QuestionPresenter(IQuestionRepository questionRepository, ICategoryRepository categoryRepository)
        {
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");

            Initialize(null, questionRepository, categoryRepository);
        }

        private void Initialize(IContext context, IQuestionRepository questionRepository, ICategoryRepository categoryRepository)
        {
            bool contextIsOwned = false;

            if (context == null)
            {
                context = ContextHelper.CreateContextFromConfiguration();
                contextIsOwned = true;
            }

            if (questionRepository == null)
            {
                questionRepository = new QuestionRepository(context, context.Location);
            }

            if (categoryRepository == null)
            {
                categoryRepository = new CategoryRepository(context);
            }

            _context = context;
            _contextIsOwned = contextIsOwned;
            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
            _categoryManager = new CategoryManager(_categoryRepository);
        }

        public void Dispose()
        {
            if (_contextIsOwned && _context != null)
            {
                _context.Dispose();
            }
        }

        #endregion

        // Actions

        public QuestionDetailViewModel ShowQuestion(params int[] categoryIDs)
        {
            categoryIDs = categoryIDs ?? new int[] { };

            // Get Categories
            List<Category> selectedCategoryBranches = GetCategories(categoryIDs);
            IEnumerable<Category> selectedCategoryNodes = _categoryManager.SelectNodesRecursive(selectedCategoryBranches);

            // Get Random Question
            Question question;
            if (selectedCategoryNodes.Count() == 0)
            {
                question = _questionRepository.TryGetRandomQuestion();
            }
            else
            {
                QuestionSelector selector = new QuestionSelector(_questionRepository, selectedCategoryNodes.ToArray());
                question = selector.TryGetRandomQuestion();
            }

            // Not Found
            if (question == null)
            {
                return new QuestionDetailViewModel { NotFound = true };
            }

            // Create ViewModel
            QuestionDetailViewModel viewModel = question.ToDetailViewModel();
            viewModel.AnswerIsVisible = false;
            viewModel.Question.Answer = null;
            viewModel.Question.Links.Clear(); // Links reveal answer.
            viewModel.SelectedCategories = selectedCategoryBranches.Select(x => x.ToViewModel()).ToList();
            return viewModel;
        }

        private List<Category> GetCategories(int[] ids)
        {
            var list = new List<Category>();

            foreach (int id in ids)
            {
                Category category = _categoryRepository.Get(id);
                list.Add(category);
            }

            return list;
        }

        public QuestionDetailViewModel ShowAnswer(QuestionDetailViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }
            if (viewModel.Question == null)
            {
                throw new ArgumentNullException("viewModel.Question");
            }

            Question model = _questionRepository.TryGet(viewModel.Question.ID);
            if (model == null)
            {
                return NotFound(viewModel.Question.ID);
            }

            QuestionDetailViewModel viewModel2 = model.ToDetailViewModel();
            viewModel2.UserAnswer = viewModel.UserAnswer;
            viewModel2.AnswerIsVisible = true;

            if (viewModel.SelectedCategories != null)
            {
                viewModel2.SelectedCategories = viewModel.SelectedCategories;
            }

            return viewModel2;
        }

        public QuestionDetailViewModel HideAnswer(QuestionDetailViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }
            if (viewModel.Question == null)
            {
                throw new ArgumentNullException("viewModel.Question");
            }

            Question model = _questionRepository.TryGet(viewModel.Question.ID);
            if (model == null)
            {
                return NotFound(viewModel.Question.ID);
            }

            QuestionDetailViewModel viewModel2 = model.ToDetailViewModel();
            viewModel2.UserAnswer = viewModel.UserAnswer;
            viewModel2.AnswerIsVisible = false;

            if (viewModel.SelectedCategories != null)
            {
                viewModel2.SelectedCategories = viewModel.SelectedCategories;
            }

            viewModel2.Question.Links.Clear(); // Links reveal answer.
            return viewModel2;
        }

        // Reusable Methods

        private QuestionDetailViewModel NotFound(int id)
        {
            var viewModel = new QuestionDetailViewModel();
            viewModel.Question = new QuestionViewModel();
            viewModel.Question.ID = id;
            viewModel.NotFound = true;
            return viewModel;
        }
    }
}
