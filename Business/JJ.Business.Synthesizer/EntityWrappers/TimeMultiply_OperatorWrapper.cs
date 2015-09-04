﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class TimeMultiply_OperatorWrapper : OperatorWrapperBase
    {
        public TimeMultiply_OperatorWrapper(Operator op)
            : base(op)
        { }

        public Outlet Signal
        {
            get { return GetInlet(OperatorConstants.TIME_MULTIPLY_SIGNAL_INDEX).InputOutlet; }
            set { GetInlet(OperatorConstants.TIME_MULTIPLY_SIGNAL_INDEX).LinkTo(value); }
        }

        public Outlet TimeMultiplier
        {
            get { return GetInlet(OperatorConstants.TIME_MULTIPLY_TIME_MULTIPLIER_INDEX).InputOutlet; }
            set { GetInlet(OperatorConstants.TIME_MULTIPLY_TIME_MULTIPLIER_INDEX).LinkTo(value); }
        }

        public Outlet Origin
        {
            get { return GetInlet(OperatorConstants.TIME_MULTIPLY_ORIGIN_INDEX).InputOutlet; }
            set { GetInlet(OperatorConstants.TIME_MULTIPLY_ORIGIN_INDEX).LinkTo(value); }
        }

        public Outlet Result
        {
            get { return GetOutlet(OperatorConstants.TIME_MULTIPLY_RESULT_INDEX); }
        }

        public static implicit operator Outlet(TimeMultiply_OperatorWrapper wrapper)
        {
            if (wrapper == null) return null;

            return wrapper.Result;
        }
    }
}
