﻿using JJ.Framework.Reflection;
using JJ.Persistence.Synthesizer;
using JJ.Business.Synthesizer.LinkTo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Business.Synthesizer.Constants;
using JJ.Framework.Validation;
using JJ.Business.Synthesizer.Validation.Entities;

namespace JJ.Business.Synthesizer.EntityWrappers
{
    public class SubstractWrapper : OperatorWrapperBase
    {
        public SubstractWrapper(Operator op)
            : base(op)
        {
            Verify();
        }

        public Outlet OperandA
        {
            get { Verify(); return _operator.Inlets[OperatorConstants.SUBSTRACT_OPERAND_A_INDEX].InputOutlet; }
            set { Verify(); _operator.Inlets[OperatorConstants.SUBSTRACT_OPERAND_A_INDEX].LinkTo(value); }
        }

        public Outlet OperandB
        {
            get { Verify(); return _operator.Inlets[OperatorConstants.SUBSTRACT_OPERAND_B_INDEX].InputOutlet; }
            set { Verify(); _operator.Inlets[OperatorConstants.SUBSTRACT_OPERAND_B_INDEX].LinkTo(value); }
        }

        public Outlet Result
        {
            get { Verify(); return _operator.Outlets[OperatorConstants.SUBSTRACT_RESULT_INDEX]; }
        }

        public static implicit operator Outlet(SubstractWrapper wrapper)
        {
            return wrapper.Result;
        }

        private void Verify()
        {
            IValidator validator = new SubstractValidator(Operator);
            validator.Verify();
        }
    }
}
