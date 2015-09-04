﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class TimePower_OperatorWrapper : OperatorWrapperBase
    {
        public TimePower_OperatorWrapper(Operator op)
            : base(op)
        { }

        public Outlet Signal
        {
            get { return GetInlet(OperatorConstants.TIME_POWER_SIGNAL_INDEX).InputOutlet; }
            set { GetInlet(OperatorConstants.TIME_POWER_SIGNAL_INDEX).LinkTo(value); }
        }

        public Outlet Exponent
        {
            get { return GetInlet(OperatorConstants.TIME_POWER_EXPONENT_INDEX).InputOutlet; }
            set { GetInlet(OperatorConstants.TIME_POWER_EXPONENT_INDEX).LinkTo(value); }
        }

        public Outlet Origin
        {
            get { return GetInlet(OperatorConstants.TIME_POWER_ORIGIN_INDEX).InputOutlet; }
            set { GetInlet(OperatorConstants.TIME_POWER_ORIGIN_INDEX).LinkTo(value); }
        }

        public Outlet Result
        {
            get { return GetOutlet(OperatorConstants.TIME_POWER_RESULT_INDEX); }
        }

        public static implicit operator Outlet(TimePower_OperatorWrapper wrapper)
        {
            if (wrapper == null) return null;

            return wrapper.Result;
        }
    }
}