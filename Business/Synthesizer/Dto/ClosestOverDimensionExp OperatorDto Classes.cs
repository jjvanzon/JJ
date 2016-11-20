﻿using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class ClosestOverDimensionExp_OperatorDto : ClosestOverDimension_OperatorDto
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.ClosestOverDimensionExp);
    }

    internal class ClosestOverDimensionExp_OperatorDto_CollectionRecalculationContinuous : ClosestOverDimensionExp_OperatorDto
    { }

    internal class ClosestOverDimensionExp_OperatorDto_CollectionRecalculationUponReset : ClosestOverDimensionExp_OperatorDto
    { }
}