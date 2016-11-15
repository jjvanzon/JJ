﻿using System.Collections.Generic;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class Add_OperatorDto : OperatorDtoBase_Vars
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Add);
    }

    internal class Add_OperatorDto_Vars : OperatorDtoBase_Vars
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Add);
    }

    internal class Add_OperatorDto_Vars_1Const : OperatorDtoBase_Vars_1Const
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Add);
    }
}
