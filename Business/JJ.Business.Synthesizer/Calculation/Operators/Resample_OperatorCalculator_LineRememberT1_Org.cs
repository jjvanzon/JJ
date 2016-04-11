﻿using JJ.Framework.Reflection.Exceptions;
using System;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    /// <summary>
    /// Not used.
    /// 
    /// This backup class is the variation on Resample_OperatorCalculator_LineRememberT1
    /// that does not take time going in reverse into consideration.
    /// 
    /// It seems to work, except for the artifacts that linear interpolation gives us.
    /// A weakness though is, that the sampling rate is remembered until the next sample,
    /// which may work poorly when a very low sampling rate is provided.
    /// </summary>
    internal class Resample_OperatorCalculator_LineRememberT1_Org : OperatorCalculatorBase_WithChildCalculators
    {
        private OperatorCalculatorBase _signalCalculator;
        private OperatorCalculatorBase _samplingRateCalculator;

        public Resample_OperatorCalculator_LineRememberT1_Org(
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

        private double _t0;
        private double _t1;
        private double _x0;
        private double _x1;
        private double _a;

        public override double Calculate(DimensionStack dimensionStack)
        {
            double t = dimensionStack.Get(DimensionEnum.Time);

            if (t >= _t1)
            {
                _t0 = _t1;
                _x0 = _x1;

                dimensionStack.Push(DimensionEnum.Time, _t1);
                double samplingRate = _samplingRateCalculator.Calculate(dimensionStack);
                dimensionStack.Pop(DimensionEnum.Time);

                if (samplingRate == 0)
                {
                    _a = 0;
                }
                else
                {
                    double dt = 1.0 / samplingRate;

                    _t1 += dt;

                    dimensionStack.Push(DimensionEnum.Time, _t1);
                    _x1 = _signalCalculator.Calculate(dimensionStack);
                    dimensionStack.Pop(DimensionEnum.Time);

                    double dx = _x1 - _x0;

                    _a = dx / dt;
                }
            }

            double x = _x0 + _a * (t - _t0);
            return x;
        }
    }
}
