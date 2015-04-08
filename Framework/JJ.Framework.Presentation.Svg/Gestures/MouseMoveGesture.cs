﻿using JJ.Framework.Presentation.Svg.Enums;
using JJ.Framework.Presentation.Svg.EventArg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Framework.Presentation.Svg.Gestures
{
    /// <summary>
    /// WinForms sends a mouse move event,
    /// even when I did not move the mouse,
    /// when in case of a move up.
    /// This gesture will block that and only send
    /// a mouse move event when you actually moved the mouse.
    /// </summary>
    public class MouseMoveGesture : GestureBase
    {
        public event EventHandler<MouseEventArgs> MouseMove;

        private float _previousPointerX;
        private float _previousPointerY;

        public override void HandleMouseDown(object sender, MouseEventArgs e)
        {
            _previousPointerX = e.X;
            _previousPointerY = e.Y;
        }

        public override void HandleMouseMove(object sender, MouseEventArgs e)
        {
            if (MouseMove == null)
            {
                return;
            }

            if (_previousPointerX != e.X ||
                _previousPointerY != e.Y)
            {
                MouseMove(sender, e);
            }

            _previousPointerX = e.X;
            _previousPointerY = e.Y;
        }
    }
}
