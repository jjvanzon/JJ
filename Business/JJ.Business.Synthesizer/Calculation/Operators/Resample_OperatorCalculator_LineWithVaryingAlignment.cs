﻿using JJ.Framework.Reflection.Exceptions;
using System;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    /// <summary>
    /// Not used.
    /// This variation on the Resample_OperatorCalculator
    /// does not work when the sampling rate gradually changes,
    /// because the alignment of sampling changes with the gradual change.
    /// </summary>
    internal class Resample_OperatorCalculator_LineWithVaryingAlignment : OperatorCalculatorBase_WithChildCalculators
    {
        private OperatorCalculatorBase _signalCalculator;
        private OperatorCalculatorBase _samplingRateCalculator;

        public Resample_OperatorCalculator_LineWithVaryingAlignment(
            OperatorCalculatorBase signalCalculator, 
            OperatorCalculatorBase samplingRateCalculator)
            : base(new OperatorCalculatorBase[]
            {
                signalCalculator,
                samplingRateCalculator
            })
        {
            if (signalCalculator == null) throw new NullException(() => signalCalculator);
            if (signalCalculator is Number_OperatorCalculator) throw new IsNotTypeException<Number_OperatorCalculator>(() => signalCalculator);
            if (samplingRateCalculator == null) throw new NullException(() => samplingRateCalculator);
            // TODO: Resample with constant sampling rate does not have specialized calculators yet. Reactivate code line after those specialized calculators have been programmed.
            //if (samplingRateCalculator is Number_OperatorCalculator) throw new IsNotTypeException<Number_OperatorCalculator>(() => samplingRateCalculator);

            _signalCalculator = signalCalculator;
            _samplingRateCalculator = samplingRateCalculator;
        }

        public override double Calculate(DimensionStack dimensionStack)
        {
            double time = dimensionStack.Get(DimensionEnum.Time);

            double samplingRate = _samplingRateCalculator.Calculate(dimensionStack);

            double samplePeriod = 1.0 / samplingRate;

            double remainder = time % samplePeriod;

            double t0 = time - remainder;
            double t1 = t0 + samplePeriod;
            double dt = t1 - t0;

            dimensionStack.Push(DimensionEnum.Time, t0);
            double x0 = _signalCalculator.Calculate(dimensionStack);
            dimensionStack.Pop(DimensionEnum.Time);

            dimensionStack.Push(DimensionEnum.Time, t1);
            double x1 = _signalCalculator.Calculate(dimensionStack);
            dimensionStack.Pop(DimensionEnum.Time);

            double dx = x1 - x0;

            double a = dx / dt;

            double x = x0 + a * (time - t0);
            return x;
        }
    }
}
