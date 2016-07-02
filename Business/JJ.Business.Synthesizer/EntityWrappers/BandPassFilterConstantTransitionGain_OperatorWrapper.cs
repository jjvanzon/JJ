﻿using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class BandPassFilterConstantTransitionGain_OperatorWrapper : OperatorWrapperBase_FilterWithFrequencyAndBandWidth
    {
        public BandPassFilterConstantTransitionGain_OperatorWrapper(Operator op)
            : base(op)
        { }

        public static implicit operator Outlet(BandPassFilterConstantTransitionGain_OperatorWrapper wrapper)
        {
            if (wrapper == null) return null;

            return wrapper.Result;
        }
    }
}