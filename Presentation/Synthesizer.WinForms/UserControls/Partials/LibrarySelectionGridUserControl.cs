﻿using System;
using System.Windows.Forms;
using JJ.Business.Synthesizer.Resources;
using JJ.Data.Canonical;
using JJ.Framework.Presentation.Resources;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.WinForms.Properties;
using JJ.Presentation.Synthesizer.WinForms.UserControls.Bases;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls.Partials
{
    internal class LibrarySelectionGridUserControl : GridUserControlBase
    {
        private DataGridViewColumn _playColumn;
        private DataGridViewColumn _openColumn;

        public new int? TryGetSelectedID() => base.TryGetSelectedID();

        public LibrarySelectionGridUserControl()
        {
            Title = CommonResourceFormatter.Select_WithName(ResourceFormatter.Library);
            IDPropertyName = nameof(IDAndName.ID);
            ColumnTitlesVisible = true;
            AddButtonVisible = false;
            RemoveButtonVisible = false;
            CloseButtonVisible = false;
            PlayButtonVisible = true;
            FullRowSelect = false;
            OpenItemButtonVisible = true;

            KeyDown += base_KeyDown;
            CellClick += base_CellClick;
        }

        protected override object GetDataSource() => ViewModel?.List;

        protected override void AddColumns()
        {
            _playColumn = AddImageColumn(Resources.PlayIconThinner);
            AddAutoSizeColumn(nameof(IDAndName.Name), CommonResourceFormatter.Name);
            _openColumn = AddImageColumn(Resources.OpenWindowIconThinner);

            // NOTE: Add ID column last. If the ID column is the first column, WinForms will make the column visible,
            // when the DataGrid becomes invisible and then visible again, 
            // even when it is due to its parent becoming visible and then invisible again.
            // Thanks, WinForms.
            // Source of solution:
            // http://stackoverflow.com/questions/6359234/datagridview-id-column-will-not-hide 
            AddHiddenColumn(nameof(IDAndName.ID));
        }

        public new LibrarySelectionPopupViewModel ViewModel
        {
            get => (LibrarySelectionPopupViewModel)base.ViewModel;
            set => base.ViewModel = value;
        }

        private void base_KeyDown(object sender, KeyEventArgs e)
        {
            int? columnIndex = TryGetColumnIndex();
            if (!columnIndex.HasValue)
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Space when columnIndex.Value == _playColumn.Index:
                    Play();
                    e.Handled = true;
                    break;

                case Keys.Space when columnIndex.Value == _openColumn.Index:
                    OpenItem();
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
                return;
            }

            // ReSharper disable once InvertIf
            if (e.ColumnIndex == _openColumn.Index)
            {
                OpenItem();
                // ReSharper disable once RedundantJumpStatement
                return;
            }
        }
    }
}
