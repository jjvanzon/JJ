﻿using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class ClosestOverDimensionExp_OperatorDto : ClosestOverDimension_OperatorDto
    {
        public override string OperatorTypeName => nameof(OperatorTypeEnum.ClosestOverDimensionExp);

        public ClosestOverDimensionExp_OperatorDto(
            OperatorDtoBase inputOperatorDto, 
            OperatorDtoBase collectionOperatorDto, 
            OperatorDtoBase fromOperatorDto, 
            OperatorDtoBase tillOperatorDto, 
            OperatorDtoBase stepOperatorDto) 
            : base(
                  inputOperatorDto, 
                  collectionOperatorDto, 
                  fromOperatorDto, 
                  tillOperatorDto, 
                  stepOperatorDto)
        { }
    }
}