﻿using JJ.Apps.SetText.PresenterInterfaces;
using JJ.Apps.SetText.Presenters;
using JJ.Apps.SetText.ViewModels;
using JJ.Framework.Persistence;
using JJ.Models.Canonical;
using JJ.Models.SetText.Persistence.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JJ.Apps.SetText.WinForms.Offline
{
    public partial class MainForm : Form
    {
        private IContext _context;
        private ISetTextPresenter _presenter;
        private SetTextViewModel _viewModel;

        public MainForm()
        {
            InitializeComponent();

            _context = CreateContext();
            _presenter = CreatePresenter(_context);
            
            Show();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void textBoxText_TextChanged(object sender, EventArgs e)
        {
            _viewModel.Text = textBoxText.Text;
        }

        private new void Show()
        {
            _viewModel = _presenter.Show();
            ApplyViewModel();
        }

        private void Save()
        {
            _viewModel = _presenter.Save(_viewModel);
            ApplyViewModel();
        }

        private void ApplyViewModel()
        {
            textBoxText.Text = _viewModel.Text;

            var sb = new StringBuilder();
            if (_viewModel.TextWasSavedMessageVisible)
            {
                sb.AppendLine("Saved!");
            }

            foreach (ValidationMessage validationMessage in _viewModel.ValidationMessages)
            {
                sb.AppendLine(validationMessage.Text);
            }

            labelValidationMessages.Text = sb.ToString();
        }

        private ISetTextPresenter CreatePresenter(IContext context)
        {
            IEntityRepository repository = RepositoryFactory.CreateRepositoryFromConfiguration<IEntityRepository>(context);
            return new SetTextPresenter(repository);
        }

        private IContext CreateContext()
        {
            IContext context = ContextFactory.CreateContextFromConfiguration();
            return context;
        }
    }
}
