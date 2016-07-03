﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.CopiedCode.FromFramework;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class LowPassFilter_OperatorCalculator_VarMaxFrequency_VarBandWidth : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _maxFrequencyCalculator;
        private readonly OperatorCalculatorBase _bandWidthCalculator;
        private readonly double _samplingRate;
        private readonly int _samplesBetweenApplyFilterVariables;

        private BiQuadFilter _biQuadFilter;
        private int _counter;

        public LowPassFilter_OperatorCalculator_VarMaxFrequency_VarBandWidth(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase maxFrequencyCalculator,
            OperatorCalculatorBase bandWidthCalculator,
            double samplingRate,
            int samplesBetweenApplyFilterVariables)
            : base(new OperatorCalculatorBase[] { signalCalculator, maxFrequencyCalculator, bandWidthCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);
            if (maxFrequencyCalculator == null) throw new NullException(() => maxFrequencyCalculator);
            if (bandWidthCalculator == null) throw new NullException(() => bandWidthCalculator);
            if (samplesBetweenApplyFilterVariables < 1) throw new LessThanException(() => samplesBetweenApplyFilterVariables, 1);

            _signalCalculator = signalCalculator;
            _maxFrequencyCalculator = maxFrequencyCalculator;
            _bandWidthCalculator = bandWidthCalculator;
            _samplingRate = samplingRate;
            _samplesBetweenApplyFilterVariables = samplesBetweenApplyFilterVariables;

            ResetNonRecursive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            if (_counter > _samplesBetweenApplyFilterVariables)
            {
                double maxFrequency = _maxFrequencyCalculator.Calculate();
                double bandWidth = _bandWidthCalculator.Calculate();

                _biQuadFilter.SetLowPassFilter(_samplingRate, maxFrequency, bandWidth);

                _counter = 0;
            }

            double signal = _signalCalculator.Calculate();
            double value = _biQuadFilter.Transform(signal);

            _counter++;

            return value;
        }

        public override void Reset()
        {
            base.Reset();

            ResetNonRecursive();
        }

        private void ResetNonRecursive()
        {
            double maxFrequency = _maxFrequencyCalculator.Calculate();
            double bandWidth = _bandWidthCalculator.Calculate();

            _biQuadFilter = BiQuadFilter.CreateLowPassFilter(_samplingRate, maxFrequency, bandWidth);

            _counter = 0;
        }
    }

    internal class LowPassFilter_OperatorCalculator_ConstMaxFrequency_ConstBandWidth : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _maxFrequency;
        private readonly double _bandWidth;
        private readonly double _samplingRate;

        private BiQuadFilter _biQuadFilter;

        public LowPassFilter_OperatorCalculator_ConstMaxFrequency_ConstBandWidth(
            OperatorCalculatorBase signalCalculator,
            double maxFrequency,
            double bandWidth,
            double samplingRate)
            : base(new OperatorCalculatorBase[] { signalCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);

            _signalCalculator = signalCalculator;
            _maxFrequency = maxFrequency;
            _bandWidth = bandWidth;
            _samplingRate = samplingRate;

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
            _biQuadFilter = BiQuadFilter.CreateLowPassFilter(_samplingRate, _maxFrequency, _bandWidth);
        }
    }
}
