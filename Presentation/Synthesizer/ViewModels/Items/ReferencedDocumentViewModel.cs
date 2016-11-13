﻿using JJ.Data.Canonical;
using System.Collections.Generic;

namespace JJ.Presentation.Synthesizer.ViewModels.Items
{
    public sealed class ReferencedDocumentViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public IList<IDAndName> Patches { get; set; }
    }
}