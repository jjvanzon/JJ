﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.CopiedCode.FromFramework;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class HighPassFilter_OperatorCalculator_AllVars
        : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _minFrequencyCalculator;
        private readonly OperatorCalculatorBase _bandWidthCalculator;
        private readonly double _samplingRate;
        private readonly int _samplesBetweenApplyFilterVariables;
        private readonly BiQuadFilter _biQuadFilter;

        private int _counter;

        public HighPassFilter_OperatorCalculator_AllVars(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase minFrequencyCalculator,
            OperatorCalculatorBase bandWidthCalculator,
            double samplingRate,
            int samplesBetweenApplyFilterVariables)
            : base(new OperatorCalculatorBase[] 
            {
                signalCalculator,
                minFrequencyCalculator,
                bandWidthCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);
            if (minFrequencyCalculator == null) throw new NullException(() => minFrequencyCalculator);
            if (bandWidthCalculator == null) throw new NullException(() => bandWidthCalculator);
            if (samplesBetweenApplyFilterVariables < 1) throw new LessThanException(() => samplesBetweenApplyFilterVariables, 1);

            _signalCalculator = signalCalculator;
            _minFrequencyCalculator = minFrequencyCalculator;
            _bandWidthCalculator = bandWidthCalculator;
            _samplingRate = samplingRate;
            _samplesBetweenApplyFilterVariables = samplesBetweenApplyFilterVariables;
            _biQuadFilter = new BiQuadFilter();

            ResetNonRecursive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            if (_counter > _samplesBetweenApplyFilterVariables)
            {
                ResetNonRecursive();
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResetNonRecursive()
        {
            double minFrequency = _minFrequencyCalculator.Calculate();
            double bandWidth = _bandWidthCalculator.Calculate();

            _biQuadFilter.SetHighPassFilterVariables(_samplingRate, minFrequency, bandWidth);

            _counter = 0;
        }
    }

    internal class HighPassFilter_OperatorCalculator_ManyVars
        : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _minFrequency;
        private readonly double _bandWidth;
        private readonly double _samplingRate;
        private readonly BiQuadFilter _biQuadFilter;

        public HighPassFilter_OperatorCalculator_ManyVars(
            OperatorCalculatorBase signalCalculator,
            double minFrequency,
            double bandWidth,
            double samplingRate)
                : base(new OperatorCalculatorBase[] { signalCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);

            _signalCalculator = signalCalculator;
            _minFrequency = minFrequency;
            _bandWidth = bandWidth;
            _samplingRate = samplingRate;
            _biQuadFilter = new BiQuadFilter();

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
            _biQuadFilter.SetHighPassFilterVariables(_samplingRate, _minFrequency, _bandWidth);
        }
    }
}
