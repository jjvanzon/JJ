﻿using System;
using JJ.Business.Synthesizer.Dto;

namespace JJ.Business.Synthesizer.Visitors
{
    internal abstract class OperatorDtoVisitorBase_AfterRewiring : OperatorDtoVisitorBase_AfterMathSimplification
    {
        protected sealed override OperatorDtoBase Visit_Random_OperatorDto_CubicAbruptSlope(Random_OperatorDto_CubicAbruptSlope dto)
        {
            throw new NotSupportedException();
        }

        protected sealed override OperatorDtoBase Visit_Random_OperatorDto_CubicEquidistant(Random_OperatorDto_CubicEquidistant dto)
        {
            throw new NotSupportedException();
        }

        protected sealed override OperatorDtoBase Visit_Random_OperatorDto_CubicSmoothSlope_LagBehind(Random_OperatorDto_CubicSmoothSlope_LagBehind dto)
        {
            throw new NotSupportedException();
        }

        protected sealed override OperatorDtoBase Visit_Random_OperatorDto_Hermite_LagBehind(Random_OperatorDto_Hermite_LagBehind dto)
        {
            throw new NotSupportedException();
        }

        protected sealed override OperatorDtoBase Visit_Random_OperatorDto_Line_LagBehind_ConstRate(Random_OperatorDto_Line_LagBehind_ConstRate dto)
        {
            throw new NotSupportedException();
        }

        protected sealed override OperatorDtoBase Visit_Random_OperatorDto_Line_LagBehind_VarRate(Random_OperatorDto_Line_LagBehind_VarRate dto)
        {
            throw new NotSupportedException();
        }
    }
}