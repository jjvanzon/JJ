﻿using System;
using System.Drawing;
using System.Windows.Forms;
using JJ.Business.Synthesizer;
using JJ.Framework.Presentation.VectorGraphics.Enums;
using JJ.Framework.Presentation.VectorGraphics.EventArg;
using JJ.Business.Synthesizer.Resources;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.VectorGraphics;
using JJ.Presentation.Synthesizer.WinForms.EventArg;
using JJ.Presentation.Synthesizer.VectorGraphics.EventArg;
using JJ.Presentation.Synthesizer.VectorGraphics.Helpers;
using JJ.Presentation.Synthesizer.ViewModels.Items;
using JJ.Framework.Presentation.Resources;
using JJ.Presentation.Synthesizer.WinForms.UserControls.Bases;
using Rectangle = JJ.Framework.Presentation.VectorGraphics.Models.Elements.Rectangle;

namespace JJ.Presentation.Synthesizer.WinForms.UserControls
{
    internal partial class CurveDetailsUserControl : DetailsOrPropertiesUserControlBase
    {
        public event EventHandler<EventArgs<int>> ChangeSelectedNodeTypeRequested;
        public event EventHandler<EventArgs<int>> CreateNodeRequested;
        public event EventHandler<MoveNodeEventArgs> NodeMoving;
        public event EventHandler<MoveNodeEventArgs> NodeMoved;
        public event EventHandler<NodeEventArgs> SelectNodeRequested;
        public event EventHandler<EventArgs<int>> ShowCurvePropertiesRequested;
        public event EventHandler<EventArgs<int>> ShowNodePropertiesRequested;

        /// <summary> Only create after SetCurveManager is called. </summary>
        private CurveDetailsViewModelToDiagramConverter _converter;

        public CurveDetailsUserControl()
        {
            InitializeComponent();

            TitleBarBackColor = SystemColors.Window;
            TitleLabelVisible = false;

            // Make sure the base's button bar is in front of the diagramControl.
            diagramControl.SendToBack();
        }

        private void CurveDetailsUserControl_Load(object sender, EventArgs e)
        {
            diagramControl.Diagram = _converter.Result.Diagram;
        }

        /// <summary>
        /// It is exceptional to pass a business logic object to a UI element,
        /// but the Curve editors use the calculation of the business layer,
        /// in order to plot the curve exactly as used in the sound calculations.
        /// </summary>
        public void SetCurveManager(CurveManager curveManager)
        {
            _converter = new CurveDetailsViewModelToDiagramConverter(
                SystemInformation.DoubleClickTime,
                SystemInformation.DoubleClickSize.Width,
                curveManager);

            _converter.Result.KeyDownGesture.KeyDown += Diagram_KeyDown;
            _converter.Result.SelectNodeGesture.SelectNodeRequested += SelectNodeGesture_NodeSelected;
            _converter.Result.MoveNodeGesture.Moving += MoveNodeGesture_Moving;
            _converter.Result.MoveNodeGesture.Moved += MoveNodeGesture_Moved;
            _converter.Result.ShowCurvePropertiesGesture.ShowCurvePropertiesRequested += ShowCurvePropertiesGesture_ShowCurvePropertiesRequested;
            _converter.Result.ChangeNodeTypeGesture.ChangeNodeTypeRequested += ChangeNodeTypeGesture_ChangeNodeTypeRequested;
            _converter.Result.ShowNodePropertiesMouseGesture.ShowNodePropertiesRequested += ShowNodePropertiesMouseGesture_ShowNodePropertiesRequested;
            _converter.Result.ShowNodePropertiesKeyboardGesture.ShowNodePropertiesRequested += ShowNodePropertiesKeyboardGesture_ShowNodePropertiesRequested;
            _converter.Result.NodeToolTipGesture.ToolTipTextRequested += NodeToolTipGesture_ToolTipTextRequested;
        }

        // Gui

        protected override void SetTitles()
        {
            TitleBarText = CommonResourceFormatter.Details_WithName(ResourceFormatter.Curve);
        }

        protected override void PositionControls()
        {
            base.PositionControls();

            diagramControl.Left = 0;
            diagramControl.Top = 0;
            diagramControl.Width = Width;
            diagramControl.Height = Height;
        }

        // Binding

        public new CurveDetailsViewModel ViewModel
        {
            get => (CurveDetailsViewModel)base.ViewModel;
            set => base.ViewModel = value;
        }

        protected override int GetID() => ViewModel.Curve.ID;

        protected override void ApplyViewModelToControls()
        {
            AssertConverter();

            _converter.Execute(ViewModel);

            diagramControl.Refresh();
        }

        // Events

        private void CurveDetailsUserControl_Resize(object sender, EventArgs e)
        {
            if (ViewModel != null)
            {
                ApplyViewModelToControls();
            }
        }

        private void CurveDetailsUserControl_Paint(object sender, PaintEventArgs e)
        {
            if (ViewModel != null)
            {
                ApplyViewModelToControls();
            }
        }

        private void CurveDetailsUserControl_AddClicked(object sender, EventArgs e)
        {
            CreateNode();

        }

        private void Diagram_KeyDown(object sender, JJ.Framework.Presentation.VectorGraphics.EventArg.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case KeyCodeEnum.Insert:
                    CreateNode();
                    break;

                case KeyCodeEnum.Delete:
                    Delete();
                    break;
            }
        }

        private void SelectNodeGesture_NodeSelected(object sender, ElementEventArgs e)
        {
            if (ViewModel == null) return;
            AssertConverter();

            int nodeID = (int)e.Element.Tag;

            SelectNodeRequested?.Invoke(this, new NodeEventArgs(ViewModel.Curve.ID, nodeID));

            _converter.Result.ShowNodePropertiesKeyboardGesture.SelectedNodeID = nodeID;
        }

        private void MoveNodeGesture_Moving(object sender, ElementEventArgs e)
        {
            if (ViewModel == null) return;
            if (NodeMoving == null) return;
            AssertConverter();

            int nodeID = (int)e.Element.Tag;

            var rectangle = (Rectangle)e.Element;

            float x = rectangle.Position.AbsoluteX + rectangle.Position.Width / 2;
            float y = rectangle.Position.AbsoluteY + rectangle.Position.Height / 2;

            var e2 = new MoveNodeEventArgs(ViewModel.Curve.ID, nodeID, x, y);

            NodeMoving(this, e2);

            ApplyViewModelToControls();

            // TODO: This kind of seems to belong in the ApplyViewModelToControls().
            // Refresh ToolTip Text
            NodeViewModel nodeViewModel = ViewModel.Nodes[nodeID];
            _converter.Result.NodeToolTipGesture.SetToolTipText(nodeViewModel.Caption);
        }

        private void MoveNodeGesture_Moved(object sender, ElementEventArgs e)
        {
            // TODO: Lots of code repetition betwen Moved and Moving events.
            if (ViewModel == null) return;
            if (NodeMoved == null) return;
            AssertConverter();

            int nodeID = (int)e.Element.Tag;

            var rectangle = (Rectangle)e.Element;

            float x = rectangle.Position.AbsoluteX + rectangle.Position.Width / 2;
            float y = rectangle.Position.AbsoluteY + rectangle.Position.Height / 2;

            var e2 = new MoveNodeEventArgs(ViewModel.Curve.ID, nodeID, x, y);

            NodeMoved(this, e2);

            ApplyViewModelToControls();

            // TODO: This kind of seems to belong in the ApplyViewModelToControls().
            // Refresh ToolTip Text
            NodeViewModel nodeViewModel = ViewModel.Nodes[nodeID];
            _converter.Result.NodeToolTipGesture.SetToolTipText(nodeViewModel.Caption);
        }

        private void ShowCurvePropertiesGesture_ShowCurvePropertiesRequested(object sender, EventArgs e)
        {
            ShowCurvePropertiesRequested?.Invoke(this, new EventArgs<int>(ViewModel.Curve.ID));
        }

        private void ChangeNodeTypeGesture_ChangeNodeTypeRequested(object sender, EventArgs e)
        {
            if (ViewModel == null) return;
            ChangeSelectedNodeTypeRequested?.Invoke(this, new EventArgs<int>(ViewModel.Curve.ID));
        }

        private void ShowNodePropertiesMouseGesture_ShowNodePropertiesRequested(object sender, IDEventArgs e)
        {
            ShowNodePropertiesRequested?.Invoke(this, new EventArgs<int>(e.ID));
        }

        private void ShowNodePropertiesKeyboardGesture_ShowNodePropertiesRequested(object sender, IDEventArgs e)
        {
            ShowNodePropertiesRequested?.Invoke(this, new EventArgs<int>(e.ID));
        }

        // TODO: This logic should be in the presenter.
        private void NodeToolTipGesture_ToolTipTextRequested(object sender, ToolTipTextEventArgs e)
        {
            if (ViewModel == null) return;

            int nodeID = VectorGraphicsTagHelper.GetNodeID(e.Element.Tag);

            NodeViewModel nodeViewModel = ViewModel.Nodes[nodeID];

            e.ToolTipText = nodeViewModel.Caption;
        }

        private void CreateNode()
        {
            if (ViewModel == null) return;
            CreateNodeRequested?.Invoke(this, new EventArgs<int>(ViewModel.Curve.ID));
        }

        private void AssertConverter()
        {
            if (_converter == null)
            {
                throw new Exception($"{nameof(_converter)} is null. Call {nameof(SetCurveManager)} first.");
            }
        }
    }
}
