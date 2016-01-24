﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class Reset_OperatorWrapper : OperatorWrapperBase
    {
        public Reset_OperatorWrapper(Operator op)
            : base(op)
        { }

        public Outlet Operand
        {
            get { return OperatorHelper.GetInputOutlet(_wrappedOperator, OperatorConstants.RESET_OPERAND_INDEX); }
            set { OperatorHelper.GetInlet(_wrappedOperator, OperatorConstants.RESET_OPERAND_INDEX).LinkTo(value); }
        }

        public Outlet Result
        {
            get { return OperatorHelper.GetOutlet(_wrappedOperator, OperatorConstants.RESET_RESULT_INDEX); }
        }

        public static implicit operator Outlet(Reset_OperatorWrapper wrapper)
        {
            if (wrapper == null) return null;

            return wrapper.Result;
        }
    }
}