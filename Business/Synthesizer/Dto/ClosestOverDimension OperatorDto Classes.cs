﻿using System.Collections.Generic;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class ClosestOverDimension_OperatorDto : OperatorDtoBase_WithDimension
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.ClosestOverDimension;
        
        public IOperatorDto InputOperatorDto { get; set; }
        public IOperatorDto CollectionOperatorDto { get; set; }
        public IOperatorDto FromOperatorDto { get; set; }
        public IOperatorDto TillOperatorDto { get; set; }
        public IOperatorDto StepOperatorDto { get; set; }

        public CollectionRecalculationEnum CollectionRecalculationEnum { get; set; }

        public override IList<IOperatorDto> InputOperatorDtos
        {
            get { return new[] { InputOperatorDto, CollectionOperatorDto, FromOperatorDto, TillOperatorDto, StepOperatorDto }; }
            set { InputOperatorDto = value[0]; CollectionOperatorDto = value[1]; FromOperatorDto = value[2]; TillOperatorDto = value[3]; StepOperatorDto = value[4]; }
        }
    }

    internal class ClosestOverDimension_OperatorDto_CollectionRecalculationContinuous : ClosestOverDimension_OperatorDto
    { }

    internal class ClosestOverDimension_OperatorDto_CollectionRecalculationUponReset : ClosestOverDimension_OperatorDto
    { }
}