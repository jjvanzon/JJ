﻿using JJ.Business.CanonicalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.Synthesizer.ViewModels
{
    public sealed class DocumentPropertiesViewModel
    {
        public bool Visible { get; set; }
        public bool Successful { get; set; }
        public IDAndName Document { get; set; }
        public IList<Message> ValidationMessages { get; set; }
    }
}
