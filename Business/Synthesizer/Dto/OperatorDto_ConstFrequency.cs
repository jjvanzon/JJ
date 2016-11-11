﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Business.Synthesizer.Dto
{
    internal abstract class OperatorDto_ConstFrequency : OperatorDto
    {
        public double Frequency { get; set; }

        public OperatorDto_ConstFrequency(double frequency)
            : base (new OperatorDto[0])
        {
            Frequency = frequency;
        }
    }
}