using System;
using System.Windows.Forms;
using JJ.Framework.Data;
using JJ.Presentation.QuestionAndAnswer.Resources;
using JJ.Presentation.QuestionAndAnswer.ViewModels;
using JJ.Presentation.QuestionAndAnswer.Presenters;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Presentation;
using JJ.Business.QuestionAndAnswer.Resources;

namespace JJ.Presentation.QuestionAndAnswer.WinForms
{
	internal partial class RandomQuestionForm : Form
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
			object viewModel = _presenter.ShowAnswer(_viewModel);
			ApplyViewModel(viewModel);
		}

		private void HideAnswer()
		{
			object viewModel = _presenter.HideAnswer(_viewModel);
			ApplyViewModel(viewModel);
		}

		private void SetTitlesAndLabels()
		{
			Text = PropertyDisplayNames.Question;
			labelAnswerTitle.Text = PropertyDisplayNames.Answer;
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
			labelQuestion.Text = null;
			labelAnswerText.Text = null;
			textBoxUserAnswer.Text = null;
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
				PersistenceHelper.CreateRepository<IUserRepository>(context),
				authenticatedUserName: null);
		}
	}
}
