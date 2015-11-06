﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Validation
{
    public class OperatorValidator_SawTooth : OperatorValidator_Base_WithoutData
    {
        public OperatorValidator_SawTooth(Operator obj)
            : base(obj, OperatorTypeEnum.SawTooth, expectedInletCount: 2, expectedOutletCount: 1)
        { }
    }
}