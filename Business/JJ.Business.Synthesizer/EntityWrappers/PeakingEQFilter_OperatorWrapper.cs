﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class PeakingEQFilter_OperatorWrapper : OperatorWrapperBase
    {
        private const int SIGNAL_INDEX = 0;
        private const int CENTER_FREQUENCY_INDEX = 1;
        private const int BAND_WIDTH_INDEX = 2;
        private const int DB_GAIN_INDEX = 3;
        private const int RESULT_INDEX = 0;

        public PeakingEQFilter_OperatorWrapper(Operator op)
            : base(op)
        { }

        public Outlet Signal
        {
            get { return SignalInlet.InputOutlet; }
            set { SignalInlet.LinkTo(value); }
        }

        public Inlet SignalInlet
        {
            get { return OperatorHelper.GetInlet(WrappedOperator, SIGNAL_INDEX); }
        }

        public Outlet CenterFrequency
        {
            get { return CenterFrequencyInlet.InputOutlet; }
            set { CenterFrequencyInlet.LinkTo(value); }
        }

        public Inlet CenterFrequencyInlet
        {
            get { return OperatorHelper.GetInlet(WrappedOperator, CENTER_FREQUENCY_INDEX); }
        }

        public Outlet BandWidth
        {
            get { return BandWidthInlet.InputOutlet; }
            set { BandWidthInlet.LinkTo(value); }
        }

        public Inlet BandWidthInlet
        {
            get { return OperatorHelper.GetInlet(WrappedOperator, BAND_WIDTH_INDEX); }
        }

        public Outlet DBGain
        {
            get { return DBGainInlet.InputOutlet; }
            set { DBGainInlet.LinkTo(value); }
        }

        public Inlet DBGainInlet
        {
            get { return OperatorHelper.GetInlet(WrappedOperator, DB_GAIN_INDEX); }
        }

        public Outlet Result
        {
            get { return OperatorHelper.GetOutlet(WrappedOperator, RESULT_INDEX); }
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

                case CENTER_FREQUENCY_INDEX:
                    {
                        string name = ResourceHelper.GetPropertyDisplayName(() => CenterFrequency);
                        return name;
                    }

                case BAND_WIDTH_INDEX:
                    {
                        string name = ResourceHelper.GetPropertyDisplayName(() => BandWidth);
                        return name;
                    }

                case DB_GAIN_INDEX:
                    {
                        string name = ResourceHelper.GetPropertyDisplayName(() => DBGain);
                        return name;
                    }

                default:
                    throw new InvalidIndexException(() => listIndex, () => WrappedOperator.Inlets.Count);
            }
        }

        public override string GetOutletDisplayName(int listIndex)
        {
            if (listIndex != 0) throw new NotEqualException(() => listIndex, 0);

            string name = ResourceHelper.GetPropertyDisplayName(() => Result);
            return name;
        }

        public static implicit operator Outlet(PeakingEQFilter_OperatorWrapper wrapper)
        {
            if (wrapper == null) return null;

            return wrapper.Result;
        }
    }
}