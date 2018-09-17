﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using JJ.Business.Synthesizer.Calculation;
//using JJ.Business.Synthesizer.Calculation.Patches;
//using JJ.Business.Synthesizer.Configuration;
//using JJ.Business.Synthesizer.Enums;
//using JJ.Business.Synthesizer.Helpers;
//using JJ.Data.Synthesizer.Entities;
//using JJ.Framework.Collections;
//using JJ.Framework.Data;
//using JJ.Framework.Exceptions.Basic;
//using JJ.Framework.Exceptions.Comparative;
//using JJ.Framework.Mathematics;
//using JJ.Framework.Testing.Data;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//// ReSharper disable UnusedVariable
//// ReSharper disable InvertIf
//// ReSharper disable CompareOfFloatsByEqualityOperator
//// ReSharper disable LocalizableElement
//// ReSharper disable SuggestVarOrType_Elsewhere

//namespace JJ.Business.Synthesizer.Tests.Helpers
//{
//    internal class TestExecutor : IDisposable
//    {
//        private const int DEFAULT_SIGNIFICANT_DIGITS = 6;

//        private static readonly string _note =
//            $"(Note: Values are tested for {DEFAULT_SIGNIFICANT_DIGITS} significant digits and NaN is converted to 0.)";

//        private IContext _context;
//        private readonly IPatchCalculator _calculator;

//        public const DimensionEnum DEFAULT_DIMENSION_ENUM = DimensionEnum.Number;

//        private TestExecutor(CalculationMethodEnum calculationMethodEnum, Func<OperatorFactory, Outlet> operatorFactoryDelegate)
//        {
//            if (operatorFactoryDelegate == null) throw new ArgumentNullException(nameof(operatorFactoryDelegate));

//            AssertInconclusiveHelper.WithConnectionInconclusiveAssertion(() => _context = PersistenceHelper.CreateContext());

//            RepositoryWrapper repositories = PersistenceHelper.CreateRepositories(_context);
//            var patchFacade = new PatchFacade(repositories, calculationMethodEnum);
//            Patch patch = patchFacade.CreatePatch();
//            var operatorFactory = new OperatorFactory(patch, repositories);
//            Outlet outlet = operatorFactoryDelegate(operatorFactory);

//            _calculator = patchFacade.CreateCalculator(outlet, 2, 1, 0, new CalculatorCache());
//        }

//        ~TestExecutor() => Dispose();

//        public void Dispose() => _context?.Dispose();

//        // Public Static Methods

//        public static double CalculateOneValue(IPatchCalculator patchCalculator, double time = 0.0)
//        {
//            const int frameCount = 1;
//            var buffer = new float[1];
//            patchCalculator.Calculate(buffer, frameCount, time);
//            return buffer[0];
//        }

//        public static void TestWithoutInputs(
//            Func<OperatorFactory, Outlet> operatorFactoryDelegate,
//            double expectedY,
//            CalculationMethodEnum calculationMethodEnum)
//        {
//            using (var testExecutor = new TestExecutor(calculationMethodEnum, operatorFactoryDelegate))
//            {
//                testExecutor.TestWith1Input(DEFAULT_DIMENSION_ENUM, ((double)default, expectedY).AsArray());
//            }
//        }

//        public static void TestWith1Input(
//            Func<OperatorFactory, Outlet> operatorFactoryDelegate,
//            Func<double, double> func,
//            IList<double> xValues,
//            CalculationMethodEnum calculationMethodEnum)
//            => TestWith1Input(operatorFactoryDelegate, func, DEFAULT_DIMENSION_ENUM, xValues, calculationMethodEnum);

//        public static void TestWith1Input(
//            Func<OperatorFactory, Outlet> operatorFactoryDelegate,
//            Func<double, double> func,
//            DimensionEnum dimensionEnum,
//            IList<double> inputValues,
//            CalculationMethodEnum calculationMethodEnum)
//        {
//            IList<double> expectedOutputValues = inputValues.Select(func).ToArray();

//            using (var testExecutor = new TestExecutor(calculationMethodEnum, operatorFactoryDelegate))
//            {
//                testExecutor.TestWith1Input(dimensionEnum, inputValues, expectedOutputValues);
//            }
//        }

//        public static void TestWith2Inputs(
//            Func<OperatorFactory, Outlet> operatorFactoryDelegate,
//            Func<double, double, double> func,
//            DimensionEnum xDimensionEnum,
//            IList<double> xValues,
//            DimensionEnum yDimensionEnum,
//            IList<double> yValues,
//            CalculationMethodEnum calculationMethodEnum)
//        {
//            IList<(double x, double y)> inputPoints = xValues.CrossJoin(yValues, (x, y) => (x, y)).ToArray();
//            IList<double> expectedOutputValues = inputPoints.Select(xy => func(xy.x, xy.y)).ToArray();

//            using (var testExecutor = new TestExecutor(calculationMethodEnum, operatorFactoryDelegate))
//            {
//                testExecutor.TestWith2Inputs(xDimensionEnum, yDimensionEnum, inputPoints, expectedOutputValues);
//            }
//        }

//        public static void TestWith3Inputs(
//            Func<OperatorFactory, Outlet> operatorFactoryDelegate,
//            Func<double, double, double, double> func,
//            DimensionEnum xDimensionEnum,
//            IList<double> xValues,
//            DimensionEnum yDimensionEnum,
//            IList<double> yValues,
//            DimensionEnum zDimensionEnum,
//            IList<double> zValues,
//            CalculationMethodEnum calculationMethodEnum)
//        {
//            IList<(double x, double y, double z)> inputPoints =
//                xValues.CrossJoin(yValues, (x, y) => (x, y))
//                       .CrossJoin(yValues, (xy, z) => (xy.x, xy.y, z))
//                       .ToArray();

//            IList<double> expectedOutputValues = inputPoints.Select(xyz => func(xyz.x, xyz.y, xyz.z)).ToArray();

//            using (var testExecutor = new TestExecutor(calculationMethodEnum, operatorFactoryDelegate))
//            {
//                testExecutor.TestWith3Inputs(xDimensionEnum, yDimensionEnum, zDimensionEnum, inputPoints, expectedOutputValues);
//            }
//        }

//        // Private Instance Methods

//        private void TestWith1Input(
//            DimensionEnum dimensionEnum,
//            IList<double> inputValues,
//            IList<double> expectedOutputValues)
//            => TestWithNInputs(
//                new[] { dimensionEnum },
//                inputValues.Select(x => new[] { x }).Cast<IList<double>>().ToArray(),
//                expectedOutputValues);

//        private void TestWith2Inputs(
//            DimensionEnum xDimensionEnum,
//            DimensionEnum yDimensionEnum,
//            IList<(double x, double y)> inputPoints,
//            IList<double> expectedOutputValues)
//            => TestWithNInputs(
//                new[] { xDimensionEnum, yDimensionEnum },
//                inputPoints.Select(x => new[] { x.x, x.y }).Cast<IList<double>>().ToArray(),
//                expectedOutputValues);

//        private void TestWith3Inputs(
//            DimensionEnum xDimensionEnum,
//            DimensionEnum yDimensionEnum,
//            DimensionEnum zDimensionEnum,
//            IList<(double x, double y, double z)> inputPoints,
//            IList<double> expectedOutputValues)
//            => TestWithNInputs(
//                new[] { xDimensionEnum, yDimensionEnum, zDimensionEnum },
//                inputPoints.Select(x => new[] { x.x, x.y, x.z }).Cast<IList<double>>().ToArray(),
//                expectedOutputValues);

//        private void TestWithNInputs(
//            IList<DimensionEnum> inputDimensionEnums,
//            IList<IList<double>> inputPoints,
//            IList<double> expectedOutputValues)
//        {
//            // Pre-Conditions
//            if (inputDimensionEnums == null) throw new ArgumentNullException(nameof(inputDimensionEnums));
//            if (inputDimensionEnums.Count == 0) throw new CollectionEmptyException(nameof(inputDimensionEnums));
//            if (inputPoints == null) throw new ArgumentNullException(nameof(inputPoints));
//            if (inputPoints.Count == 0) throw new CollectionEmptyException(nameof(inputPoints));
//            if (expectedOutputValues == null) throw new ArgumentNullException(nameof(expectedOutputValues));
//            if (expectedOutputValues.Count == 0) throw new CollectionEmptyException(nameof(expectedOutputValues));

//            if (inputPoints.Count != expectedOutputValues.Count)
//            {
//                throw new NotEqualException(() => inputPoints.Count, () => expectedOutputValues.Count);
//            }

//            for (var i = 0; i < inputPoints.Count; i++)
//            {
//                if (inputPoints[i].Count != inputDimensionEnums.Count)
//                {
//                    throw new NotEqualException(() => inputPoints[i].Count, () => inputDimensionEnums.Count);
//                }
//            }

//            // Arrange
//            var buffer = new float[1];

//            // Execute
//            int? timeDimensionIndex = inputDimensionEnums.TryGetIndexOf(x => x == DimensionEnum.Time);

//            if (timeDimensionIndex.HasValue)
//            {
//                double firstTimeValue = inputPoints[0][timeDimensionIndex.Value];
//                _calculator.Reset(firstTimeValue);
//            }

//            var actualOutputValues = new double[inputPoints.Count];

//            for (var pointIndex = 0; pointIndex < inputPoints.Count; pointIndex++)
//            {
//                IList<double> inputValues = inputPoints[pointIndex];

//                Array.Clear(buffer, 0, buffer.Length);

//                // Set Values
//                for (var dimensionIndex = 0; dimensionIndex < inputDimensionEnums.Count; dimensionIndex++)
//                {
//                    DimensionEnum inputDimensionEnum = inputDimensionEnums[dimensionIndex];
//                    double inputValue = inputValues[dimensionIndex];
//                    _calculator.SetValue(inputDimensionEnum, inputValue);
//                }

//                // Determine Time
//                double time = 0;

//                if (timeDimensionIndex.HasValue)
//                {
//                    time = inputValues[timeDimensionIndex.Value];
//                }

//                // Calculate Value
//                _calculator.Calculate(buffer, buffer.Length, time);
//                double actualOutputValue = buffer[0];

//                actualOutputValues[pointIndex] = actualOutputValue;
//            }

//            // Assert
//            for (var i = 0; i < inputPoints.Count; i++)
//            {
//                IList<double> inputValues = inputPoints[i];
//                double expectedOutputValue = expectedOutputValues[i];
//                double actualOutputValue = actualOutputValues[i];

//                float canonicalExpectedOutputValue = ToCanonical(expectedOutputValue);
//                float canonicalActualOutputValue = ToCanonical(actualOutputValue);

//                string pointDescriptor = GetPointDescriptor(inputDimensionEnums, inputValues, i);

//                if (canonicalExpectedOutputValue != canonicalActualOutputValue)
//                {
//                    Assert.Fail(
//                        $"{pointDescriptor} " +
//                        $"should have result = {canonicalExpectedOutputValue}, " +
//                        $"but has result = {canonicalActualOutputValue} instead. {_note}");
//                }
//                else
//                {
//                    Console.WriteLine($"{pointDescriptor} => {canonicalActualOutputValue}");
//                }
//            }

//            Console.WriteLine(_note);
//        }

//        private string GetPointDescriptor(IList<DimensionEnum> inputDimensionEnums, IList<double> inputValues, int i)
//        {
//            if (inputValues.Count != inputDimensionEnums.Count)
//            {
//                throw new NotEqualException(() => inputValues.Count, () => inputDimensionEnums.Count);
//            }

//            string concatenatedInputValues = string.Join(", ", inputDimensionEnums.Zip(inputValues).Select(x => $"{x.Item1}={x.Item2}"));
//            string pointDescriptor = $"Tested point [{i}] = ({concatenatedInputValues})";
//            return pointDescriptor;
//        }

//        /// <summary> Converts to float, rounds to significant digits and converts NaN to 0 which winmm would trip over. </summary>
//        private static float ToCanonical(double input)
//        {
//            var output = (float)input;

//            output = MathHelper.RoundToSignificantDigits(output, DEFAULT_SIGNIFICANT_DIGITS);

//            // Calculation engine will not output NaN.
//            if (float.IsNaN(output))
//            {
//                output = 0;
//            }

//            return output;
//        }
//    }
//}