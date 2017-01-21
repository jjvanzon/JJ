﻿using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class And_OperatorDto : And_OperatorDto_VarA_VarB
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.And;
    }

    internal class And_OperatorDto_ConstA_ConstB : OperatorDtoBase_ConstA_ConstB
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.And;
    }

    internal class And_OperatorDto_ConstA_VarB : OperatorDtoBase_ConstA_VarB
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.And;
    }

    internal class And_OperatorDto_VarA_ConstB : OperatorDtoBase_VarA_ConstB
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.And;
    }

    internal class And_OperatorDto_VarA_VarB : OperatorDtoBase_VarA_VarB
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.And;
    }
}