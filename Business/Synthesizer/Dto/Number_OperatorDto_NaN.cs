﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Enums;
using JJ.Demos.Synthesizer.NanoOptimization.Helpers;

namespace JJ.Business.Synthesizer.Dto
{
    internal class Number_OperatorDto_NaN : Number_OperatorDto
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Number);

        public Number_OperatorDto_NaN()
            : base(Double.NaN)
        { }
    }
}
