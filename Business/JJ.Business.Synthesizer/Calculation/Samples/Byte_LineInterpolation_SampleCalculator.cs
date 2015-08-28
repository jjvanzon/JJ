﻿using JJ.Data.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace JJ.Business.Synthesizer.Calculation.Samples
{
    internal class Byte_LineInterpolation_SampleCalculator : LineInterpolation_SampleCalculatorBase
    {
        public Byte_LineInterpolation_SampleCalculator(Sample sample, byte[] bytes)
            : base(sample, bytes)
        { }

        protected override double ReadValue(BinaryReader binaryReader)
        {
            return binaryReader.ReadByte() - 128;
        }
    }
}