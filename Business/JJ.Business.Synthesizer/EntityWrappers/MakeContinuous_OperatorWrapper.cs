﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer;
using System.Collections.Generic;
using JJ.Framework.Reflection.Exceptions;
using JJ.Business.Synthesizer.Resources;
using System;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class MakeContinuous_OperatorWrapper : OperatorWrapperBase_WithDimensionAndResampleInterpolationType
    {
        private const int RESULT_INDEX = 0;

        public MakeContinuous_OperatorWrapper(Operator op)
            : base(op)
        { }

        /// <summary> Executes a loop, so prevent calling it multiple times. summary>
        public IList<Outlet> Operands
        {
            get { return OperatorHelper.GetSortedInputOutlets(WrappedOperator); }
        }

        public Outlet Result
        {
            get { return OperatorHelper.GetOutlet(WrappedOperator, RESULT_INDEX); }
        }

        public override string GetInletDisplayName(int listIndex)
        {
            if (listIndex < 0) throw new InvalidIndexException(() => listIndex, () => WrappedOperator.Inlets.Count);
            if (listIndex > WrappedOperator.Inlets.Count) throw new InvalidIndexException(() => listIndex, () => WrappedOperator.Inlets.Count);

            string name = String.Format("{0} {1}", PropertyDisplayNames.Inlet, listIndex + 1);
            return name;
        }

        public override string GetOutletDisplayName(int listIndex)
        {
            if (listIndex != 0) throw new NotEqualException(() => listIndex, 0);

            string name = ResourceHelper.GetPropertyDisplayName(() => Result);
            return name;
        }

        public static implicit operator Outlet(MakeContinuous_OperatorWrapper wrapper)
        {
            if (wrapper == null) return null;
            
            return wrapper.Result;
        }
    }
}