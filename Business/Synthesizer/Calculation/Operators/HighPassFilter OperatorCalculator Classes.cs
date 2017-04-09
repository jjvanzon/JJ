﻿using System.Runtime.CompilerServices;
using JJ.Framework.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class HighPassFilter_OperatorCalculator_AllVars
        : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _minFrequencyCalculator;
        private readonly OperatorCalculatorBase _bandWidthCalculator;
        private readonly double _targetSamplingRate;
        private readonly double _nyquistFrequency;
        private readonly int _samplesBetweenApplyFilterVariables;
        private readonly BiQuadFilter _biQuadFilter;

        private int _counter;

        public HighPassFilter_OperatorCalculator_AllVars(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase minFrequencyCalculator,
            OperatorCalculatorBase bandWidthCalculator,
            double targetSamplingRate,
            int samplesBetweenApplyFilterVariables)
            : base(new[] 
            {
                signalCalculator,
                minFrequencyCalculator,
                bandWidthCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);
            if (samplesBetweenApplyFilterVariables < 1) throw new LessThanException(() => samplesBetweenApplyFilterVariables, 1);

            _signalCalculator = signalCalculator;
            _minFrequencyCalculator = minFrequencyCalculator ?? throw new NullException(() => minFrequencyCalculator);
            _bandWidthCalculator = bandWidthCalculator ?? throw new NullException(() => bandWidthCalculator);
            _targetSamplingRate = targetSamplingRate;
            _samplesBetweenApplyFilterVariables = samplesBetweenApplyFilterVariables;
            _biQuadFilter = new BiQuadFilter();

            _nyquistFrequency = _targetSamplingRate / 2.0;

            ResetNonRecursive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            if (_counter > _samplesBetweenApplyFilterVariables)
            {
                SetFilterVariables();
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
            SetFilterVariables();
            _counter = 0;
            _biQuadFilter.ResetSamples();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetFilterVariables()
        {
            double minFrequency = _minFrequencyCalculator.Calculate();
            double bandWidth = _bandWidthCalculator.Calculate();

            if (minFrequency > _nyquistFrequency) minFrequency = _nyquistFrequency;

            _biQuadFilter.SetHighPassFilterVariables(_targetSamplingRate, minFrequency, bandWidth);
        }
    }

    internal class HighPassFilter_OperatorCalculator_ManyConsts
        : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _minFrequency;
        private readonly double _bandWidth;
        private readonly double _targetSamplingRate;
        private readonly BiQuadFilter _biQuadFilter;

        public HighPassFilter_OperatorCalculator_ManyConsts(
            OperatorCalculatorBase signalCalculator,
            double minFrequency,
            double bandWidth,
            double targetSamplingRate)
                : base(new[] { signalCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertFilterFrequency(minFrequency, targetSamplingRate);

            _signalCalculator = signalCalculator;
            _minFrequency = minFrequency;
            _bandWidth = bandWidth;
            _targetSamplingRate = targetSamplingRate;
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
            _biQuadFilter.SetHighPassFilterVariables(_targetSamplingRate, _minFrequency, _bandWidth);
            _biQuadFilter.ResetSamples();
        }
    }
}
