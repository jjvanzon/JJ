﻿using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Resources;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.WinForms.UserControls.Bases;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
	internal partial class LibraryPropertiesUserControl : PropertiesUserControlBase
	{
		public LibraryPropertiesUserControl()
	    {
	        InitializeComponent();

            TitleBarUserControl.TitleBarElement.ButtonBarElement.PictureButtonExpand.Visible = true;
	        TitleBarUserControl.TitleBarElement.ButtonBarElement.PictureButtonPlay.Visible = true;
	    }

        protected override void AddProperties()
		{
			AddProperty(labelNameTitle, labelNameValue);
			AddProperty(labelAlias, textBoxAlias);
		}

		protected override void SetTitles()
		{
			TitleBarUserControl.TitleBarElement.TitleLabel.Text = CommonResourceFormatter.Properties_WithName(ResourceFormatter.Library);
			labelNameTitle.Text = CommonResourceFormatter.Name;
			labelAlias.Text = ResourceFormatter.Alias;
		}

		public new LibraryPropertiesViewModel ViewModel
		{
			// ReSharper disable once MemberCanBePrivate.Global
			get => (LibraryPropertiesViewModel)base.ViewModel;
			set => base.ViewModel = value;
		}

		protected override int GetID() => ViewModel.DocumentReferenceID;

		protected override void ApplyViewModelToControls()
		{
			labelNameValue.Text = ViewModel.Name;
			textBoxAlias.Text = ViewModel.Alias;
		}

		protected override void ApplyControlsToViewModel()
		{
			ViewModel.Name = labelNameValue.Text;
			ViewModel.Alias = textBoxAlias.Text;
		}
	}
}
