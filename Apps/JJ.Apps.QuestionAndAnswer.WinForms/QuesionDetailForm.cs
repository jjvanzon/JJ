﻿using System;
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
using JJ.Apps.QuestionAndAnswer.Helpers;
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;

namespace JJ.Apps.QuestionAndAnswer.WinForms
{
    public partial class QuesionDetailForm : Form
    {
        private readonly QuestionPresenter _presenter;

        private QuestionDetailViewModel _viewModel;

        public QuesionDetailForm()
        {
            InitializeComponent();

            _presenter = new QuestionPresenter();

            SetTexts();
            NextQuestion();
        }

        public void NextQuestion()
        {
            _viewModel = _presenter.NextQuestion();
            ApplyViewModel();
        }

        public void ShowAnswer()
        {
            _viewModel = _presenter.ShowAnswer(_viewModel);
            ApplyViewModel();
        }

        private void SetTexts()
        {
            Text = Titles.Question;
            labelAnswerTitle.Text = Labels.Answer;
            buttonNextQuestion.Text = Titles.NextQuestion;
            buttonShowAnswer.Text = Titles.ShowAnswer;
        }

        private void ApplyViewModel()
        {
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
