﻿using System;
using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.Enums;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class SawDown_WithConstFrequency_WithConstPhaseShift_OperatorCalculator : OperatorCalculatorBase
    {
        private readonly double _frequency;
        private readonly double _phaseShift;
        private readonly int _dimensionIndex;
        private readonly DimensionStack _dimensionStack;

        public SawDown_WithConstFrequency_WithConstPhaseShift_OperatorCalculator(
            double frequency, 
            double phaseShift,
            DimensionEnum dimensionEnum,
            DimensionStack dimensionStack)
        {
            if (frequency == 0) throw new ZeroException(() => frequency);
            OperatorCalculatorHelper.AssertDimensionEnum(dimensionEnum);
            if (dimensionStack == null) throw new NullException(() => dimensionStack);

            _frequency = frequency;
            _phaseShift = phaseShift;
            _dimensionIndex = (int)dimensionEnum;
            _dimensionStack = dimensionStack;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double position = _dimensionStack.Get(_dimensionIndex);
            double shiftedPhase = position * _frequency + _phaseShift;
            double value = 1.0 - (2.0 * shiftedPhase % 2.0);
            return value;
        }
    }

    internal class SawDown_WithConstFrequency_WithVarPhaseShift_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly double _frequency;
        private readonly OperatorCalculatorBase _phaseShiftCalculator;
        private readonly int _dimensionIndex;
        private readonly DimensionStack _dimensionStack;

        public SawDown_WithConstFrequency_WithVarPhaseShift_OperatorCalculator(
            double frequency,
            OperatorCalculatorBase phaseShiftCalculator,
            DimensionEnum dimensionEnum,
            DimensionStack dimensionStack)
            : base(new OperatorCalculatorBase[] { phaseShiftCalculator })
        {
            if (frequency == 0) throw new ZeroException(() => frequency);
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(phaseShiftCalculator, () => phaseShiftCalculator);
            OperatorCalculatorHelper.AssertDimensionEnum(dimensionEnum);
            if (dimensionStack == null) throw new NullException(() => dimensionStack);

            _frequency = frequency;
            _phaseShiftCalculator = phaseShiftCalculator;
            _dimensionIndex = (int)dimensionEnum;
            _dimensionStack = dimensionStack;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double position = _dimensionStack.Get(_dimensionIndex);

            double phaseShift = _phaseShiftCalculator.Calculate();

            double phase = position * _frequency;

            double shiftedPhase = phase + phaseShift;
            double value = 1 - (2 * shiftedPhase % 2);

            return value;
        }
    }

    internal class SawDown_WithVarFrequency_WithConstPhaseShift_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _frequencyCalculator;
        private readonly int _dimensionIndex;
        private readonly DimensionStack _dimensionStack;

        private double _phase;
        private double _previousPosition;

        public SawDown_WithVarFrequency_WithConstPhaseShift_OperatorCalculator(
            OperatorCalculatorBase frequencyCalculator,
            double phaseShift,
            DimensionEnum dimensionEnum,
            DimensionStack dimensionStack)
            : base(new OperatorCalculatorBase[] { frequencyCalculator })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(frequencyCalculator, () => frequencyCalculator);
            OperatorCalculatorHelper.AssertDimensionEnum(dimensionEnum);
            if (dimensionStack == null) throw new NullException(() => dimensionStack);

            _frequencyCalculator = frequencyCalculator;
            _dimensionIndex = (int)dimensionEnum;
            _dimensionStack = dimensionStack;

            // TODO: Assert so it does not become NaN.
            _phase = phaseShift;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double position = _dimensionStack.Get(_dimensionIndex);

            double frequency = _frequencyCalculator.Calculate();

            double positionChange = position - _previousPosition;
            _phase = _phase + positionChange * frequency;

            double value = 1 - (2 * _phase % 2);

            _previousPosition = position;

            return value;
        }

        public override void Reset()
        {
            double position = _dimensionStack.Get(_dimensionIndex);

            _previousPosition = position;
            _phase = 0.0;

            base.Reset();
        }
    }

    internal class SawDown_WithVarFrequency_WithVarPhaseShift_OperatorCalculator : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _frequencyCalculator;
        private readonly OperatorCalculatorBase _phaseShiftCalculator;
        private readonly int _dimensionIndex;
        private readonly DimensionStack _dimensionStack;

        private double _phase;
        private double _previousPosition;

        public SawDown_WithVarFrequency_WithVarPhaseShift_OperatorCalculator(
            OperatorCalculatorBase frequencyCalculator,
            OperatorCalculatorBase phaseShiftCalculator,
            DimensionEnum dimensionEnum,
            DimensionStack dimensionStack)
            : base(new OperatorCalculatorBase[] { frequencyCalculator, phaseShiftCalculator })
        {
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(frequencyCalculator, () => frequencyCalculator);
            OperatorCalculatorHelper.AssertOperatorCalculatorBase(phaseShiftCalculator, () => phaseShiftCalculator);
            OperatorCalculatorHelper.AssertDimensionEnum(dimensionEnum);
            if (dimensionStack == null) throw new NullException(() => dimensionStack);

            _frequencyCalculator = frequencyCalculator;
            _phaseShiftCalculator = phaseShiftCalculator;
            _dimensionIndex = (int)dimensionEnum;
            _dimensionStack = dimensionStack;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double Calculate()
        {
            double position = _dimensionStack.Get(_dimensionIndex);

            double frequency = _frequencyCalculator.Calculate();
            double phaseShift = _phaseShiftCalculator.Calculate();

            double positionChange = position - _previousPosition;
            _phase = _phase + positionChange * frequency;

            double shiftedPhase = _phase + phaseShift;
            double value = 1 - (2 * shiftedPhase % 2);

            _previousPosition = position;

            return value;
        }

        public override void Reset()
        {
            double position = _dimensionStack.Get(_dimensionIndex);

            _previousPosition = position;
            _phase = 0.0;

            base.Reset();
        }
    }
}
