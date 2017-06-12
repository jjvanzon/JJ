﻿using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.WinForms.UserControls.Bases;
using JJ.Data.Canonical;
using JJ.Business.Synthesizer.Resources;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    internal partial class OperatorPropertiesUserControl_ForPatchOutlet 
        : OperatorPropertiesUserControlBase
    {
        public OperatorPropertiesUserControl_ForPatchOutlet() => InitializeComponent();

        // Gui

        protected override void SetTitles()
        {
            base.SetTitles();

            labelNumber.Text = ResourceFormatter.Number;
            labelDimension.Text = ResourceFormatter.Dimension;
            labelNameOrDimensionHidden.Text = ResourceFormatter.NameOrDimensionHidden;
        }

        protected override void AddProperties()
        {
            AddProperty(_labelOperatorTypeTitle, _labelOperatorTypeValue);
            AddProperty(labelDimension, comboBoxDimension);
            AddProperty(_labelName, _textBoxName);
            AddProperty(labelNumber, numericUpDownNumber);
            AddProperty(labelNameOrDimensionHidden, checkBoxNameOrDimensionHidden);
        }

        // Binding

        public new OperatorPropertiesViewModel_ForPatchOutlet ViewModel
        {
            get => (OperatorPropertiesViewModel_ForPatchOutlet)base.ViewModel;
            set => base.ViewModel = value;
        }

        protected override void ApplyViewModelToControls()
        {
            base.ApplyViewModelToControls();

            numericUpDownNumber.Value = ViewModel.Number;
            if (comboBoxDimension.DataSource == null)
            {
                comboBoxDimension.ValueMember = nameof(IDAndName.ID);
                comboBoxDimension.DisplayMember = nameof(IDAndName.Name);
                comboBoxDimension.DataSource = ViewModel.DimensionLookup;
            }
            comboBoxDimension.SelectedValue = ViewModel.Dimension?.ID ?? 0;

            checkBoxNameOrDimensionHidden.Checked = ViewModel.NameOrDimensionHidden;
        }

        protected override void ApplyControlsToViewModel()
        {
            base.ApplyControlsToViewModel();
            
            ViewModel.Number = (int)numericUpDownNumber.Value;
            ViewModel.Dimension = (IDAndName)comboBoxDimension.SelectedItem;
            ViewModel.NameOrDimensionHidden = checkBoxNameOrDimensionHidden.Checked;
        }
    }
}
