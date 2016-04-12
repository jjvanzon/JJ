﻿using JJ.Presentation.Synthesizer.WinForms.UserControls.Partials;
namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    partial class CurvePropertiesUserControl
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelProperties = new System.Windows.Forms.TableLayoutPanel();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelXDimension = new System.Windows.Forms.Label();
            this.labelYDimension = new System.Windows.Forms.Label();
            this.comboBoxXDimension = new System.Windows.Forms.ComboBox();
            this.comboBoxYDimension = new System.Windows.Forms.ComboBox();
            this.titleBarUserControl = new JJ.Presentation.Synthesizer.WinForms.UserControls.Partials.TitleBarUserControl();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutPanelProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelProperties, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.titleBarUserControl, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(10, 10);
            this.tableLayoutPanelMain.TabIndex = 8;
            // 
            // tableLayoutPanelProperties
            // 
            this.tableLayoutPanelProperties.ColumnCount = 2;
            this.tableLayoutPanelProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 147F));
            this.tableLayoutPanelProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProperties.Controls.Add(this.labelName, 0, 0);
            this.tableLayoutPanelProperties.Controls.Add(this.textBoxName, 1, 0);
            this.tableLayoutPanelProperties.Controls.Add(this.labelXDimension, 0, 1);
            this.tableLayoutPanelProperties.Controls.Add(this.labelYDimension, 0, 2);
            this.tableLayoutPanelProperties.Controls.Add(this.comboBoxXDimension, 1, 1);
            this.tableLayoutPanelProperties.Controls.Add(this.comboBoxYDimension, 1, 2);
            this.tableLayoutPanelProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProperties.Location = new System.Drawing.Point(4, 30);
            this.tableLayoutPanelProperties.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanelProperties.Name = "tableLayoutPanelProperties";
            this.tableLayoutPanelProperties.RowCount = 4;
            this.tableLayoutPanelProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelProperties.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProperties.Size = new System.Drawing.Size(10, 10);
            this.tableLayoutPanelProperties.TabIndex = 8;
            // 
            // labelName
            // 
            this.labelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelName.Location = new System.Drawing.Point(0, 0);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(147, 30);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "labelName";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxName
            // 
            this.textBoxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxName.Location = new System.Drawing.Point(147, 0);
            this.textBoxName.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(10, 22);
            this.textBoxName.TabIndex = 11;
            // 
            // labelXDimension
            // 
            this.labelXDimension.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelXDimension.Location = new System.Drawing.Point(0, 30);
            this.labelXDimension.Margin = new System.Windows.Forms.Padding(0);
            this.labelXDimension.Name = "labelXDimension";
            this.labelXDimension.Size = new System.Drawing.Size(147, 30);
            this.labelXDimension.TabIndex = 21;
            this.labelXDimension.Text = "labelXDimension";
            this.labelXDimension.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelYDimension
            // 
            this.labelYDimension.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelYDimension.Location = new System.Drawing.Point(0, 60);
            this.labelYDimension.Margin = new System.Windows.Forms.Padding(0);
            this.labelYDimension.Name = "labelYDimension";
            this.labelYDimension.Size = new System.Drawing.Size(147, 30);
            this.labelYDimension.TabIndex = 22;
            this.labelYDimension.Text = "labelYDimension";
            this.labelYDimension.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxXDimension
            // 
            this.comboBoxXDimension.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxXDimension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxXDimension.FormattingEnabled = true;
            this.comboBoxXDimension.Location = new System.Drawing.Point(147, 30);
            this.comboBoxXDimension.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxXDimension.Name = "comboBoxXDimension";
            this.comboBoxXDimension.Size = new System.Drawing.Size(10, 24);
            this.comboBoxXDimension.TabIndex = 23;
            // 
            // comboBoxYDimension
            // 
            this.comboBoxYDimension.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxYDimension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxYDimension.FormattingEnabled = true;
            this.comboBoxYDimension.Location = new System.Drawing.Point(147, 60);
            this.comboBoxYDimension.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxYDimension.Name = "comboBoxYDimension";
            this.comboBoxYDimension.Size = new System.Drawing.Size(10, 24);
            this.comboBoxYDimension.TabIndex = 24;
            // 
            // titleBarUserControl
            // 
            this.titleBarUserControl.AddButtonVisible = false;
            this.titleBarUserControl.BackColor = System.Drawing.SystemColors.Control;
            this.titleBarUserControl.CloseButtonVisible = true;
            this.titleBarUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleBarUserControl.Location = new System.Drawing.Point(0, 0);
            this.titleBarUserControl.Margin = new System.Windows.Forms.Padding(0);
            this.titleBarUserControl.Name = "titleBarUserControl";
            this.titleBarUserControl.RemoveButtonVisible = false;
            this.titleBarUserControl.Size = new System.Drawing.Size(18, 26);
            this.titleBarUserControl.TabIndex = 7;
            this.titleBarUserControl.CloseClicked += new System.EventHandler(this.titleBarUserControl_CloseClicked);
            // 
            // CurvePropertiesUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CurvePropertiesUserControl";
            this.Size = new System.Drawing.Size(10, 10);
            this.Load += new System.EventHandler(this.CurvePropertiesUserControl_Load);
            this.Leave += new System.EventHandler(this.CurvePropertiesUserControl_Leave);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelProperties.ResumeLayout(false);
            this.tableLayoutPanelProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private TitleBarUserControl titleBarUserControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProperties;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelXDimension;
        private System.Windows.Forms.Label labelYDimension;
        private System.Windows.Forms.ComboBox comboBoxXDimension;
        private System.Windows.Forms.ComboBox comboBoxYDimension;
    }
}
