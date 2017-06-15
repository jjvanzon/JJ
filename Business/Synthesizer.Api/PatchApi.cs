﻿using System.Collections.Generic;
using JJ.Business.Synthesizer.Api.Helpers;
using JJ.Business.Synthesizer.Calculation.Patches;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Calculation;
using JJ.Data.Synthesizer.Entities;

namespace JJ.Business.Synthesizer.Api
{
    public class PatchApi
    {
        private readonly PatchManager _patchManager;

        public Patch Patch => _patchManager.Patch;

        public PatchApi()
        {
            _patchManager = new PatchManager(RepositoryHelper.Repositories);
            _patchManager.CreatePatch();
        }

        public OperatorWrapper_WithUnderlyingPatch Absolute(Outlet number = null)
            => _patchManager.Absolute(number);

        public Add_OperatorWrapper Add(params Outlet[] items) 
            => _patchManager.Add(items);

        public Add_OperatorWrapper Add(IList<Outlet> items)
            => _patchManager.Add(items);

        public OperatorWrapper_WithUnderlyingPatch And(Outlet a = null, Outlet b = null)
            => _patchManager.And(a, b);

        public AllPassFilter_OperatorWrapper AllPassFilter(
            Outlet sound = null, 
            Outlet centerFrequency = null, 
            Outlet width = null)
            => _patchManager.AllPassFilter(sound, centerFrequency, width);

        public AverageFollower_OperatorWrapper AverageFollower(
            Outlet signal = null,
            Outlet sliceLength = null,
            Outlet sampleCount = null,
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.AverageFollower(signal, sliceLength, sampleCount, standardDimension, customDimension);

        public AverageOverDimension_OperatorWrapper AverageOverDimension(
            Outlet signal = null,
            Outlet from = null,
            Outlet till = null,
            Outlet step = null,
            DimensionEnum standardDimension = DimensionEnum.Undefined,
            string customDimension = null,
            CollectionRecalculationEnum collectionRecalculation = CollectionRecalculationEnum.Continuous)
            => _patchManager.AverageOverDimension(signal, from, till, step, standardDimension, customDimension, collectionRecalculation);

        public AverageOverInlets_OperatorWrapper AverageOverInlets(params Outlet[] items)
            => _patchManager.AverageOverInlets(items);

        public AverageOverInlets_OperatorWrapper AverageOverInlets(IList<Outlet> items)
            => _patchManager.AverageOverInlets(items);

        public BandPassFilterConstantPeakGain_OperatorWrapper BandPassFilterConstantPeakGain(
            Outlet sound = null,
            Outlet centerFrequency = null,
            Outlet width = null)
            => _patchManager.BandPassFilterConstantPeakGain(sound, centerFrequency, width);

        public BandPassFilterConstantTransitionGain_OperatorWrapper BandPassFilterConstantTransitionGain(
            Outlet sound = null,
            Outlet centerFrequency = null,
            Outlet width = null)
            => _patchManager.BandPassFilterConstantTransitionGain(sound, centerFrequency, width);

        public Cache_OperatorWrapper Cache(
            Outlet signal = null,
            Outlet start = null,
            Outlet end = null,
            Outlet samplingRate = null,
            InterpolationTypeEnum interpolationType = InterpolationTypeEnum.Line,
            SpeakerSetupEnum speakerSetup = SpeakerSetupEnum.Mono,
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.Cache(
                signal, 
                start, 
                end, 
                samplingRate, 
                interpolationType, 
                speakerSetup, 
                standardDimension,
                customDimension);

        public ChangeTrigger_OperatorWrapper ChangeTrigger(Outlet passThrough, Outlet reset)
            => _patchManager.ChangeTrigger(passThrough, reset);

        public ClosestOverInlets_OperatorWrapper ClosestOverInlets(Outlet input, params Outlet[] items)
            => _patchManager.ClosestOverInlets(input, items);

        public ClosestOverInlets_OperatorWrapper ClosestOverInlets(Outlet input, IList<Outlet> items)
            => _patchManager.ClosestOverInlets(input, items);

        public ClosestOverInletsExp_OperatorWrapper ClosestExpOverInlets(Outlet input, params Outlet[] items)
            => _patchManager.ClosestOverInletsExp(input, items);

        public ClosestOverInletsExp_OperatorWrapper ClosestExpOverInlets(Outlet input, IList<Outlet> items)
            => _patchManager.ClosestOverInletsExp(input, items);

        public ClosestOverDimension_OperatorWrapper ClosestOverDimension(
            Outlet input = null,
            Outlet collection = null,
            Outlet from = null,
            Outlet till = null,
            Outlet step = null,
            DimensionEnum standardDimension = DimensionEnum.Undefined,
            string customDimension = null,
            CollectionRecalculationEnum collectionRecalculation = CollectionRecalculationEnum.Continuous)
            => _patchManager.ClosestOverDimension(input, from, till, step, collection, standardDimension, customDimension, collectionRecalculation);

        public ClosestOverDimensionExp_OperatorWrapper ClosestOverDimensionExp(
            Outlet input = null,
            Outlet collection = null,
            Outlet from = null,
            Outlet till = null,
            Outlet step = null,
            DimensionEnum standardDimension = DimensionEnum.Undefined,
            string customDimension = null,
            CollectionRecalculationEnum collectionRecalculation = CollectionRecalculationEnum.Continuous)
            => _patchManager.ClosestOverDimensionExp(input, from, till, step, collection, standardDimension, customDimension, collectionRecalculation);

        public Curve_OperatorWrapper Curve(Curve curve = null, DimensionEnum standardDimension = DimensionEnum.Time, string customDimension = null)
            => _patchManager.Curve(curve, standardDimension, customDimension);

        public OperatorWrapper_WithUnderlyingPatch CustomOperator()
            => _patchManager.CustomOperator();

        public OperatorWrapper_WithUnderlyingPatch CustomOperator(Patch underlyingPatch)
            => _patchManager.CustomOperator(underlyingPatch);

        /// <param name="underlyingPatch">The Patch to base the CustomOperator on.</param>
        public OperatorWrapper_WithUnderlyingPatch CustomOperator(Patch underlyingPatch, params Outlet[] operands)
            => _patchManager.CustomOperator(underlyingPatch, operands);

        /// <param name="underlyingPatch">The Patch to base the CustomOperator on.</param>
        public OperatorWrapper_WithUnderlyingPatch CustomOperator(Patch underlyingPatch, IList<Outlet> operands)
            => _patchManager.CustomOperator(underlyingPatch, operands);

        public Divide_OperatorWrapper Divide(Outlet a = null, Outlet b = null, Outlet origin = null)
            => _patchManager.Divide(a, b, origin);

        public OperatorWrapper_WithUnderlyingPatch Equal(Outlet a = null, Outlet b = null)
            => _patchManager.Equal(a, b);

        public Exponent_OperatorWrapper Exponent(Outlet low = null, Outlet high = null, Outlet ratio = null)
            => _patchManager.Exponent(low, high, ratio);

        public GetDimension_OperatorWrapper GetDimension(DimensionEnum standardDimension = DimensionEnum.Undefined, string customDimension = null)
            => _patchManager.GetDimension(standardDimension, customDimension);

        public OperatorWrapper_WithUnderlyingPatch GreaterThan(Outlet a = null, Outlet b = null)
            => _patchManager.GreaterThan(a, b);

        public OperatorWrapper_WithUnderlyingPatch GreaterThanOrEqual(Outlet a = null, Outlet b = null)
            => _patchManager.GreaterThanOrEqual(a, b);

        public HighPassFilter_OperatorWrapper HighPassFilter(
            Outlet sound = null, 
            Outlet minFrequency = null,
            Outlet blobVolume = null)
            => _patchManager.HighPassFilter(sound, minFrequency, blobVolume);

        public HighShelfFilter_OperatorWrapper HighShelfFilter(
            Outlet sound = null,
            Outlet transitionFrequency = null,
            Outlet transitionSlope = null,
            Outlet dbGain = null)
            => _patchManager.HighShelfFilter(sound, transitionFrequency, transitionSlope, dbGain);

        public If_OperatorWrapper If(Outlet condition = null, Outlet then = null, Outlet @else = null)
            => _patchManager.If(condition, then, @else);

        public OperatorWrapper_WithUnderlyingPatch LessThan(Outlet a = null, Outlet b = null)
            => _patchManager.LessThan(a, b);

        public OperatorWrapper_WithUnderlyingPatch LessThanOrEqual(Outlet a = null, Outlet b = null)
            => _patchManager.LessThanOrEqual(a, b);

        public Loop_OperatorWrapper Loop(
            Outlet signal = null,
            Outlet skip = null,
            Outlet loopStartMarker = null,
            Outlet loopEndMarker = null,
            Outlet releaseEndMarker = null,
            Outlet noteDuration = null,
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.Loop(signal, skip, loopStartMarker, loopEndMarker, releaseEndMarker, noteDuration, standardDimension, customDimension);

        public LowPassFilter_OperatorWrapper LowPassFilter(
            Outlet sound = null, 
            Outlet maxFrequency = null,
            Outlet width = null)
            => _patchManager.LowPassFilter(sound, maxFrequency, width);

        public LowShelfFilter_OperatorWrapper LowShelfFilter(
            Outlet sound = null,
            Outlet shelfFrequency = null,
            Outlet shelfSlope = null,
            Outlet dbGain = null)
            => _patchManager.LowShelfFilter(sound, shelfFrequency, shelfSlope, dbGain);

        public InletsToDimension_OperatorWrapper InletsToDimension(ResampleInterpolationTypeEnum interpolation, DimensionEnum standardDimension, params Outlet[] operands)
            => _patchManager.InletsToDimension(interpolation, standardDimension, operands);

        public InletsToDimension_OperatorWrapper InletsToDimension(ResampleInterpolationTypeEnum interpolation, params Outlet[] operands)
            => _patchManager.InletsToDimension(interpolation, operands);

        public InletsToDimension_OperatorWrapper InletsToDimension(DimensionEnum standardDimension, params Outlet[] operands)
            => _patchManager.InletsToDimension(standardDimension, operands);

        public InletsToDimension_OperatorWrapper InletsToDimension(params Outlet[] operands)
            => _patchManager.InletsToDimension(operands);

        public InletsToDimension_OperatorWrapper InletsToDimension(IList<Outlet> operands, ResampleInterpolationTypeEnum interpolation, DimensionEnum standardDimension)
            => _patchManager.InletsToDimension(operands, interpolation, standardDimension);

        public InletsToDimension_OperatorWrapper InletsToDimension(IList<Outlet> operands, ResampleInterpolationTypeEnum interpolation)
            => _patchManager.InletsToDimension(operands, interpolation);

        public InletsToDimension_OperatorWrapper InletsToDimension(IList<Outlet> operands, DimensionEnum standardDimension)
            => _patchManager.InletsToDimension(operands, standardDimension);

        public InletsToDimension_OperatorWrapper InletsToDimension(IList<Outlet> operands)
            => _patchManager.InletsToDimension(operands);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(Outlet operand, DimensionEnum standardDimension, string customDimension, int outletCount)
            => _patchManager.DimensionToOutlets(operand, standardDimension, customDimension, outletCount);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(Outlet operand, DimensionEnum standardDimension, string customDimension)
            => _patchManager.DimensionToOutlets(operand, standardDimension, customDimension);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(Outlet operand, DimensionEnum standardDimension, int outletCount)
            => _patchManager.DimensionToOutlets(operand, standardDimension, outletCount);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(Outlet operand, DimensionEnum standardDimension)
            => _patchManager.DimensionToOutlets(operand, standardDimension);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(Outlet operand, string customDimension, int outletCount)
            => _patchManager.DimensionToOutlets(operand, customDimension, outletCount);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(Outlet operand, string customDimension)
            => _patchManager.DimensionToOutlets(operand, customDimension);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(Outlet operand, int outletCount)
            => _patchManager.DimensionToOutlets(operand, outletCount);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(Outlet operand)
            => _patchManager.DimensionToOutlets(operand);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(DimensionEnum standardDimension, string customDimension, int outletCount)
            => _patchManager.DimensionToOutlets(standardDimension, customDimension, outletCount);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(DimensionEnum standardDimension, string customDimension)
            => _patchManager.DimensionToOutlets(standardDimension, customDimension);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(DimensionEnum standardDimension, int outletCount)
            => _patchManager.DimensionToOutlets(standardDimension, outletCount);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(DimensionEnum standardDimension)
            => _patchManager.DimensionToOutlets(standardDimension);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(int outletCount, string customDimension)
            => _patchManager.DimensionToOutlets(outletCount, customDimension);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(string customDimension)
            => _patchManager.DimensionToOutlets(customDimension);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets(int outletCount)
            => _patchManager.DimensionToOutlets(outletCount);

        public DimensionToOutlets_OperatorWrapper DimensionToOutlets()
            => _patchManager.DimensionToOutlets();

        public MaxOverInlets_OperatorWrapper MaxOverInlets(params Outlet[] operands)
            => _patchManager.MaxOverInlets(operands);

        public MaxOverInlets_OperatorWrapper MaxOverInlets(IList<Outlet> operands)
            => _patchManager.MaxOverInlets(operands);

        public MaxFollower_OperatorWrapper MaxFollower(
            Outlet signal = null,
            Outlet sliceLength = null,
            Outlet sampleCount = null,
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.MaxFollower(signal, sliceLength, sampleCount, standardDimension, customDimension);

        public MaxOverDimension_OperatorWrapper MaxOverDimension(
            Outlet signal = null,
            Outlet from = null, 
            Outlet till = null, 
            Outlet step = null, 
            DimensionEnum standardDimension = DimensionEnum.Undefined,
            string customDimension = null,
            CollectionRecalculationEnum collectionRecalculation = CollectionRecalculationEnum.Continuous)
            => _patchManager.MaxOverDimension(signal, from, till, step, standardDimension, customDimension, collectionRecalculation);

        public MinOverInlets_OperatorWrapper MinOverInlets(params Outlet[] operands)
            => _patchManager.MinOverInlets(operands);

        public MinOverInlets_OperatorWrapper MinOverInlets(IList<Outlet> operands)
            => _patchManager.MinOverInlets(operands);

        public MinFollower_OperatorWrapper MinFollower(
            Outlet signal = null, 
            Outlet sliceLength = null, 
            Outlet sampleCount = null,
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.MinFollower(signal, sliceLength, sampleCount, standardDimension);

        public MinOverDimension_OperatorWrapper MinOverDimension(
            Outlet signal = null,
            Outlet from = null,
            Outlet till = null,
            Outlet step = null,
            DimensionEnum standardDimension = DimensionEnum.Undefined,
            string customDimension = null,
            CollectionRecalculationEnum collectionRecalculation = CollectionRecalculationEnum.Continuous)
            => _patchManager.MinOverDimension(signal, from, till, step, standardDimension, customDimension, collectionRecalculation);

        public Multiply_OperatorWrapper Multiply(params Outlet[] operands)
            => _patchManager.Multiply(operands);

        public Multiply_OperatorWrapper Multiply(IList<Outlet> operands)
            => _patchManager.Multiply(operands);

        public MultiplyWithOrigin_OperatorWrapper MultiplyWithOrigin(Outlet a = null, Outlet b = null, Outlet origin = null)
            => _patchManager.MultiplyWithOrigin(a, b, origin);

        public Squash_OperatorWrapper Squash(
            Outlet signal = null, 
            Outlet factor = null, 
            Outlet origin = null,
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.Squash(signal, factor, origin, standardDimension, customDimension);

        public Negative_OperatorWrapper Negative(Outlet number = null)
            => _patchManager.Negative(number);

        public Noise_OperatorWrapper Noise(DimensionEnum standardDimension = DimensionEnum.Time, string customDimension = null)
            => _patchManager.Noise(standardDimension, customDimension);

        public Not_OperatorWrapper Not(Outlet number = null)
            => _patchManager.Not(number);

        public NotchFilter_OperatorWrapper NotchFilter(
            Outlet sound = null, 
            Outlet centerFrequency = null, 
            Outlet width = null)
            => _patchManager.NotchFilter(sound, centerFrequency, width);

        public OperatorWrapper_WithUnderlyingPatch NotEqual(Outlet a = null, Outlet b = null)
            => _patchManager.NotEqual(a, b);

        public Number_OperatorWrapper Number(double number = 0)
            => _patchManager.Number(number);

        public OneOverX_OperatorWrapper OneOverX(Outlet number = null)
            => _patchManager.OneOverX(number);

        public Or_OperatorWrapper Or(Outlet a = null, Outlet b = null)
            => _patchManager.Or(a, b);

        public PatchInlet_OperatorWrapper PatchInlet()
            => _patchManager.PatchInlet();

        public PatchInlet_OperatorWrapper PatchInlet(DimensionEnum dimension)
            => _patchManager.PatchInlet(dimension);

        public PatchInlet_OperatorWrapper PatchInlet(string name)
            => _patchManager.PatchInlet(name);

        public PatchInlet_OperatorWrapper PatchInlet(string name, double defaultValue)
            => _patchManager.PatchInlet(name, defaultValue);

        public PatchInlet_OperatorWrapper PatchInlet(DimensionEnum dimension, double defaultValue)
            => _patchManager.PatchInlet(dimension, defaultValue);

        public PatchOutlet_OperatorWrapper PatchOutlet(Outlet input = null)
            => _patchManager.PatchOutlet(input);

        public PatchOutlet_OperatorWrapper PatchOutlet(DimensionEnum dimension, Outlet input = null)
            => _patchManager.PatchOutlet(dimension, input);

        public PatchOutlet_OperatorWrapper PatchOutlet(string name, Outlet input = null)
            => _patchManager.PatchOutlet(name, input);

        public PeakingEQFilter_OperatorWrapper PeakingEQFilter(
            Outlet sound = null,
            Outlet centerFrequency = null,
            Outlet width = null,
            Outlet dbGain = null)
            => _patchManager.PeakingEQFilter(sound, centerFrequency, width, dbGain);

        public Power_OperatorWrapper Power(Outlet @base = null, Outlet exponent = null)
            => _patchManager.Power(@base, exponent);

        public Pulse_OperatorWrapper Pulse(
            Outlet frequency = null, 
            Outlet width = null, 
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.Pulse(frequency, width, standardDimension, customDimension);

        public PulseTrigger_OperatorWrapper PulseTrigger(Outlet calculation, Outlet reset)
            => _patchManager.PulseTrigger(calculation, reset);

        public Random_OperatorWrapper Random(Outlet rate = null, DimensionEnum standardDimension = DimensionEnum.Time, string customDimension = null)
            => _patchManager.Random(rate, standardDimension, customDimension);

        public RangeOverDimension_OperatorWrapper RangeOverDimension(
            Outlet from = null,
            Outlet till = null,
            Outlet step = null,
            DimensionEnum standardDimension = DimensionEnum.Undefined,
            string customDimension = null)
            => _patchManager.RangeOverDimension(from, till, step, standardDimension, customDimension);

        public RangeOverOutlets_OperatorWrapper RangeOverOutlets(
            Outlet from = null,
            Outlet step = null,
            DimensionEnum standardDimension = DimensionEnum.Undefined,
            string customDimension = null,
            int? outletCount = null)
            => _patchManager.RangeOverOutlets(from, step, standardDimension, customDimension, outletCount);

        public Interpolate_OperatorWrapper Interpolate(
            Outlet signal = null,
            Outlet samplingRate = null,
            ResampleInterpolationTypeEnum interpolationType = ResampleInterpolationTypeEnum.CubicSmoothSlope,
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.Interpolate(signal, samplingRate, interpolationType, standardDimension, customDimension);

        public Reset_OperatorWrapper Reset(Outlet passThrough = null, int? listIndex = null)
            => _patchManager.Reset(passThrough, listIndex);

        public Reverse_OperatorWrapper Reverse(
            Outlet signal = null, 
            Outlet factor = null,
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.Reverse(signal, factor, standardDimension, customDimension);

        public Round_OperatorWrapper Round(Outlet signal = null, Outlet step = null, Outlet offset = null)
            => _patchManager.Round(signal, step, offset);

        public Sample_OperatorWrapper Sample(Sample sample = null, Outlet frequency = null, DimensionEnum standardDimension = DimensionEnum.Time, string customDimension = null)
            => _patchManager.Sample(sample, frequency, standardDimension, customDimension);

        public SawDown_OperatorWrapper SawDown(Outlet frequency = null, DimensionEnum standardDimension = DimensionEnum.Time, string customDimension = null)
            => _patchManager.SawDown(frequency, standardDimension, customDimension);

        public SawUp_OperatorWrapper SawUp(Outlet frequency = null, DimensionEnum standardDimension = DimensionEnum.Time, string customDimension = null)
            => _patchManager.SawUp(frequency, standardDimension, customDimension);

        public Scaler_OperatorWrapper Scaler(
            Outlet signal = null,
            Outlet sourceValueA = null,
            Outlet sourceValueB = null,
            Outlet targetValueA = null,
            Outlet targetValueB = null)
            => _patchManager.Scaler(signal, sourceValueA, sourceValueB, targetValueA, targetValueB);

        public SetDimension_OperatorWrapper SetDimension(
            Outlet calculation = null, 
            Outlet number = null, 
            DimensionEnum standardDimension = DimensionEnum.Undefined, 
            string customDimension = null)
            => _patchManager.SetDimension(calculation, number, standardDimension, customDimension);

        public Shift_OperatorWrapper Shift(
            Outlet signal = null, 
            Outlet difference = null,
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.Shift(signal, difference, standardDimension, customDimension);

        public OperatorWrapper_WithUnderlyingPatch Sine(Outlet frequency = null) => _patchManager.Sine(frequency);

        public SortOverInlets_OperatorWrapper SortOverInlets(params Outlet[] operands)
            => _patchManager.SortOverInlets(operands);

        public SortOverInlets_OperatorWrapper SortOverInlets(IList<Outlet> operands)
            => _patchManager.SortOverInlets(operands);

        public SortOverDimension_OperatorWrapper SortOverDimension(
            Outlet signal = null,
            Outlet from = null,
            Outlet till = null,
            Outlet step = null,
            DimensionEnum standardDimension = DimensionEnum.Undefined,
            string customDimension = null,
            CollectionRecalculationEnum collectionRecalculation = CollectionRecalculationEnum.Continuous)
            => _patchManager.SortOverDimension(signal, from, till, step, standardDimension, customDimension, collectionRecalculation);

        public Spectrum_OperatorWrapper Spectrum(
            Outlet sound = null,
            Outlet start = null,
            Outlet end = null,
            Outlet samplingRate = null,
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.Spectrum(sound, start, end, samplingRate, standardDimension, customDimension);

        public Square_OperatorWrapper Square(Outlet frequency = null, DimensionEnum standardDimension = DimensionEnum.Time, string customDimension = null)
            => _patchManager.Square(frequency, standardDimension, customDimension);

        public Stretch_OperatorWrapper Stretch(
            Outlet signal = null, 
            Outlet factor = null, 
            Outlet origin = null, 
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.Stretch(signal, factor, origin, standardDimension, customDimension);

        public Subtract_OperatorWrapper Subtract(Outlet a = null, Outlet b = null)
            => _patchManager.Subtract(a, b);

        public SumOverDimension_OperatorWrapper SumOverDimension(
            Outlet signal = null,
            Outlet from = null,
            Outlet till = null,
            Outlet step = null,
            DimensionEnum standardDimension = DimensionEnum.Undefined,
            string customDimension = null,
            CollectionRecalculationEnum collectionRecalculation = CollectionRecalculationEnum.Continuous)
            => _patchManager.SumOverDimension(signal, from, till, step, standardDimension, customDimension, collectionRecalculation);

        public SumFollower_OperatorWrapper SumFollower(
            Outlet signal = null,
            Outlet sliceLength = null,
            Outlet sampleCount = null,
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.SumFollower(signal, sliceLength, sampleCount, standardDimension, customDimension);

        public TimePower_OperatorWrapper TimePower(
            Outlet signal = null, 
            Outlet exponent = null, 
            Outlet origin = null,
            DimensionEnum standardDimension = DimensionEnum.Time,
            string customDimension = null)
            => _patchManager.TimePower(signal, exponent, origin, standardDimension, customDimension);

        public ToggleTrigger_OperatorWrapper ToggleTrigger(Outlet passThrough, Outlet reset)
            => _patchManager.ToggleTrigger(passThrough, reset);

        public Triangle_OperatorWrapper Triangle(Outlet frequency = null, DimensionEnum standardDimension = DimensionEnum.Time, string customDimension = null)
            => _patchManager.Triangle(frequency, standardDimension, customDimension);

        public IPatchCalculator CreateCalculator(
            Outlet outlet,
            int samplingRate,
            int channelCount,
            int channelIndex,
            CalculatorCache calculatorCache,
            bool mustSubstituteSineForUnfilledInSignalPatchInlets = true)
            => _patchManager.CreateCalculator(outlet, samplingRate, channelCount, channelIndex, calculatorCache, mustSubstituteSineForUnfilledInSignalPatchInlets);
    }
}
