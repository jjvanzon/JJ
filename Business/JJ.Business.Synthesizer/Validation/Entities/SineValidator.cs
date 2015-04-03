﻿using JJ.Business.Synthesizer.Names;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Presentation.Resources;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Validation;
using JJ.Persistence.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.Validation.Entities
{
    public class SineValidator : NonSpecializedOperatorValidatorBase
    {
        public SineValidator(Operator obj)
            : base(obj, 
                PropertyNames.Sine, 4,
                PropertyNames.Volume, PropertyNames.Pitch, PropertyNames.Level, PropertyNames.PhaseStart, 
                PropertyNames.Result)
        { }
    }
}
