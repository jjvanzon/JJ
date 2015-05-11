﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JJ.Framework.Presentation.WinForms;
using JJ.Framework.Data;
using JJ.Presentation.Synthesizer.Presenters;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.WinForms.Helpers;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Presentation.Resources;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Presentation;
using JJ.Presentation.Synthesizer.WinForms.EventArg;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    internal partial class DocumentPropertiesUserControl : UserControl
    {
        public event EventHandler CloseRequested;
        public event EventHandler LoseFocusRequested;

        /// <summary> virtually not nullable </summary>
        private DocumentPropertiesViewModel _viewModel;

        public DocumentPropertiesUserControl()
        {
            InitializeComponent();

            SetTitles();

            this.AutomaticallyAssignTabIndexes();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DocumentPropertiesViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (value == null) throw new NullException(() => value);
                _viewModel = value;
                ApplyViewModelToControls();
            }
        }

        // Gui

        private void SetTitles()
        {
            titleBarUserControl1.Text = CommonTitleFormatter.ObjectProperties(PropertyDisplayNames.Document);
            labelIDTitle.Text = CommonTitles.ID;
            labelName.Text = CommonTitles.Name;
        }

        private void ApplyViewModelToControls()
        {
            labelIDValue.Text = _viewModel.Document.ID.ToString();
            textBoxName.Text = _viewModel.Document.Name;
        }

        private void ApplyControlsToViewModel()
        {
            _viewModel.Document.Name = textBoxName.Text;
        }

        // Actions

        private void Close()
        {
            if (CloseRequested != null)
            {
                ApplyControlsToViewModel();
                CloseRequested(this, EventArgs.Empty);
            }
        }


        private void LoseFocus()
        {
            if (LoseFocusRequested != null)
            {
                ApplyControlsToViewModel();
                LoseFocusRequested(this, EventArgs.Empty);
            }
        }

        // Events

        private void titleBarUserControl1_CloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void DocumentDetailsUserControl_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                textBoxName.Focus();
            }
        }

        // This event goes off when I call DocumentPropertiesUserControl.SetFocus after clicking on a DataGridView,
        // but does not go off when I call DocumentPropertiesUserControl.SetFocus after clicking on a TreeView.
        // Thanks, WinForms...
        private void DocumentPropertiesUserControl_Enter(object sender, EventArgs e)
        {
            textBoxName.Focus();
        }

        // This event does not go off, if not clicked on a control that according to WinForms can get focus.
        private void DocumentPropertiesUserControl_Leave(object sender, EventArgs e)
        {
            LoseFocus();
        }
    }
}
