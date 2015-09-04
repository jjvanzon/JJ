﻿using JJ.Business.CanonicalModel;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using System.Collections.Generic;

namespace JJ.Presentation.Synthesizer.ViewModels
{
    public sealed class OperatorPropertiesViewModel
    {
        // Properties put directly here, instead of entity view model,
        // because entity view model is too elaborate.

        public int ID { get; set; }
        public string Name { get; set; }
        /// <summary> not editable </summary>
        public OperatorTypeViewModel OperatorType { get; set; }

        public bool Visible { get; set; }
        public bool Successful { get; set; }
        public IList<Message> ValidationMessages { get; set; }
    }
}
