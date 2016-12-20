﻿using System;
using JJ.Business.Synthesizer.Dto;

namespace JJ.Business.Synthesizer.Visitors
{
    internal class SpecialVisitation_OperatorDtoVisitor : OperatorDtoVisitorBase
    {
        /// <summary>
        /// Special visitation means writing out the underlying patches of custom operators,
        /// handling multiple outlets and other special things like enumerating variable inputs and reset operators.
        /// </summary>
        public SpecialVisitation_OperatorDtoVisitor()
        { }

        public OperatorDtoBase Execute(OperatorDtoBase dto)
        {
            return Visit_OperatorDto_Polymorphic(dto);
        }

        protected override OperatorDtoBase Visit_CustomOperator_OperatorDto(CustomOperator_OperatorDto dto)
        {
            throw new NotImplementedException();
            return base.Visit_CustomOperator_OperatorDto(dto);
        }

        protected override OperatorDtoBase Visit_DimensionToOutlets_OperatorDto(DimensionToOutlets_OperatorDto dto)
        {
            throw new NotImplementedException();
            return base.Visit_DimensionToOutlets_OperatorDto(dto);
        }

        protected override OperatorDtoBase Visit_PatchInlet_OperatorDto(PatchInlet_OperatorDto dto)
        {
            base.Visit_PatchInlet_OperatorDto(dto);

            throw new NotImplementedException();

            return dto;
        }

        protected override OperatorDtoBase Visit_PatchOutlet_OperatorDto(PatchOutlet_OperatorDto dto)
        {
            base.Visit_PatchOutlet_OperatorDto(dto);

            throw new NotImplementedException();

            return dto;
        }

        protected override OperatorDtoBase Visit_RangeOverOutlets_OperatorDto_ConstFrom_ConstStep(RangeOverOutlets_OperatorDto_ConstFrom_ConstStep dto)
        {
            base.Visit_RangeOverOutlets_OperatorDto_ConstFrom_ConstStep(dto);

            // TODO: Has no calculator. Why not?
            // Because each outlet becomes a constant.
            //var calculator = new RangeOverOutlets_OperatorCalculator_ConstFrom_ConstStep
            throw new NotImplementedException();

            return dto;
        }

        protected override OperatorDtoBase Visit_RangeOverOutlets_OperatorDto_ConstFrom_VarStep(RangeOverOutlets_OperatorDto_ConstFrom_VarStep dto)
        {
            base.Visit_RangeOverOutlets_OperatorDto_ConstFrom_VarStep(dto);

            //var calculator = new RangeOverOutlets_OperatorCalculator_ConstFrom_VarStep(dto.From, _stack.Pop());
            throw new NotImplementedException();

            return dto;
        }

        protected override OperatorDtoBase Visit_RangeOverOutlets_OperatorDto_VarFrom_ConstStep(RangeOverOutlets_OperatorDto_VarFrom_ConstStep dto)
        {
            throw new NotImplementedException();
            return base.Visit_RangeOverOutlets_OperatorDto_VarFrom_ConstStep(dto);
        }

        protected override OperatorDtoBase Visit_RangeOverOutlets_OperatorDto_VarFrom_VarStep(RangeOverOutlets_OperatorDto_VarFrom_VarStep dto)
        {
            throw new NotImplementedException();
            return base.Visit_RangeOverOutlets_OperatorDto_VarFrom_VarStep(dto);
        }

        protected override OperatorDtoBase Visit_Reset_OperatorDto(Reset_OperatorDto dto)
        {
            throw new NotImplementedException();
            return base.Visit_Reset_OperatorDto(dto);
        }
    }
}
