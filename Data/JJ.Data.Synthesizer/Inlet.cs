﻿using JJ.Data.Synthesizer.Helpers;
using System.Diagnostics;

namespace JJ.Data.Synthesizer
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Inlet
    {
        public virtual int ID { get; set; }

        /// <summary> Optional. Currently (2105-11-05) only relevant for CustomOperators. </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// This number is often used as a key to a specific inlet within an operator.
        /// 'Name' is another alternative key, which is currently only used for CustomOperators (2015-11-13).
        /// </summary>
        public virtual int ListIndex { get; set; }

        /// <summary> parent </summary>
        public virtual Operator Operator { get; set; }

        /// <summary> nullable </summary>
        public virtual Outlet InputOutlet { get; set; }

        private string DebuggerDisplay
        {
            get { return DebugHelper.GetDebuggerDisplay(this); }
        }
    }
}
