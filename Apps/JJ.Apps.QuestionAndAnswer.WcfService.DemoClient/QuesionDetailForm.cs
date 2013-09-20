using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JJ.Apps.QuestionAndAnswer.WcfService.DemoClient.QuestionService;

namespace JJ.Apps.QuestionAndAnswer.WcfService.DemoClient
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
            NextQuestion();
        }

        public void NextQuestion()
        {
            _viewModel = _controller.NextQuestion();
            ApplyViewModel();
        }

        public void ShowAnswer()
        {
            _viewModel = _controller.ShowAnswer(_viewModel);
            ApplyViewModel();
        }

        private void SetTexts()
        {
            Text = Titles.Question + " [Service Client]";
            labelAnswerTitle.Text = Labels.Answer;
            buttonNextQuestion.Text = Titles.NextQuestion;
            buttonShowAnswer.Text = Titles.ShowAnswer;
        }

        private void ApplyViewModel()
        {
            if (_viewModel.NotFound)
            {
                labelQuestion.Text = "";
                labelAnswerText.Text = "";
                textBoxUserAnswer.Text = "";
                labelAnswerText.Visible = false;
                MessageBox.Show(Messages.QuestionNotFound);
                return;
            }

            labelQuestion.Text = _viewModel.Question;
            labelAnswerText.Text = _viewModel.Answer;
            textBoxUserAnswer.Text = _viewModel.UserAnswer;
            labelAnswerText.Visible = _viewModel.AnswerIsVisible;
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
    }
}
