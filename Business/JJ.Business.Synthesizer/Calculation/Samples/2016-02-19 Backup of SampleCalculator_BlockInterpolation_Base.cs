﻿//using JJ.Business.Synthesizer.Calculation.Arrays;
//using JJ.Business.Synthesizer.Extensions;
//using JJ.Data.Synthesizer;

//namespace JJ.Business.Synthesizer.Calculation.Samples
//{
//    internal abstract class SampleCalculator_BlockInterpolation_Base : SampleCalculatorBase
//    {
//        private ArrayCalculator_MinTimeZero_Block[] _arrayCalculators;

//        public SampleCalculator_BlockInterpolation_Base(Sample sample, byte[] bytes)
//            : base(sample, bytes)
//        {
//            int channelCount = sample.GetChannelCount();
//            _arrayCalculators = new ArrayCalculator_MinTimeZero_Block[channelCount];
//            for (int i = 0; i < channelCount; i++)
//            {
//                _arrayCalculators[i] = new ArrayCalculator_MinTimeZero_Block(_samples[0], _rate);
//            }
//        }

//        public override double CalculateValue(double time, int channelIndex)
//        {
//            return _arrayCalculators[channelIndex].CalculateValue(time);
//        }
//    }
//}