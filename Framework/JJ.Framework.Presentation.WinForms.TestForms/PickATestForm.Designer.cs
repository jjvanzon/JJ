﻿namespace JJ.Framework.Presentation.WinForms.TestForms
{
    partial class PickATestForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.diagramControl1 = new JJ.Framework.Presentation.WinForms.DiagramControl();
            this.buttonShowHierarchyTestForm = new System.Windows.Forms.Button();
            this.buttonShowSvgWithFlatClone_TestForm = new System.Windows.Forms.Button();
            this.buttonShowSvgWithoutCloning_TestForm = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonShowHelloWorldTestForm = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // diagramControl1
            // 
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramControl1.Location = new System.Drawing.Point(0, 0);
            this.diagramControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(315, 271);
            this.diagramControl1.Diagram = null;
            this.diagramControl1.TabIndex = 0;
            // 
            // buttonShowHierarchyTestForm
            // 
            this.buttonShowHierarchyTestForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowHierarchyTestForm.Location = new System.Drawing.Point(19, 80);
            this.buttonShowHierarchyTestForm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonShowHierarchyTestForm.Name = "buttonShowHierarchyTestForm";
            this.buttonShowHierarchyTestForm.Size = new System.Drawing.Size(277, 50);
            this.buttonShowHierarchyTestForm.TabIndex = 1;
            this.buttonShowHierarchyTestForm.Text = "Show Hierarchy Test";
            this.buttonShowHierarchyTestForm.UseVisualStyleBackColor = true;
            this.buttonShowHierarchyTestForm.Click += new System.EventHandler(this.buttonShowHierarchyTestForm_Click);
            // 
            // buttonShowSvgWithFlatClone_TestForm
            // 
            this.buttonShowSvgWithFlatClone_TestForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowSvgWithFlatClone_TestForm.Location = new System.Drawing.Point(19, 140);
            this.buttonShowSvgWithFlatClone_TestForm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonShowSvgWithFlatClone_TestForm.Name = "buttonShowSvgWithFlatClone_TestForm";
            this.buttonShowSvgWithFlatClone_TestForm.Size = new System.Drawing.Size(277, 50);
            this.buttonShowSvgWithFlatClone_TestForm.TabIndex = 3;
            this.buttonShowSvgWithFlatClone_TestForm.Text = "Show Svg With Flat Clone Test";
            this.buttonShowSvgWithFlatClone_TestForm.UseVisualStyleBackColor = true;
            this.buttonShowSvgWithFlatClone_TestForm.Click += new System.EventHandler(this.buttonShowSvgWithFlatClone_TestForm_Click);
            // 
            // buttonShowSvgWithoutCloning_TestForm
            // 
            this.buttonShowSvgWithoutCloning_TestForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowSvgWithoutCloning_TestForm.Location = new System.Drawing.Point(19, 200);
            this.buttonShowSvgWithoutCloning_TestForm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonShowSvgWithoutCloning_TestForm.Name = "buttonShowSvgWithoutCloning_TestForm";
            this.buttonShowSvgWithoutCloning_TestForm.Size = new System.Drawing.Size(277, 51);
            this.buttonShowSvgWithoutCloning_TestForm.TabIndex = 4;
            this.buttonShowSvgWithoutCloning_TestForm.Text = "Show Svg Without Cloning Test";
            this.buttonShowSvgWithoutCloning_TestForm.UseVisualStyleBackColor = true;
            this.buttonShowSvgWithoutCloning_TestForm.Click += new System.EventHandler(this.buttonShowSvgWithoutCloning_TestForm_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.buttonShowSvgWithoutCloning_TestForm, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonShowHierarchyTestForm, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonShowHelloWorldTestForm, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonShowSvgWithFlatClone_TestForm, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(15);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(315, 271);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // buttonShowHelloWorldTestForm
            // 
            this.buttonShowHelloWorldTestForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowHelloWorldTestForm.Location = new System.Drawing.Point(19, 20);
            this.buttonShowHelloWorldTestForm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonShowHelloWorldTestForm.Name = "buttonShowHelloWorldTestForm";
            this.buttonShowHelloWorldTestForm.Size = new System.Drawing.Size(277, 50);
            this.buttonShowHelloWorldTestForm.TabIndex = 2;
            this.buttonShowHelloWorldTestForm.Text = "Show Hello World Test";
            this.buttonShowHelloWorldTestForm.UseVisualStyleBackColor = true;
            this.buttonShowHelloWorldTestForm.Click += new System.EventHandler(this.buttonShowHelloWorldTestForm_Click);
            // 
            // PickATestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 271);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.diagramControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PickATestForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DiagramControl diagramControl1;
        private System.Windows.Forms.Button buttonShowHierarchyTestForm;
        private System.Windows.Forms.Button buttonShowSvgWithFlatClone_TestForm;
        private System.Windows.Forms.Button buttonShowSvgWithoutCloning_TestForm;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonShowHelloWorldTestForm;
    }
}

