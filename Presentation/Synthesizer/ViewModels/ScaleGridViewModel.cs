﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Data.Canonical;

namespace JJ.Presentation.Synthesizer.ViewModels
{
    public sealed class ScaleGridViewModel : ViewModelBase
    {
        public int DocumentID { get; set; }
        public Dictionary<int, IDAndName> Dictionary { get; set; }
    }
}