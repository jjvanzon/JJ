﻿using System;
using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Warnings.Operators
{
    internal class MinOverDimension_OperatorWarningValidator : OperatorWarningValidator_Base_SpecificInletsFilledIn
    {
        public MinOverDimension_OperatorWarningValidator(Operator obj)
            : base(
                  obj,
                  OperatorConstants.AGGREGATE_OVER_DIMENSION_SIGNAL_INDEX,
                  OperatorConstants.AGGREGATE_OVER_DIMENSION_TILL_INDEX)
        { }
    }
}