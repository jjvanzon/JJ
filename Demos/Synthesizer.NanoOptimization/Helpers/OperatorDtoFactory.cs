﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Demos.Synthesizer.NanoOptimization.Dto;

namespace JJ.Demos.Synthesizer.NanoOptimization.Helpers
{
    internal static class OperatorDtoFactory
    {
        public static OperatorDtoBase CreateOperatorDto_8Partials()
        {
            VariableInput_OperatorDto frequency_OperatorDto = Create_Frequency_VariableInput_OperatorDto();

            int partialCount = 8;

            IList<OperatorDtoBase> partialOperatorDtos = new List<OperatorDtoBase>(partialCount);

            for (int i = 0; i < partialCount; i++)
            {
                OperatorDtoBase partialOperatorDto = CreateOperatorDto_SinglePartial(frequency_OperatorDto);
                partialOperatorDtos.Add(partialOperatorDto);
            }

            OperatorDtoBase operatorDto = new Add_OperatorDto(partialOperatorDtos);
            return operatorDto;
        }

        public static OperatorDtoBase CreateOperatorDto_SinglePartial()
        {
            VariableInput_OperatorDto frequency_OperatorDto = Create_Frequency_VariableInput_OperatorDto();
            OperatorDtoBase operatorDto = CreateOperatorDto_SinglePartial(frequency_OperatorDto);
            return operatorDto;
        }

        public static OperatorDtoBase CreateOperatorDto_SinglePartial(VariableInput_OperatorDto frequency_VariableInput_OperatorDto)
        {
            double volume = 10.0;
            double phaseShift = 0.25;

            var dto = new Multiply_OperatorDto
            (
                new Shift_OperatorDto
                (
                    new Sine_OperatorDto
                    (
                        frequency_VariableInput_OperatorDto
                    ),
                    new Number_OperatorDto(phaseShift)
                ),
                new Number_OperatorDto(volume)
            );

            return dto;
        }


        private static VariableInput_OperatorDto Create_Frequency_VariableInput_OperatorDto()
        {
            double frequency = 440.0;
            return new VariableInput_OperatorDto(frequency);
        }
    }
}