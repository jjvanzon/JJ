﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Framework.Presentation.Resources;
using JJ.Business.Synthesizer.Resources;
using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Canonical;
using JJ.Presentation.Synthesizer.WinForms.Helpers;
using JJ.Framework.Presentation.WinForms.Extensions;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    internal partial class OperatorPropertiesUserControl_ForCurve : UserControl
    {
        public event EventHandler CloseRequested;
        public event EventHandler LoseFocusRequested;

        private OperatorPropertiesViewModel_ForCurve _viewModel;

        public OperatorPropertiesUserControl_ForCurve()
        {
            InitializeComponent();

            SetTitles();

            this.AutomaticallyAssignTabIndexes();
        }

        private void OperatorPropertiesUserControl_ForCurve_Load(object sender, EventArgs e)
        {
            ApplyStyling();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OperatorPropertiesViewModel_ForCurve ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                ApplyViewModelToControls();
            }
        }

        // Gui

        private void SetTitles()
        {
            titleBarUserControl.Text = CommonTitleFormatter.ObjectProperties(PropertyDisplayNames.Operator);

            labelName.Text = CommonTitles.Name;
            labelOperatorTypeTitle.Text = PropertyDisplayNames.OperatorType + ":";
            labelCurve.Text = PropertyDisplayNames.Curve;

            labelOperatorTypeValue.Text = PropertyDisplayNames.Curve;
        }

        private void ApplyStyling()
        {
            StyleHelper.SetPropertyLabelColumnSize(tableLayoutPanelProperties);
        }

        // Binding

        private void ApplyViewModelToControls()
        {
            if (_viewModel == null) return;

            textBoxName.Text = _viewModel.Name;

            if (_viewModel.Curve != null)
            {
                comboBoxCurve.SelectedValue = _viewModel.Curve.ID;
            }
            else
            {
                comboBoxCurve.SelectedValue = 0;
            }
        }

        public void SetCurveLookup(IList<IDAndName> curveLookup)
        {
            // Always refill the lookup, so changes to the curve collection are reflected.
            int? selectedID = TryGetSelectedCurveID();
            comboBoxCurve.DataSource = null; // Do this or WinForms will not refresh the list.
            comboBoxCurve.ValueMember = PropertyNames.ID;
            comboBoxCurve.DisplayMember = PropertyNames.Name;
            comboBoxCurve.DataSource = curveLookup;
            if (selectedID != null)
            {
                comboBoxCurve.SelectedValue = selectedID;
            }
        }

        private int? TryGetSelectedCurveID()
        {
            if (comboBoxCurve.DataSource == null) return null;
            IDAndName idAndName = (IDAndName)comboBoxCurve.SelectedItem;
            if (idAndName == null) return null;
            return idAndName.ID;
        }

        private void ApplyControlsToViewModel()
        {
            if (_viewModel == null) return;

            _viewModel.Name = textBoxName.Text;
            _viewModel.Curve = (IDAndName)comboBoxCurve.SelectedItem;
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

        private void titleBarUserControl_CloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        // This event does not go off, if not clicked on a control that according to WinForms can get focus.
        private void OperatorPropertiesUserControl_ForCurve_Leave(object sender, EventArgs e)
        {
            // This Visible check is there because the leave event (lose focus) goes off after I closed, 
            // making it want to save again, even though view model is empty
            // which makes it say that now clear fields are required.
            if (Visible) 
            {
                LoseFocus();
            }
        }
    }
}
