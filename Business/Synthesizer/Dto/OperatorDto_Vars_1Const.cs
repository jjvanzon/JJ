﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Business.Synthesizer.Dto
{
    internal abstract class OperatorDto_Vars_1Const : OperatorDto
    {
        public IList<OperatorDto> Vars
        {
            get { return InputOperatorDtos; }
            set { InputOperatorDtos = value; }
        }

        public double ConstValue { get; set; }

        public OperatorDto_Vars_1Const(IList<OperatorDto> vars, double constValue)
            : base(vars)
        { }
    }
}
