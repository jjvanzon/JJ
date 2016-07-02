﻿using System;
using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Reflection.Exceptions;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public abstract class OperatorWrapperBase_AggregateFollower : OperatorWrapperBase
    {
        private const int SIGNAL_INDEX = 0;
        private const int SLICE_LENGTH_INDEX = 1;
        private const int SAMPLE_COUNT_INDEX = 2;
        private const int RESULT_INDEX = 0;

        public OperatorWrapperBase_AggregateFollower(Operator op)
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

        public Outlet SliceLength
        {
            get { return SliceLengthInlet.InputOutlet; }
            set { SliceLengthInlet.LinkTo(value); }
        }

        public Inlet SliceLengthInlet
        {
            get { return OperatorHelper.GetInlet(WrappedOperator, SLICE_LENGTH_INDEX); }
        }

        public Outlet SampleCount
        {
            get { return SampleCountInlet.InputOutlet; }
            set { SampleCountInlet.LinkTo(value); }
        }

        public Inlet SampleCountInlet
        {
            get { return OperatorHelper.GetInlet(WrappedOperator, SAMPLE_COUNT_INDEX); }
        }

        public Outlet Result
        {
            get { return OperatorHelper.GetOutlet(WrappedOperator, RESULT_INDEX); }
        }

        public DimensionEnum Dimension
        {
            get { return DataPropertyParser.GetEnum<DimensionEnum>(WrappedOperator, PropertyNames.Dimension); }
            set { DataPropertyParser.SetValue(WrappedOperator, PropertyNames.Dimension, value); }
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

                case SLICE_LENGTH_INDEX:
                    {
                        string name = ResourceHelper.GetPropertyDisplayName(() => SliceLength);
                        return name;
                    }

                case SAMPLE_COUNT_INDEX:
                    {
                        string name = ResourceHelper.GetPropertyDisplayName(() => SampleCount);
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
    }
}
