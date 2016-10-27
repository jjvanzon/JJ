﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Demos.Synthesizer.NanoOptimization.Helpers;

namespace JJ.Demos.Synthesizer.NanoOptimization.Dto
{
    internal class Multiply_OperatorDto_VarA_ConstB : OperatorDto_VarA_ConstB
    {
        public override string OperatorName => OperatorNames.Multiply;

        public Multiply_OperatorDto_VarA_ConstB(OperatorDto aOperatorDto, double b)
            : base(aOperatorDto, b)
        { }
    }
}
