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
    public class QuestionPresenter : IDisposable
    {
        private IContext _context;
        private bool _contextIsOwned;
        private IQuestionRepository _questionRepository;

        // Constructors

        public QuestionPresenter()
        {
            Initialize(null, null);
        }

        public QuestionPresenter(IContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            Initialize(context, null);
        }

        public QuestionPresenter(IQuestionRepository questionRepository)
        {
            if (questionRepository == null) throw new ArgumentNullException("questionRepository");

            Initialize(null, questionRepository);
        }

        private void Initialize(IContext context, IQuestionRepository questionRepository)
        {
            bool contextIsOwned = false;

            if (context == null)
            {
                context = ContextHelper.CreateContext();
                contextIsOwned = true;
            }

            if (questionRepository == null)
            {
                questionRepository = new QuestionRepository(context, context.Location);
            }

            _context = context;
            _contextIsOwned = contextIsOwned;
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

        public QuestionDetailViewModel NextQuestion()
        {
            Question model = _questionRepository.TryGetRandomQuestion();

            // Temporary
            /*ICategoryRepository categoryRepository = new CategoryRepository(_context);
            CategoryManager categoryManager = new CategoryManager(categoryRepository);
            Category category = categoryManager.TryGetCategory("Css3", "Selectors");
            if (category == null)
            {
                return Question(null);
            }
            QuestionSelector selector = new QuestionSelector(_questionRepository, category);
            Question model = selector.TryGetRandomQuestion();*/

            return PresentQuestion(model);
        }

        public QuestionDetailViewModel ShowQuestion(int id)
        {
            Question model = _questionRepository.TryGet(id);

            return PresentQuestion(model);
        }

        private QuestionDetailViewModel PresentQuestion(Question model)
        {
            if (model == null)
            {
                return new QuestionDetailViewModel { NotFound = true };
            }

            QuestionDetailViewModel viewModel = model.ToViewModel();
            viewModel.AnswerIsVisible = false;
            viewModel.Answer = null;
            viewModel.Links.Clear();

            return viewModel;
        }

        public QuestionDetailViewModel ShowAnswer(QuestionDetailViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            Question model = _questionRepository.TryGet(viewModel.ID);
            if (model == null)
            {
                return NotFound(viewModel.ID);
            }

            QuestionDetailViewModel viewModel2 = model.ToViewModel();
            viewModel2.UserAnswer = viewModel.UserAnswer;
            viewModel2.AnswerIsVisible = true;
            return viewModel2;
        }


        public QuestionDetailViewModel HideAnswer(QuestionDetailViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            Question model = _questionRepository.TryGet(viewModel.ID);
            if (model == null)
            {
                return NotFound(viewModel.ID);
            }

            QuestionDetailViewModel viewModel2 = model.ToViewModel();
            viewModel2.UserAnswer = viewModel.UserAnswer;
            viewModel2.AnswerIsVisible = false;
            viewModel2.Links.Clear();
            return viewModel2;
        }

        private QuestionDetailViewModel NotFound(int id)
        {
            return new QuestionDetailViewModel
            {
                ID = id,
                NotFound = true
            };
        }
    }
}
