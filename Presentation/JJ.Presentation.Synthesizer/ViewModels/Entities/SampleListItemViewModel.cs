﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Presentation.Synthesizer.ViewModels.Entities
{
    public sealed class SampleListItemViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int SamplingRate { get; set; }
        public string SampleDataType { get; set; }
        public string SpeakerSetup { get; set; }
    }
}
