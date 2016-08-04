﻿using JJ.Data.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class Not_OperatorWrapper : OperatorWrapperBase
    {
        private const int X_INDEX = 0;
        private const int RESULT_INDEX = 0;

        public Not_OperatorWrapper(Operator op)
            : base(op)
        { }

        public Outlet X
        {
            get { return OperatorHelper.GetInputOutlet(WrappedOperator, X_INDEX); }
            set { OperatorHelper.GetInlet(WrappedOperator, X_INDEX).LinkTo(value); }
        }

        public Outlet Result
        {
            get { return OperatorHelper.GetOutlet(WrappedOperator, RESULT_INDEX); }
        }

        public override string GetInletDisplayName(int listIndex)
        {
            if (listIndex != 0) throw new NotEqualException(() => listIndex, 0);

            string name = ResourceHelper.GetPropertyDisplayName(() => X);
            return name;
        }

        public override string GetOutletDisplayName(int listIndex)
        {
            if (listIndex != 0) throw new NotEqualException(() => listIndex, 0);

            string name = ResourceHelper.GetPropertyDisplayName(() => Result);
            return name;
        }

        public static implicit operator Outlet(Not_OperatorWrapper wrapper) => wrapper?.Result;
    }
}
