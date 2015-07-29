﻿using JJ.Business.CanonicalModel;
using JJ.Business.Synthesizer.Enums;
using JJ.Framework.Presentation;
using JJ.Presentation.Synthesizer.Helpers;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using JJ.Presentation.Synthesizer.ViewModels.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.Synthesizer.ViewModels
{
    public sealed class ChildDocumentGridViewModel
    {
        public int RootDocumentID { get; set; }
        // TODO: Refactor out the enum. You should not put that in a view model.
        public ChildDocumentTypeEnum ChildDocumentTypeEnum { get; set; }
        public bool Visible { get; set; }
        public IList<ChildDocumentListItemViewModel> List { get; set; }
    }
}
