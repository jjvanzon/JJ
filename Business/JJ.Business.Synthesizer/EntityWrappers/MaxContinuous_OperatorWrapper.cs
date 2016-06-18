﻿using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class MaxContinuous_OperatorWrapper : OperatorWrapperBase_ContinuousAggregate
    {
        public MaxContinuous_OperatorWrapper(Operator op)
            : base(op)
        { }

        public static implicit operator Outlet(MaxContinuous_OperatorWrapper wrapper)
        {
            if (wrapper == null) return null;

            return wrapper.Result;
        }
    }
}