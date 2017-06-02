﻿using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer.Entities;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public abstract class OperatorWrapperBase_WithAAndB : OperatorWrapperBase_WithResultOutlet
    {
        public OperatorWrapperBase_WithAAndB(Operator op)
            : base(op)
        { }

        public Outlet A
        {
            get => AInlet.InputOutlet;
            set => AInlet.LinkTo(value);
        }

        public Inlet AInlet => OperatorHelper.GetInlet(WrappedOperator, DimensionEnum.A);

        public Outlet B
        {
            get => BInlet.InputOutlet;
            set => BInlet.LinkTo(value);
        }

        public Inlet BInlet => OperatorHelper.GetInlet(WrappedOperator, DimensionEnum.B);
    }
}
