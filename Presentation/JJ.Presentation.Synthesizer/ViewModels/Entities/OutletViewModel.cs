﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.Synthesizer.ViewModels.Entities
{
    public class OutletViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public OperatorViewModel Operator { get; set; }
    }
}
