﻿using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class MaxOverInlets_OperatorDto : OperatorDtoBase_Vars
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.MaxOverInlets);
    }

    internal class MaxOverInlets_OperatorDto_Vars_1Const : OperatorDtoBase_Vars_1Const
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.MaxOverInlets);
    }

    internal class MaxOverInlets_OperatorDto_AllVars : OperatorDtoBase_Vars
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.MaxOverInlets);
    }

    internal class MaxOverInlets_OperatorDto_1Var_1Const : OperatorDtoBase_VarA_ConstB
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.MaxOverInlets);
    }

    internal class MaxOverInlets_OperatorDto_2Vars : OperatorDtoBase_VarA_VarB
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.MaxOverInlets);
    }
}