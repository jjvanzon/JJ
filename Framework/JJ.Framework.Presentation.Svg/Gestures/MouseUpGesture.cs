﻿using JJ.Framework.Presentation.Svg.EventArg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Framework.Presentation.Svg.Gestures
{
    public class MouseUpGesture : GestureBase
    {
        public event EventHandler<MouseEventArgs> MouseUp;

        public override void HandleMouseUp(object sender, MouseEventArgs e)
        {
            if (MouseUp != null)
            {
                MouseUp(sender, e);
            }
        }
    }
}
