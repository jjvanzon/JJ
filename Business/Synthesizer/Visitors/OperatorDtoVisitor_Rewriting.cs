﻿using JJ.Business.Synthesizer.Dto.Operators;
using JJ.Business.Synthesizer.Helpers;

namespace JJ.Business.Synthesizer.Visitors
{
	internal class OperatorDtoVisitor_Rewriting : OperatorDtoVisitorBase_AfterMathSimplification
	{
		// General

		public IOperatorDto Execute(IOperatorDto dto)
		{
			return Visit_OperatorDto_Polymorphic(dto);
		}

		protected override IOperatorDto Visit_OperatorDto_Polymorphic(IOperatorDto dto)
		{
			return WithAlreadyProcessedCheck(dto, () => base.Visit_OperatorDto_Polymorphic(dto));
		}

		// InletsToDimension

		protected override IOperatorDto Visit_InletsToDimension_OperatorDto_CubicAbruptSlope(InletsToDimension_OperatorDto_CubicAbruptSlope dto)
		{
			return Process_InletsToDimension_OperatorDto(dto, new Interpolate_OperatorDto_CubicAbruptSlope());
		}

		protected override IOperatorDto Visit_InletsToDimension_OperatorDto_CubicEquidistant(InletsToDimension_OperatorDto_CubicEquidistant dto)
		{
			return Process_InletsToDimension_OperatorDto(dto, new Interpolate_OperatorDto_CubicEquidistant());
		}

		protected override IOperatorDto Visit_InletsToDimension_OperatorDto_CubicSmoothSlope_LagBehind(InletsToDimension_OperatorDto_CubicSmoothSlope_LagBehind dto)
		{
			return Process_InletsToDimension_OperatorDto(dto, new Interpolate_OperatorDto_CubicSmoothSlope_LagBehind());
		}

		protected override IOperatorDto Visit_InletsToDimension_OperatorDto_Hermite_LagBehind(InletsToDimension_OperatorDto_Hermite_LagBehind dto)
		{
			return Process_InletsToDimension_OperatorDto(dto, new Interpolate_OperatorDto_Hermite_LagBehind());
		}

		protected override IOperatorDto Visit_InletsToDimension_OperatorDto_Line(InletsToDimension_OperatorDto_Line sourceDto)
		{
			var intermediateDto = new InletsToDimension_OperatorDto_Stripe_LagBehind();
			DtoCloner.CloneProperties(sourceDto, intermediateDto);
			intermediateDto.ResampleInterpolationTypeEnum = sourceDto.ResampleInterpolationTypeEnum;

			var destDto = new Interpolate_OperatorDto_Line_LagBehind_ConstSamplingRate();
			DtoCloner.CloneProperties(sourceDto, destDto);
			destDto.Signal = intermediateDto;
			destDto.Position = sourceDto.Position;
			destDto.SamplingRate = 1.0;
			destDto.ResampleInterpolationTypeEnum = sourceDto.ResampleInterpolationTypeEnum;

			return destDto;
		}

		private static IOperatorDto Process_InletsToDimension_OperatorDto(InletsToDimension_OperatorDto sourceDto, Interpolate_OperatorDto destDto)
		{
			var intermediateDto = new InletsToDimension_OperatorDto_Stripe_LagBehind();
			DtoCloner.CloneProperties(sourceDto, intermediateDto);
			intermediateDto.ResampleInterpolationTypeEnum = sourceDto.ResampleInterpolationTypeEnum;

			DtoCloner.CloneProperties(sourceDto, destDto);
			destDto.Signal = intermediateDto;
			destDto.Position = sourceDto.Position;
			destDto.SamplingRate = 1.0;
			destDto.ResampleInterpolationTypeEnum = sourceDto.ResampleInterpolationTypeEnum;

			return destDto;
		}

		// Random

		protected override IOperatorDto Visit_Random_OperatorDto_CubicAbruptSlope(Random_OperatorDto_CubicAbruptSlope dto)
		{
			return Process_Random_OperatorDto(dto, new Interpolate_OperatorDto_CubicAbruptSlope());
		}

		protected override IOperatorDto Visit_Random_OperatorDto_CubicEquidistant(Random_OperatorDto_CubicEquidistant dto)
		{
			return Process_Random_OperatorDto(dto, new Interpolate_OperatorDto_CubicEquidistant());
		}

		protected override IOperatorDto Visit_Random_OperatorDto_CubicSmoothSlope_LagBehind(Random_OperatorDto_CubicSmoothSlope_LagBehind dto)
		{
			return Process_Random_OperatorDto(dto, new Interpolate_OperatorDto_CubicSmoothSlope_LagBehind());
		}

		protected override IOperatorDto Visit_Random_OperatorDto_Hermite_LagBehind(Random_OperatorDto_Hermite_LagBehind dto)
		{
			return Process_Random_OperatorDto(dto, new Interpolate_OperatorDto_Hermite_LagBehind());
		}
		
		protected override IOperatorDto Visit_Random_OperatorDto_Line_LagBehind_ConstRate(Random_OperatorDto_Line_LagBehind_ConstRate sourceDto)
		{
			var intermediateDto = new Random_OperatorDto_Stripe_LagBehind();
			DtoCloner.CloneProperties(sourceDto, intermediateDto);

			var destDto = new Interpolate_OperatorDto_Line_LagBehind_ConstSamplingRate();
			DtoCloner.CloneProperties(sourceDto, destDto);
			destDto.Signal = intermediateDto;
			destDto.SamplingRate = sourceDto.Rate;
			destDto.Position = sourceDto.Position;
			destDto.ResampleInterpolationTypeEnum = sourceDto.ResampleInterpolationTypeEnum;

			return destDto;
		}

		protected override IOperatorDto Visit_Random_OperatorDto_Line_LagBehind_VarRate(Random_OperatorDto_Line_LagBehind_VarRate dto)
		{
			return Process_Random_OperatorDto(dto, new Interpolate_OperatorDto_Line_LagBehind_VarSamplingRate());
		}

		private static IOperatorDto Process_Random_OperatorDto(Random_OperatorDto sourceDto, Interpolate_OperatorDto destDto)
		{
			var intermediateDto = new Random_OperatorDto_Stripe_LagBehind();
			DtoCloner.CloneProperties(sourceDto, intermediateDto);

			DtoCloner.CloneProperties(sourceDto, destDto);
			destDto.Signal = intermediateDto;
			destDto.SamplingRate = sourceDto.Rate;
			destDto.Position = sourceDto.Position;
			destDto.ResampleInterpolationTypeEnum = sourceDto.ResampleInterpolationTypeEnum;

			return destDto;
		}
	}
}
