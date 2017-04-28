﻿using System.Windows.Forms;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Presentation.Resources;
using JJ.Presentation.Synthesizer.WinForms.UserControls.Bases;
using JJ.Presentation.Synthesizer.ViewModels.Items;
using JJ.Presentation.Synthesizer.WinForms.Properties;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    internal partial class LibraryGridUserControl : GridUserControlBase
    {
        private DataGridViewColumn _playColumn;

        public LibraryGridUserControl()
        {
            InitializeComponent();

            Title = ResourceFormatter.Libraries;
            IDPropertyName = nameof(LibraryListItemViewModel.DocumentReferenceID);
            ColumnTitlesVisible = true;
            PlayButtonVisible = true;

            KeyDown += base_KeyDown;
            CellClick += base_CellClick;
        }

        protected override object GetDataSource() => ViewModel?.List;

        protected override void AddColumns()
        {
            AddHiddenColumn(nameof(LibraryListItemViewModel.DocumentReferenceID));
            _playColumn = AddImageColumn(Resources.PlayIcon);
            AddAutoSizeColumn(nameof(LibraryListItemViewModel.ReferencedDocumentName), CommonResourceFormatter.Name);
            AddColumnWithWidth(nameof(LibraryListItemViewModel.Alias), ResourceFormatter.Alias, 180);
        }

        public new LibraryGridViewModel ViewModel
        {
            get => (LibraryGridViewModel)base.ViewModel;
            set => base.ViewModel = value;
        }

        private void base_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    Play();
                    e.Handled = true;
                    break;
            }
        }

        private void base_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ViewModel == null) return;

            if (e.RowIndex == -1)
            {
                return;
            }

            if (e.ColumnIndex == _playColumn.Index)
            {
                Play();
            }
        }
    }
}
