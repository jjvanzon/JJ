﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Validation
{
    internal class OperatorValidator_SawUp : OperatorValidator_Base_WithoutData
    {
        public OperatorValidator_SawUp(Operator obj)
            : base(obj, OperatorTypeEnum.SawUp, expectedInletCount: 2, expectedOutletCount: 1)
        { }
    }
}