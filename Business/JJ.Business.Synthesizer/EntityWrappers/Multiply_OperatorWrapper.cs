﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class Multiply_OperatorWrapper : OperatorWrapperBase
    {
        public Multiply_OperatorWrapper(Operator op)
            :base(op)
        { }

        public Outlet OperandA
        {
            get { return OperatorHelper.GetInlet(_operator, OperatorConstants.MULTIPLY_OPERAND_A_INDEX).InputOutlet; }
            set { OperatorHelper.GetInlet(_operator, OperatorConstants.MULTIPLY_OPERAND_A_INDEX).LinkTo(value); }
        }

        public Outlet OperandB
        {
            get { return OperatorHelper.GetInlet(_operator, OperatorConstants.MULTIPLY_OPERAND_B_INDEX).InputOutlet; }
            set { OperatorHelper.GetInlet(_operator, OperatorConstants.MULTIPLY_OPERAND_B_INDEX).LinkTo(value); }
        }

        public Outlet Origin
        {
            get { return OperatorHelper.GetInlet(_operator, OperatorConstants.MULTIPLY_ORIGIN_INDEX).InputOutlet; }
            set { OperatorHelper.GetInlet(_operator, OperatorConstants.MULTIPLY_ORIGIN_INDEX).LinkTo(value); }
        }

        public Outlet Result
        {
            get { return OperatorHelper.GetOutlet(_operator, OperatorConstants.MULTIPLY_RESULT_INDEX); }
        }

        public static implicit operator Outlet(Multiply_OperatorWrapper wrapper)
        {
            if (wrapper == null) return null;

            return wrapper.Result;
        }
    }
}
