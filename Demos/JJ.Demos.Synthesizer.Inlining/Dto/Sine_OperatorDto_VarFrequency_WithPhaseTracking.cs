﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Demos.Synthesizer.Inlining.Dto
{
    internal class Sine_OperatorDto_VarFrequency_WithPhaseTracking : OperatorDto_VarFrequency
    {
        public Sine_OperatorDto_VarFrequency_WithPhaseTracking(InletDto frequencyInletDto)
            : base(frequencyInletDto)
        { }
    }
}
