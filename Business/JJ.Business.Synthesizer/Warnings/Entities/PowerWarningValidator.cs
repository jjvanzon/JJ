﻿using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Validation;
using JJ.Data.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.Warnings.Entities
{
    public class PowerWarningValidator : FirstXInletsNotFilledInWarningValidatorBase
    {
        public PowerWarningValidator(Operator obj)
            : base(obj, inletCount: 2)
        { }
    }
}
