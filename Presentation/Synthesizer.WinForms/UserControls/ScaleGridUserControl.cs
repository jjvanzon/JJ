﻿using System;
using System.Linq;
using System.Windows.Forms;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.WinForms.EventArg;
using JJ.Business.Synthesizer.Resources;
using JJ.Presentation.Synthesizer.WinForms.UserControls.Bases;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    internal partial class ScaleGridUserControl : UserControlBase
    {
        private const string ID_COLUMN_NAME = "IDColumn";

        public event EventHandler<EventArgs<int>> CreateRequested;
        public event EventHandler<EventArgs<int>> DeleteRequested;
        public event EventHandler CloseRequested;
        public event EventHandler<EventArgs<int>> ShowDetailsRequested;

        public ScaleGridUserControl()
        {
            InitializeComponent();
            SetTitles();
        }

        // Gui

        private void SetTitles()
        {
            titleBarUserControl.Text = ResourceFormatter.Scales;
        }

        // Binding

        public new ScaleGridViewModel ViewModel
        {
            get { return (ScaleGridViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void ApplyViewModelToControls()
        {
            if (ViewModel == null)
            {
                return;
            }

            specializedDataGridView.DataSource = ViewModel.Dictionary.Values.ToArray();
        }

        // Actions

        private void Create()
        {
            var e = new EventArgs<int>(ViewModel.DocumentID);
            CreateRequested?.Invoke(this, e);
        }

        private void Delete()
        {
            int? id = TryGetSelectedID();
            if (!id.HasValue)
            {
                return;
            }

            var e = new EventArgs<int>(id.Value);
            DeleteRequested?.Invoke(this, e);
        }

        private void Close()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ShowDetails()
        {
            int? id = TryGetSelectedID();
            // ReSharper disable once InvertIf
            if (id.HasValue)
            {
                var e = new EventArgs<int>(id.Value);
                ShowDetailsRequested?.Invoke(this, e);
            }
        }

        // Events

        private void titleBarUserControl_AddClicked(object sender, EventArgs e)
        {
            Create();
        }

        private void titleBarUserControl_RemoveClicked(object sender, EventArgs e)
        {
            Delete();
        }

        private void titleBarUserControl_CloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void specializedDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    Delete();
                    break;

                case Keys.Enter:
                    ShowDetails();
                    break;
            }
        }

        private void specializedDataGridView_DoubleClick(object sender, EventArgs e)
        {
            ShowDetails();
        }

        // Helpers

        private int? TryGetSelectedID()
        {
            if (specializedDataGridView.CurrentRow == null)
            {
                return null;
            }

            DataGridViewCell cell = specializedDataGridView.CurrentRow.Cells[ID_COLUMN_NAME];
            int id = (int)cell.Value;
            return id;
        }
    }
}
