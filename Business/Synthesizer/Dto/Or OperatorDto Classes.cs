﻿using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class Or_OperatorDto : Or_OperatorDto_VarA_VarB
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Or);
    }

    internal class Or_OperatorDto_ConstA_ConstB : OperatorDtoBase_ConstA_ConstB
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Or);
    }

    internal class Or_OperatorDto_ConstA_VarB : OperatorDtoBase_ConstA_VarB
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Or);
    }

    internal class Or_OperatorDto_VarA_ConstB : OperatorDtoBase_VarA_ConstB
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Or);
    }

    internal class Or_OperatorDto_VarA_VarB : OperatorDtoBase_VarA_VarB
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Or);
    }
}