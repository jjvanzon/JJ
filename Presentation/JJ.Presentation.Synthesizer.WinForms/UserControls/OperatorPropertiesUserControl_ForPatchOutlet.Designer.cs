﻿using JJ.Presentation.Synthesizer.WinForms.UserControls.Partials;
namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    partial class OperatorPropertiesUserControl_ForPatchOutlet
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
            this.labelNumber = new System.Windows.Forms.Label();
            this.numericUpDownNumber = new System.Windows.Forms.NumericUpDown();
            this.labelDimension = new System.Windows.Forms.Label();
            this.comboBoxDimension = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this._labelName.TabIndex = 6;
            this._labelName.Text = "Name";
            // 
            // labelOperatorTypeTitle
            // 
            this._labelOperatorTypeTitle.TabIndex = 4;
            this._labelOperatorTypeTitle.Text = "Type:";
            // 
            // labelOperatorTypeValue
            // 
            this._labelOperatorTypeValue.TabIndex = 5;
            // 
            // textBoxCustomDimensionName
            // 
            this._textBoxCustomDimensionName.TabIndex = 11;
            // 
            // labelStandardDimension
            // 
            this._labelStandardDimension.TabIndex = 8;
            this._labelStandardDimension.Text = "Standard Dimension";
            // 
            // comboBoxStandardDimension
            // 
            this._comboBoxStandardDimension.TabIndex = 9;
            // 
            // labelCustomDimensionName
            // 
            this._labelCustomDimensionName.TabIndex = 10;
            this._labelCustomDimensionName.Text = "Custom Dimension";
            // 
            // textBoxName
            // 
            this._textBoxName.TabIndex = 7;
            // 
            // labelNumber
            // 
            this.labelNumber.Location = new System.Drawing.Point(0, 59);
            this.labelNumber.Margin = new System.Windows.Forms.Padding(0);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(147, 30);
            this.labelNumber.TabIndex = 14;
            this.labelNumber.Text = "labelNumber";
            this.labelNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownNumber
            // 
            this.numericUpDownNumber.Location = new System.Drawing.Point(147, 59);
            this.numericUpDownNumber.Margin = new System.Windows.Forms.Padding(0);
            this.numericUpDownNumber.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numericUpDownNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownNumber.Name = "numericUpDownNumber";
            this.numericUpDownNumber.Size = new System.Drawing.Size(13, 22);
            this.numericUpDownNumber.TabIndex = 18;
            this.numericUpDownNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelDimension
            // 
            this.labelDimension.Location = new System.Drawing.Point(76, 190);
            this.labelDimension.Margin = new System.Windows.Forms.Padding(0);
            this.labelDimension.Name = "labelDimension";
            this.labelDimension.Size = new System.Drawing.Size(147, 30);
            this.labelDimension.TabIndex = 40;
            this.labelDimension.Text = "labelDimension";
            this.labelDimension.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxDimension
            // 
            this.comboBoxDimension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDimension.FormattingEnabled = true;
            this.comboBoxDimension.Location = new System.Drawing.Point(239, 200);
            this.comboBoxDimension.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxDimension.Name = "comboBoxDimension";
            this.comboBoxDimension.Size = new System.Drawing.Size(353, 24);
            this.comboBoxDimension.TabIndex = 39;
            // 
            // OperatorPropertiesUserControl_ForPatchOutlet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.labelDimension);
            this.Controls.Add(this.comboBoxDimension);
            this.Controls.Add(this.labelNumber);
            this.Controls.Add(this.numericUpDownNumber);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OperatorPropertiesUserControl_ForPatchOutlet";
            this.Size = new System.Drawing.Size(669, 414);
            this.TitleBarText = "Operator Properties";
            this.Controls.SetChildIndex(this._textBoxName, 0);
            this.Controls.SetChildIndex(this._labelOperatorTypeValue, 0);
            this.Controls.SetChildIndex(this._labelOperatorTypeTitle, 0);
            this.Controls.SetChildIndex(this._labelName, 0);
            this.Controls.SetChildIndex(this._textBoxCustomDimensionName, 0);
            this.Controls.SetChildIndex(this._labelStandardDimension, 0);
            this.Controls.SetChildIndex(this._comboBoxStandardDimension, 0);
            this.Controls.SetChildIndex(this._labelCustomDimensionName, 0);
            this.Controls.SetChildIndex(this.numericUpDownNumber, 0);
            this.Controls.SetChildIndex(this.labelNumber, 0);
            this.Controls.SetChildIndex(this.comboBoxDimension, 0);
            this.Controls.SetChildIndex(this.labelDimension, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelNumber;
        private System.Windows.Forms.NumericUpDown numericUpDownNumber;
        protected System.Windows.Forms.Label labelDimension;
        protected System.Windows.Forms.ComboBox comboBoxDimension;
    }
}
