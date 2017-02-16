﻿using System.Collections.Generic;

namespace JJ.Business.Synthesizer.Dto
{
    internal abstract class OperatorDtoBase_Vars_1Const : OperatorDtoBase
    {
        public IList<IOperatorDto> Vars { get; set; }
        public double ConstValue { get; set; }

        public override IList<IOperatorDto> InputOperatorDtos
        {
            get { return Vars; }
            set { Vars = value; }
        }
    }
}
