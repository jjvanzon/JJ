using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JJ.Framework.Persistence;
using JJ.Apps.QuestionAndAnswer.Resources;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;
using JJ.Framework.Presentation;
using JJ.Models.QuestionAndAnswer.Persistence.Repositories;

namespace JJ.Apps.QuestionAndAnswer.WinForms
{
    public partial class RandomQuestionForm : Form
    {
        private IContext _context;
        private RandomQuestionPresenter _presenter;
        private RandomQuestionViewModel _viewModel;

        public RandomQuestionForm()
        {
            InitializeComponent();
        }

        private void RandomQuestionForm_Load(object sender, EventArgs e)
        {
            _context = PersistenceHelper.CreateContext();
            _presenter = CreatePresenter(_context);

            SetTitlesAndLabels();
            NextQuestion();
        }

        private void RandomQuestionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        private void NextQuestion()
        {
            object viewModel = _presenter.Show();
            ApplyViewModel(viewModel);
        }

        private void ShowAnswer()
        {
            object viewModel = _presenter.ShowAnswer(_viewModel, null);
            ApplyViewModel(viewModel);
        }

        private void HideAnswer()
        {
            object viewModel = _presenter.HideAnswer(_viewModel, null);
            ApplyViewModel(viewModel);
        }

        private void SetTitlesAndLabels()
        {
            Text = Titles.Question;
            labelAnswerTitle.Text = Labels.Answer;
            buttonNextQuestion.Text = Titles.NextQuestion;
            buttonShowAnswer.Text = Titles.ShowAnswer;
            buttonHideAnswer.Text = Titles.HideAnswer;
        }

        private void ApplyViewModel(object viewModel)
        {
            var randomQuestionViewModel = viewModel as RandomQuestionViewModel;
            if (randomQuestionViewModel != null)
            {
                _viewModel = randomQuestionViewModel;
                ApplyRandomQuestionViewModel(randomQuestionViewModel);
                return;
            }

            var notFoundViewModel = viewModel as QuestionNotFoundViewModel;
            if (notFoundViewModel != null)
            {
                ApplyNotFoundViewModel(notFoundViewModel);
                return;
            }

            throw new UnexpectedViewModelTypeException(viewModel);
        }

        private void ApplyRandomQuestionViewModel(RandomQuestionViewModel viewModel)
        {
            labelQuestion.Text = viewModel.Question.Text;
            labelAnswerText.Text = viewModel.Question.Answer;
            textBoxUserAnswer.Text = viewModel.UserAnswer;
            labelAnswerText.Visible = viewModel.AnswerIsVisible;

            buttonShowAnswer.Visible = !viewModel.AnswerIsVisible;
            buttonHideAnswer.Visible = viewModel.AnswerIsVisible;

            buttonShowAnswer.Enabled = true;
        }

        private void ApplyNotFoundViewModel(QuestionNotFoundViewModel viewModel)
        {
            labelQuestion.Text = "";
            labelAnswerText.Text = "";
            textBoxUserAnswer.Text = "";
            labelAnswerText.Visible = false;
            buttonShowAnswer.Visible = true;
            buttonShowAnswer.Enabled = false;
            buttonHideAnswer.Visible = false;
            MessageBox.Show(Messages.NoQuestionsFound);
            return;
        }

        private void buttonShowAnswer_Click(object sender, EventArgs e)
        {
            ShowAnswer();
        }

        private void buttonNextQuestion_Click(object sender, EventArgs e)
        {
            NextQuestion();
        }

        private void textBoxUserAnswer_TextChanged(object sender, EventArgs e)
        {
            _viewModel.UserAnswer = textBoxUserAnswer.Text;
        }

        private void buttonHideAnswer_Click(object sender, EventArgs e)
        {
            HideAnswer();
        }

        private RandomQuestionPresenter CreatePresenter(IContext context)
        {
            return new RandomQuestionPresenter(
                PersistenceHelper.CreateRepository<IQuestionRepository>(context),
                PersistenceHelper.CreateRepository<ICategoryRepository>(context),
                PersistenceHelper.CreateRepository<IQuestionFlagRepository>(context),
                PersistenceHelper.CreateRepository<IFlagStatusRepository>(context),
                PersistenceHelper.CreateRepository<IUserRepository>(context));
        }
    }
}
