﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Reflection.Exceptions;
using JJ.Business.Synthesizer.Enums;
using System;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class SawDown_OperatorWrapper : OperatorWrapperBase
    {
        private const int FREQUENCY_INDEX = 0;
        private const int PHASE_SHIFT_INDEX = 1;
        private const int RESULT_INDEX = 0;

        public SawDown_OperatorWrapper(Operator op)
            : base(op)
        { }

        public Outlet Frequency
        {
            get { return FrequencyInlet.InputOutlet; }
            set { FrequencyInlet.LinkTo(value); }
        }

        public Inlet FrequencyInlet
        {
            get { return OperatorHelper.GetInlet(WrappedOperator, FREQUENCY_INDEX); }
        }

        public Outlet PhaseShift
        {
            get { return PhaseShiftInlet.InputOutlet; }
            set { PhaseShiftInlet.LinkTo(value); }
        }

        public Inlet PhaseShiftInlet
        {
            get { return OperatorHelper.GetInlet(WrappedOperator, PHASE_SHIFT_INDEX); }
        }

        public Outlet Result
        {
            get { return OperatorHelper.GetOutlet(WrappedOperator, RESULT_INDEX); }
        }

        public override string GetInletDisplayName(int listIndex)
        {
            switch (listIndex)
            {
                case FREQUENCY_INDEX:
                    {
                        string name = ResourceHelper.GetPropertyDisplayName(() => Frequency);
                        return name;
                    }

                case PHASE_SHIFT_INDEX:
                    {
                        string name = ResourceHelper.GetPropertyDisplayName(() => PhaseShift);
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

        public static implicit operator Outlet(SawDown_OperatorWrapper wrapper)
        {
            if (wrapper == null) return null;

            return wrapper.Result;
        }
    }
}