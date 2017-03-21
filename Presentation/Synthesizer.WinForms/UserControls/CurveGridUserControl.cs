﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Business.Synthesizer.Resources;
using JJ.Presentation.Synthesizer.WinForms.UserControls.Bases;
using JJ.Framework.Presentation.Resources;
using JJ.Presentation.Synthesizer.ViewModels.Items;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    internal partial class CurveGridUserControl : GridUserControlBase
    {
        public CurveGridUserControl()
        {
            InitializeComponent();

            Title = ResourceFormatter.Curves;
            IDPropertyName = PropertyNames.ID;
            ColumnTitlesVisible = true;
        }

        protected override object GetDataSource() => ViewModel?.List;

        protected override void AddColumns()
        {
            AddColumn(nameof(CurveListItemViewModel.ID), null, visible: false);
            AddColumn(nameof(CurveListItemViewModel.Name), CommonResourceFormatter.Name, autoSize: true);
            AddColumn(nameof(CurveListItemViewModel.UsedIn), ResourceFormatter.UsedIn, widthInPixels: 180);
        }

        public new CurveGridViewModel ViewModel
        {
            get { return (CurveGridViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
