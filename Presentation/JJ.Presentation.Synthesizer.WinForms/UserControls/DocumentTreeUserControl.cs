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
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Presentation.Resources;
using JJ.Presentation.Synthesizer.WinForms.EventArg;
using JJ.Presentation.Synthesizer.WinForms.Forms;
using JJ.Business.CanonicalModel;
using JJ.Presentation.Synthesizer.ViewModels.Partials;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using JJ.Presentation.Synthesizer.Resources;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    internal partial class DocumentTreeUserControl : UserControl
    {
        public event EventHandler CloseRequested;
        public event EventHandler<Int32EventArgs> DocumentPropertiesRequested;
        public event EventHandler<Int32EventArgs> ExpandNodeRequested;
        public event EventHandler<Int32EventArgs> CollapseNodeRequested;
        public event EventHandler ShowInstrumentsRequested;
        public event EventHandler ShowEffectsRequested;
        public event EventHandler ShowSamplesRequested;
        public event EventHandler ShowCurvesRequested;
        public event EventHandler ShowPatchesRequested;
        public event EventHandler ShowAudioFileOutputsRequested;

        /// <summary> virtually not nullable </summary>
        private DocumentTreeViewModel _viewModel;

        private TreeNode _instrumentsTreeNode;
        private TreeNode _effectsTreeNode;
        private TreeNode _samplesTreeNode;
        private TreeNode _curvesTreeNode;
        private TreeNode _patchesTreeNode;
        private TreeNode _audioFileOutputsTreeNode;

        public DocumentTreeUserControl()
        {
            InitializeComponent();
            SetTitles();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DocumentTreeViewModel ViewModel
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
            titleBarUserControl.Text = Titles.DocumentTree;
            propertiesToolStripMenuItemDocumentProperties.Text = "&" + CommonTitles.Properties;
        }

        private bool applyViewModelIsBusy = false;

        private void ApplyViewModel()
        {
            applyViewModelIsBusy = true;

            treeView.SuspendLayout();

            treeView.Nodes.Clear();

            if (_viewModel == null)
            {
                treeView.ResumeLayout();
                return;
            }

            var documentTreeNode = new TreeNode(_viewModel.Name);
            // Temporarily (2015-05-25) outcommented to make isChildDocumentNode work (see further down in the code).
            //documentTreeNode.Tag = _viewModel.ID;
            documentTreeNode.ContextMenuStrip = contextMenuStripDocument;
            treeView.Nodes.Add(documentTreeNode);

            AddChildNodesRecursive(documentTreeNode, _viewModel);

            // TODO: Uncomment when the referenced documents functionality is programmed.
            //var referencedDocumentsTreeNode = new TreeNode(PropertyDisplayNames.ReferencedDocuments);
            //documentTreeNode.Nodes.Add(referencedDocumentsTreeNode);

            //foreach (ReferencedDocumentViewModel referencedDocumentViewModel in _viewModel.ReferencedDocuments.List)
            //{
            //    var referencedDocumentTreeNode = new TreeNode(referencedDocumentViewModel.Name);
            //    referencedDocumentTreeNode.Tag = referencedDocumentViewModel.Keys.ID;
            //    referencedDocumentsTreeNode.Nodes.Add(referencedDocumentTreeNode);

            //    foreach (IDAndName instrumentViewModel in referencedDocumentViewModel.Instruments)
            //    {
            //        var instrumentTreeNode = new TreeNode(instrumentViewModel.Name);
            //        instrumentTreeNode.Tag = instrumentViewModel.ID;
            //        referencedDocumentTreeNode.Nodes.Add(instrumentTreeNode);
            //    }

            //    foreach (IDAndName effectViewModel in referencedDocumentViewModel.Effects)
            //    {
            //        var effectTreeNode = new TreeNode(effectViewModel.Name);
            //        effectTreeNode.Tag = effectViewModel.ID;
            //        referencedDocumentTreeNode.Nodes.Add(effectTreeNode);
            //    }

            //    referencedDocumentTreeNode.Expand();
            //}

            //referencedDocumentsTreeNode.Expand();

            documentTreeNode.Expand();

            treeView.ResumeLayout();

            applyViewModelIsBusy = false;
        }

        private void AddChildNodesRecursive(TreeNode parentNode, DocumentTreeViewModel parentViewModel)
        {
            _instrumentsTreeNode = new TreeNode(PropertyDisplayNames.Instruments);
            parentNode.Nodes.Add(_instrumentsTreeNode);

            foreach (ChildDocumentTreeNodeViewModel instrumentViewModel in parentViewModel.Instruments)
            {
                var instrumentTreeNode = new TreeNode(instrumentViewModel.Name);
                instrumentTreeNode.Tag = instrumentViewModel.Keys.NodeIndex;
                _instrumentsTreeNode.Nodes.Add(instrumentTreeNode);

                AddChildDocumentChildNodesRecursive(instrumentTreeNode, instrumentViewModel);

                if (instrumentViewModel.IsExpanded)
                {
                    instrumentTreeNode.Expand();
                }
                else
                {
                    instrumentTreeNode.Collapse();
                }
            }

            _instrumentsTreeNode.Expand();

            _effectsTreeNode = new TreeNode(PropertyDisplayNames.Effects);
            parentNode.Nodes.Add(_effectsTreeNode);

            foreach (ChildDocumentTreeNodeViewModel effectViewModel in parentViewModel.Effects)
            {
                var effectTreeNode = new TreeNode(effectViewModel.Name);
                effectTreeNode.Tag = effectViewModel.Keys.NodeIndex;
                _effectsTreeNode.Nodes.Add(effectTreeNode);

                AddChildDocumentChildNodesRecursive(effectTreeNode, effectViewModel);

                if (effectViewModel.IsExpanded)
                {
                    effectTreeNode.Expand();
                }
                else
                {
                    effectTreeNode.Collapse();
                }
            }

            _effectsTreeNode.Expand();

            _samplesTreeNode = new TreeNode(PropertyDisplayNames.Samples);
            parentNode.Nodes.Add(_samplesTreeNode);

            _curvesTreeNode = new TreeNode(PropertyDisplayNames.Curves);
            parentNode.Nodes.Add(_curvesTreeNode);

            _patchesTreeNode = new TreeNode(PropertyDisplayNames.Patches);
            parentNode.Nodes.Add(_patchesTreeNode);

            _audioFileOutputsTreeNode = new TreeNode(PropertyDisplayNames.AudioFileOutputs);
            parentNode.Nodes.Add(_audioFileOutputsTreeNode);
        }

        private void AddChildDocumentChildNodesRecursive(TreeNode parentNode, ChildDocumentTreeNodeViewModel parentViewModel)
        {
            var samplesTreeNode = new TreeNode(PropertyDisplayNames.Samples);
            parentNode.Nodes.Add(samplesTreeNode);

            var curvesTreeNode = new TreeNode(PropertyDisplayNames.Curves);
            parentNode.Nodes.Add(curvesTreeNode);

            var patchesTreeNode = new TreeNode(PropertyDisplayNames.Patches);
            parentNode.Nodes.Add(patchesTreeNode);
        }

        // Actions

        private void Close()
        {
            if (CloseRequested != null)
            {
                CloseRequested(this, EventArgs.Empty);
            }
        }

        private void ShowDocumentProperties()
        {
            if (DocumentPropertiesRequested != null)
            {
                DocumentPropertiesRequested(this, new Int32EventArgs(_viewModel.ID)); // TODO: At some point I am going to have to get it from the TreeNode.Tag, instead of the ViewModel.
            }
        }

        // Events

        private void titleBarUserControl_CloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDocumentProperties();
        }

        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (applyViewModelIsBusy)
            {
                return;
            }

            int nodeIndex;
            // TODO: This is a bad assumption. Refactor that later.
            bool isChildDocumentNode = Int32.TryParse(Convert.ToString(e.Node.Tag), out nodeIndex);
            if (isChildDocumentNode)
            {
                if (ExpandNodeRequested != null)
                {
                    ExpandNodeRequested(this, new Int32EventArgs(nodeIndex));
                }
            }
        }

        private void treeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (applyViewModelIsBusy)
            {
                return;
            }

            int nodeIndex;
            // TODO: This is a bad assumption. Refactor that later.
            bool isChildDocumentNode = Int32.TryParse(Convert.ToString(e.Node.Tag), out nodeIndex);
            if (isChildDocumentNode)
            {
                if (CollapseNodeRequested != null)
                {
                    CollapseNodeRequested(this, new Int32EventArgs(nodeIndex));
                }
            }
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == _instrumentsTreeNode)
            {
                if (ShowInstrumentsRequested != null)
                {
                    ShowInstrumentsRequested(this, EventArgs.Empty);
                }
            }

            if (e.Node == _effectsTreeNode)
            {
                if (ShowEffectsRequested != null)
                {
                    ShowEffectsRequested(this, EventArgs.Empty);
                }
            }

            if (e.Node == _samplesTreeNode)
            {
                if (ShowSamplesRequested != null)
                {
                    ShowSamplesRequested(this, EventArgs.Empty);
                }
            }

            if (e.Node == _curvesTreeNode)
            {
                if (ShowCurvesRequested != null)
                {
                    ShowCurvesRequested(this, EventArgs.Empty);
                }
            }

            if (e.Node == _patchesTreeNode)
            {
                if (ShowPatchesRequested != null)
                {
                    ShowPatchesRequested(this, EventArgs.Empty);
                }
            }
            
            if (e.Node == _audioFileOutputsTreeNode)
            {
                if (ShowAudioFileOutputsRequested != null)
                {
                    ShowAudioFileOutputsRequested(this, EventArgs.Empty);
                }
            }
        }
    }
}
