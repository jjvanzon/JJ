﻿using JJ.Business.Synthesizer.Dto;
using JJ.Framework.Exceptions;

namespace JJ.Business.Synthesizer.Visitors
{
    /// <summary>
    /// This combinator class may currently seem yet another useless layer,
    /// but when multiple calculation output formats become possible, you need a combinator on this level.
    /// </summary>
    internal class OperatorDtoPreProcessingExecutor
    {
        private readonly int _samplingRate;
        private readonly int _targetChannelCount;

        public OperatorDtoPreProcessingExecutor(int samplingRate, int targetChannelCount)
        {
            _samplingRate = samplingRate;
            _targetChannelCount = targetChannelCount;
        }

        public OperatorDtoBase Execute(OperatorDtoBase dto)
        {
            if (dto == null) throw new NullException(() => dto);

            new OperatorDtoVisitor_InfrastructureVariables(_samplingRate, _targetChannelCount).Execute(dto);
            dto = new OperatorDtoVisitor_MathSimplification().Execute(dto);
            dto = new OperatorDtoVisitor_MachineOptimization().Execute(dto);
            dto = new OperatorDtoVisitor_Rewiring().Execute(dto);
            dto = new OperatorDtoVisitor_ProgrammerLaziness().Execute(dto);
            new OperatorDtoVisitor_DimensionStackLevels().Execute(dto);

            return dto;
        }
    }
}
