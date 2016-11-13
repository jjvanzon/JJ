﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Presentation.VectorGraphics.Enums;
using JJ.Framework.Presentation.VectorGraphics.EventArg;
using JJ.Framework.Presentation.VectorGraphics.Gestures;
using JJ.Presentation.Synthesizer.VectorGraphics.EventArg;

namespace JJ.Presentation.Synthesizer.VectorGraphics.Gestures
{
    /// <summary>
    /// Keyboard gesture must be separated from the mouse gesture,
    /// because keyword gesture is tied to the whole diagram,
    /// while mouse gesture is tied to a specific element.
    /// </summary>
    public class ShowOperatorPropertiesKeyboardGesture : GestureBase
    {
        public EventHandler<IDEventArgs> ShowOperatorPropertiesRequested;

        public int SelectedOperatorID { get; set; }

        protected override void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == KeyCodeEnum.Enter)
            {
                ShowOperatorPropertiesRequested?.Invoke(sender, new IDEventArgs(SelectedOperatorID));
            }
        }
    }
}