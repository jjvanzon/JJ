﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;
using System;
using System.Linq;
using System.Collections.Generic;

namespace JJ.Business.Synthesizer.Validation.Operators
{
    internal class MaxOverInlets_OperatorValidator : OperatorValidator_Base_VariableInletCountOneOutlet
    {
        public MaxOverInlets_OperatorValidator(Operator obj)
            : base(obj, OperatorTypeEnum.MaxOverInlets, expectedDataKeys: new string[0])
        { }
    }
}