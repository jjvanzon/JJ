﻿using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class Add_OperatorDto : OperatorDtoBase_Vars
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Add);
    }

    internal class Add_OperatorDto_Vars_Consts : OperatorDtoBase_Vars_Consts
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Add);
    }

    internal class Add_OperatorDto_Vars_NoConsts : OperatorDtoBase_Vars
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Add);
    }

    internal class Add_OperatorDto_NoVars_NoConsts : OperatorDtoBase_WithoutInputOperatorDtos
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Add);
    }

    internal class Add_OperatorDto_NoVars_Consts : OperatorDtoBase_Consts
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Add);
    }

    /// <summary>
    /// Only created in MathSimplification_OperatorDtoVisitor,
    /// not yet in ClassSpecialization_OperatorDtoVisitor.
    /// </summary>
    internal class Add_OperatorDto_Vars_1Const : OperatorDtoBase_Vars_1Const
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.Add);
    }
}