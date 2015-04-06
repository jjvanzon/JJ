﻿using JJ.Framework.Presentation.Svg.Enums;
using JJ.Framework.Presentation.Svg.EventArg;
using JJ.Framework.Presentation.Svg.Gestures;
using JJ.Framework.Presentation.Svg.Models.Elements;
using JJ.Framework.Presentation.Svg.Visitors;
using JJ.Framework.Reflection.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JJ.Framework.Presentation.Svg.Models.Elements
{
    public class Diagram
    {
        public Diagram()
        {
            Elements = new DiagramElements(this);

            _canvas = new Rectangle();
            _canvas.Diagram = this;
            _canvas.ZIndex = Int32.MinValue;

            _gestureHandler = new GestureHandler(this);
        }

        private Rectangle _canvas;
        /// <summary> read-only. TODO: Rename to Canvas? </summary>
        [DebuggerHidden]
        public Rectangle Canvas
        {
            get { return _canvas; }
        }

        public DiagramElements Elements { get; private set; }

        public void Recalculate()
        {
             var visitor = new CalculationVisitor();
             _elementsOrderByZIndex = visitor.Execute(this);
        }

        private IList<Element> _elementsOrderByZIndex = new Element[0];
        public IEnumerable<Element> EnumerateElementsByZIndex()
        {
            for (int i = 0; i < _elementsOrderByZIndex.Count; i++)
            {
                yield return _elementsOrderByZIndex[i];
            }
        }

        // Gestures

        private readonly GestureHandler _gestureHandler;

        public void MouseDown(MouseEventArgs e)
        {
            _gestureHandler.MouseDown(e);

            Recalculate();
        }

        public void MouseMove(MouseEventArgs e)
        {
            _gestureHandler.MouseMove(e);

            if (e.MouseButtonEnum != MouseButtonEnum.None)
            {
                Recalculate();
            }
        }

        public void MouseUp(MouseEventArgs e)
        {
            _gestureHandler.MouseUp(e);

            Recalculate();
        }
    }
}
