using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JJ.Presentation.QuestionAndAnswer.AppService.DemoClient.RandomQuestionService;

namespace JJ.Presentation.QuestionAndAnswer.AppService.DemoClient
{
    internal partial class RandomQuesionForm : Form
    {
        private QuestionController _controller;
        private RandomQuestionViewModel _viewModel;

        public RandomQuesionForm()
        {
            InitializeComponent();

            _controller = new QuestionController();

            SaveTexts();
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

        private void SaveTexts()
        {
            Text = ResourceHelper.PropertyDisplayNames.Question + " [Service Client]";
            labelAnswerTitle.Text = ResourceHelper.PropertyDisplayNames.Answer;
            buttonNextQuestion.Text = ResourceHelper.Titles.NextQuestion;
            buttonShowAnswer.Text = ResourceHelper.Titles.ShowAnswer;
            buttonHideAnswer.Text = ResourceHelper.Titles.HideAnswer;
        }

        private void ApplyViewModel()
        {
            // TODO: Polymorphic web method results should return either QuestionNotFoundviewModel or RandomQuestionViewModel.
            bool isNotFound = String.IsNullOrEmpty(_viewModel.Question.Text);
            if (isNotFound)
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

            buttonShowAnswer.Enabled = true;
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
