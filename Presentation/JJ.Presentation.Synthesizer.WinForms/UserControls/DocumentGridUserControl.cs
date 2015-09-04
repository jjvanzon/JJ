﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Business.Synthesizer.Resources;
using JJ.Presentation.Synthesizer.WinForms.EventArg;
using JJ.Framework.Presentation.WinForms.EventArg;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    internal partial class DocumentGridUserControl : UserControl
    {
        private const string ID_COLUMN_NAME = "IDColumn";

        public event EventHandler<Int32EventArgs> ShowRequested;
        public event EventHandler CreateRequested;
        public event EventHandler<Int32EventArgs> OpenRequested;
        public event EventHandler<Int32EventArgs> DeleteRequested;
        public event EventHandler CloseRequested;

        private DocumentGridViewModel _viewModel;

        public DocumentGridUserControl()
        {
            InitializeComponent();
            SetTitles();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DocumentGridViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                ApplyViewModel();
            }
        }

        // Gui

        private void SetTitles()
        {
            titleBarUserControl.Text = PropertyDisplayNames.Documents;
        }

        private void ApplyViewModel()
        {
            if (_viewModel == null) return;

            pagerControl.PagerViewModel = _viewModel.Pager;
            dataGridView.DataSource = _viewModel.List;
        }

        // Actions

        private void Show(int pageNumber)
        {
            if (ShowRequested != null)
            {
                ShowRequested(this, new Int32EventArgs(pageNumber));
            }
        }

        private void Create()
        {
            if (CreateRequested != null)
            {
                CreateRequested(this, EventArgs.Empty);
            }
        }

        private void Open()
        {
            if (OpenRequested != null)
            {
                int? id = TryGetSelectedID();
                if (id.HasValue)
                {
                    OpenRequested(this, new Int32EventArgs(id.Value));
                }
            }
        }

        private void Delete()
        {
            if (DeleteRequested != null)
            {
                int? id = TryGetSelectedID();
                if (id.HasValue)
                {
                    DeleteRequested(this, new Int32EventArgs(id.Value));
                }
            }
        }

        private void Close()
        {
            if (CloseRequested != null)
            {
                CloseRequested(this, EventArgs.Empty);
            }
        }

        // Events

        private void pagerControl_GoToFirstPageClicked(object sender, EventArgs e)
        {
            Show(1);
        }

        private void pagerControl_GoToPreviousPageClicked(object sender, EventArgs e)
        {
            if (_viewModel == null) return;

            Show(_viewModel.Pager.PageNumber - 1);
        }

        private void pagerControl_PageNumberClicked(object sender, PageNumberEventArgs e)
        {
            Show(e.PageNumber);
        }

        private void pagerControl_GoToNextPageClicked(object sender, EventArgs e)
        {
            if (_viewModel == null) return;

            Show(_viewModel.Pager.PageNumber + 1);
        }

        private void pagerControl_GoToLastPageClicked(object sender, EventArgs e)
        {
            if (_viewModel == null) return;

            Show(_viewModel.Pager.PageCount);
        }

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

        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    Delete();
                    break;

                case Keys.Enter:
                    Open();
                    break;
            }
        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            Open();
        }

        // Helpers

        private int? TryGetSelectedID()
        {
            if (dataGridView.CurrentRow != null)
            {
                DataGridViewCell cell = dataGridView.CurrentRow.Cells[ID_COLUMN_NAME];
                int id = (int)cell.Value;
                return id;
            }

            return null;
        }
    }
}
