﻿namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    partial class SampleGridUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.specializedDataGridView = new JJ.Presentation.Synthesizer.WinForms.UserControls.Partials.SpecializedDataGridView();
            this.titleBarUserControl = new JJ.Presentation.Synthesizer.WinForms.UserControls.Partials.TitleBarUserControl();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.IDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SampleDataTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpeakerSetupColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SamplingRateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActiveColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsedInColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.specializedDataGridView)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // specializedDataGridView
            // 
            this.specializedDataGridView.AllowUserToAddRows = false;
            this.specializedDataGridView.AllowUserToDeleteRows = false;
            this.specializedDataGridView.AllowUserToResizeRows = false;
            this.specializedDataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.specializedDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.specializedDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.specializedDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.specializedDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IDColumn,
            this.NameColumn,
            this.SampleDataTypeColumn,
            this.SpeakerSetupColumn,
            this.SamplingRateColumn,
            this.IsActiveColumn,
            this.UsedInColumn});
            this.specializedDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.specializedDataGridView.Location = new System.Drawing.Point(0, 26);
            this.specializedDataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.specializedDataGridView.Name = "specializedDataGridView";
            this.specializedDataGridView.RowHeadersVisible = false;
            this.specializedDataGridView.Size = new System.Drawing.Size(985, 215);
            this.specializedDataGridView.TabIndex = 0;
            this.specializedDataGridView.DoubleClick += new System.EventHandler(this.specializedDataGridView_DoubleClick);
            this.specializedDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.specializedDataGridView_KeyDown);
            // 
            // titleBarUserControl
            // 
            this.titleBarUserControl.AddButtonVisible = true;
            this.titleBarUserControl.BackColor = System.Drawing.SystemColors.Control;
            this.titleBarUserControl.CloseButtonVisible = true;
            this.titleBarUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleBarUserControl.Location = new System.Drawing.Point(0, 0);
            this.titleBarUserControl.Margin = new System.Windows.Forms.Padding(0);
            this.titleBarUserControl.Name = "titleBarUserControl";
            this.titleBarUserControl.RemoveButtonVisible = true;
            this.titleBarUserControl.Size = new System.Drawing.Size(985, 26);
            this.titleBarUserControl.TabIndex = 2;
            this.titleBarUserControl.CloseClicked += new System.EventHandler(this.titleBarUserControl_CloseClicked);
            this.titleBarUserControl.RemoveClicked += new System.EventHandler(this.titleBarUserControl_RemoveClicked);
            this.titleBarUserControl.AddClicked += new System.EventHandler(this.titleBarUserControl_AddClicked);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.titleBarUserControl, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.specializedDataGridView, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(985, 241);
            this.tableLayoutPanel.TabIndex = 4;
            // 
            // IDColumn
            // 
            this.IDColumn.DataPropertyName = "ID";
            this.IDColumn.HeaderText = "ID";
            this.IDColumn.Name = "IDColumn";
            this.IDColumn.ReadOnly = true;
            this.IDColumn.Visible = false;
            this.IDColumn.Width = 80;
            // 
            // NameColumn
            // 
            this.NameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NameColumn.DataPropertyName = "Name";
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            // 
            // SampleDataTypeColumn
            // 
            this.SampleDataTypeColumn.DataPropertyName = "SampleDataType";
            this.SampleDataTypeColumn.HeaderText = "SampleDataType";
            this.SampleDataTypeColumn.Name = "SampleDataTypeColumn";
            this.SampleDataTypeColumn.ReadOnly = true;
            this.SampleDataTypeColumn.Width = 120;
            // 
            // SpeakerSetupColumn
            // 
            this.SpeakerSetupColumn.DataPropertyName = "SpeakerSetup";
            this.SpeakerSetupColumn.HeaderText = "SpeakerSetup";
            this.SpeakerSetupColumn.Name = "SpeakerSetupColumn";
            this.SpeakerSetupColumn.ReadOnly = true;
            this.SpeakerSetupColumn.Width = 120;
            // 
            // SamplingRateColumn
            // 
            this.SamplingRateColumn.DataPropertyName = "SamplingRate";
            this.SamplingRateColumn.HeaderText = "SamplingRate";
            this.SamplingRateColumn.Name = "SamplingRateColumn";
            this.SamplingRateColumn.ReadOnly = true;
            this.SamplingRateColumn.Width = 120;
            // 
            // IsActiveColumn
            // 
            this.IsActiveColumn.DataPropertyName = "IsActive";
            this.IsActiveColumn.HeaderText = "IsActive";
            this.IsActiveColumn.Name = "IsActiveColumn";
            this.IsActiveColumn.ReadOnly = true;
            // 
            // UsedInColumn
            // 
            this.UsedInColumn.DataPropertyName = "UsedIn";
            this.UsedInColumn.HeaderText = "UsedIn";
            this.UsedInColumn.Name = "UsedInColumn";
            this.UsedInColumn.ReadOnly = true;
            this.UsedInColumn.Width = 180;
            // 
            // SampleGridUserControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "SampleGridUserControl";
            this.Size = new System.Drawing.Size(985, 241);
            ((System.ComponentModel.ISupportInitialize)(this.specializedDataGridView)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Partials.SpecializedDataGridView specializedDataGridView;
        private Partials.TitleBarUserControl titleBarUserControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SampleDataTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpeakerSetupColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SamplingRateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsActiveColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsedInColumn;
    }
}