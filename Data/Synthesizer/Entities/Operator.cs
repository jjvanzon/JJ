﻿using System.Collections.Generic;
using System.Diagnostics;
using JJ.Data.Synthesizer.Helpers;

// ReSharper disable VirtualMemberCallInConstructor

namespace JJ.Data.Synthesizer.Entities
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
    public class Operator
    {
        public Operator()
        {
            Inlets = new List<Inlet>();
            Outlets = new List<Outlet>();
        }

        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        /// <summary> 
        /// Nullable.
        /// OperatorTypeEnum can also be determined by UnderlyingPatch.
        /// Use GetOperatorTypeEnum to get a single value derived from both OperatorType or UnderlyingPatch.
        /// </summary>
        public virtual OperatorType OperatorType { get; set; }
        /// <summary> parent </summary>
        public virtual Patch Patch { get; set; }
        public virtual Dimension StandardDimension { get; set; }
        public virtual string CustomDimensionName { get; set; }
        public virtual string Data { get; set; }
        public virtual IList<Inlet> Inlets { get; set; }
        public virtual IList<Outlet> Outlets { get; set; }
        /// <summary> nullable (for now) </summary>
        public virtual Patch UnderlyingPatch { get; set; }

        private string DebuggerDisplay => DebuggerDisplayFormatter.GetDebuggerDisplay(this);
    }
}
