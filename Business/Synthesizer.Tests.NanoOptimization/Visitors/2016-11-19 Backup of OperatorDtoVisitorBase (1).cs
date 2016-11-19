﻿//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
//using JJ.Business.Synthesizer.Tests.NanoOptimization.Dto;
//using JJ.Framework.Reflection.Exceptions;

//namespace JJ.Business.Synthesizer.Tests.NanoOptimization.Visitors
//{
//    internal abstract class OperatorDtoVisitorBase
//    {
//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_OperatorDto_Polymorphic(OperatorDtoBase dto)
//        {
//            {
//                var castedDto = dto as Multiply_OperatorDto;
//                if (castedDto != null)
//                {
//                    return Visit_Multiply_OperatorDto(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Multiply_OperatorDto_VarA_VarB;
//                if (castedDto != null)
//                {
//                    return Visit_Multiply_OperatorDto_VarA_VarB(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Multiply_OperatorDto_VarA_ConstB;
//                if (castedDto != null)
//                {
//                    return Visit_Multiply_OperatorDto_VarA_ConstB(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Multiply_OperatorDto_ConstA_VarB;
//                if (castedDto != null)
//                {
//                    return Visit_Multiply_OperatorDto_ConstA_VarB(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Multiply_OperatorDto_ConstA_ConstB;
//                if (castedDto != null)
//                {
//                    return Visit_Multiply_OperatorDto_ConstA_ConstB(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Add_OperatorDto;
//                if (castedDto != null)
//                {
//                    return Visit_Add_OperatorDto(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Add_OperatorDto_Vars_NoConsts;
//                if (castedDto != null)
//                {
//                    return Visit_Add_OperatorDto_Vars_NoConsts(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Add_OperatorDto_Vars_1Const;
//                if (castedDto != null)
//                {
//                    return Visit_Add_OperatorDto_Vars_1Const(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Sine_OperatorDto;
//                if (castedDto != null)
//                {
//                    return Visit_Sine_OperatorDto(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Sine_OperatorDto_VarFrequency_WithPhaseTracking;
//                if (castedDto != null)
//                {
//                    return Visit_Sine_OperatorDto_VarFrequency_WithPhaseTracking(castedDto);
//                }
//            }
//            {
//                var castedDto = dto as Sine_OperatorDto_VarFrequency_NoPhaseTracking;
//                if (castedDto != null)
//                {
//                    return Visit_Sine_OperatorDto_VarFrequency_NoPhaseTracking(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Sine_OperatorDto_ConstFrequency_NoOriginShifting;
//                if (castedDto != null)
//                {
//                    return Visit_Sine_OperatorDto_ConstFrequency_NoOriginShifting(castedDto);
//                }
//            }
//            {
//                var castedDto = dto as Sine_OperatorDto_ConstFrequency_WithOriginShifting;
//                if (castedDto != null)
//                {
//                    return Visit_Sine_OperatorDto_ConstFrequency_WithOriginShifting(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Number_OperatorDto_NaN;
//                if (castedDto != null)
//                {
//                    return Visit_Number_OperatorDto_NaN(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Number_OperatorDto_One;
//                if (castedDto != null)
//                {
//                    return Visit_Number_OperatorDto_One(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Number_OperatorDto_Zero;
//                if (castedDto != null)
//                {
//                    return Visit_Number_OperatorDto_Zero(castedDto);
//                }
//            }

//            // Order-Dependence:
//            // Because of polymorphism, this has to be executed after the derived Number_OperatorDto classes.
//            // This is only a problem in case of numbers, because after specialisation, 
//            // the base class will still be used in the structure.
//            {
//                var castedDto = dto as Number_OperatorDto;
//                if (castedDto != null)
//                {
//                    return Visit_Number_OperatorDto(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as VariableInput_OperatorDto;
//                if (castedDto != null)
//                {
//                    return Visit_VariableInput_OperatorDto(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Shift_OperatorDto;
//                if (castedDto != null)
//                {
//                    return Visit_Shift_OperatorDto(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Shift_OperatorDto_ConstSignal_ConstDistance;
//                if (castedDto != null)
//                {
//                    return Visit_Shift_OperatorDto_ConstSignal_ConstDistance(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Shift_OperatorDto_ConstSignal_VarDistance;
//                if (castedDto != null)
//                {
//                    return Visit_Shift_OperatorDto_ConstSignal_VarDistance(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Shift_OperatorDto_VarSignal_ConstDistance;
//                if (castedDto != null)
//                {
//                    return Visit_Shift_OperatorDto_VarSignal_ConstDistance(castedDto);
//                }
//            }

//            {
//                var castedDto = dto as Shift_OperatorDto_VarSignal_VarDistance;
//                if (castedDto != null)
//                {
//                    return Visit_Shift_OperatorDto_VarSignal_VarDistance(castedDto);
//                }
//            }

//            throw new UnexpectedTypeException(() => dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_OperatorDto_Base(OperatorDtoBase dto)
//        {
//            dto.InputOperatorDtos = VisitInputOperatorDtos(dto.InputOperatorDtos);

//            return dto;
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual IList<OperatorDtoBase> VisitInputOperatorDtos(IList<OperatorDtoBase> operatorDtos)
//        {
//            for (int i = 0; i < operatorDtos.Count; i++)
//            {
//                OperatorDtoBase operatorDto = operatorDtos[i];
//                operatorDtos[i] = VisitOperatorDto(operatorDto);
//            }

//            return operatorDtos;
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase VisitOperatorDto(OperatorDtoBase dto)
//        {
//            return Visit_OperatorDto_Polymorphic(dto);
//        }

//        // Add

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Add_OperatorDto(Add_OperatorDto dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Add_OperatorDto_Vars_NoConsts(Add_OperatorDto_Vars_NoConsts dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Add_OperatorDto_Vars_1Const(Add_OperatorDto_Vars_1Const dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        // Multiply

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Multiply_OperatorDto(Multiply_OperatorDto dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Multiply_OperatorDto_VarA_VarB(Multiply_OperatorDto_VarA_VarB dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Multiply_OperatorDto_VarA_ConstB(Multiply_OperatorDto_VarA_ConstB dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Multiply_OperatorDto_ConstA_VarB(Multiply_OperatorDto_ConstA_VarB dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Multiply_OperatorDto_ConstA_ConstB(Multiply_OperatorDto_ConstA_ConstB dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        // Number

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Number_OperatorDto(Number_OperatorDto dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Number_OperatorDto_NaN(Number_OperatorDto_NaN dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Number_OperatorDto_One(Number_OperatorDto_One dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Number_OperatorDto_Zero(Number_OperatorDto_Zero dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        // Shift

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Shift_OperatorDto(Shift_OperatorDto dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Shift_OperatorDto_VarSignal_VarDistance(Shift_OperatorDto_VarSignal_VarDistance dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Shift_OperatorDto_VarSignal_ConstDistance(Shift_OperatorDto_VarSignal_ConstDistance dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Shift_OperatorDto_ConstSignal_VarDistance(Shift_OperatorDto_ConstSignal_VarDistance dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Shift_OperatorDto_ConstSignal_ConstDistance(Shift_OperatorDto_ConstSignal_ConstDistance dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        // Sine

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Sine_OperatorDto(Sine_OperatorDto dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Sine_OperatorDto_VarFrequency_WithPhaseTracking(Sine_OperatorDto_VarFrequency_WithPhaseTracking dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Sine_OperatorDto_VarFrequency_NoPhaseTracking(Sine_OperatorDto_VarFrequency_NoPhaseTracking dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Sine_OperatorDto_ConstFrequency_NoOriginShifting(Sine_OperatorDto_ConstFrequency_NoOriginShifting dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_Sine_OperatorDto_ConstFrequency_WithOriginShifting(Sine_OperatorDto_ConstFrequency_WithOriginShifting dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }

//        // VariableInput

//        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
//        protected virtual OperatorDtoBase Visit_VariableInput_OperatorDto(VariableInput_OperatorDto dto)
//        {
//            return Visit_OperatorDto_Base(dto);
//        }
//    }
//}
