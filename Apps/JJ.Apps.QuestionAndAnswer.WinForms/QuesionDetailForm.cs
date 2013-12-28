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
using JJ.Apps.QuestionAndAnswer.ViewModels;
using JJ.Apps.QuestionAndAnswer.Presenters;
using JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces;

namespace JJ.Apps.QuestionAndAnswer.WinForms
{
    public partial class QuesionDetailForm : Form
    {
        private readonly QuestionPresenter _presenter;

        private QuestionDetailViewModel _viewModel;

        public QuesionDetailForm()
        {
            InitializeComponent();

            _presenter = CreatePresenter();

            SetTexts();
            NextQuestion();
        }

        private void NextQuestion()
        {
            _viewModel = _presenter.ShowQuestion();
            ApplyViewModel();
        }

        private void ShowAnswer()
        {
            _viewModel = _presenter.ShowAnswer(_viewModel, null);
            ApplyViewModel();
        }

        private void HideAnswer()
        {
            _viewModel = _presenter.HideAnswer(_viewModel, null);
            ApplyViewModel();
        }

        private void SetTexts()
        {
            Text = Titles.Question;
            labelAnswerTitle.Text = Labels.Answer;
            buttonNextQuestion.Text = Titles.NextQuestion;
            buttonShowAnswer.Text = Titles.ShowAnswer;
            buttonHideAnswer.Text = Titles.HideAnswer;
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
                MessageBox.Show(Messages.NoQuestionsFound);
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

        private QuestionPresenter CreatePresenter()
        {
            IContext context = ContextHelper.CreateContextFromConfiguration();
            ICategoryRepository categoryRepository = RepositoryFactory.CreateCategoryRepository(context);
            IQuestionRepository questionRepository = RepositoryFactory.CreateQuestionRepository(context);
            IQuestionFlagRepository questionFlagRepository = RepositoryFactory.CreateQuestionFlagRepository(context);
            IFlagStatusRepository flagStatusRepository = RepositoryFactory.CreateFlagStatusRepository(context);
            IUserRepository userRepository = RepositoryFactory.CreateUserRepository(context);
            return new QuestionPresenter(questionRepository, categoryRepository, questionFlagRepository, flagStatusRepository, userRepository);
        }
    }
}
