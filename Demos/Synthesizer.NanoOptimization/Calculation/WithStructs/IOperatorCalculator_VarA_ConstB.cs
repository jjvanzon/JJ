﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Business.Synthesizer.Tests.NanoOptimization.Calculation.WithStructs
{
    internal interface IOperatorCalculator_VarA_ConstB : IOperatorCalculator
    {
        IOperatorCalculator ACalculator { get; set; }
        double B { get; set; }
    }
}
