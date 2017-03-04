﻿using System;
using System.Windows.Forms;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Framework.Presentation.Resources;
using JJ.Business.Synthesizer.Resources;
using JJ.Presentation.Synthesizer.WinForms.EventArg;
using JJ.Framework.Presentation.WinForms.Extensions;
using JJ.Presentation.Synthesizer.WinForms.UserControls.Bases;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    internal partial class DocumentDetailsUserControl : UserControlBase
    {
        public event EventHandler SaveRequested;
        public event EventHandler<EventArgs<int>> DeleteRequested;
        public event EventHandler CloseRequested;

        public DocumentDetailsUserControl()
        {
            InitializeComponent();

            SetTitles();

            this.AutomaticallyAssignTabIndexes();
        }

        // Gui

        public new DocumentDetailsViewModel ViewModel
        {
            get { return (DocumentDetailsViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        private void SetTitles()
        {
            titleBarUserControl.Text = PropertyDisplayNames.Document;
            labelIDTitle.Text = CommonTitlesFormatter.ID;
            labelName.Text = CommonTitlesFormatter.Name;
            buttonSave.Text = CommonTitlesFormatter.Save;
            buttonDelete.Text = CommonTitlesFormatter.Delete;
        }

        protected override void ApplyViewModelToControls()
        {
            if (ViewModel == null) return;

            labelIDValue.Text = ViewModel.Document.ID.ToString();
            textBoxName.Text = ViewModel.Document.Name;

            labelIDTitle.Visible = ViewModel.IDVisible;
            labelIDValue.Visible = ViewModel.IDVisible;

            buttonDelete.Visible = ViewModel.CanDelete;
        }

        private void ApplyControlsToViewModel()
        {
            if (ViewModel == null) return;

            ViewModel.Document.Name = textBoxName.Text;
        }

        // Actions

        private void Save()
        {
            ApplyControlsToViewModel();
            SaveRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Close()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Delete()
        {
            if (ViewModel == null) return;

            var e = new EventArgs<int>(ViewModel.Document.ID);
            DeleteRequested?.Invoke(this, e);
        }

        // Events

        private void titleBarUserControl_CloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void DocumentDetailsUserControl_VisibleChanged(object sender, EventArgs e)
        {
            // ReSharper disable once InvertIf
            if (Visible)
            {
                textBoxName.Focus();
                textBoxName.Select(0, 0);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // ReSharper disable once InvertIf
            if (keyData == Keys.Enter)
            {
                buttonSave.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
