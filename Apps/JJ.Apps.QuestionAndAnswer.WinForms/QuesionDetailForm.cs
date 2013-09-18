using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Apps.QuestionAndAnswer.Resources;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JJ.Apps.QuestionAndAnswer.WinForms
{
    public partial class QuesionDetailForm : Form
    {
        private IContext _context;
        private QuestionPresenter _presenter;
        private QuestionDetailViewModel _viewModel;

        public QuesionDetailForm()
        {
            InitializeComponent();

            ApplyResourceTexts();

            _context = PersistenceHelper.CreateContext();
            _presenter = new QuestionPresenter(_context);

            ShowQuestion();
        }

        private void QuesionDetailForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        private void ApplyResourceTexts()
        {
            Text = Titles.Question;
            labelAnswerTitle.Text = Labels.Answer;
            buttonNextQuestion.Text = Titles.NextQuestion;
            buttonShowAnswer.Text = Titles.ShowAnswer;
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

        private void ShowQuestion()
        {
            _viewModel = _presenter.ShowQuestion();
            ApplyViewModel();
        }

        private void ShowAnswer()
        {
            _viewModel = _presenter.ShowAnswer(_viewModel);
            ApplyViewModel();
        }

        private void NextQuestion()
        {
            // TODO: Design presenter so it could be both stateless and stateful. Keep in mind that the present is supposed to be a navigation model.
            _presenter = new QuestionPresenter(_context);
            _viewModel = _presenter.ShowQuestion();
            ApplyViewModel();
        }

        private void ApplyViewModel()
        {
            labelQuestion.Text = _viewModel.Question;
            labelAnswerText.Text = _viewModel.Answer;
            textBoxUserAnswer.Text = _viewModel.UserAnswer;
            labelAnswerText.Visible = _viewModel.AnswerIsVisible;
        }
    }
}
