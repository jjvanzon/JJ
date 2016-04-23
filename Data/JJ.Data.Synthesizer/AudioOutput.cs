﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Data.Synthesizer
{
    public class AudioOutput
    {
        public virtual int ID { get; set; }
        public virtual SpeakerSetup SpeakerSetup { get; set; }
        public virtual int SamplingRate { get; set; }
        public virtual int MaxConcurrentNotes { get; set; }
        public virtual double BufferDuration { get; set; }
        
        // No parent reference to document, 
        // because 1-to-1 inverse property are a disaster in NHibernate.
    }
}
