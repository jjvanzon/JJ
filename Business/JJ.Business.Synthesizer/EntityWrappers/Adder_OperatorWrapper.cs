﻿using JJ.Data.Synthesizer;
using System.Collections.Generic;
using System;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class Adder_OperatorWrapper : OperatorWrapperBase_VariableInletCountOneOutlet
    {
        public Adder_OperatorWrapper(Operator op)
            : base(op)
        { }

        public static implicit operator Outlet(Adder_OperatorWrapper wrapper)
        {
            if (wrapper == null) return null;
            
            return wrapper.Result;
        }
    }
}