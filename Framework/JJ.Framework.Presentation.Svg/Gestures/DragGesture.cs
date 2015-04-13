﻿using JJ.Framework.Presentation.Svg.EventArg;
using JJ.Framework.Presentation.Svg.Models.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Framework.Presentation.Svg.Gestures
{
    public class DragGesture : GestureBase
    {
        ~DragGesture()
        {
            if (_canvasMouseMoveGesture != null)
            {
                if (_diagram != null)
                {
                    _diagram.Canvas.Gestures.Remove(_canvasMouseMoveGesture);
                }
                _canvasMouseMoveGesture.MouseMove -= _canvas_MouseMove;
            }

            if (_canvasMouseUpGesture != null)
            {
                if (_diagram != null)
                {
                    _diagram.Canvas.Gestures.Remove(_canvasMouseUpGesture);
                }
                _canvasMouseUpGesture.MouseUp -= _canvas_MouseUp;
            }
        }

        public event EventHandler<DraggingEventArgs> Dragging;
        public event EventHandler DragCancelled;

        public Element DraggedElement { get; private set; }

        private Diagram _diagram;
        private MouseMoveGesture _canvasMouseMoveGesture;
        private MouseUpGesture _canvasMouseUpGesture;

        public override void HandleMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Element != null)
            {
                DraggedElement = e.Element;

                _diagram = DraggedElement.Diagram;
                if (_diagram != null)
                {
                    _canvasMouseMoveGesture = new MouseMoveGesture();
                    _canvasMouseMoveGesture.MouseMove += _canvas_MouseMove;
                    _diagram.Canvas.Gestures.Add(_canvasMouseMoveGesture);

                    _canvasMouseUpGesture = new MouseUpGesture();
                    _canvasMouseUpGesture.MouseUp += _canvas_MouseUp;
                    _diagram.Canvas.Gestures.Add(_canvasMouseUpGesture);
                }
            }
        }

        public override void HandleMouseMove(object sender, MouseEventArgs e)
        {
            if (DraggedElement != null)
            {
                if (Dragging != null)
                {
                    Dragging(sender, new DraggingEventArgs(DraggedElement, e.X, e.Y));
                }
            }
        }

        private void _canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (DraggedElement != null)
            {
                if (Dragging != null)
                {
                    Dragging(sender, new DraggingEventArgs(DraggedElement, e.X, e.Y));
                }
            }
        }

        public override void HandleMouseUp(object sender, MouseEventArgs e)
        {
            if (DragCancelled != null)
            {
                DragCancelled(sender, EventArgs.Empty);
            }

            DraggedElement = null;
        }

        private void _canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (DraggedElement != null)
            {
                DraggedElement = null;

                if (DragCancelled != null)
                {
                    DragCancelled(sender, EventArgs.Empty);
                }
            }
        }
    }
}
