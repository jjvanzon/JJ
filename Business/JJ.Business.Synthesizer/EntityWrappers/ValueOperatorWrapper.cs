﻿using JJ.Business.Synthesizer.Constants;
using JJ.Business.Synthesizer.Validation.Entities;
using JJ.Framework.Reflection;
using JJ.Framework.Validation;
using JJ.Persistence.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class ValueOperatorWrapper : OperatorWrapperBase
    {
        public ValueOperatorWrapper(Operator op)
            : base(op)
        {
            Verify();
        }

        public Outlet Result
        {
            get { Verify(); return Operator.Outlets[OperatorConstants.VALUE_OPERATOR_RESULT_INDEX]; }
        }

        public double Value
        {
            get { Verify(); return Operator.AsValueOperator.Value; }
            set { Verify(); Operator.AsValueOperator.Value = value; }
        }

        public static implicit operator Outlet(ValueOperatorWrapper wrapper)
        {
            return wrapper.Result;
        }

        public static implicit operator double(ValueOperatorWrapper wrapper)
        {
            return wrapper.Value;
        }

        private void Verify()
        {
            IValidator validator = new ValueOperatorValidator(Operator);
            validator.Verify();
        }
    }
}