﻿using JJ.Framework.Presentation.Svg.EventArg;
using JJ.Framework.Presentation.Svg.Models.Elements;
using JJ.Framework.Reflection.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Framework.Presentation.Svg.Gestures
{
    public class DropGesture : GestureBase
    {
        public event EventHandler<DroppedEventArgs> Dropped;

        private IList<DragGesture> _dragGestures;

        public DropGesture(params DragGesture[] dragGestures)
            : this((IList<DragGesture>)dragGestures)
        { }

        public DropGesture(IList<DragGesture> dragGestures)
        {
            if (dragGestures == null) throw new NullException(() => dragGestures);

            _dragGestures = dragGestures;
        }

        public override void FireMouseDown(object sender, MouseEventArgs e)
        { }

        public override void FireMouseMove(object sender, MouseEventArgs e)
        { }

        public override void FireMouseUp(object sender, MouseEventArgs e)
        {
            foreach (DragGesture dragGesture in _dragGestures)
            {
                if (dragGesture.DraggedElement != null)
                {
                    if (e.Element != null)
                    {
                        if (Dropped != null)
                        {
                            Dropped(sender, new DroppedEventArgs(dragGesture.DraggedElement, e.Element));
                        }
                    }
                }
            }
        }
    }
}
