﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.LinkTo;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class Unbundle_OperatorWrapper : OperatorWrapperBase
    {
        public Unbundle_OperatorWrapper(Operator op)
            : base(op)
        { }

        public Outlet Operand
        {
            get { return OperatorHelper.GetInlet(_operator, OperatorConstants.UNBUNDLE_OPERAND_INDEX).InputOutlet; }
            set { OperatorHelper.GetInlet(_operator, OperatorConstants.UNBUNDLE_OPERAND_INDEX).LinkTo(value); }
        }

        public IList<Outlet> Results
        {
            get { return _operator.Outlets; }
        }
    }
}