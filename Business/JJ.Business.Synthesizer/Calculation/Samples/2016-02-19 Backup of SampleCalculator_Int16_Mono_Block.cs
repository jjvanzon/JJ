﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using JJ.Business.Synthesizer.Calculation.Arrays;
//using JJ.Business.Synthesizer.Extensions;
//using JJ.Business.Synthesizer.Validation;
//using JJ.Data.Synthesizer;
//using JJ.Framework.Reflection.Exceptions;
//using JJ.Framework.Validation;

//namespace JJ.Business.Synthesizer.Calculation.Samples
//{
//    internal class SampleCalculator_Int16_Mono_Block : SampleCalculatorBase
//    {
//        private ArrayCalculator_MinTimeZero_Block _arrayCalculator;

//        public SampleCalculator_Int16_Mono_Block(Sample sample, byte[] bytes)
//            : base(sample, bytes)
//        {
//            if (sample.GetChannelCount() != 1) throw new NotEqualException(() => sample.GetChannelCount(), 1);

//            double[][] samples = SampleCalculatorHelper.ReadInt16Samples(sample, bytes);
//            double[] samples2 = samples.Single();

//            _arrayCalculator = new ArrayCalculator_MinTimeZero_Block(samples2, _rate);
//        }

//        public override double CalculateValue(double time, int channelIndex)
//        {
//            return _arrayCalculator.CalculateValue(time);
//        }
//    }
//}
