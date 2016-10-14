﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Demos.Synthesizer.Inlining.Dto
{
    internal class Multiply_OperatorDto_VarA_ConstB : Multiply_OperatorDto
    {
        public double BValue { get; set; }

        public Multiply_OperatorDto_VarA_ConstB(InletDto aInletDto, InletDto bInletDto, double bValue)
            : base(aInletDto, bInletDto)
        {
            BValue = bValue;
        }
    }
}
