﻿using System.Diagnostics;
using JJ.Data.Synthesizer.Helpers;

namespace JJ.Data.Synthesizer
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Node
    {
        public virtual int ID { get; set; }
        public virtual double X { get; set; }
        public virtual double Y { get; set; }

        /// <summary> Currently not used. (2015-11-16) </summary>
        public virtual double Direction { get; set; }
        public virtual NodeType NodeType { get; set; }

        /// <summary> parent </summary>
        public virtual Curve Curve { get; set; }

        private string DebuggerDisplay
        {
            get { return DebugHelper.GetDebuggerDisplay(this); }
        }
    }
}
