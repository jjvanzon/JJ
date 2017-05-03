﻿using System.Collections.Generic;
using JJ.Data.Canonical;

namespace JJ.Presentation.Synthesizer.ViewModels
{
    public sealed class LibraryPatchGridViewModel : ViewModelBase
    {
        public int LowerDocumentReferenceID { get; set; }
        public string Group { get; set; }
        public IList<IDAndName> List { get; set; }
        public int? OutletIDToPlay { get; set; }
    }
}
