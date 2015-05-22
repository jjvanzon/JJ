﻿using JJ.Business.CanonicalModel;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.Synthesizer.ViewModels
{
    public sealed class PatchDetailsViewModel
    {
        public bool Visible { get; set; }

        public PatchViewModel Patch { get; set; }

        public IList<OperatorTypeViewModel> OperatorToolboxItems { get; set; }

        public OperatorViewModel SelectedOperator { get; set; }
        public string SelectedValue { get; set; }

        public List<Message> ValidationMessages { get; set; }

        [Obsolete]
        public bool SavedMessageVisible { get; set; }
    }
}
