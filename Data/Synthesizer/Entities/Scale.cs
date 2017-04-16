﻿using System.Collections.Generic;
using System.Diagnostics;
using JJ.Data.Synthesizer.Helpers;

// ReSharper disable VirtualMemberCallInConstructor

namespace JJ.Data.Synthesizer.Entities
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
    public class Scale
    {
        public Scale()
        {
            Tones = new List<Tone>();
        }

        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual Document Document { get; set; }
        public virtual ScaleType ScaleType { get; set; }
        public virtual double? BaseFrequency { get; set; }
        public virtual IList<Tone> Tones { get; set; }

        private string DebuggerDisplay => DebugHelper.GetDebuggerDisplay(this);
    }
}