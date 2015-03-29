﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using JJ.Framework.Presentation.Svg.Models;
using JJ.Framework.Presentation.Svg.Visitors;
using SvgElements = JJ.Framework.Presentation.Svg.Models.Elements;
using JJ.Framework.Presentation.Drawing;
using JJ.Framework.Presentation.Svg;

namespace JJ.Framework.Presentation.WinForms
{
    public partial class DiagramControl : UserControl
    {
        public Diagram Diagram { get; set; }

        private ControlGraphicsBuffer _graphicsBuffer;

        public DiagramControl()
        {
            InitializeComponent();

            _graphicsBuffer = new ControlGraphicsBuffer(this);
        }

        private void DiagramControl_Paint(object sender, PaintEventArgs e)
        {
            if (Diagram == null)
            {
                return;
            }

            Diagram.RootRectangle.Width = Width;
            Diagram.RootRectangle.Height = Height;

            Diagram.Recalculate();

            SvgDrawer.Draw(Diagram, _graphicsBuffer.Graphics);

            _graphicsBuffer.DrawBuffer();
        }
    }
}
