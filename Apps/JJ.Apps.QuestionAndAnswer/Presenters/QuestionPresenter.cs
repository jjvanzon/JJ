using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Persistence;
using JJ.Models.QuestionAndAnswer;
using JJ.Models.QuestionAndAnswer.Persistence;
using JJ.Business.QuestionAndAnswer;
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.ViewModels.Helpers;

namespace JJ.Apps.QuestionAndAnswer.Presenters
{
    public class QuestionPresenter : IDisposable
    {
        private TextualQuestion _model;
        private IContext _context;
        private bool _contextIsOwned;

        public QuestionDetailViewModel ShowQuestion()
        {
            return _model.ToViewModel();
        }

        public QuestionDetailViewModel ShowAnswer(QuestionDetailViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            QuestionDetailViewModel viewModel2 = _model.ToViewModel();
            viewModel2.UserAnswer = viewModel.UserAnswer;
            viewModel2.AnswerIsVisible = true;
            return viewModel2;
        }

        public QuestionDetailViewModel ShowAnswer()
        {
            QuestionDetailViewModel viewModel = _model.ToViewModel();
            viewModel.AnswerIsVisible = true;
            return viewModel;
        }

        // Constructors

        public QuestionPresenter(int? id = null)
        {
            InitializeModel(null, null, null, id);
        }

        public QuestionPresenter(IContext context, int? id = null)
        {
            if (context == null) throw new ArgumentNullException("context");
            InitializeModel(context, null, null, id);
        }

        public QuestionPresenter(ITextualQuestionRepository repository, int? id = null)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            InitializeModel(null, repository, null, id);
        }

        public QuestionPresenter(TextualQuestion textualQuestion)
        {
            if (textualQuestion == null) throw new ArgumentNullException("textualQuestion");
            InitializeModel(null, null, textualQuestion, null);
        }

        private void InitializeModel(IContext context, ITextualQuestionRepository repository, TextualQuestion model, int? id)
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

            if (model == null)
            {
                if (id.HasValue)
                {
                    model = repository.Get(id.Value);
                }
                else
                {
                    model = repository.GetRandomTextualQuestion();
                }
            }

            if (model == null)
            {
                throw new Exception("model cannot be null.");
            }

            _model = model;
            _context = context;
            _contextIsOwned = contextIsOwned;
        }

        public void Dispose()
        {
            if (_contextIsOwned && _context != null)
            {
                _context.Dispose();
            }
        }
    }
}
