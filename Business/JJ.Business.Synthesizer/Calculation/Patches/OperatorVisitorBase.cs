﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Extensions;
using JJ.Data.Synthesizer;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.Calculation.Patches
{
    internal abstract class OperatorVisitorBase
    {
        private readonly IDictionary<OperatorTypeEnum, Action<Operator>> _delegateDictionary;

        public OperatorVisitorBase()
        {
            _delegateDictionary = new Dictionary<OperatorTypeEnum, Action<Operator>>
            {
                { OperatorTypeEnum.Absolute, VisitAbsolute },
                { OperatorTypeEnum.Add, VisitAdd },
                { OperatorTypeEnum.Adder, VisitAdder },
                { OperatorTypeEnum.And, VisitAnd },
                { OperatorTypeEnum.Average, VisitAverage },
                { OperatorTypeEnum.Bundle, VisitBundle },
                { OperatorTypeEnum.Cache, VisitCache },
                { OperatorTypeEnum.ChangeTrigger, VisitChangeTrigger },
                { OperatorTypeEnum.Curve, VisitCurveOperator },
                { OperatorTypeEnum.CustomOperator, VisitCustomOperator },
                { OperatorTypeEnum.Delay, VisitDelay },
                { OperatorTypeEnum.Divide, VisitDivide },
                { OperatorTypeEnum.Earlier, VisitEarlier },
                { OperatorTypeEnum.Equal, VisitEqual },
                { OperatorTypeEnum.Exponent, VisitExponent },
                { OperatorTypeEnum.Filter, VisitFilter },
                { OperatorTypeEnum.GetDimension, VisitGetDimension },
                { OperatorTypeEnum.GreaterThan, VisitGreaterThan },
                { OperatorTypeEnum.GreaterThanOrEqual, VisitGreaterThanOrEqual },
                { OperatorTypeEnum.Hold, VisitHold },
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
                { OperatorTypeEnum.Negative, VisitNegative },
                { OperatorTypeEnum.Noise, VisitNoise },
                { OperatorTypeEnum.Not, VisitNot },
                { OperatorTypeEnum.NotEqual, VisitNotEqual },
                { OperatorTypeEnum.Number, VisitNumber },
                { OperatorTypeEnum.OneOverX, VisitOneOverX },
                { OperatorTypeEnum.Or, VisitOr },
                { OperatorTypeEnum.PatchInlet, VisitPatchInlet },
                { OperatorTypeEnum.PatchOutlet, VisitPatchOutlet },
                { OperatorTypeEnum.Power, VisitPower },
                { OperatorTypeEnum.Pulse, VisitPulse },
                { OperatorTypeEnum.PulseTrigger, VisitPulseTrigger },
                { OperatorTypeEnum.Random, VisitRandom },
                { OperatorTypeEnum.Range, VisitRange },
                { OperatorTypeEnum.Resample, VisitResample },
                { OperatorTypeEnum.Reset, VisitReset },
                { OperatorTypeEnum.Reverse, VisitReverse },
                { OperatorTypeEnum.Round, VisitRound },
                { OperatorTypeEnum.Sample, VisitSampleOperator },
                { OperatorTypeEnum.SawDown, VisitSawDown },
                { OperatorTypeEnum.SawUp, VisitSawUp },
                { OperatorTypeEnum.Scaler, VisitScaler },
                { OperatorTypeEnum.Select, VisitSelect },
                { OperatorTypeEnum.SetDimension, VisitSetDimension },
                { OperatorTypeEnum.Shift, VisitShift },
                { OperatorTypeEnum.Sine, VisitSine },
                { OperatorTypeEnum.SlowDown, VisitSlowDown },
                { OperatorTypeEnum.Spectrum, VisitSpectrum },
                { OperatorTypeEnum.SpeedUp, VisitSpeedUp },
                { OperatorTypeEnum.Square, VisitSquare },
                { OperatorTypeEnum.Stretch, VisitStretch },
                { OperatorTypeEnum.Subtract, VisitSubtract },
                { OperatorTypeEnum.TimePower, VisitTimePower },
                { OperatorTypeEnum.ToggleTrigger, VisitToggleTrigger },
                { OperatorTypeEnum.Triangle, VisitTriangle },
                { OperatorTypeEnum.Unbundle, VisitUnbundle },
            };
        }

        [DebuggerHidden]
        protected virtual void VisitOperatorPolymorphic(Operator op)
        {
            if (op == null) throw new NullException(() => op);

            Action<Operator> action;
            if (_delegateDictionary.TryGetValue(op.GetOperatorTypeEnum(), out action))
            {
                action(op);
            }
        }

        [DebuggerHidden]
        protected virtual void VisitOperatorBase(Operator op)
        {
            if (op == null) throw new NullException(() => op);

            // TODO: Low priority: Is the trick below not specific to the OptimizedPatchCalculatorVisitor?

            // Reverse the order of evaluating the inlet,
            // so that the first inlet will be the last one pushed
            // so it will be the first one popped.
            IList<Inlet> inlets = op.Inlets.OrderByDescending(x => x.ListIndex).ToArray();
            foreach (Inlet inlet in inlets)
            {
                VisitInlet(inlet);
            }
        }

        [DebuggerHidden]
        protected virtual void VisitInlet(Inlet inlet)
        {
            Outlet outlet = inlet.InputOutlet;

            if (outlet != null)
            {
                VisitOutlet(outlet);
            }
        }

        [DebuggerHidden]
        protected virtual void VisitOutlet(Outlet outlet)
        {
            Operator op = outlet.Operator;
            VisitOperatorPolymorphic(op);
        }

        [DebuggerHidden]
        protected virtual void VisitAbsolute(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitAdd(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitAdder(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitAnd(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitAverage(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitBundle(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitChangeTrigger(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitCache(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitCurveOperator(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitCustomOperator(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitDelay(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitDivide(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitEarlier(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitEqual(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitExponent(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitFilter(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitGetDimension(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitGreaterThan(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitGreaterThanOrEqual(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitHold(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitHighPassFilter(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitIf(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitLessThan(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitLessThanOrEqual(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitLoop(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitLowPassFilter(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitMaximum(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitMinimum(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitMultiply(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitNarrower(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitNegative(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitNoise(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitNot(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitNotEqual(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitNumber(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitOneOverX(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitOr(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitPatchInlet(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitPatchOutlet(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitPower(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitPulse(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitPulseTrigger(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitRandom(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitRange(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitResample(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitReset(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitReverse(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitRound(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitSampleOperator(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitSawDown(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitSawUp(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitScaler(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitSelect(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitSetDimension(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitShift(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitSine(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitSlowDown(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitSpectrum(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitSpeedUp(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitSquare(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitStretch(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitSubtract(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitTimePower(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitTriangle(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitToggleTrigger(Operator op)
        {
            VisitOperatorBase(op);
        }

        [DebuggerHidden]
        protected virtual void VisitUnbundle(Operator op)
        {
            VisitOperatorBase(op);
        }
    }
}
