﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Framework.Presentation.Resources;
using JJ.Business.Synthesizer.Resources;
using System.Linq;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    internal partial class CurvePropertiesUserControl : PropertiesUserControlBase
    {
        public CurvePropertiesUserControl()
        {
            InitializeComponent();
        }

        // Gui

        protected override void AddProperties()
        {
            AddProperty(labelName, textBoxName);
        }

        protected override void SetTitles()
        {
            TitleBarText = CommonTitleFormatter.ObjectProperties(PropertyDisplayNames.Curve);
            labelName.Text = CommonTitles.Name;
        }

        // Binding

        private new CurvePropertiesViewModel ViewModel => (CurvePropertiesViewModel)base.ViewModel;

        protected override int GetID()
        {
            return ViewModel.ID;
        }

        protected override void ApplyViewModelToControls()
        {
            textBoxName.Text = ViewModel.Name;
        }

        protected override void ApplyControlsToViewModel()
        {
            ViewModel.Name = textBoxName.Text;
        }
    }
}
