﻿using System;
using System.Diagnostics;
using JJ.Business.SynthesizerPrototype.Dto;
using JJ.Business.SynthesizerPrototype.Tests.Helpers;
using JJ.Business.SynthesizerPrototype.Tests.Helpers.WithInheritance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JJ.Business.SynthesizerPrototype.WithInheritance.CopiedCode.From_JJ_Business_SynthesizerPrototype;

namespace JJ.Business.SynthesizerPrototype.Tests
{
    [TestClass]
    public class Synthesizer_PerformanceTests_WithInheritance
    {
        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithoutTime_8Partials_50_000_Iterations_WithInheritance_NoDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorStructure_8Partials(dimensionStack);

            var stopWatch = Stopwatch.StartNew();

            for (int i = 0; i < 50000; i++)
            {
                calculator.Calculate();
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(50000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithoutTime_8Partials_50_000_Iterations_WithInheritance_WithDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            OperatorDtoBase dto = OperatorDtoFactory.CreateOperatorDto_8Partials();
            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorFromDto(dto, dimensionStack);

            var stopWatch = Stopwatch.StartNew();

            for (int i = 0; i < 50000; i++)
            {
                calculator.Calculate();
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(50000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithoutTime_8Partials_500_000_Iterations_WithInheritance_NoDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorStructure_8Partials(dimensionStack);

            var stopWatch = Stopwatch.StartNew();

            for (int i = 0; i < 500000; i++)
            {
                calculator.Calculate();
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(500000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithoutTime_8Partials_500_000_Iterations_WithInheritance_WithDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            OperatorDtoBase dto = OperatorDtoFactory.CreateOperatorDto_8Partials();
            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorFromDto(dto, dimensionStack);

            var stopWatch = Stopwatch.StartNew();

            for (int i = 0; i < 500000; i++)
            {
                calculator.Calculate();
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(500000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithoutTime_SinglePartial_50_000_Iterations_WithInheritance_NoDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorStructure_SinglePartial(dimensionStack);

            var stopWatch = Stopwatch.StartNew();

            for (int i = 0; i < 50000; i++)
            {
                calculator.Calculate();
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(50000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithoutTime_SinglePartial_50_000_Iterations_WithInheritance_WithDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            OperatorDtoBase dto = OperatorDtoFactory.CreateOperatorDto_SinglePartial();
            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorFromDto(dto, dimensionStack);

            var stopWatch = Stopwatch.StartNew();

            for (int i = 0; i < 50000; i++)
            {
                calculator.Calculate();
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(50000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithoutTime_SinglePartial_500_000_Iterations_WithInheritance_NoDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorStructure_SinglePartial(dimensionStack);

            var stopWatch = Stopwatch.StartNew();

            for (int i = 0; i < 500000; i++)
            {
                calculator.Calculate();
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(500000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithoutTime_SinglePartial_500_000_Iterations_WithInheritance_WithDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            OperatorDtoBase dto = OperatorDtoFactory.CreateOperatorDto_8Partials();
            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorFromDto(dto, dimensionStack);

            var stopWatch = Stopwatch.StartNew();

            for (int i = 0; i < 500000; i++)
            {
                calculator.Calculate();
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(500000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithTime_8Partials_50_000_Iterations_WithInheritance_NoDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorStructure_8Partials(dimensionStack);

            double t = 0.0;
            double dt = 1.0 / 50000.0;

            var stopWatch = Stopwatch.StartNew();

            while (t <= 1.0)
            {
                dimensionStack.Set(t);

                double value = calculator.Calculate();

                t += dt;
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(50000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithTime_8Partials_50_000_Iterations_WithInheritance_WithDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            OperatorDtoBase dto = OperatorDtoFactory.CreateOperatorDto_8Partials();
            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorFromDto(dto, dimensionStack);

            double t = 0.0;
            double dt = 1.0 / 50000.0;

            var stopWatch = Stopwatch.StartNew();

            while (t <= 1.0)
            {
                dimensionStack.Set(t);

                double value = calculator.Calculate();

                t += dt;
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(50000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithTime_8Partials_500_000_Iterations_WithInheritance_NoDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorStructure_8Partials(dimensionStack);

            double t = 0.0;
            double dt = 1.0 / 500000.0;

            var stopWatch = Stopwatch.StartNew();

            while (t <= 1.0)
            {
                dimensionStack.Set(t);

                double value = calculator.Calculate();

                t += dt;
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(500000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithTime_8Partials_500_000_Iterations_WithInheritance_WithDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            OperatorDtoBase dto = OperatorDtoFactory.CreateOperatorDto_8Partials();
            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorFromDto(dto, dimensionStack);

            double t = 0.0;
            double dt = 1.0 / 500000.0;

            var stopWatch = Stopwatch.StartNew();

            while (t <= 1.0)
            {
                dimensionStack.Set(t);

                double value = calculator.Calculate();

                t += dt;
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(500000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithTime_SinglePartial_50_000_Iterations_WithInheritance_NoDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorStructure_SinglePartial(dimensionStack);

            double t = 0.0;
            double dt = 1.0 / 50000.0;

            var stopWatch = Stopwatch.StartNew();

            while (t <= 1.0)
            {
                dimensionStack.Set(t);

                double value = calculator.Calculate();

                t += dt;
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(50000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithTime_SinglePartial_50_000_Iterations_WithInheritance_WithDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            OperatorDtoBase dto = OperatorDtoFactory.CreateOperatorDto_8Partials();
            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorFromDto(dto, dimensionStack);

            double t = 0.0;
            double dt = 1.0 / 50000.0;

            var stopWatch = Stopwatch.StartNew();

            while (t <= 1.0)
            {
                dimensionStack.Set(t);

                double value = calculator.Calculate();

                t += dt;
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(50000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithTime_SinglePartial_500_000_Iterations_WithInheritance_NoDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorStructure_SinglePartial(dimensionStack);

            double t = 0.0;
            double dt = 1.0 / 500000.0;

            var stopWatch = Stopwatch.StartNew();

            while (t <= 1.0)
            {
                dimensionStack.Set(t);

                double value = calculator.Calculate();

                t += dt;
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(500000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }

        [TestMethod]
        public void PerformanceTest_SynthesizerPrototype_WithTime_SinglePartial_500_000_Iterations_WithInheritance_WithDto()
        {
            var dimensionStack = new DimensionStack();
            dimensionStack.Push(0.0);

            OperatorDtoBase dto = OperatorDtoFactory.CreateOperatorDto_SinglePartial();
            var calculator = OperatorCalculatorFactory.CreateOperatorCalculatorFromDto(dto, dimensionStack);

            double t = 0.0;
            double dt = 1.0 / 500000.0;

            var stopWatch = Stopwatch.StartNew();

            while (t <= 1.0)
            {
                dimensionStack.Set(t);

                double value = calculator.Calculate();

                t += dt;
            }

            stopWatch.Stop();

            string message = TestHelper.GetPerformanceInfoMessage(500000, stopWatch.Elapsed);

            Assert.Inconclusive(message);
        }
    }
}
