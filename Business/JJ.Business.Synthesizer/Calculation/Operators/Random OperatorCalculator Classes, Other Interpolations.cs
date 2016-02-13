﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Calculation.Random;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class Random_OperatorCalculator_LineRememberT0 : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _resampleOperator;

        public Random_OperatorCalculator_LineRememberT0(
            RandomCalculatorBase randomCalculator,
            double randomCalculatorOffset,
            OperatorCalculatorBase rateCalculator,
            OperatorCalculatorBase phaseShiftCalculator)
            : base(new OperatorCalculatorBase[] { rateCalculator, phaseShiftCalculator })
        {
            // Hack in a piece of patch, to reuse the Resample_OperatorCalculator's capability of
            // handling many types of interpolation.

            // Create a second Random operator calculator.
            var randomCalculator2 = new Random_VarFrequency_VarPhaseShift_OperatorCalculator(
                randomCalculator, randomCalculatorOffset, rateCalculator, phaseShiftCalculator);

            // Lead their outputs to a Resample operator calculator
            _resampleOperator = new Resample_OperatorCalculator_LineRememberT0(
                signalCalculator: randomCalculator2,
                samplingRateCalculator: rateCalculator);
        }

        public override double Calculate(double time, int channelIndex)
        {
            return _resampleOperator.Calculate(time, channelIndex);
        }
    }

    internal class Random_OperatorCalculator_LineRememberT1 : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _resampleOperator;

        public Random_OperatorCalculator_LineRememberT1(
            RandomCalculatorBase randomCalculator,
            double randomCalculatorOffset,
            OperatorCalculatorBase rateCalculator,
            OperatorCalculatorBase phaseShiftCalculator)
            : base(new OperatorCalculatorBase[] { rateCalculator, phaseShiftCalculator })
        {
            // Hack in a piece of patch, to reuse the Resample_OperatorCalculator's capability of
            // handling many types of interpolation.

            // Create a second Random operator calculator.
            var randomCalculator2 = new Random_VarFrequency_VarPhaseShift_OperatorCalculator(
                randomCalculator, randomCalculatorOffset, rateCalculator, phaseShiftCalculator);

            // Lead their outputs to a Resample operator calculator
            _resampleOperator = new Resample_OperatorCalculator_LineRememberT1(
                signalCalculator: randomCalculator2,
                samplingRateCalculator: rateCalculator);
        }

        public override double Calculate(double time, int channelIndex)
        {
            return _resampleOperator.Calculate(time, channelIndex);
        }
    }

    internal class Random_OperatorCalculator_CubicEquidistant : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _resampleOperator;

        public Random_OperatorCalculator_CubicEquidistant(
            RandomCalculatorBase randomCalculator,
            double randomCalculatorOffset,
            OperatorCalculatorBase rateCalculator,
            OperatorCalculatorBase phaseShiftCalculator)
            : base(new OperatorCalculatorBase[] { rateCalculator, phaseShiftCalculator })
        {
            // Hack in a piece of patch, to reuse the Resample_OperatorCalculator's capability of
            // handling many types of interpolation.

            // Create a second Random operator calculator.
            var randomCalculator2 = new Random_VarFrequency_VarPhaseShift_OperatorCalculator(
                randomCalculator, randomCalculatorOffset, rateCalculator, phaseShiftCalculator);

            // Lead their outputs to a Resample operator calculator
            _resampleOperator = new Resample_OperatorCalculator_CubicEquidistant(
                signalCalculator: randomCalculator2,
                samplingRateCalculator: rateCalculator);
        }

        public override double Calculate(double time, int channelIndex)
        {
            return _resampleOperator.Calculate(time, channelIndex);
        }
    }

    internal class Random_OperatorCalculator_CubicAbruptInclination : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _resampleOperator;

        public Random_OperatorCalculator_CubicAbruptInclination(
            RandomCalculatorBase randomCalculator,
            double randomCalculatorOffset,
            OperatorCalculatorBase rateCalculator,
            OperatorCalculatorBase phaseShiftCalculator)
            : base(new OperatorCalculatorBase[] { rateCalculator, phaseShiftCalculator })
        {
            // Hack in a piece of patch, to reuse the Resample_OperatorCalculator's capability of
            // handling many types of interpolation.

            // Create a second Random operator calculator.
            var randomCalculator2 = new Random_VarFrequency_VarPhaseShift_OperatorCalculator(
                randomCalculator, randomCalculatorOffset, rateCalculator, phaseShiftCalculator);

            // Lead their outputs to a Resample operator calculator
            _resampleOperator = new Resample_OperatorCalculator_CubicAbruptInclination(
                signalCalculator: randomCalculator2,
                samplingRateCalculator: rateCalculator);
        }

        public override double Calculate(double time, int channelIndex)
        {
            return _resampleOperator.Calculate(time, channelIndex);
        }
    }

    internal class Random_OperatorCalculator_CubicSmoothInclination : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _resampleOperator;

        public Random_OperatorCalculator_CubicSmoothInclination(
            RandomCalculatorBase randomCalculator,
            double randomCalculatorOffset,
            OperatorCalculatorBase rateCalculator,
            OperatorCalculatorBase phaseShiftCalculator)
            : base(new OperatorCalculatorBase[] { rateCalculator, phaseShiftCalculator })
        {
            // Hack in a piece of patch, to reuse the Resample_OperatorCalculator's capability of
            // handling many types of interpolation.

            // Create a second Random operator calculator.
            var randomCalculator2 = new Random_VarFrequency_VarPhaseShift_OperatorCalculator(
                randomCalculator, randomCalculatorOffset, rateCalculator, phaseShiftCalculator);

            // Lead their outputs to a Resample operator calculator
            _resampleOperator = new Resample_OperatorCalculator_CubicSmoothInclination(
                signalCalculator: randomCalculator2,
                samplingRateCalculator: rateCalculator);
        }

        public override double Calculate(double time, int channelIndex)
        {
            return _resampleOperator.Calculate(time, channelIndex);
        }
    }

    internal class Random_OperatorCalculator_Hermite : OperatorCalculatorBase_WithChildCalculators
    {
        private readonly OperatorCalculatorBase _resampleOperator;

        public Random_OperatorCalculator_Hermite(
            RandomCalculatorBase randomCalculator,
            double randomCalculatorOffset,
            OperatorCalculatorBase rateCalculator,
            OperatorCalculatorBase phaseShiftCalculator)
            : base(new OperatorCalculatorBase[] { rateCalculator, phaseShiftCalculator })
        {
            // Hack in a piece of patch, to reuse the Resample_OperatorCalculator's capability of
            // handling many types of interpolation.

            // Create a second Random operator calculator.
            var randomCalculator2 = new Random_VarFrequency_VarPhaseShift_OperatorCalculator(
                randomCalculator, randomCalculatorOffset, rateCalculator, phaseShiftCalculator);

            // Lead their outputs to a Resample operator calculator
            _resampleOperator = new Resample_OperatorCalculator_Hermite(
                signalCalculator: randomCalculator2,
                samplingRateCalculator: rateCalculator);
        }

        public override double Calculate(double time, int channelIndex)
        {
            return _resampleOperator.Calculate(time, channelIndex);
        }
    }
}