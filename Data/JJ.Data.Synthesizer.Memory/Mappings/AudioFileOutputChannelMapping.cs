﻿using JJ.Data.Synthesizer.Memory.Helpers;
using JJ.Framework.Data.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Data.Synthesizer.Memory.Mappings
{
    public class AudioFileOutputChannelMapping : MemoryMapping<AudioFileOutputChannel>
    {
        public AudioFileOutputChannelMapping()
        {
            IdentityType = IdentityType.Assigned;
            IdentityPropertyName = PropertyNames.ID;
        }
    }
}
