﻿using System;
using System.Collections.Generic;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class Triangle_OperatorDto : OperatorDtoBase_VarFrequency
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.Triangle;
    }

    internal class Triangle_OperatorDto_ZeroFrequency : OperatorDtoBase_ZeroFrequency
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.Triangle;
    }

    internal class Triangle_OperatorDto_ConstFrequency_NoOriginShifting : OperatorDtoBase_ConstFrequency
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.Triangle;
    }

    internal class Triangle_OperatorDto_ConstFrequency_WithOriginShifting : OperatorDtoBase_ConstFrequency
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.Triangle;
    }

    internal class Triangle_OperatorDto_VarFrequency_NoPhaseTracking : OperatorDtoBase_VarFrequency
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.Triangle;
    }

    internal class Triangle_OperatorDto_VarFrequency_WithPhaseTracking : OperatorDtoBase_VarFrequency
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.Triangle;
    }
}
