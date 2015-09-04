﻿using System.Collections.Generic;

namespace JJ.Data.Synthesizer
{
    public class SpeakerSetup
    {
        public SpeakerSetup()
        {
            SpeakerSetupChannels = new List<SpeakerSetupChannel>();
        }

        public virtual int ID { get; set; }
        public virtual string Name { get; set; }

        /// <summary>
        /// bridge entity, count is above zero
        /// </summary>
        public virtual IList<SpeakerSetupChannel> SpeakerSetupChannels { get; set; }
    }
}
