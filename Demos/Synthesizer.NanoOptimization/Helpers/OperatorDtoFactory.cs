﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Demos.Synthesizer.NanoOptimization.Dto;

namespace JJ.Demos.Synthesizer.NanoOptimization.Helpers
{
    internal static class OperatorDtoFactory
    {
        public static OperatorDto CreateOperatorDto_8Partials()
        {
            int partialCount = 8;

            IList<InletDto> addInletDtos = new List<InletDto>(partialCount);

            for (int i = 0; i < partialCount; i++)
            {
                OperatorDto partialOperatorDto = CreateOperatorDto_SinglePartial();
                addInletDtos.Add(new InletDto { InputOperatorDto = partialOperatorDto });
            }

            OperatorDto operatorDto = new Add_OperatorDto_Vars(addInletDtos);
            return operatorDto;
        }

        public static OperatorDto CreateOperatorDto_8Partials_WithMultiple2VarAdds_InsteadOfSingle8VarAdd()
        {
            OperatorDto operatorDto = CreateOperatorDto_SinglePartial();

            for (int i = 0; i < 6; i++)
            {
                OperatorDto nextPartial_OperatorDto = CreateOperatorDto_SinglePartial();

                operatorDto = new Add_OperatorDto(new InletDto(operatorDto), new InletDto(nextPartial_OperatorDto));
            }

            return operatorDto;
        }

        public static OperatorDto CreateOperatorDto_SinglePartial()
        {
            double frequency = 440.0;
            double volume = 10.0;
            double phaseShift = 0.25;

            var dto = new Multiply_OperatorDto
            (
                new InletDto
                (
                    new Shift_OperatorDto
                    (
                        new InletDto
                        (
                            new Sine_OperatorDto
                            (
                                new InletDto
                                (
                                    new VariableInput_OperatorDto(frequency)
                                )
                            )
                        ),
                        new InletDto
                        (
                            new Number_OperatorDto(phaseShift)
                        )
                    )
                ),
                new InletDto
                {
                    InputOperatorDto = new Number_OperatorDto(volume)
                }
            );

            return dto;
        }
    }
}