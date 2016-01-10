﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class SpeedUp_OperatorWrapper : OperatorWrapperBase
    {
        public SpeedUp_OperatorWrapper(Operator op)
            : base(op)
        { }

        public Outlet Signal
        {
            get { return OperatorHelper.GetInputOutlet(_wrappedOperator, OperatorConstants.SPEED_UP_SIGNAL_INDEX); }
            set { OperatorHelper.GetInlet(_wrappedOperator, OperatorConstants.SPEED_UP_SIGNAL_INDEX).LinkTo(value); }
        }

        public Outlet Factor
        {
            get { return OperatorHelper.GetInputOutlet(_wrappedOperator, OperatorConstants.SPEED_UP_FACTOR_INDEX); }
            set { OperatorHelper.GetInlet(_wrappedOperator, OperatorConstants.SPEED_UP_FACTOR_INDEX).LinkTo(value); }
        }

        public Outlet Result
        {
            get { return OperatorHelper.GetOutlet(_wrappedOperator, OperatorConstants.SPEED_UP_RESULT_INDEX); }
        }

        public static implicit operator Outlet(SpeedUp_OperatorWrapper wrapper)
        {
            if (wrapper == null) return null;

            return wrapper.Result;
        }
    }
}
