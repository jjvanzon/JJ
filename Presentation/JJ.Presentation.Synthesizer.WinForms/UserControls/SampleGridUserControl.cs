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
using JJ.Presentation.Synthesizer.WinForms.EventArg;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Presentation.Resources;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    internal partial class SampleGridUserControl : UserControl
    {
        private const string ID_COLUMN_NAME = "IDColumn";

        public event EventHandler<NullableInt32EventArgs> CreateRequested;
        public event EventHandler<Int32EventArgs> DeleteRequested;
        public event EventHandler CloseRequested;
        public event EventHandler<Int32EventArgs> ShowPropertiesRequested;

        /// <summary> virtually not nullable </summary>
        private SampleGridViewModel _viewModel;

        public SampleGridUserControl()
        {
            InitializeComponent();
            SetTitles();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SampleGridViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (value == null) throw new NullException(() => value);
                _viewModel = value;
                ApplyViewModel();
            }
        }

        // Gui

        private void SetTitles()
        {
            titleBarUserControl.Text = PropertyDisplayNames.Samples;
            NameColumn.HeaderText = CommonTitles.Name;
            AudioFileFormatColumn.HeaderText = PropertyDisplayNames.AudioFileFormat;
            SampleDataTypeColumn.HeaderText = PropertyDisplayNames.SampleDataType;
            SpeakerSetupColumn.HeaderText = PropertyDisplayNames.SpeakerSetup;
            SamplingRateColumn.HeaderText = PropertyDisplayNames.SamplingRate;
            IsActiveColumn.HeaderText = PropertyDisplayNames.IsActive;
        }

        private void ApplyViewModel()
        {
            specializedDataGridView.DataSource = _viewModel.List.Select(x => new
            {
                x.ID,
                x.Name,
                IsActive = x.IsActive ? CommonTitles.Yes : CommonTitles.No,
                x.SamplingRate,
                x.SampleDataType,
                x.SpeakerSetup,
                x.AudioFileFormat,
            }).ToArray();
        }

        // Actions

        private void Create()
        {
            if (CreateRequested != null)
            {
                var e = new NullableInt32EventArgs(ViewModel.DocumentID);
                CreateRequested(this, e);
            }
        }

        private void Delete()
        {
            if (DeleteRequested != null)
            {
                int? id = TryGetSelectedID();
                if (id.HasValue)
                {
                    var e = new Int32EventArgs(id.Value);
                    DeleteRequested(this, e);
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

        private void ShowProperties()
        {
            if (ShowPropertiesRequested != null)
            {
                int? id = TryGetSelectedID();
                if (id.HasValue)
                {
                    var e = new Int32EventArgs(id.Value);
                    ShowPropertiesRequested(this, e);
                }
            }
        }

        // Events

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

        private void specializedDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    Delete();
                    break;

                case Keys.Enter:
                    ShowProperties();
                    break;
            }
        }

        private void specializedDataGridView_DoubleClick(object sender, EventArgs e)
        {
            ShowProperties();
        }

        // Helpers

        private int? TryGetSelectedID()
        {
            if (specializedDataGridView.CurrentRow != null)
            {
                DataGridViewCell cell = specializedDataGridView.CurrentRow.Cells[ID_COLUMN_NAME];
                int id = Convert.ToInt32(cell.Value);
                return id;
            }

            return null;
        }
    }
}
