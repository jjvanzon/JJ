using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JJ.Apps.QuestionAndAnswer.AppService.DemoClient.QuestionService;

namespace JJ.Apps.QuestionAndAnswer.AppService.DemoClient
{
    public partial class QuesionDetailForm : Form
    {
        private QuestionController _controller;
        private QuestionDetailViewModel _viewModel;

        public QuesionDetailForm()
        {
            InitializeComponent();

            _controller = new QuestionController();

            SetTexts();
            ShowQuestion();
        }

        public void ShowQuestion()
        {
            _viewModel = _controller.ShowQuestion();
            ApplyViewModel();
        }

        public void ShowAnswer()
        {
            _viewModel = _controller.ShowAnswer(_viewModel);
            ApplyViewModel();
        }

        private void HideAnswer()
        {
            _viewModel = _controller.HideAnswer(_viewModel);
            ApplyViewModel();
        }

        private void SetTexts()
        {
            Text = ResourceHelper.Titles.Question + " [Service Client]";
            labelAnswerTitle.Text = ResourceHelper.Labels.Answer;
            buttonNextQuestion.Text = ResourceHelper.Titles.NextQuestion;
            buttonShowAnswer.Text = ResourceHelper.Titles.ShowAnswer;
            buttonHideAnswer.Text = ResourceHelper.Titles.HideAnswer;
        }

        private void ApplyViewModel()
        {
            if (_viewModel.NotFound)
            {
                labelQuestion.Text = "";
                labelAnswerText.Text = "";
                textBoxUserAnswer.Text = "";
                labelAnswerText.Visible = false;
                buttonShowAnswer.Visible = true;
                buttonShowAnswer.Enabled = false;
                buttonHideAnswer.Visible = false;
                MessageBox.Show(ResourceHelper.Messages.NoQuestionsFound);
                return;
            }

            labelQuestion.Text = _viewModel.Question.Text;
            labelAnswerText.Text = _viewModel.Question.Answer;
            textBoxUserAnswer.Text = _viewModel.UserAnswer;
            labelAnswerText.Visible = _viewModel.AnswerIsVisible;

            buttonShowAnswer.Visible = !_viewModel.AnswerIsVisible;
            buttonHideAnswer.Visible = _viewModel.AnswerIsVisible;
        }

        private void buttonShowAnswer_Click(object sender, EventArgs e)
        {
            ShowAnswer();
        }

        private void buttonHideAnswer_Click(object sender, EventArgs e)
        {
            HideAnswer();
        }

        private void buttonNextQuestion_Click(object sender, EventArgs e)
        {
            ShowQuestion();
        }

        private void textBoxUserAnswer_TextChanged(object sender, EventArgs e)
        {
            _viewModel.UserAnswer = textBoxUserAnswer.Text;
        }
    }
}
