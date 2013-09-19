using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Helpers;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionPresenter : IDisposable
    {
        private IContext _context;
        private bool _contextIsOwned;
        private ITextualQuestionRepository _repository;
        private TextualQuestion _model;

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

        public QuestionPresenter(ITextualQuestionRepository repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");

            Initialize(null, repository);
        }

        private void Initialize(IContext context, ITextualQuestionRepository repository)
        {
            bool contextIsOwned = false;

            if (context == null)
            {
                context = PersistenceHelper.CreateContext();
                contextIsOwned = true;
            }

            if (repository == null)
            {
                repository = new TextualQuestionRepository(context, context.Location);
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
            _model = _repository.GetRandomTextualQuestion();

            return _model.ToViewModel();
        }

        public QuestionDetailViewModel ShowQuestion(int id)
        {
            _model = _repository.Get(id);

            return _model.ToViewModel();
        }

        public QuestionDetailViewModel ShowAnswer(QuestionDetailViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            // This version will not show question information if you do not provide it in the viewModel.
            //viewModel.AnswerIsVisible = true;
            //return viewModel;

            // This version will show question information even if you do not provide it in the viewModel.
            _model = _repository.Get(viewModel.ID);
            QuestionDetailViewModel viewModel2 = _model.ToViewModel();
            viewModel2.UserAnswer = viewModel.UserAnswer;
            viewModel2.AnswerIsVisible = true;
            return viewModel2;
        }
    }
}
