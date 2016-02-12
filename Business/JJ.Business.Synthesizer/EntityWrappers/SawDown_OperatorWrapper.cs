﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class SawDown_OperatorWrapper : OperatorWrapperBase
    {
        public SawDown_OperatorWrapper(Operator op)
            : base(op)
        { }

        public Outlet Frequency
        {
            get { return OperatorHelper.GetInputOutlet(_wrappedOperator, OperatorConstants.SAW_DOWN_FREQUENCY_INDEX); }
            set { OperatorHelper.GetInlet(_wrappedOperator, OperatorConstants.SAW_DOWN_FREQUENCY_INDEX).LinkTo(value); }
        }

        public Outlet PhaseShift
        {
            get { return OperatorHelper.GetInputOutlet(_wrappedOperator, OperatorConstants.SAW_DOWN_PHASE_SHIFT_INDEX); }
            set { OperatorHelper.GetInlet(_wrappedOperator, OperatorConstants.SAW_DOWN_PHASE_SHIFT_INDEX).LinkTo(value); }
        }

        public Outlet Result
        {
            get { return OperatorHelper.GetOutlet(_wrappedOperator, OperatorConstants.SAW_DOWN_RESULT_INDEX); }
        }

        public static implicit operator Outlet(SawDown_OperatorWrapper wrapper)
        {
            if (wrapper == null) return null;

            return wrapper.Result;
        }
    }
}