﻿using JJ.Presentation.Synthesizer.WinForms.UserControls.Partials;
namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    partial class OperatorPropertiesUserControl
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
            this.tableLayoutPanelProperties = new System.Windows.Forms.TableLayoutPanel();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelOperatorTypeTitle = new System.Windows.Forms.Label();
            this.labelOperatorTypeValue = new System.Windows.Forms.Label();
            this.tableLayoutPanelProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelProperties
            // 
            this.tableLayoutPanelProperties.ColumnCount = 2;
            this.tableLayoutPanelProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 147F));
            this.tableLayoutPanelProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProperties.Controls.Add(this.labelName, 0, 1);
            this.tableLayoutPanelProperties.Controls.Add(this.textBoxName, 1, 1);
            this.tableLayoutPanelProperties.Controls.Add(this.labelOperatorTypeTitle, 0, 0);
            this.tableLayoutPanelProperties.Controls.Add(this.labelOperatorTypeValue, 1, 0);
            this.tableLayoutPanelProperties.Location = new System.Drawing.Point(4, 30);
            this.tableLayoutPanelProperties.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanelProperties.Name = "tableLayoutPanelProperties";
            this.tableLayoutPanelProperties.RowCount = 3;
            this.tableLayoutPanelProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelProperties.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProperties.Size = new System.Drawing.Size(533, 337);
            this.tableLayoutPanelProperties.TabIndex = 8;
            // 
            // labelName
            // 
            this.labelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelName.Location = new System.Drawing.Point(0, 30);
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
            this.textBoxName.Location = new System.Drawing.Point(147, 30);
            this.textBoxName.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(386, 22);
            this.textBoxName.TabIndex = 11;
            // 
            // labelOperatorTypeTitle
            // 
            this.labelOperatorTypeTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOperatorTypeTitle.Location = new System.Drawing.Point(0, 0);
            this.labelOperatorTypeTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelOperatorTypeTitle.Name = "labelOperatorTypeTitle";
            this.labelOperatorTypeTitle.Size = new System.Drawing.Size(147, 30);
            this.labelOperatorTypeTitle.TabIndex = 12;
            this.labelOperatorTypeTitle.Text = "labelOperatorTypeTitle";
            this.labelOperatorTypeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelOperatorTypeValue
            // 
            this.labelOperatorTypeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOperatorTypeValue.Location = new System.Drawing.Point(147, 0);
            this.labelOperatorTypeValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelOperatorTypeValue.Name = "labelOperatorTypeValue";
            this.labelOperatorTypeValue.Size = new System.Drawing.Size(386, 30);
            this.labelOperatorTypeValue.TabIndex = 13;
            this.labelOperatorTypeValue.Text = "labelOperatorTypeValue";
            this.labelOperatorTypeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OperatorPropertiesUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.tableLayoutPanelProperties);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OperatorPropertiesUserControl";
            this.Size = new System.Drawing.Size(541, 371);
            this.tableLayoutPanelProperties.ResumeLayout(false);
            this.tableLayoutPanelProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProperties;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelOperatorTypeTitle;
        private System.Windows.Forms.Label labelOperatorTypeValue;
    }
}
