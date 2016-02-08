﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Presentation.Synthesizer.ViewModels.Entities;

namespace JJ.Presentation.Synthesizer.ViewModels
{
    public sealed class CurrentPatchesViewModel : ViewModelBase
    {
        public IList<CurrentPatchItemViewModel> List { get; set; }
    }
}
