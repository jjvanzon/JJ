﻿using JJ.Presentation.Synthesizer.WinForms.UserControls.Partials;
namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
	partial class DocumentTreeUserControl
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.titleBarUserControl = new JJ.Presentation.Synthesizer.WinForms.UserControls.Partials.TitleBarUserControl();
            this.treeView = new System.Windows.Forms.TreeView();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.titleBarUserControl, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.treeView, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(333, 494);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // titleBarUserControl
            // 
            this.titleBarUserControl.AddButtonVisible = false;
            this.titleBarUserControl.AddToInstrumentButtonVisible = true;
            this.titleBarUserControl.BackColor = System.Drawing.SystemColors.Control;
            this.titleBarUserControl.BrowseButtonVisible = true;
            this.titleBarUserControl.CloseButtonVisible = true;
            this.titleBarUserControl.DeleteButtonVisible = false;
            this.titleBarUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleBarUserControl.ExpandButtonVisible = false;
            this.titleBarUserControl.Location = new System.Drawing.Point(0, 0);
            this.titleBarUserControl.Margin = new System.Windows.Forms.Padding(0);
            this.titleBarUserControl.Name = "titleBarUserControl";
            this.titleBarUserControl.NewButtonVisible = true;
            this.titleBarUserControl.PlayButtonVisible = true;
            this.titleBarUserControl.RedoButtonVisible = true;
            this.titleBarUserControl.RefreshButtonVisible = true;
            this.titleBarUserControl.RenameButtonVisible = true;
            this.titleBarUserControl.SaveButtonVisible = true;
            this.titleBarUserControl.Size = new System.Drawing.Size(333, 24);
            this.titleBarUserControl.TabIndex = 3;
            this.titleBarUserControl.TreeStructureButtonVisible = true;
            this.titleBarUserControl.UndoButtonVisible = true;
            this.titleBarUserControl.AddToInstrumentClicked += new System.EventHandler(this.TitleBarUserControl_AddToInstrumentClicked);
            this.titleBarUserControl.BrowseClicked += new System.EventHandler(this.TitleBarUserControl_BrowseClicked);
            this.titleBarUserControl.CloseClicked += new System.EventHandler(this.TitleBarUserControl_CloseClicked);
            this.titleBarUserControl.DeleteClicked += new System.EventHandler(this.TitleBarUserControl_DeleteClicked);
            this.titleBarUserControl.ExpandClicked += new System.EventHandler(this.TitleBarUserControl_OpenClicked);
            this.titleBarUserControl.NewClicked += new System.EventHandler(this.TitleBarUserControl_NewClicked);
            this.titleBarUserControl.PlayClicked += new System.EventHandler(this.TitleBarUserControl_PlayClicked);
            this.titleBarUserControl.RedoClicked += new System.EventHandler(this.TitleBarUserControl_RedoClicked);
            this.titleBarUserControl.RefreshClicked += new System.EventHandler(this.TitleBarUserControl_RefreshClicked);
            this.titleBarUserControl.RenameClicked += new System.EventHandler(this.TitleBarUserControl_RenameClicked);
            this.titleBarUserControl.SaveClicked += new System.EventHandler(this.TitleBarUserControl_SaveClicked);
            this.titleBarUserControl.TreeStructureClicked += new System.EventHandler(this.TitleBarUserControl_TreeStructureClicked);
            this.titleBarUserControl.UndoClicked += new System.EventHandler(this.TitleBarUserControl_UndoClicked);
            // 
            // treeView
            // 
            this.treeView.BackColor = System.Drawing.SystemColors.Control;
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(0, 26);
            this.treeView.Margin = new System.Windows.Forms.Padding(0);
            this.treeView.Name = "treeView";
            this.treeView.ShowLines = false;
            this.treeView.Size = new System.Drawing.Size(333, 468);
            this.treeView.TabIndex = 4;
            this.treeView.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.TreeView_NodeMouseHover);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
            this.treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_NodeMouseDoubleClick);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 100000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100000;
            this.toolTip.UseAnimation = false;
            this.toolTip.UseFading = false;
            // 
            // DocumentTreeUserControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "DocumentTreeUserControl";
            this.Size = new System.Drawing.Size(333, 494);
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private TitleBarUserControl titleBarUserControl;
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.ToolTip toolTip;
	}
}
