﻿using System.Runtime.CompilerServices;
using JJ.Framework.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class BandPassFilterConstantTransitionGain_OperatorCalculator_VarCenterFrequency_VarWidth
        : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _soundCalculator;
        private readonly OperatorCalculatorBase _centerFrequencyCalculator;
        private readonly OperatorCalculatorBase _widthCalculator;
        private readonly double _targetSamplingRate;
        private readonly double _nyquistFrequency;
        private readonly int _samplesBetweenApplyFilterVariables;
        private readonly BiQuadFilter _biQuadFilter;

        private int _counter;

        public BandPassFilterConstantTransitionGain_OperatorCalculator_VarCenterFrequency_VarWidth(
            OperatorCalculatorBase soundCalculator,
            OperatorCalculatorBase centerFrequencyCalculator,
            OperatorCalculatorBase widthCalculator,
            double targetSamplingRate,
            int samplesBetweenApplyFilterVariables)
                : base(new[]
                {
                    soundCalculator,
                    centerFrequencyCalculator,
                    widthCalculator
                })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(soundCalculator, () => soundCalculator);
            if (samplesBetweenApplyFilterVariables < 1) throw new LessThanException(() => samplesBetweenApplyFilterVariables, 1);

            _soundCalculator = soundCalculator;
            _centerFrequencyCalculator = centerFrequencyCalculator ?? throw new NullException(() => centerFrequencyCalculator);
            _widthCalculator = widthCalculator ?? throw new NullException(() => widthCalculator);
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

            double sound = _soundCalculator.Calculate();
            double result = _biQuadFilter.Transform(sound);

            _counter++;

            return result;
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
            double centerFrequency = _centerFrequencyCalculator.Calculate();
            double width = _widthCalculator.Calculate();

            if (centerFrequency > _nyquistFrequency) centerFrequency = _nyquistFrequency;

            _biQuadFilter.SetBandPassFilterConstantSkirtGainVariables(_targetSamplingRate, centerFrequency, width);
        }
    }

    internal class BandPassFilterConstantTransitionGain_OperatorCalculator_ConstCenterFrequency_ConstWidth
        : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _soundCalculator;
        private readonly double _centerFrequency;
        private readonly double _width;
        private readonly double _targetSamplingRate;
        private readonly BiQuadFilter _biQuadFilter;

        public BandPassFilterConstantTransitionGain_OperatorCalculator_ConstCenterFrequency_ConstWidth(
            OperatorCalculatorBase soundCalculator,
            double centerFrequency,
            double width,
            double targetSamplingRate)
            : base(new[] { soundCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(soundCalculator, () => soundCalculator);
            OperatorCalculatorHelper.AssertFilterFrequency(centerFrequency, targetSamplingRate);

            _soundCalculator = soundCalculator;
            _centerFrequency = centerFrequency;
            _width = width;
            _targetSamplingRate = targetSamplingRate;
            _biQuadFilter = new BiQuadFilter();

            ResetNonRecursive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double sound = _soundCalculator.Calculate();
            double result = _biQuadFilter.Transform(sound);
            return result;
        }

        public override void Reset()
        {
            base.Reset();

            ResetNonRecursive();
        }

        private void ResetNonRecursive()
        {
            _biQuadFilter.SetBandPassFilterConstantSkirtGainVariables(_targetSamplingRate, _centerFrequency, _width);
            _biQuadFilter.ResetSamples();
        }
    }
}