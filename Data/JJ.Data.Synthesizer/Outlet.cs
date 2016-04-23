﻿using JJ.Data.Synthesizer.Helpers;
using System.Collections.Generic;
using System.Diagnostics;

namespace JJ.Data.Synthesizer
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Outlet
    {
        public Outlet()
        {
            ConnectedInlets = new List<Inlet>();
            InAudioFileOutputs = new List<AudioFileOutput>();
        }

        public virtual int ID { get; set; }

        /// <summary> optional </summary>
        public virtual string Name { get; set; }

        /// <summary> nullable </summary>
        public virtual Dimension Dimension { get; set; }

        /// <summary> This number is often used as a key to a specific outlet within an operator. 'Name' is another alternative key. </summary>
        public virtual int ListIndex { get; set; }

        /// <summary> parent </summary>
        public virtual Operator Operator { get; set; }

        public virtual IList<Inlet> ConnectedInlets { get; set; }
        public virtual IList<AudioFileOutput> InAudioFileOutputs { get; set; }

        private string DebuggerDisplay
        {
            get { return DebugHelper.GetDebuggerDisplay(this); }
        }
    }
}