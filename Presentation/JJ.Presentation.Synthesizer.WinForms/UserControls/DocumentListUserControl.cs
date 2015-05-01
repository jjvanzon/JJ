﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JJ.Framework.Presentation.WinForms;
using JJ.Framework.Data;
using JJ.Presentation.Synthesizer.Presenters;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.WinForms.Helpers;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Presentation;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Presentation.Resources;
using JJ.Presentation.Synthesizer.WinForms.EventArg;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    internal partial class DocumentListUserControl : UserControl
    {
        public event EventHandler CloseRequested;
        public event EventHandler<DocumentDetailsViewEventArgs> DetailsViewRequested;

        private DocumentListViewModel _viewModel;

        public DocumentListUserControl()
        {
            InitializeComponent();

            SetTitles();
        }

        // Persistence

        private IContext _context;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IContext Context
        {
            get { return _context; }
            set 
            {
                if (value == null) throw new NullException(() => value);
                _context = value;
            }
        }

        // Actions

        public void Show(int pageNumber = 1)
        {
            DocumentListPresenter presenter = CreatePresenter();
            _viewModel = presenter.Show(pageNumber);
            ApplyViewModel();

            base.Show();
        }

        private void Add()
        {
            DocumentListPresenter presenter = CreatePresenter();
            DocumentDetailsViewModel viewModel2 = presenter.Create();

            // TODO: I would almost say you may want something like a controller.
            // Perhaps it should control a giant view model with everything in it...

            if (DetailsViewRequested != null)
            {
                var e = new DocumentDetailsViewEventArgs(viewModel2);
                DetailsViewRequested(this, e);
            }
        }

        private void Close()
        {
            if (CloseRequested != null)
            {
                CloseRequested(this, EventArgs.Empty);
            }
        }

        // Gui

        private void ApplyViewModel()
        {
            if (_viewModel == null)
            {
                pagerControl.PagerViewModel = null;
                dataGridView.DataSource = null;
            }

            pagerControl.PagerViewModel = _viewModel.Pager;

            dataGridView.DataSource = _viewModel.List;
        }

        private void SetTitles()
        {
            titleBarUserControl.Text = PropertyDisplayNames.Documents;
            IDColumn.HeaderText = CommonTitles.ID;
            NameColumn.HeaderText = CommonTitles.Name;
        }

        // Events

        private void pagerControl_GoToFirstPageClicked(object sender, EventArgs e)
        {
            Show(1);
        }

        private void pagerControl_GoToPreviousPageClicked(object sender, EventArgs e)
        {
            Show(_viewModel.Pager.PageNumber - 1);
        }

        private void pagerControl_PageNumberClicked(object sender, PageNumberEventArgs e)
        {
            Show(e.PageNumber);
        }

        private void pagerControl_GoToNextPageClicked(object sender, EventArgs e)
        {
            Show(_viewModel.Pager.PageNumber + 1);
        }

        private void pagerControl_GoToLastPageClicked(object sender, EventArgs e)
        {
            Show(_viewModel.Pager.PageCount - 1);
        }

        private void titleBarUserControl_AddClicked(object sender, EventArgs e)
        {
            Add();
        }

        private void titleBarUserControl_RemoveClicked(object sender, EventArgs e)
        {
            //Remove();
        }

        private void titleBarUserControl_CloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        // Helpers

        private DocumentListPresenter CreatePresenter()
        {
            if (_context == null) throw new Exception("Assign Context first.");

            return new DocumentListPresenter(PersistenceHelper.CreateRepository<IDocumentRepository>(_context));
        }
    }
}
