﻿using JJ.Business.CanonicalModel;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels.Keys;
using JJ.Presentation.Synthesizer.ViewModels.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Presentation.Synthesizer.ViewModels.Entities
{
    public sealed class ChildDocumentViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public IDAndName ChildDocumentType { get; set; }

        public SampleGridViewModel SampleGrid { get; set; }
        public IList<SamplePropertiesViewModel> SamplePropertiesList { get; set; }

        public CurveGridViewModel CurveGrid { get; set; }
        public IList<CurveDetailsViewModel> CurveDetailsList { get; set; }

        public PatchGridViewModel PatchGrid { get; set; }
        public IList<PatchDetailsViewModel> PatchDetailsList { get; set; }
    }
}
