﻿using System;
using System.Collections.Generic;

namespace JJ.Data.Synthesizer
{
    /// <summary>
    /// Does not have much functional use yet.
    /// </summary>
    public class Patch
    {
        public Patch()
        {
            Operators = new List<Operator>();
        }

        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Operator> Operators { get; set; }

        /// <summary> nullable </summary>
        public virtual Document Document { get; set; }
    }
}
