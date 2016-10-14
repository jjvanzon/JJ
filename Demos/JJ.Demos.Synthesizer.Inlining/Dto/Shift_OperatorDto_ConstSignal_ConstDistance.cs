﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Demos.Synthesizer.Inlining.Dto
{
    internal class Shift_OperatorDto_ConstSignal_ConstDistance : Shift_OperatorDto
    {
        public double SignalValue { get; set; }
        public double DistanceValue { get; set; }

        public Shift_OperatorDto_ConstSignal_ConstDistance(InletDto signalInletDto, InletDto distanceInletDto, double signalValue, double distanceValue)
            : base(signalInletDto, distanceInletDto)
        {
            SignalValue = signalValue;
            DistanceValue = distanceValue;
        }
    }
}