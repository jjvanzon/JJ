﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Business.Synthesizer.Constants;
using JJ.Framework.Validation;
using JJ.Business.Synthesizer.Validation;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class DivideWrapper : OperatorWrapperBase
    {
        public DivideWrapper(Operator op)
            :base(op)
        { }

        public Outlet Numerator
        {
            get { return GetInlet(OperatorConstants.DIVIDE_NUMERATOR_INDEX).InputOutlet; }
            set { GetInlet(OperatorConstants.DIVIDE_NUMERATOR_INDEX).LinkTo(value); }
        }

        public Outlet Denominator
        {
            get { return GetInlet(OperatorConstants.DIVIDE_DENOMINATOR_INDEX).InputOutlet; }
            set { GetInlet(OperatorConstants.DIVIDE_DENOMINATOR_INDEX).LinkTo(value); }
        }

        public Outlet Origin
        {
            get { return GetInlet(OperatorConstants.DIVIDE_ORIGIN_INDEX).InputOutlet; }
            set { GetInlet(OperatorConstants.DIVIDE_ORIGIN_INDEX).LinkTo(value); }
        }

        public Outlet Result
        {
            get { return GetOutlet(OperatorConstants.DIVIDE_RESULT_INDEX); }
        }

        public static implicit operator Outlet(DivideWrapper wrapper)
        {
            return wrapper.Result;
        }
    }
}
