﻿using System;
using JJ.Framework.Presentation.VectorGraphics.EventArg;
using JJ.Framework.Presentation.VectorGraphics.Gestures;

namespace JJ.Presentation.Synthesizer.VectorGraphics.Gestures
{
    public class ShowPatchPropertiesGesture : GestureBase
    {
        public event EventHandler ShowPatchPropertiesRequested;

        private readonly DoubleClickGesture _doubleClickGesture;

        internal ShowPatchPropertiesGesture(int doubleClickSpeedInMilliseconds, int doubleClickDeltaInPixels)
        {
            _doubleClickGesture = new DoubleClickGesture(doubleClickSpeedInMilliseconds, doubleClickDeltaInPixels);
            _doubleClickGesture.DoubleClick += _doubleClickGesture_DoubleClick;
        }

        ~ShowPatchPropertiesGesture()
        {
            if (_doubleClickGesture != null)
            {
                _doubleClickGesture.DoubleClick -= _doubleClickGesture_DoubleClick;
            }
        }

        protected override void HandleMouseDown(object sender, MouseEventArgs e)
        {
            _doubleClickGesture.Internals.HandleMouseDown(sender, e);
        }

        protected override void HandleMouseMove(object sender, MouseEventArgs e)
        {
            _doubleClickGesture.Internals.HandleMouseMove(sender, e);
        }

        protected override void HandleMouseUp(object sender, MouseEventArgs e)
        {
            _doubleClickGesture.Internals.HandleMouseUp(sender, e);
        }

        private void _doubleClickGesture_DoubleClick(object sender, ElementEventArgs e)
        {
            ShowPatchPropertiesRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}