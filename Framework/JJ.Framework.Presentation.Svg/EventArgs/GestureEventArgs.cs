﻿using JJ.Framework.Presentation.Svg.Gestures;
using JJ.Framework.Presentation.Svg.Models.Elements;
using JJ.Framework.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Framework.Presentation.Svg.EventArgs
{
    public class GestureEventArgs : System.EventArgs
    {
        public GestureEventArgs(IGesture gesture, ElementBase element)
        {
            // TODO: Enable null check again.
            //if (gesture == null) throw new NullException(() => gesture);
            if (element == null) throw new NullException(() => element);

            Gesture = gesture;
            Element = element;
        }

        public IGesture Gesture { get; private set; }
        public ElementBase Element { get; private set; }
    }
}
