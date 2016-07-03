﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.CopiedCode.FromFramework;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class AllPassFilter_OperatorCalculator_VarCenterFrequency_VarBandWidth 
        : OperatorCalculatorBase_WithChildCalculators
    {
        private const int SAMPLES_PER_SET_FILTER_CALL = 100;

        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _centerFrequencyCalculator;
        private readonly OperatorCalculatorBase _bandWidthCalculator;
        private readonly double _samplingRate;

        private BiQuadFilter _biQuadFilter;
        private int _counter;

        public AllPassFilter_OperatorCalculator_VarCenterFrequency_VarBandWidth(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase centerFrequencyCalculator,
            OperatorCalculatorBase bandWidthCalculator,
            double samplingRate)
            : base(new OperatorCalculatorBase[] { signalCalculator, centerFrequencyCalculator, bandWidthCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);
            if (centerFrequencyCalculator == null) throw new NullException(() => centerFrequencyCalculator);
            if (bandWidthCalculator == null) throw new NullException(() => bandWidthCalculator);

            _signalCalculator = signalCalculator;
            _centerFrequencyCalculator = centerFrequencyCalculator;
            _bandWidthCalculator = bandWidthCalculator;
            _samplingRate = samplingRate;

            ResetNonRecursive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            if (_counter > SAMPLES_PER_SET_FILTER_CALL)
            {
                double centerFrequency = _centerFrequencyCalculator.Calculate();
                double bandWidth = _bandWidthCalculator.Calculate();

                _biQuadFilter.SetAllPassFilter(_samplingRate, centerFrequency, bandWidth);

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
            double centerFrequency = _centerFrequencyCalculator.Calculate();
            double bandWidth = _bandWidthCalculator.Calculate();
            _biQuadFilter = BiQuadFilter.CreateAllPassFilter(_samplingRate, centerFrequency, bandWidth);

            _counter = 0;
        }
    }

    internal class AllPassFilter_OperatorCalculator_ConstCenterFrequency_ConstBandWidth : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly double _centerFrequency;
        private readonly double _bandWidth;
        private readonly double _samplingRate;

        private BiQuadFilter _biQuadFilter;

        public AllPassFilter_OperatorCalculator_ConstCenterFrequency_ConstBandWidth(
            OperatorCalculatorBase signalCalculator,
            double centerFrequency,
            double bandWidth,
            double samplingRate)
            : base(new OperatorCalculatorBase[] { signalCalculator })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);

            _signalCalculator = signalCalculator;
            _centerFrequency = centerFrequency;
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
            _biQuadFilter = BiQuadFilter.CreateAllPassFilter(_samplingRate, _centerFrequency, _bandWidth);
        }
    }
}
