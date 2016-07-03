﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.CodeCopies.FromFramework;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class LowShelfFilter_ManyConstants_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private const double ASSUMED_SAMPLE_RATE = 44100.0;
        private const double DEFAULT_BAND_WIDTH = 1.0;

        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _frequency;
        private readonly double _dbGain;
        private readonly double _shelfSlope;

        private BiQuadFilter _biQuadFilter;

        public LowShelfFilter_ManyConstants_OperatorCalculator(
            OperatorCalculatorBase signalCalculator,
            double frequency,
            double dbGain,
            double shelfSlope)
            : base(new OperatorCalculatorBase[] { signalCalculator })
        {
            if (signalCalculator == null) throw new NullException(() => signalCalculator);
            if (signalCalculator is Number_OperatorCalculator) throw new IsTypeException<Number_OperatorCalculator>(() => signalCalculator);

            _signalCalculator = signalCalculator;
            _frequency = frequency;
            _dbGain = dbGain;
            _shelfSlope = shelfSlope;

            ResetNonRecursive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double signal = _signalCalculator.Calculate();

            double value = _biQuadFilter.Transform(signal);

            return value;
        }

        public override void Reset()
        {
            base.Reset();

            ResetNonRecursive();
        }

        private void ResetNonRecursive()
        {
            _biQuadFilter = BiQuadFilter.CreateLowShelf(ASSUMED_SAMPLE_RATE, _frequency, _shelfSlope, _dbGain);
        }
    }
}
