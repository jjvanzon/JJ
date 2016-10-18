﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Demos.Synthesizer.Inlining.Calculation;
using JJ.Demos.Synthesizer.Inlining.Calculation.Operators.WithInheritance;
using JJ.Demos.Synthesizer.Inlining.Dto;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Demos.Synthesizer.Inlining.Visitors.WithInheritance
{
    internal class OperatorDtoToOperatorCalculatorVisitor : OperatorDtoVisitorBase_AfterMathSimplification
    {
        private readonly DimensionStack _dimensionStack;
        private readonly Stack<OperatorCalculatorBase> _stack = new Stack<OperatorCalculatorBase>();

        public OperatorDtoToOperatorCalculatorVisitor(DimensionStack dimensionStack)
        {
            if (dimensionStack == null) throw new NullException(() => dimensionStack);
            _dimensionStack = dimensionStack;
        }

        public OperatorCalculatorBase Execute(OperatorDto dto)
        {
            var preProcessingVisitor = new PreProcessing_OperatorDtoVisitor();
            dto = preProcessingVisitor.Execute(dto);

            Visit_OperatorDto_Polymorphic(dto);

            if (_stack.Count != 1)
            {
                throw new NotEqualException(() => _stack.Count, 1);
            }

            return _stack.Pop();
        }


        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override IList<InletDto> VisitInletDtos(IList<InletDto> inletDtos)
        {
            // Reverse the order, so calculators pop off the stack in the right order.
            for (int i = inletDtos.Count - 1; i >= 0; i--)
            {
                InletDto inletDto = inletDtos[i];
                VisitInletDto(inletDto);
            }

            return inletDtos;
        }

        // Add

        protected override OperatorDto Visit_Add_OperatorDto_VarA_ConstB(Add_OperatorDto_VarA_ConstB dto)
        {
            base.Visit_Add_OperatorDto_VarA_ConstB(dto);

            OperatorCalculatorBase aCalculator = _stack.Pop();

            var calculator = new Add_OperatorCalculator_VarA_ConstB(aCalculator, dto.B);

            _stack.Push(calculator);

            return dto;
        }

        protected override OperatorDto Visit_Add_OperatorDto_VarA_VarB(Add_OperatorDto_VarA_VarB dto)
        {
            base.Visit_Add_OperatorDto_VarA_VarB(dto);

            OperatorCalculatorBase aCalculator = _stack.Pop();
            OperatorCalculatorBase bCalculator = _stack.Pop();

            var calculator = new Add_OperatorCalculator_VarA_VarB(aCalculator, bCalculator);

            _stack.Push(calculator);

            return dto;
        }

        // Multiply

        protected override OperatorDto Visit_Multiply_OperatorDto_VarA_ConstB(Multiply_OperatorDto_VarA_ConstB dto)
        {
            base.Visit_Multiply_OperatorDto_VarA_ConstB(dto);

            OperatorCalculatorBase aCalculator = _stack.Pop();

            var calculator = new Multiply_OperatorCalculator_VarA_ConstB(aCalculator, dto.B);

            _stack.Push(calculator);

            return dto;
        }

        protected override OperatorDto Visit_Multiply_OperatorDto_VarA_VarB(Multiply_OperatorDto_VarA_VarB dto)
        {
            base.Visit_Multiply_OperatorDto_VarA_VarB(dto);

            OperatorCalculatorBase aCalculator = _stack.Pop();
            OperatorCalculatorBase bCalculator = _stack.Pop();

            var calculator = new Multiply_OperatorCalculator_VarA_VarB(aCalculator, bCalculator);

            _stack.Push(calculator);

            return dto;
        }

        // Number

        protected override OperatorDto Visit_Number_OperatorDto_Concrete(Number_OperatorDto dto)
        {
            base.Visit_Number_OperatorDto_Concrete(dto);

            var calculator = new Number_OperatorCalculator(dto.Number);

            _stack.Push(calculator);

            return dto;
        }

        protected override OperatorDto Visit_Number_OperatorDto_NaN(Number_OperatorDto_NaN dto)
        {
            base.Visit_Number_OperatorDto_NaN(dto);

            var calculator = new Number_OperatorCalculator_NaN();

            _stack.Push(calculator);

            return dto;
        }

        protected override OperatorDto Visit_Number_OperatorDto_One(Number_OperatorDto_One dto)
        {
            base.Visit_Number_OperatorDto_One(dto);

            var calculator = new Number_OperatorCalculator_One();

            _stack.Push(calculator);

            return dto;
        }

        protected override OperatorDto Visit_Number_OperatorDto_Zero(Number_OperatorDto_Zero dto)
        {
            base.Visit_Number_OperatorDto_Zero(dto);

            var calculator = new Number_OperatorCalculator_Zero();

            _stack.Push(calculator);

            return dto;
        }

        // Shift

        protected override OperatorDto Visit_Shift_OperatorDto_VarSignal_ConstDistance(Shift_OperatorDto_VarSignal_ConstDistance dto)
        {
            base.Visit_Shift_OperatorDto_VarSignal_ConstDistance(dto);

            OperatorCalculatorBase signalCalculator = _stack.Pop();

            var calculator = new Shift_OperatorCalculator_VarSignal_ConstDifference(signalCalculator, dto.Distance, _dimensionStack);

            _stack.Push(calculator);

            return dto;
        }

        protected override OperatorDto Visit_Shift_OperatorDto_VarSignal_VarDistance(Shift_OperatorDto_VarSignal_VarDistance dto)
        {
            base.Visit_Shift_OperatorDto_VarSignal_VarDistance(dto);

            OperatorCalculatorBase signalCalculator = _stack.Pop();
            OperatorCalculatorBase distanceCalculator = _stack.Pop();

            var calculator = new Shift_OperatorCalculator_VarSignal_VarDifference(signalCalculator, distanceCalculator, _dimensionStack);

            _stack.Push(calculator);

            return dto;
        }

        // Sine

        protected override OperatorDto Visit_Sine_OperatorDto_ConstFrequency_NoOriginShifting(Sine_OperatorDto_ConstFrequency_NoOriginShifting dto)
        {
            base.Visit_Sine_OperatorDto_ConstFrequency_NoOriginShifting(dto);

            var calculator = new Sine_OperatorCalculator_ConstFrequency_NoOriginShifting(dto.Frequency, _dimensionStack);

            _stack.Push(calculator);

            return dto;
        }

        protected override OperatorDto Visit_Sine_OperatorDto_VarFrequency_NoPhaseTracking(Sine_OperatorDto_VarFrequency_NoPhaseTracking dto)
        {
            base.Visit_Sine_OperatorDto_VarFrequency_NoPhaseTracking(dto);

            OperatorCalculatorBase frequencyCalculator = _stack.Pop();

            var calculator = new Sine_OperatorCalculator_VarFrequency_NoPhaseTracking(frequencyCalculator, _dimensionStack);

            _stack.Push(calculator);

            return dto;
        }

        protected override OperatorDto Visit_Sine_OperatorDto_VarFrequency_WithPhaseTracking(Sine_OperatorDto_VarFrequency_WithPhaseTracking dto)
        {
            base.Visit_Sine_OperatorDto_VarFrequency_WithPhaseTracking(dto);

            OperatorCalculatorBase frequencyCalculator = _stack.Pop();

            var calculator = new Sine_OperatorCalculator_VarFrequency_WithPhaseTracking(frequencyCalculator, _dimensionStack);

            _stack.Push(calculator);

            return dto;
        }

        // VariableInput

        protected override OperatorDto Visit_VariableInput_OperatorDto(VariableInput_OperatorDto dto)
        {
            base.Visit_VariableInput_OperatorDto(dto);

            var calculator = new VariableInput_OperatorCalculator(dto.DefaultValue);

            _stack.Push(calculator);

            return dto;
        }
    }
}