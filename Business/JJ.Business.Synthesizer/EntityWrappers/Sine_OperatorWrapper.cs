﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class Sine_OperatorWrapper : OperatorWrapperBase
    {
        public Sine_OperatorWrapper(Operator op)
            : base(op)
        { }

        public Outlet Frequency
        {
            get { return OperatorHelper.GetInputOutlet(_operator, OperatorConstants.SINE_FREQUENCY_INDEX); }
            set { OperatorHelper.GetInlet(_operator, OperatorConstants.SINE_FREQUENCY_INDEX).LinkTo(value); }
        }

        public Outlet PhaseShift
        {
            get { return OperatorHelper.GetInlet(_operator, OperatorConstants.SINE_PHASE_SHIFT_INDEX).InputOutlet; }
            set { OperatorHelper.GetInlet(_operator, OperatorConstants.SINE_PHASE_SHIFT_INDEX).LinkTo(value); }
        }

        public Outlet Result
        {
            get { return OperatorHelper.GetOutlet(_operator, OperatorConstants.SINE_RESULT_INDEX); }
        }

        public static implicit operator Outlet(Sine_OperatorWrapper wrapper)
        {
            if (wrapper == null) return null;

            return wrapper.Result;
        }
    }
}