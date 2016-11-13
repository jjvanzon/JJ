﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.Helpers;
using JJ.Framework.Common;
using JJ.Framework.Mathematics;
using Lomont;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Spectrum_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _signalCalculator;
        private readonly OperatorCalculatorBase _startCalculator;
        private readonly OperatorCalculatorBase _endCalculator;
        private readonly OperatorCalculatorBase _frequencyCountCalculator;
        private readonly DimensionStack _dimensionStack;
        private readonly int _nextDimensionStackIndex;
        private readonly int _previousDimensionStackIndex;

        private readonly LomontFFT _lomontFFT;

        private double[] _harmonicVolumes;
        private int _maxPosition;

        public Spectrum_OperatorCalculator(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase startCalculator,
            OperatorCalculatorBase endCalculator,
            OperatorCalculatorBase frequencyCountCalculator,
            DimensionStack dimensionStack)
            : base(new OperatorCalculatorBase[]
            {
                signalCalculator,
                startCalculator,
                endCalculator,
                frequencyCountCalculator
            })
        {
            OperatorCalculatorHelper.AssertChildOperatorCalculator(signalCalculator, () => signalCalculator);
            OperatorCalculatorHelper.AssertChildOperatorCalculator_OnlyUsedUponResetState(startCalculator, () => startCalculator);
            OperatorCalculatorHelper.AssertChildOperatorCalculator_OnlyUsedUponResetState(endCalculator, () => endCalculator);
            OperatorCalculatorHelper.AssertChildOperatorCalculator_OnlyUsedUponResetState(frequencyCountCalculator, () => frequencyCountCalculator);
            OperatorCalculatorHelper.AssertDimensionStack(dimensionStack);

            _signalCalculator = signalCalculator;
            _startCalculator = startCalculator;
            _endCalculator = endCalculator;
            _frequencyCountCalculator = frequencyCountCalculator;
            _dimensionStack = dimensionStack;
            _previousDimensionStackIndex = dimensionStack.CurrentIndex;
            _nextDimensionStackIndex = dimensionStack.CurrentIndex + 1;

            _lomontFFT = new LomontFFT();

            ResetPrivate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
#if !USE_INVAR_INDICES
            double position = _dimensionStack.Get();
#else
            double time = _dimensionStack.Get(_previousDimensionStackIndex);
#endif
#if ASSERT_INVAR_INDICES
            OperatorCalculatorHelper.AssertStackIndex(_dimensionStack, _previousDimensionStackIndex);
#endif

            if (position < 0) position = 0;
            if (position > _maxPosition) position = _maxPosition;

            int i = (int)position;

            double frequency = _harmonicVolumes[i];

            return frequency;
        }

        public override void Reset()
        {
            ResetPrivate();

            // NOTE: Do not call base.
            // The Spectrum Operator is an exception to the rule.
            // Reset for spectrum means NOT resetting the signal,
            // but just recalculating the spectrum.
        }

        private void ResetPrivate()
        {
#if !USE_INVAR_INDICES
            double time = _dimensionStack.Get();
#else
            double time = _dimensionStack.Get(_previousDimensionStackIndex);
#endif
#if ASSERT_INVAR_INDICES
            OperatorCalculatorHelper.AssertStackIndex(_dimensionStack, _previousDimensionStackIndex);
#endif
            _harmonicVolumes = CreateHarmonicVolumes();
            _maxPosition = _harmonicVolumes.Length - 1;
        }

        private double[] CreateHarmonicVolumes()
        {
            double start = _startCalculator.Calculate();
            double end = _endCalculator.Calculate();
            double frequencyCountDouble = _frequencyCountCalculator.Calculate();

            // We need a lot of lenience in this code, because validity is dependent on user input,
            // and we cannot obtrusively interrupt the user with validation messages, 
            // because he is busy making music and the show must go on.
            bool startIsValid = StartIsValid(start);
            bool endIsValid = EndIsValid(end);
            bool frequencyCountIsValid = FrequencyCountIsValid(frequencyCountDouble);
            bool startComparedToEndIsValid = end > start;
            bool allValuesAreValid = startIsValid &&
                                     endIsValid &&
                                     frequencyCountIsValid &&
                                     startComparedToEndIsValid;
            if (!allValuesAreValid)
            {
                return CreateNaNHarmonicVolumes();
            }

            int frequencyCount = (int)frequencyCountDouble;
            int frequencyCountTimesTwo = frequencyCount * 2;
            double[] harmonicVolumes = new double[frequencyCount];

            // FFT requires an array size twice as large as the number of frequencies I want.
            double[] data = new double[frequencyCountTimesTwo];
            double dt = (end - start) / frequencyCountTimesTwo;

            double t = start;
            for (int i = 0; i < frequencyCountTimesTwo; i++)
            {
#if !USE_INVAR_INDICES
                _dimensionStack.Push(t);
#else
                _dimensionStack.Set(_nextDimensionStackIndex, t);
#endif
#if ASSERT_INVAR_INDICES
                OperatorCalculatorHelper.AssertStackIndex(_dimensionStack, _nextDimensionStackIndex);
#endif

                double value = _signalCalculator.Calculate();
#if !USE_INVAR_INDICES
                _dimensionStack.Pop();
#endif
                data[i] = value;

                t += dt;
            }

            // TODO: Not sure which is faster: forward or backward.
            _lomontFFT.RealFFT(data, forward: true);

            int j = 0;
            for (int i = 3; i < frequencyCountTimesTwo; i += 2)
            {
                harmonicVolumes[j] = data[i];
                j++;
            }

            // TODO: The FFT algorithm I borrowed does not give me the last frequency.
            harmonicVolumes[frequencyCount - 1] = 0;

            return harmonicVolumes;
        }

        private static bool StartIsValid(double start)
        {
            return !DoubleHelper.IsSpecialValue(start);
        }

        private static bool EndIsValid(double end)
        {
            return !DoubleHelper.IsSpecialValue(end);
        }

        private static bool FrequencyCountIsValid(double frequencyCount)
        {
            if (!ConversionHelper.CanCastToInt32(frequencyCount))
            {
                return false;
            }

            if (frequencyCount < 2.0)
            {
                return false;
            }

            if (!MathHelper.IsPowerOf2((int)frequencyCount))
            {
                return false;
            }

            return true;
        }

        private double[] CreateNaNHarmonicVolumes()
        {
            return new double[] { Double.NaN };
        }
    }
}