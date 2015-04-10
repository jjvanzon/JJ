﻿using JJ.Framework.Presentation.Svg.Models.Elements;
using JJ.Framework.Reflection.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.Synthesizer.Svg.EventArg
{
    public class ToolTipTextEventArgs : EventArgs
    {
        public Element Element { get; private set; }
        public string ToolTipText { get; set; }

        public ToolTipTextEventArgs(Element element)
        {
            if (element == null) throw new NullException(() => element);

            Element = element;
        }
    }
}
