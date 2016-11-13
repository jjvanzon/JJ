﻿namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    partial class PatchDetailsUserControl
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

            UnbindVectorGraphicsEvents();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.diagramControl1 = new JJ.Framework.Presentation.WinForms.Controls.DiagramControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.tableLayoutPanelToolboxAndPatch = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelToolboxAndPatch.SuspendLayout();
            this.SuspendLayout();
            // 
            // diagramControl1
            // 
            this.diagramControl1.BackColor = System.Drawing.SystemColors.Window;
            this.diagramControl1.Diagram = null;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramControl1.Location = new System.Drawing.Point(285, 0);
            this.diagramControl1.Margin = new System.Windows.Forms.Padding(0);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.Size = new System.Drawing.Size(513, 275);
            this.diagramControl1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Font = new System.Drawing.Font("Verdana", 8F);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(19, 19);
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(285, 275);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // buttonPlay
            // 
            this.buttonPlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPlay.FlatAppearance.BorderSize = 0;
            this.buttonPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPlay.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPlay.Location = new System.Drawing.Point(0, 275);
            this.buttonPlay.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(285, 24);
            this.buttonPlay.TabIndex = 3;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // tableLayoutPanelToolboxAndPatch
            // 
            this.tableLayoutPanelToolboxAndPatch.ColumnCount = 2;
            this.tableLayoutPanelToolboxAndPatch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 285F));
            this.tableLayoutPanelToolboxAndPatch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelToolboxAndPatch.Controls.Add(this.buttonPlay, 0, 1);
            this.tableLayoutPanelToolboxAndPatch.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanelToolboxAndPatch.Controls.Add(this.diagramControl1, 1, 0);
            this.tableLayoutPanelToolboxAndPatch.Location = new System.Drawing.Point(0, 26);
            this.tableLayoutPanelToolboxAndPatch.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelToolboxAndPatch.Name = "tableLayoutPanelToolboxAndPatch";
            this.tableLayoutPanelToolboxAndPatch.RowCount = 2;
            this.tableLayoutPanelToolboxAndPatch.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelToolboxAndPatch.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelToolboxAndPatch.Size = new System.Drawing.Size(713, 299);
            this.tableLayoutPanelToolboxAndPatch.TabIndex = 2;
            // 
            // PatchDetailsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.tableLayoutPanelToolboxAndPatch);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PatchDetailsUserControl";
            this.Size = new System.Drawing.Size(713, 325);
            this.tableLayoutPanelToolboxAndPatch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.Presentation.WinForms.Controls.DiagramControl diagramControl1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelToolboxAndPatch;
    }
}