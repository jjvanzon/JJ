﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Business.SynthesizerPrototype.Dto;
using JJ.Business.SynthesizerPrototype.Helpers;

namespace JJ.Business.SynthesizerPrototype.Visitors
{
    internal class OperatorDtoVisitor_MathSimplification : OperatorDtoVisitorBase_ClassSpecialization
    {
        // General

        public OperatorDtoBase Execute(OperatorDtoBase dto)
        {
            return Visit_OperatorDto_Polymorphic(dto);
        }

        [DebuggerHidden]
        protected override OperatorDtoBase Visit_OperatorDto_Polymorphic(OperatorDtoBase dto)
        {
            // NaN / Infinity

            OperatorDtoBase dto2 = base.Visit_OperatorDto_Polymorphic(dto); // Depth-first, so deeply pre-calculated NaN's can be picked up.

            bool anyInputsHaveSpecialValue = dto2.InputOperatorDtos.Any(x => MathPropertiesHelper.GetMathPropertiesDto(x).IsConstSpecialValue);
            if (anyInputsHaveSpecialValue)
            {
                return new Number_OperatorDto_NaN();
            }

            return dto2;
        }

        // Add

        protected override OperatorDtoBase Visit_Add_OperatorDto_NoVars_Consts(Add_OperatorDto_NoVars_Consts dto)
        {
            return Process_NoVars_Consts(dto, Enumerable.Sum);
        }

        protected override OperatorDtoBase Visit_Add_OperatorDto_NoVars_NoConsts(Add_OperatorDto_NoVars_NoConsts dto)
        {
            return Process_NoVars_NoConsts(dto);
        }

        protected override OperatorDtoBase Visit_Add_OperatorDto_Vars_NoConsts(Add_OperatorDto_Vars_NoConsts dto)
        {
            return Process_Vars_NoConsts(dto);
        }

        protected override OperatorDtoBase Visit_Add_OperatorDto_Vars_Consts(Add_OperatorDto_Vars_Consts dto)
        {
            base.Visit_Add_OperatorDto_Vars_Consts(dto);

            // Pre-calculate
            double constValue = dto.Consts.Sum();
            return new Add_OperatorDto_Vars_1Const { Vars = dto.Vars, ConstValue = constValue };
        }

        protected override OperatorDtoBase Visit_Add_OperatorDto_Vars_1Const(Add_OperatorDto_Vars_1Const dto)
        {
            base.Visit_Add_OperatorDto_Vars_1Const(dto);

            MathPropertiesDto constMathProperties = MathPropertiesHelper.GetMathPropertiesDto(dto.ConstValue);

            // Identity
            if (constMathProperties.IsConstZero)
            {
                return new Add_OperatorDto_Vars_NoConsts { Vars = dto.Vars };
            }

            return dto;
        }

        // Multiply

        protected override OperatorDtoBase Visit_Multiply_OperatorDto_ConstA_ConstB(Multiply_OperatorDto_ConstA_ConstB dto)
        {
            base.Visit_Multiply_OperatorDto_ConstA_ConstB(dto);

            // Pre-calculate
            return new Number_OperatorDto { Number = dto.A * dto.B };
        }

        protected override OperatorDtoBase Visit_Multiply_OperatorDto_ConstA_VarB(Multiply_OperatorDto_ConstA_VarB dto)
        {
            base.Visit_Multiply_OperatorDto_ConstA_VarB(dto);

            // Commute
            return new Multiply_OperatorDto_VarA_ConstB { AOperatorDto = dto.BOperatorDto, B = dto.A };
        }

        protected override OperatorDtoBase Visit_Multiply_OperatorDto_VarA_ConstB(Multiply_OperatorDto_VarA_ConstB dto)
        {
            base.Visit_Multiply_OperatorDto_VarA_ConstB(dto);

            MathPropertiesDto bMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(dto.B);

            // 0
            if (bMathPropertiesDto.IsConstZero)
            {
                return new Number_OperatorDto_Zero();
            }

            // Identity
            if (bMathPropertiesDto.IsConstOne)
            {
                return dto.AOperatorDto;
            }

            return dto;
        }

        // Shift

        protected override OperatorDtoBase Visit_Shift_OperatorDto_ConstSignal_ConstDistance(Shift_OperatorDto_ConstSignal_ConstDistance dto)
        {
            return Process_ConstSignal_Identity(dto.Signal);
        }

        protected override OperatorDtoBase Visit_Shift_OperatorDto_ConstSignal_VarDistance(Shift_OperatorDto_ConstSignal_VarDistance dto)
        {
            return Process_ConstSignal_Identity(dto.Signal);
        }

        protected override OperatorDtoBase Visit_Shift_OperatorDto_VarSignal_ConstDistance(Shift_OperatorDto_VarSignal_ConstDistance dto)
        {
            base.Visit_Shift_OperatorDto_VarSignal_ConstDistance(dto);

            MathPropertiesDto distanceMathProperties = MathPropertiesHelper.GetMathPropertiesDto(dto.Distance);

            if (distanceMathProperties.IsConstZero)
            {
                // Identity
                return dto.SignalOperatorDto;
            }

            return dto;
        }

        protected override OperatorDtoBase Visit_Shift_OperatorDto_VarSignal_VarDistance(Shift_OperatorDto_VarSignal_VarDistance dto)
        {
            return Process_Nothing(dto);
        }

        // Sine

        protected override OperatorDtoBase Visit_Sine_OperatorDto_ZeroFrequency(Sine_OperatorDto_ZeroFrequency dto)
        {
            base.Visit_Sine_OperatorDto_ZeroFrequency(dto);

            // 0
            return new Number_OperatorDto_Zero();
        }

        // Helpers

        /// <summary> 
        /// For overrides that do not add any processing. 
        /// They are overridden for maintainability purposes,
        /// so only new virtual methods show up when typing 'override'.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private OperatorDtoBase Process_Nothing(OperatorDtoBase dto)
        {
            return Visit_OperatorDto_Base(dto);
        }

        private OperatorDtoBase Process_NoVars_Consts(
            OperatorDtoBase_Consts dto,
            Func<IEnumerable<double>, double> aggregationDelegate)
        {
            base.Visit_OperatorDto_Base(dto);

            // Pre-calculate
            double result = aggregationDelegate(dto.Consts);

            return new Number_OperatorDto { Number = result };
        }

        private OperatorDtoBase Process_NoVars_NoConsts(OperatorDtoBase dto)
        {
            base.Visit_OperatorDto_Base(dto);

            // 0
            return new Number_OperatorDto_Zero();
        }

        private OperatorDtoBase Process_Vars_NoConsts(OperatorDtoBase_Vars dto)
        {
            base.Visit_OperatorDto_Base(dto);

            switch (dto.Vars.Count)
            {
                case 0:
                    // 0
                    return new Number_OperatorDto_Zero();

                case 1:
                    return dto.Vars[0];

                default:
                    return dto;
            }
        }

        private OperatorDtoBase Process_ConstSignal_Identity(double signal)
        {
            // Identity
            return new Number_OperatorDto { Number = signal };
        }
    }
}