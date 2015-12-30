﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Api.Helpers;
using JJ.Business.Synthesizer.Calculation.Patches;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Managers;
using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Api
{
    public class PatchApi
    {
        private readonly PatchManager _patchManager;

        public Patch Patch
        {
            get { return _patchManager.Patch; }
        }

        public PatchApi()
        {
            _patchManager = new PatchManager(RepositoryHelper.PatchRepositories);
            _patchManager.CreatePatch();
        }

        public Add_OperatorWrapper Add(Outlet operandA = null, Outlet operandB = null)
        {
            return _patchManager.Add(operandA, operandB);
        }

        public Adder_OperatorWrapper Adder(params Outlet[] operands)
        {
            return _patchManager.Adder(operands);
        }

        public Adder_OperatorWrapper Adder(IList<Outlet> operands)
        {
            return _patchManager.Adder(operands);
        }

        public Curve_OperatorWrapper Curve(Curve curve = null)
        {
            return _patchManager.Curve(curve);
        }

        public CustomOperator_OperatorWrapper CustomOperator()
        {
            return _patchManager.CustomOperator();
        }

        public CustomOperator_OperatorWrapper CustomOperator(Patch underlyingPatch)
        {
            return _patchManager.CustomOperator(underlyingPatch);
        }

        /// <param name="underlyingPatch">The Patch to base the CustomOperator on.</param>
        public CustomOperator_OperatorWrapper CustomOperator(Patch underlyingPatch, params Outlet[] operands)
        {
            return _patchManager.CustomOperator(underlyingPatch, operands);
        }

        /// <param name="underlyingPatch">The Patch to base the CustomOperator on.</param>
        public CustomOperator_OperatorWrapper CustomOperator(Patch underlyingPatch, IList<Outlet> operands)
        {
            return _patchManager.CustomOperator(underlyingPatch, operands);
        }

        public Delay_OperatorWrapper Delay(Outlet signal = null, Outlet timeDifference = null)
        {
            return _patchManager.Delay(signal, timeDifference);
        }

        public Divide_OperatorWrapper Divide(Outlet numerator = null, Outlet denominator = null, Outlet origin = null)
        {
            return _patchManager.Divide(numerator, denominator, origin);
        }

        public Exponent_OperatorWrapper Exponent(Outlet low = null, Outlet high = null, Outlet ratio = null)
        {
            return _patchManager.Exponent(low, high, ratio);
        }

        public Loop_OperatorWrapper Loop(
            Outlet signal = null,
            Outlet attack = null,
            Outlet start = null,
            Outlet sustain = null,
            Outlet end = null,
            Outlet release = null)
        {
            return _patchManager.Loop(signal, attack, start, sustain, end, release);
        }

        public Multiply_OperatorWrapper Multiply(Outlet operandA = null, Outlet operandB = null, Outlet origin = null)
        {
            return _patchManager.Multiply(operandA, operandB, origin);
        }

        public Number_OperatorWrapper Number(double number = 0)
        {
            return _patchManager.Number(number);
        }

        public PatchInlet_OperatorWrapper Inlet(InletTypeEnum inletTypeEnum)
        {
            return _patchManager.Inlet(inletTypeEnum);
        }

        public PatchInlet_OperatorWrapper Inlet(InletTypeEnum inletTypeEnum, double defaultValue)
        {
            return _patchManager.Inlet(inletTypeEnum, defaultValue);
        }

        public PatchInlet_OperatorWrapper Inlet(string name)
        {
            return _patchManager.Inlet(name);
        }

        public PatchInlet_OperatorWrapper Inlet(string name, double defaultValue)
        {
            return _patchManager.Inlet(name, defaultValue);
        }

        public PatchInlet_OperatorWrapper Inlet()
        {
            return _patchManager.Inlet();
        }

        public PatchOutlet_OperatorWrapper Outlet(Outlet input = null)
        {
            return _patchManager.Outlet(input);
        }

        public Power_OperatorWrapper Power(Outlet @base = null, Outlet exponent = null)
        {
            return _patchManager.Power(@base, exponent);
        }

        public Resample_OperatorWrapper Resample(Outlet signal = null, Outlet samplingRate = null)
        {
            return _patchManager.Resample(signal, samplingRate);
        }

        public SawTooth_OperatorWrapper SawTooth(Outlet frequency = null, Outlet phaseShift = null)
        {
            return _patchManager.SawTooth(frequency, phaseShift);
        }

        public Sample_OperatorWrapper Sample(Sample sample = null)
        {
            return _patchManager.Sample(sample);
        }

        public Select_OperatorWrapper Select(Outlet signal = null, Outlet time = null)
        {
            return _patchManager.Select(signal, time);
        }

        public Sine_OperatorWrapper Sine(Outlet frequency = null, Outlet phaseShift = null)
        {
            return _patchManager.Sine(frequency, phaseShift);
        }

        public Subtract_OperatorWrapper Subtract(Outlet operandA = null, Outlet operandB = null)
        {
            return _patchManager.Subtract(operandA, operandB);
        }

        public SpeedUp_OperatorWrapper SpeedUp(Outlet signal = null, Outlet timeDivider = null, Outlet origin = null)
        {
            return _patchManager.SpeedUp(signal, timeDivider, origin);
        }

        public SlowDown_OperatorWrapper SlowDown(Outlet signal = null, Outlet timeMultiplier = null, Outlet origin = null)
        {
            return _patchManager.SlowDown(signal, timeMultiplier, origin);
        }

        public SquareWave_OperatorWrapper SquareWave(Outlet frequency = null, Outlet phaseShift = null)
        {
            return _patchManager.SquareWave(frequency, phaseShift);
        }

        public TimePower_OperatorWrapper TimePower(Outlet signal = null, Outlet exponent = null, Outlet origin = null)
        {
            return _patchManager.TimePower(signal, exponent, origin);
        }

        public Earlier_OperatorWrapper Earlier(Outlet signal = null, Outlet timeDifference = null)
        {
            return _patchManager.Earlier(signal, timeDifference);
        }

        public TriangleWave_OperatorWrapper TriangleWave(Outlet frequency = null, Outlet phaseShift = null)
        {
            return _patchManager.TriangleWave(frequency, phaseShift);
        }

        public WhiteNoise_OperatorWrapper WhiteNoise()
        {
            return _patchManager.WhiteNoise();
        }

        public IPatchCalculator CreateOptimizedCalculator(params Outlet[] channelOutlets)
        {
            return _patchManager.CreateOptimizedCalculator(channelOutlets);
        }

        public IPatchCalculator CreateInterpretedCalculator(params Outlet[] channelOutlets)
        {
            return _patchManager.CreateInterpretedCalculator(channelOutlets);
        }
    }
}
