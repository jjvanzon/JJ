﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JJ.Business.Synthesizer.Tests.NanoOptimization.Dto;
using JJ.Business.Synthesizer.Tests.NanoOptimization.Helpers;
using JJ.Framework.Common;

namespace JJ.Business.Synthesizer.Tests.NanoOptimization.Visitors
{
    internal class ClassSpecialization_OperatorDtoVisitor : OperatorDtoVisitorBase
    {
        public OperatorDtoBase Execute(OperatorDtoBase operatorDto)
        {
            return Visit_OperatorDto_Polymorphic(operatorDto);
        }

        protected override OperatorDtoBase Visit_Add_OperatorDto(Add_OperatorDto dto)
        {
            base.Visit_Add_OperatorDto(dto);

            IList<OperatorDtoBase> inputOperatorDtos = dto.InputOperatorDtos;

            inputOperatorDtos = TruncateOperatorDtoList(inputOperatorDtos, x => x.Sum());

            // Get rid of const zero.
            inputOperatorDtos.TryRemoveFirst(x => MathPropertiesHelper.GetMathPropertiesDto(x).IsConstZero);

            switch (inputOperatorDtos.Count)
            {
                case 0:
                    {
                        OperatorDtoBase dto2 = new Number_OperatorDto_Zero();
                        return dto2;
                    }

                case 1:
                    {
                        OperatorDtoBase dto2 = inputOperatorDtos[0];
                        return dto2;
                    }

                default:
                    OperatorDtoBase constInputOperatorDto = inputOperatorDtos.Where(x => MathPropertiesHelper.GetMathPropertiesDto(x).IsConst)
                                                                         .SingleOrDefault();
                    if (constInputOperatorDto == null)
                    {
                        OperatorDtoBase dto2 = new Add_OperatorDto_Vars { Vars = inputOperatorDtos };
                        return dto2;
                    }
                    else
                    {
                        IList<OperatorDtoBase> varInputOperatorDtos = inputOperatorDtos.Except(constInputOperatorDto).ToArray();
                        double constValue = MathPropertiesHelper.GetMathPropertiesDto(constInputOperatorDto).Value;
                        OperatorDtoBase dto2 = new Add_OperatorDto_Vars_1Const { Vars = varInputOperatorDtos, ConstValue = constValue };
                        return dto2;
                    }
            }
        }

        protected override OperatorDtoBase Visit_Multiply_OperatorDto(Multiply_OperatorDto dto)
        {
            base.Visit_Multiply_OperatorDto(dto);

            OperatorDtoBase aOperatorDto = dto.AOperatorDto;
            OperatorDtoBase bOperatorDto = dto.BOperatorDto;

            MathPropertiesDto aMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(aOperatorDto);
            MathPropertiesDto bMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(bOperatorDto);

            if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsConst)
            {
                return new Multiply_OperatorDto_ConstA_ConstB { A = aMathPropertiesDto.Value, B = bMathPropertiesDto.Value };
            }

            if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsConst)
            {
                return new Multiply_OperatorDto_VarA_ConstB { AOperatorDto = aOperatorDto, B = bMathPropertiesDto.Value };
            }

            if (aMathPropertiesDto.IsConst && bMathPropertiesDto.IsVar)
            {
                return new Multiply_OperatorDto_ConstA_VarB { A = aMathPropertiesDto.Value, BOperatorDto = bOperatorDto };
            }

            if (aMathPropertiesDto.IsVar && bMathPropertiesDto.IsVar)
            {
                return new Multiply_OperatorDto_VarA_VarB { AOperatorDto = aOperatorDto, BOperatorDto = bOperatorDto };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_Shift_OperatorDto(Shift_OperatorDto dto)
        {
            base.Visit_Shift_OperatorDto(dto);

            OperatorDtoBase signalOperatorDto = dto.SignalOperatorDto;
            OperatorDtoBase distanceOperatorDto = dto.DistanceOperatorDto;

            MathPropertiesDto signalMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(signalOperatorDto);
            MathPropertiesDto distanceMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(distanceOperatorDto);

            if (signalMathPropertiesDto.IsConst && distanceMathPropertiesDto.IsConst)
            {
                return new Shift_OperatorDto_ConstSignal_ConstDistance { SignalValue = signalMathPropertiesDto.Value, Distance = distanceMathPropertiesDto.Value };
            }

            if (signalMathPropertiesDto.IsVar && distanceMathPropertiesDto.IsConst)
            {
                return new Shift_OperatorDto_VarSignal_ConstDistance { SignalOperatorDto = signalOperatorDto, Distance = distanceMathPropertiesDto.Value };
            }

            if (signalMathPropertiesDto.IsConst && distanceMathPropertiesDto.IsVar)
            {
                return new Shift_OperatorDto_ConstSignal_VarDistance { SignalValue = signalMathPropertiesDto.Value, DistanceOperatorDto = distanceOperatorDto };
            }

            if (signalMathPropertiesDto.IsVar && distanceMathPropertiesDto.IsVar)
            {
                return new Shift_OperatorDto_VarSignal_VarDistance { SignalOperatorDto = signalOperatorDto, DistanceOperatorDto = distanceOperatorDto };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        protected override OperatorDtoBase Visit_Sine_OperatorDto(Sine_OperatorDto dto)
        {
            base.Visit_Sine_OperatorDto(dto);

            OperatorDtoBase frequencyOperatorDto = dto.FrequencyOperatorDto;
            MathPropertiesDto frequencyMathPropertiesDto = MathPropertiesHelper.GetMathPropertiesDto(frequencyOperatorDto);

            if (frequencyMathPropertiesDto.IsConst)
            {
                return new Sine_OperatorDto_ConstFrequency_WithOriginShifting { Frequency = frequencyMathPropertiesDto.Value };
            }

            if (frequencyMathPropertiesDto.IsVar)
            {
                return new Sine_OperatorDto_VarFrequency_WithPhaseTracking { FrequencyOperatorDto = frequencyOperatorDto };
            }

            throw new VisitationCannotBeHandledException(MethodBase.GetCurrentMethod());
        }

        // Helpers

        /// <summary> Collapses invariant operands to one and gets rid of nulls. </summary>
        /// <param name="constantsCombiningDelegate">The method that combines multiple invariant doubles to one.</param>
        private static IList<OperatorDtoBase> TruncateOperatorDtoList(
            IList<OperatorDtoBase> operatorDtos,
            Func<IEnumerable<double>, double> constantsCombiningDelegate)
        {
            IList<OperatorDtoBase> constOperatorDtos = operatorDtos.Where(x => MathPropertiesHelper.GetMathPropertiesDto(x).IsConst).ToArray();
            IList<OperatorDtoBase> varOperatorDtos = operatorDtos.Except(constOperatorDtos).ToArray();

            OperatorDtoBase aggregatedConstOperatorDto = null;
            if (constOperatorDtos.Count != 0)
            {
                IEnumerable<double> consts = constOperatorDtos.Select(x => MathPropertiesHelper.GetMathPropertiesDto(x).Value);
                double aggregatedConsts = constantsCombiningDelegate(consts);
                aggregatedConstOperatorDto = new Number_OperatorDto { Number = aggregatedConsts };
            }

            IList<OperatorDtoBase> truncatedOperatorDtoList = varOperatorDtos.Union(aggregatedConstOperatorDto)
                                                                             .Where(x => x != null)
                                                                             .ToList();
            return truncatedOperatorDtoList;
        }
    }
}