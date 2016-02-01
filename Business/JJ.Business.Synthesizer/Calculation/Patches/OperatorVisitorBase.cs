﻿using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Enums;
using JJ.Framework.Reflection.Exceptions;
using JJ.Data.Synthesizer;
using System;
using System.Linq;
using System.Collections.Generic;

namespace JJ.Business.Synthesizer.Calculation.Patches
{
    internal abstract class OperatorVisitorBase
    {
        private readonly IDictionary<OperatorTypeEnum, Action<Operator>> _delegateDictionary;

        public OperatorVisitorBase()
        {
            _delegateDictionary = new Dictionary<OperatorTypeEnum, Action<Operator>>
            {
                { OperatorTypeEnum.Add, VisitAdd },
                { OperatorTypeEnum.Adder, VisitAdder },
                { OperatorTypeEnum.And, VisitAnd },
                { OperatorTypeEnum.Average, VisitAverage },
                { OperatorTypeEnum.Bundle, VisitBundle },
                { OperatorTypeEnum.Curve, VisitCurveOperator },
                { OperatorTypeEnum.CustomOperator, VisitCustomOperator },
                { OperatorTypeEnum.Delay, VisitDelay },
                { OperatorTypeEnum.Divide, VisitDivide },
                { OperatorTypeEnum.Earlier, VisitEarlier },
                { OperatorTypeEnum.Equal, VisitEqual },
                { OperatorTypeEnum.Exponent, VisitExponent },
                { OperatorTypeEnum.GreaterThan, VisitGreaterThan },
                { OperatorTypeEnum.GreaterThanOrEqual, VisitGreaterThanOrEqual },
                { OperatorTypeEnum.HighPassFilter, VisitHighPassFilter },
                { OperatorTypeEnum.If, VisitIf },
                { OperatorTypeEnum.LessThan, VisitLessThan },
                { OperatorTypeEnum.LessThanOrEqual, VisitLessThanOrEqual },
                { OperatorTypeEnum.Loop, VisitLoop },
                { OperatorTypeEnum.LowPassFilter, VisitLowPassFilter },
                { OperatorTypeEnum.Maximum, VisitMaximum },
                { OperatorTypeEnum.Minimum, VisitMinimum },
                { OperatorTypeEnum.Multiply, VisitMultiply },
                { OperatorTypeEnum.Narrower, VisitNarrower },
                { OperatorTypeEnum.Not, VisitNot },
                { OperatorTypeEnum.NotEqual, VisitNotEqual },
                { OperatorTypeEnum.Number, VisitNumber },
                { OperatorTypeEnum.Or, VisitOr },
                { OperatorTypeEnum.PatchInlet, VisitPatchInlet },
                { OperatorTypeEnum.PatchOutlet, VisitPatchOutlet },
                { OperatorTypeEnum.Power, VisitPower },
                { OperatorTypeEnum.Pulse, VisitPulse },
                { OperatorTypeEnum.Random, VisitRandom },
                { OperatorTypeEnum.Resample, VisitResample },
                { OperatorTypeEnum.Reset, VisitReset },
                { OperatorTypeEnum.Sample, VisitSampleOperator },
                { OperatorTypeEnum.SawUp, VisitSawUp },
                { OperatorTypeEnum.Select, VisitSelect },
                { OperatorTypeEnum.Shift, VisitShift },
                { OperatorTypeEnum.Sine, VisitSine },
                { OperatorTypeEnum.SlowDown, VisitSlowDown },
                { OperatorTypeEnum.Spectrum, VisitSpectrum },
                { OperatorTypeEnum.SpeedUp, VisitSpeedUp },
                { OperatorTypeEnum.Square, VisitSquare },
                { OperatorTypeEnum.Stretch, VisitStretch },
                { OperatorTypeEnum.Subtract, VisitSubtract },
                { OperatorTypeEnum.TimePower, VisitTimePower },
                { OperatorTypeEnum.Triangle, VisitTriangle },
                { OperatorTypeEnum.Unbundle, VisitUnbundle },
                { OperatorTypeEnum.Noise, VisitNoise },
            };
        }

        protected virtual void VisitOperator(Operator op)
        {
            if (op == null) throw new NullException(() => op);

            // TODO: Is the trick below not specific to the OptimizedPatchCalculatorVisitor?

            // Reverse the order of evaluating the inlet,
            // so that the first inlet will be the last one pushed
            // so it will be the first one popped.
            IList<Inlet> inlets = op.Inlets.OrderByDescending(x => x.ListIndex).ToArray();
            foreach (Inlet inlet in inlets)
            {
                VisitInlet(inlet);
            }

            Action<Operator> action;
            if (_delegateDictionary.TryGetValue(op.GetOperatorTypeEnum(), out action))
            {
                action(op);
            }
        }

        protected virtual void VisitInlet(Inlet inlet)
        {
            Outlet outlet = inlet.InputOutlet;

            if (outlet != null)
            {
                VisitOutlet(outlet);
            }
        }

        protected virtual void VisitOutlet(Outlet outlet)
        {
            Operator op = outlet.Operator;
            VisitOperator(op);
        }

        /// <summary> does nothing </summary>
        protected virtual void VisitAdd(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitAdder(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitAverage(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitBundle(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitCurveOperator(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitCustomOperator(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitDelay(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitDivide(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitEarlier(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitExponent(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitIf(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitHighPassFilter(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitLoop(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitLowPassFilter(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitMinimum(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitMaximum(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitMultiply(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitNarrower(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitNumber(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitPatchInlet(Operator obj) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitPatchOutlet(Operator obj) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitPower(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitPulse(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitRandom(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitResample(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitReset(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitSampleOperator(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitSawUp(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitSelect(Operator obj) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitShift(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitSine(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitSlowDown(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitSpectrum(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitSpeedUp(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitSquare(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitStretch(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitSubtract(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitTimePower(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitTriangle(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitUnbundle(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitNoise(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitEqual(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitNotEqual(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitLessThan(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitGreaterThan(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitLessThanOrEqual(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitGreaterThanOrEqual(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitAnd(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitOr(Operator op) { }

        /// <summary> does nothing </summary>
        protected virtual void VisitNot(Operator op) { }
    }
}
