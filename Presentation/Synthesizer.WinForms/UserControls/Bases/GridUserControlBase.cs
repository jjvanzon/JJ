﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using JetBrains.Annotations;
using JJ.Framework.Exceptions;
using JJ.Presentation.Synthesizer.WinForms.EventArg;
using JJ.Presentation.Synthesizer.WinForms.Helpers;
using JJ.Presentation.Synthesizer.WinForms.UserControls.Partials;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls.Bases
{
    internal class GridUserControlBase : UserControlBase
    {
        private const int DEFAULT_COLUMN_WIDTH_IN_PIXELS = 120;

        public event EventHandler AddRequested;
        public event EventHandler<EventArgs<int>> RemoveRequested;
        public event EventHandler CloseRequested;
        public event EventHandler<EventArgs<int>> ShowItemRequested;
        public event EventHandler<EventArgs<int>> PlayRequested;

        public new event KeyEventHandler KeyDown
        {
            add => _specializedDataGridView.KeyDown += value;
            remove => _specializedDataGridView.KeyDown -= value;
        }

        public event DataGridViewCellEventHandler CellClick
        {
            add => _specializedDataGridView.CellClick += value;
            remove => _specializedDataGridView.CellClick -= value;
        }

        [NotNull] private readonly TitleBarUserControl _titleBarUserControl;
        [NotNull] private readonly SpecializedDataGridView _specializedDataGridView;
        [NotNull] private readonly TableLayoutPanel _tableLayoutPanel;

        private int _columnCounter = 1;

        public GridUserControlBase()
        {
            _tableLayoutPanel = CreateTableLayoutPanel();
            Controls.Add(_tableLayoutPanel);

            _titleBarUserControl = CreateTitleBarUserControl();
            _tableLayoutPanel.Controls.Add(_titleBarUserControl, 0, 0);

            _specializedDataGridView = CreateSpecializedDataGridView();
            _tableLayoutPanel.Controls.Add(_specializedDataGridView, 0, 1);

            AutoScaleMode = AutoScaleMode.None;

            // ReSharper disable once VirtualMemberCallInConstructor
            AddColumns();
        }

        // Create Controls

        private TableLayoutPanel CreateTableLayoutPanel()
        {
            var tableLayoutPanel = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            tableLayoutPanel.Name = nameof(tableLayoutPanel);
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, StyleHelper.TitleBarHeight));
            tableLayoutPanel.RowStyles.Add(new RowStyle());

            return tableLayoutPanel;
        }

        private TitleBarUserControl CreateTitleBarUserControl()
        {
            var titleBarUserControl = new TitleBarUserControl
            {
                AddButtonVisible = true,
                CloseButtonVisible = true,
                RemoveButtonVisible = true,
                SaveButtonVisible = false,
                PlayButtonVisible = false,
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Name = nameof(_titleBarUserControl),
            };

            titleBarUserControl.CloseClicked += _titleBarUserControl_CloseClicked;
            titleBarUserControl.RemoveClicked += _titleBarUserControl_RemoveClicked;
            titleBarUserControl.AddClicked += _titleBarUserControl_AddClicked;
            titleBarUserControl.PlayClicked += _titleBarUserControl_PlayClicked;

            return titleBarUserControl;
        }

        private SpecializedDataGridView CreateSpecializedDataGridView()
        {
            var specializedDataGridView = new SpecializedDataGridView
            {
                Name = nameof(_specializedDataGridView),
                Dock = DockStyle.Fill,
                Visible = true
            };

            specializedDataGridView.DoubleClick += _specializedDataGridView_DoubleClick;
            specializedDataGridView.KeyDown += _specializedDataGridView_KeyDown;

            return specializedDataGridView;
        }

        // Gui

        protected string Title
        {
            get => _titleBarUserControl.Text;
            set => _titleBarUserControl.Text = value;
        }

        protected bool ColumnTitlesVisible
        {
            get => _specializedDataGridView.ColumnHeadersVisible;
            set => _specializedDataGridView.ColumnHeadersVisible = value;
        }

        [DefaultValue(true)]
        protected bool AddButtonVisible
        {
            get => _titleBarUserControl.AddButtonVisible;
            set => _titleBarUserControl.AddButtonVisible = value;
        }

        [DefaultValue(true)]
        protected bool RemoveButtonVisible
        {
            get => _titleBarUserControl.RemoveButtonVisible;
            set => _titleBarUserControl.RemoveButtonVisible = value;
        }

        [DefaultValue(true)]
        protected bool CloseButtonVisible
        {
            get => _titleBarUserControl.CloseButtonVisible;
            set => _titleBarUserControl.CloseButtonVisible = value;
        }

        protected bool PlayButtonVisible
        {
            get => _titleBarUserControl.PlayButtonVisible;
            set => _titleBarUserControl.PlayButtonVisible = value;
        }

        /// <summary> does nothing </summary>
        protected virtual void AddColumns() { }

        protected DataGridViewTextBoxColumn AddHiddenColumn([NotNull] string dataPropertyName)
        {
            DataGridViewTextBoxColumn dataGridViewColumn = CreateTextBoxColumn(dataPropertyName);
            dataGridViewColumn.Visible = false;

            return dataGridViewColumn;
        }

        protected DataGridViewTextBoxColumn AddAutoSizeColumn([NotNull] string dataPropertyName, string title)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new NullOrEmptyException(() => title);

            DataGridViewTextBoxColumn dataGridViewColumn = CreateTextBoxColumn(dataPropertyName);
            dataGridViewColumn.HeaderText = title;
            dataGridViewColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            return dataGridViewColumn;
        }

        protected DataGridViewTextBoxColumn AddColumnWithWidth([NotNull] string dataPropertyName, string title, int widthInPixels)
        {
            DataGridViewTextBoxColumn dataGridViewColumn = CreateTextBoxColumn(dataPropertyName);
            dataGridViewColumn.HeaderText = title;
            dataGridViewColumn.Width = widthInPixels;

            return dataGridViewColumn;
        }

        protected DataGridViewImageColumn AddImageColumn(Image image)
        {
            var dataGridViewColumn = new DataGridViewImageColumn
            {
                Image = image,
                Name = $"{nameof(image)}Column{_columnCounter++}",
                HeaderText = "",
                Visible = true,
                Resizable = DataGridViewTriState.False,
                Width = image.Width + 2,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = SystemColors.Control
                }
            };

            _specializedDataGridView.Columns.Add(dataGridViewColumn);

            return dataGridViewColumn;
        }

        protected void AddColumn([NotNull] string dataPropertyName, string title)
        {
            DataGridViewColumn dataGridViewColumn = CreateTextBoxColumn(dataPropertyName);
            dataGridViewColumn.HeaderText = title;
            dataGridViewColumn.Width = DEFAULT_COLUMN_WIDTH_IN_PIXELS;
        }

        private DataGridViewTextBoxColumn CreateTextBoxColumn([NotNull] string dataPropertyName)
        {
            if (string.IsNullOrEmpty(dataPropertyName)) throw new NullOrEmptyException(() => dataPropertyName);

            var dataGridViewColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = dataPropertyName,
                Name = dataPropertyName + "Column",
                ReadOnly = true,
                Visible = true
            };

            _specializedDataGridView.Columns.Add(dataGridViewColumn);

            return dataGridViewColumn;
        }

        // Binding

        protected virtual object GetDataSource() => null;
        protected string IDPropertyName { get; set; }
        protected override void ApplyViewModelToControls() => _specializedDataGridView.DataSource = GetDataSource();

        // Events

        private void _titleBarUserControl_AddClicked(object sender, EventArgs e) => Add();
        private void _titleBarUserControl_RemoveClicked(object sender, EventArgs e) => Remove();
        private void _titleBarUserControl_CloseClicked(object sender, EventArgs e) => Close();
        private void _specializedDataGridView_DoubleClick(object sender, EventArgs e) => OpenItem();
        private void _titleBarUserControl_PlayClicked(object sender, EventArgs e) => Play();

        private void _specializedDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            switch (e.KeyCode)
            {
                case Keys.Delete:
                    Remove();
                    break;

                case Keys.Enter:
                    OpenItem();
                    break;

                default:
                    e.Handled = false;
                    break;
            }
        }

        // Actions

        private void Add()
        {
            if (ViewModel == null) return;

            AddRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Remove()
        {
            if (ViewModel == null) return;

            int? id = TryGetSelectedID();
            if (id.HasValue)
            {
                RemoveRequested?.Invoke(this, new EventArgs<int>(id.Value));
            }
        }

        private void Close()
        {
            if (ViewModel == null) return;

            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OpenItem()
        {
            if (ViewModel == null) return;

            int? id = TryGetSelectedID();
            if (id.HasValue)
            {
                ShowItemRequested?.Invoke(this, new EventArgs<int>(id.Value));
            }
        }

        protected void Play()
        {
            if (ViewModel == null) return;

            int? id = TryGetSelectedID();
            if (id.HasValue)
            {
                PlayRequested?.Invoke(this, new EventArgs<int>(id.Value));
            }
        }

        // Helpers

        protected int? TryGetSelectedID()
        {
            if (_specializedDataGridView.CurrentRow == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(IDPropertyName))
            {
                throw new NullException(() => IDPropertyName);
            }

            string idColumnName = $"{IDPropertyName}Column";

            DataGridViewCell cell = _specializedDataGridView.CurrentRow.Cells[idColumnName];
            int id = (int)cell.Value;
            return id;
        }
    }
}
