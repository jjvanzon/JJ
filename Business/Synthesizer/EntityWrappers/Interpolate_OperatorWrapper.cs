﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Exceptions;
using JJ.Business.Synthesizer.Enums;
using System;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class Interpolate_OperatorWrapper : OperatorWrapperBase_WithResult
    {
        private const int SIGNAL_INDEX = 0;
        private const int SAMPLING_RATE_INDEX = 1;

        public Interpolate_OperatorWrapper(Operator op)
            : base(op)
        { }

        public Outlet Signal
        {
            get { return SignalInlet.InputOutlet; }
            set { SignalInlet.LinkTo(value); }
        }

        public Inlet SignalInlet => OperatorHelper.GetInlet(WrappedOperator, SIGNAL_INDEX);

        public Outlet SamplingRate
        {
            get { return SamplingRateInlet.InputOutlet; }
            set { SamplingRateInlet.LinkTo(value); }
        }

        public Inlet SamplingRateInlet => OperatorHelper.GetInlet(WrappedOperator, SAMPLING_RATE_INDEX);

        public ResampleInterpolationTypeEnum InterpolationType
        {
            get { return DataPropertyParser.GetEnum<ResampleInterpolationTypeEnum>(WrappedOperator, PropertyNames.InterpolationType); }
            set { DataPropertyParser.SetValue(WrappedOperator, PropertyNames.InterpolationType, value); }
        }

        public override string GetInletDisplayName(int listIndex)
        {
            switch (listIndex)
            {
                case SIGNAL_INDEX:
                    {
                        string name = ResourceHelper.GetPropertyDisplayName(() => Signal);
                        return name;
                    }

                case SAMPLING_RATE_INDEX:
                    {
                        string name = ResourceHelper.GetPropertyDisplayName(() => SamplingRate);
                        return name;
                    }

                default:
                    throw new InvalidIndexException(() => listIndex, () => WrappedOperator.Inlets.Count);
            }
        }
    }
}
