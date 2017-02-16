﻿using System.Collections.Generic;
using JJ.Business.Synthesizer.Enums;

namespace JJ.Business.Synthesizer.Dto
{
    internal class If_OperatorDto : If_OperatorDto_VarCondition_VarThen_VarElse
    { }

    internal class If_OperatorDto_VarCondition_VarThen_VarElse : OperatorDtoBase
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.If;

        public IOperatorDto ConditionOperatorDto { get; set; }
        public IOperatorDto ThenOperatorDto { get; set; }
        public IOperatorDto ElseOperatorDto { get; set; }

        public override IList<IOperatorDto> InputOperatorDtos
        {
            get { return new[] { ConditionOperatorDto, ThenOperatorDto, ElseOperatorDto }; }
            set { ConditionOperatorDto = value[0]; ThenOperatorDto = value[1]; ElseOperatorDto = value[2]; }
        }
    }

    internal class If_OperatorDto_VarCondition_VarThen_ConstElse : OperatorDtoBase
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.If;

        public IOperatorDto ConditionOperatorDto { get; set; }
        public IOperatorDto ThenOperatorDto { get; set; }
        public double Else { get; set; }

        public override IList<IOperatorDto> InputOperatorDtos
        {
            get { return new[] { ConditionOperatorDto, ThenOperatorDto }; }
            set { ConditionOperatorDto = value[0]; ThenOperatorDto = value[1]; }
        }
    }

    internal class If_OperatorDto_VarCondition_ConstThen_VarElse : OperatorDtoBase
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.If;

        public IOperatorDto ConditionOperatorDto { get; set; }
        public double Then { get; set; }
        public IOperatorDto ElseOperatorDto { get; set; }

        public override IList<IOperatorDto> InputOperatorDtos
        {
            get { return new[] { ConditionOperatorDto, ElseOperatorDto }; }
            set { ConditionOperatorDto = value[0]; ElseOperatorDto = value[1]; }
        }
    }

    internal class If_OperatorDto_VarCondition_ConstThen_ConstElse : OperatorDtoBase
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.If;

        public IOperatorDto ConditionOperatorDto { get; set; }
        public double Then { get; set; }
        public double Else { get; set; }

        public override IList<IOperatorDto> InputOperatorDtos
        {
            get { return new[] { ConditionOperatorDto }; }
            set { ConditionOperatorDto = value[0]; }
        }
    }

    internal class If_OperatorDto_ConstCondition_VarThen_VarElse : OperatorDtoBase
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.If;

        public double Condition { get; set; }
        public IOperatorDto ThenOperatorDto { get; set; }
        public IOperatorDto ElseOperatorDto { get; set; }

        public override IList<IOperatorDto> InputOperatorDtos
        {
            get { return new[] { ThenOperatorDto, ElseOperatorDto }; }
            set { ThenOperatorDto = value[0]; ElseOperatorDto = value[1]; }
        }
    }

    internal class If_OperatorDto_ConstCondition_VarThen_ConstElse : OperatorDtoBase
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.If;

        public double Condition { get; set; }
        public IOperatorDto ThenOperatorDto { get; set; }
        public double Else { get; set; }

        public override IList<IOperatorDto> InputOperatorDtos
        {
            get { return new[] { ThenOperatorDto }; }
            set { ThenOperatorDto = value[0]; }
        }
    }

    internal class If_OperatorDto_ConstCondition_ConstThen_VarElse : OperatorDtoBase
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.If;

        public double Condition { get; set; }
        public double Then { get; set; }
        public IOperatorDto ElseOperatorDto { get; set; }

        public override IList<IOperatorDto> InputOperatorDtos
        {
            get { return new[] { ElseOperatorDto }; }
            set { ElseOperatorDto = value[0]; }
        }
    }

    internal class If_OperatorDto_ConstCondition_ConstThen_ConstElse : OperatorDtoBase_WithoutInputOperatorDtos
    {
        public override OperatorTypeEnum OperatorTypeEnum => OperatorTypeEnum.If;

        public double Condition { get; set; }
        public double Then { get; set; }
        public double Else { get; set; }
    }
}