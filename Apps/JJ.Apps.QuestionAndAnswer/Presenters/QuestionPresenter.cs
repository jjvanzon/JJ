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

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionPresenter : IDisposable
    {
        private IContext _context;
        private bool _contextIsOwned;
        private IQuestionRepository _repository;

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

        public QuestionPresenter(IQuestionRepository repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");

            Initialize(null, repository);
        }

        private void Initialize(IContext context, IQuestionRepository repository)
        {
            bool contextIsOwned = false;

            if (context == null)
            {
                context = ContextHelper.CreateContext();
                contextIsOwned = true;
            }

            if (repository == null)
            {
                repository = new QuestionRepository(context, context.Location);
            }

            _context = context;
            _contextIsOwned = contextIsOwned;
            _repository = repository;
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
            Question model = _repository.TryGetRandomQuestion();

            return Question(model);
        }

        public QuestionDetailViewModel ShowQuestion(int id)
        {
            Question model = _repository.TryGet(id);

            return Question(model);
        }

        private QuestionDetailViewModel Question(Question model)
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

            Question model = _repository.TryGet(viewModel.ID);
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

            // This version will not show question information if you do not provide it in the viewModel, but is faster and less code.
            //viewModel.AnswerIsVisible = false;
            //return viewModel;

            Question model = _repository.TryGet(viewModel.ID);
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
