﻿using JJ.Presentation.Synthesizer.ViewModels.Entities;
using System.Collections.Generic;

namespace JJ.Presentation.Synthesizer.ViewModels
{
    public sealed class SampleGridViewModel : ViewModelBase
    {
        public int DocumentID { get; set; }
        public IList<SampleListItemViewModel> List { get; set; }
    }
}
