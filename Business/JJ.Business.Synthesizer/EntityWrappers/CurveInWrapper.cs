﻿using JJ.Business.Synthesizer.Constants;
using JJ.Framework.Reflection;
using JJ.Persistence.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class CurveInWrapper
    {

        private CurveIn _curveIn;

        public CurveInWrapper(CurveIn curveIn)
        {
            if (curveIn == null) throw new NullException(() => curveIn);
            _curveIn = curveIn;
        }

        public Curve Curve
        {
            get { return _curveIn.Curve; }
            set { _curveIn.Curve = value; }
        }

        public Outlet Result
        {
            get { return _curveIn.Operator.Outlets[OperatorConstants.CURVE_IN_RESULT_INDEX]; }
        }

        public static implicit operator Outlet(CurveInWrapper wrapper)
        {
            return wrapper.Result;
        }
    }
}
