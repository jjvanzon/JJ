﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JJ.Framework.Common;
using JJ.Framework.Data;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Calculation.Patches;
using JJ.Business.Synthesizer.Tests.Helpers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.LinkTo;
using JJ.Business.Synthesizer.Api;
using JJ.Business.Synthesizer.Calculation;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Business;
using JJ.Framework.Collections;

namespace JJ.Business.Synthesizer.Tests
{
    [TestClass]
    public class SynthesizerTests
    {
        private const int DEFAULT_SAMPLING_RATE = 44100;
        private const int DEFAULT_CHANNEL_COUNT = 1;
        private const int DEFAULT_CHANNEL_INDEX = 0;

        [TestMethod]
        public void Test_Synthesizer()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                RepositoryWrapper repositories = PersistenceHelper.CreateRepositories(context);

                var patchManager = new PatchManager(repositories);
                Patch patch = patchManager.CreatePatch();
                var x = new OperatorFactory(patch, repositories);

                var add = x.Add(x.Number(2), x.Number(3));
                var subtract = x.Subtract(add, x.Number(1));

                IPatchCalculator calculator1 = patchManager.CreateCalculator(
                    add,
                    DEFAULT_SAMPLING_RATE,
                    DEFAULT_CHANNEL_COUNT,
                    DEFAULT_CHANNEL_INDEX,
                    new CalculatorCache());
                double value = TestHelper.CalculateOneValue(calculator1);
                Assert.AreEqual(5, value, 0.0001);

                IPatchCalculator calculator2 = patchManager.CreateCalculator(
                    subtract,
                    DEFAULT_SAMPLING_RATE,
                    DEFAULT_CHANNEL_COUNT,
                    DEFAULT_CHANNEL_INDEX,
                    new CalculatorCache());
                value = TestHelper.CalculateOneValue(calculator2);
                Assert.AreEqual(4, value, 0.0001);

                // Test recursive validator
                CultureHelper.SetThreadCultureName("nl-NL");

                add.Inputs[0] = null;
                var valueOperatorWrapper = new Number_OperatorWrapper(subtract.Inputs[DimensionEnum.B].Operator);
                valueOperatorWrapper.Number = 0;
                subtract.WrappedOperator.Inlets[0].Name = "134";

                //IValidator validator2 = new OperatorValidator_Recursive(subtract.Operator, repositories.CurveRepository, repositories.SampleRepository, repositories.DocumentRepository, alreadyDone: new HashSet<object>());
                //IValidator warningValidator = new OperatorWarningValidator_Recursive(subtract.Operator, repositories.SampleRepository);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_AddValidator_IsValidTrue()
        {
            //var op = new Operator
            //{
            //    Inlets = new Inlet[]
            //    { 
            //        new Inlet { Name = "qwer"},
            //        new Inlet { Name = "asdf" },
            //    },
            //    Outlets = new Outlet[]
            //    {
            //        new Outlet { Name = "zxcv" }
            //    }
            //});

            //var op2 = new Operator();

            //IValidator validator1 = new OperatorValidator_Add(
            //IValidator validator2 = new OperatorValidator_Add(new Operator());

            //bool isValid = validator1.IsValid &&
            //               validator2.IsValid;

            //Assert.IsTrue(isValid);

            Assert.Inconclusive("Test method was outcommented");
        }

        [TestMethod]
        public void Test_Synthesizer_Add()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                RepositoryWrapper repositories = PersistenceHelper.CreateRepositories(context);

                var patchManager = new PatchManager(repositories);
                Patch patch = patchManager.CreatePatch();

                var x = new OperatorFactory(patch, repositories);

                Number_OperatorWrapper val1 = x.Number(1);
                Number_OperatorWrapper val2 = x.Number(2);
                Number_OperatorWrapper val3 = x.Number(3);
                OperatorWrapper add = x.Add(val1, val2, val3);

                //IValidator validator = new OperatorValidator_Adder(adder.Operator);
                //validator.Verify();

                IPatchCalculator calculator = patchManager.CreateCalculator(
                    add,
                    DEFAULT_SAMPLING_RATE,
                    DEFAULT_CHANNEL_COUNT,
                    DEFAULT_CHANNEL_INDEX,
                    new CalculatorCache());
                double value = TestHelper.CalculateOneValue(calculator);

                //adder.Operator.Inlets[0].Name = "qwer";
                //IValidator validator2 = new OperatorValidator_Adder(adder.Operator);
                //validator2.Verify();
            }
        }

        [TestMethod]
        public void Test_Synthesizer_ShorterCodeNotation()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                RepositoryWrapper repositories = PersistenceHelper.CreateRepositories(context);

                var patchManager = new PatchManager(repositories);
                Patch patch = patchManager.CreatePatch();
                var x = new OperatorFactory(patch, repositories);

                var subtract = x.Subtract(x.Add(x.Number(2), x.Number(3)), x.Number(1));

                var subtract2 = x.Subtract(
                    x.Add(
                        x.Number(2),
                        x.Number(3)
                    ),
                    x.Number(1)
                );
            }
        }

        [TestMethod]
        public void Test_Synthesizer_SineWithCurve_InterpretedMode()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                RepositoryWrapper repositories = PersistenceHelper.CreateRepositories(context);

                CurveManager curveManager = new CurveManager(new CurveRepositories(repositories));
                Curve curve = curveManager.Create(1, 0, 1, 0.8, null, null, 0.8, 0);

                var patchManager = new PatchManager(repositories);
                Patch patch = patchManager.CreatePatch();
                var x = new OperatorFactory(patch, repositories);

                var outlet = x.MultiplyWithOrigin(x.Curve(curve), x.Sine(x.Number(440)));

                CultureHelper.SetThreadCultureName("nl-NL");

                //IValidator[] validators = 
                //{
                //    new OperatorValidator_Versatile(outlet.Operator, repositories.DocumentRepository),
                //    new OperatorWarningValidator_Versatile(outlet.Operator)
                //};
                //validators.ForEach(y => y.Verify());

                VoidResult result = curveManager.SaveCurveWithRelatedEntities(curve);
                if (!result.Successful)
                {
                    string messages = string.Join(", ", result.Messages);
                    throw new Exception(messages);
                }

                var calculator = patchManager.CreateCalculator(outlet, DEFAULT_SAMPLING_RATE, DEFAULT_CHANNEL_COUNT, DEFAULT_CHANNEL_INDEX, new CalculatorCache());

                var times = new[]
                {
                    0.00,
                    0.05,
                    0.10,
                    0.15,
                    0.20,
                    0.25,
                    0.30,
                    0.35,
                    0.40,
                    0.45,
                    0.50,
                    0.55,
                    0.60,
                    0.65,
                    0.70,
                    0.75,
                    0.80,
                    0.85,
                    0.90,
                    0.95,
                    1.00
                };

                var values = new double[times.Length];

                foreach (double time in times)
                {
                    values[0] = TestHelper.CalculateOneValue(calculator, time);
                }
                ;
            }
        }

        [TestMethod]
        public void Test_Synthesizer_OptimizedPatchCalculator()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                RepositoryWrapper repositories = PersistenceHelper.CreateRepositories(context);

                var patchManager = new PatchManager(repositories);
                Patch patch = patchManager.CreatePatch();
                var x = new OperatorFactory(patch, repositories);

                Outlet outlet = x.Add(x.Number(1), x.Number(2));
                IPatchCalculator calculator = patchManager.CreateCalculator(
                    outlet,
                    DEFAULT_SAMPLING_RATE,
                    DEFAULT_CHANNEL_COUNT,
                    DEFAULT_CHANNEL_INDEX,
                    new CalculatorCache());
                double result = TestHelper.CalculateOneValue(calculator);
                Assert.AreEqual(3.0, result, 0.0001);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_PatchCalculator_WithNullInlet()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                RepositoryWrapper repositories = PersistenceHelper.CreateRepositories(context);

                var patchManager = new PatchManager(repositories);
                Patch patch = patchManager.CreatePatch();
                var x = new OperatorFactory(patch, repositories);

                Outlet outlet = x.Add(null, x.Number(2));
                IPatchCalculator calculator = patchManager.CreateCalculator(
                    outlet,
                    DEFAULT_SAMPLING_RATE,
                    DEFAULT_CHANNEL_COUNT,
                    DEFAULT_CHANNEL_INDEX,
                    new CalculatorCache());
                double result = TestHelper.CalculateOneValue(calculator);
                Assert.AreEqual(2.0, result, 0.000000001);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_PatchCalculator_Nulls()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                RepositoryWrapper repositories = PersistenceHelper.CreateRepositories(context);

                var patchManager = new PatchManager(repositories);
                Patch patch = patchManager.CreatePatch();
                var x = new OperatorFactory(patch, repositories);

                Outlet outlet = x.Add(x.Number(1), x.Add(x.Number(2), null));
                IPatchCalculator calculator = patchManager.CreateCalculator(
                    outlet,
                    DEFAULT_SAMPLING_RATE,
                    DEFAULT_CHANNEL_COUNT,
                    DEFAULT_CHANNEL_INDEX,
                    new CalculatorCache());
                double result = TestHelper.CalculateOneValue(calculator);
                Assert.AreEqual(3.0, result, 0.000000001);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_PatchCalculator_NestedOperators()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                RepositoryWrapper repositories = PersistenceHelper.CreateRepositories(context);

                var patchManager = new PatchManager(repositories);
                Patch patch = patchManager.CreatePatch();
                var x = new OperatorFactory(patch, repositories);

                Outlet outlet = x.Add(x.Add(x.Number(1), x.Number(2)), x.Number(4));
                IPatchCalculator calculator = patchManager.CreateCalculator(
                    outlet,
                    DEFAULT_SAMPLING_RATE,
                    DEFAULT_CHANNEL_COUNT,
                    DEFAULT_CHANNEL_INDEX,
                    new CalculatorCache());
                double result = TestHelper.CalculateOneValue(calculator);
                Assert.AreEqual(7.0, result, 0.000000001);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_NoiseOperator()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                var repositories = PersistenceHelper.CreateRepositories(context);

                var audioFileOutputManager = new AudioFileOutputManager(new AudioFileOutputRepositories(repositories));
                var patchManager = new PatchManager(repositories);
                Patch patch = patchManager.CreatePatch();
                var x = new OperatorFactory(patch, repositories);

                Outlet outlet = x.MultiplyWithOrigin(x.Noise(), x.Number(short.MaxValue));

                IPatchCalculator patchCalculator = patchManager.CreateCalculator(
                    outlet,
                    DEFAULT_SAMPLING_RATE,
                    DEFAULT_CHANNEL_COUNT,
                    DEFAULT_CHANNEL_INDEX,
                    new CalculatorCache());

                AudioFileOutput audioFileOutput = audioFileOutputManager.Create();
                audioFileOutput.FilePath = "Test_Synthesizer_NoiseOperator.wav";
                audioFileOutput.Duration = 20;
                audioFileOutput.LinkTo(outlet);

                // Execute once to fill cache(s).
                audioFileOutputManager.WriteFile(audioFileOutput, patchCalculator);

                Stopwatch sw = Stopwatch.StartNew();
                audioFileOutputManager.WriteFile(audioFileOutput, patchCalculator);
                sw.Stop();

                double ratio = sw.Elapsed.TotalSeconds / audioFileOutput.Duration;
                string message = $"Ratio: {ratio * 100:0.00}%, {sw.ElapsedMilliseconds}ms.";

                // Also test interpreted calculator
                IPatchCalculator calculator = patchManager.CreateCalculator(
                    outlet,
                    DEFAULT_SAMPLING_RATE,
                    DEFAULT_CHANNEL_COUNT,
                    DEFAULT_CHANNEL_INDEX,
                    new CalculatorCache());

                double value;

                value = TestHelper.CalculateOneValue(calculator, 0.2);
                value = TestHelper.CalculateOneValue(calculator, 0.2);

                value = TestHelper.CalculateOneValue(calculator, 0.3);
                value = TestHelper.CalculateOneValue(calculator, 0.3);

                Assert.Inconclusive(message);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_InterpolateOperator_ConstantSamplingRate_Noise()
        {
            const double duration = 2;
            const int outputSamplingRate = 100;
            const int alternativeSamplingRate = 25;
            const int amplification = 20000;

            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                RepositoryWrapper repositories = PersistenceHelper.CreateRepositories(context);

                var audioFileOutputManager = new AudioFileOutputManager(new AudioFileOutputRepositories(repositories));
                var patchManager = new PatchManager(repositories);
                Patch patch = patchManager.CreatePatch();
                var x = new OperatorFactory(patch, repositories);

                Outlet noise = x.MultiplyWithOrigin(x.Noise(), x.Number(amplification));
                Outlet interpolatedNoise = x.Interpolate(noise, x.Number(alternativeSamplingRate));

                IPatchCalculator patchCalculator;

                AudioFileOutput audioFileOutput = audioFileOutputManager.Create();
                audioFileOutput.Duration = duration;

                audioFileOutput.FilePath = "Test_Synthesizer_InterpolateOperator_ConstantSamplingRate_Noise_Input.wav";
                audioFileOutput.SamplingRate = outputSamplingRate;
                audioFileOutput.LinkTo(noise);
                patchCalculator = patchManager.CreateCalculator(noise, DEFAULT_SAMPLING_RATE, DEFAULT_CHANNEL_COUNT, DEFAULT_CHANNEL_INDEX, new CalculatorCache());
                audioFileOutputManager.WriteFile(audioFileOutput, patchCalculator);

                audioFileOutput.FilePath = "Test_Synthesizer_InterpolateOperator_ConstantSamplingRate_Noise_WithLowerSamplingRate.wav";
                audioFileOutput.SamplingRate = alternativeSamplingRate;
                audioFileOutput.LinkTo(noise);
                patchCalculator = patchManager.CreateCalculator(noise, DEFAULT_SAMPLING_RATE, DEFAULT_CHANNEL_COUNT, DEFAULT_CHANNEL_INDEX, new CalculatorCache());
                audioFileOutputManager.WriteFile(audioFileOutput, patchCalculator);

                audioFileOutput.FilePath = "Test_Synthesizer_InterpolateOperator_ConstantSamplingRate_Noise_WithInterpolateOperator.wav";
                audioFileOutput.SamplingRate = outputSamplingRate;
                audioFileOutput.LinkTo(interpolatedNoise);
                patchCalculator = patchManager.CreateCalculator(
                    interpolatedNoise,
                    DEFAULT_SAMPLING_RATE,
                    DEFAULT_CHANNEL_COUNT,
                    DEFAULT_CHANNEL_INDEX,
                    new CalculatorCache());

                // Only test performance here and not in the other tests.

                // Execute once to fill cache(s).
                audioFileOutputManager.WriteFile(audioFileOutput, patchCalculator);

                Stopwatch sw = Stopwatch.StartNew();
                audioFileOutputManager.WriteFile(audioFileOutput, patchCalculator);
                sw.Stop();

                double ratio = sw.Elapsed.TotalSeconds / audioFileOutput.Duration;
                string message = $"Ratio: {ratio * 100:0.00}%, {sw.ElapsedMilliseconds}ms.";

                //// Also test interpreted calculator
                //IPatchCalculator calculator = patchManager.CreateCalculator(false, outlet);
                //double value = calculator.Calculate(0.2);
                //value = calculator.Calculate(0.2);
                //value = calculator.Calculate(0.3);
                //value = calculator.Calculate(0.3);

                Assert.Inconclusive(message);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_InterpolateOperator_Sine()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                RepositoryWrapper repositories = PersistenceHelper.CreateRepositories(context);

                AudioFileOutputManager audioFileOutputManager = new AudioFileOutputManager(new AudioFileOutputRepositories(repositories));

                var patchManager = new PatchManager(repositories);
                Patch patch = patchManager.CreatePatch();
                var x = new OperatorFactory(patch, repositories);

                const double volume = 1;
                const double frequency = 1.0;
                Outlet sine = x.Multiply(x.Number(volume), x.Sine(x.Number(frequency)));

                const double newSamplingRate = 4;
                Outlet interpolated = x.Interpolate(sine, x.Number(newSamplingRate));

                IPatchCalculator patchCalculator;

                AudioFileOutput audioFileOutput = audioFileOutputManager.Create();
                audioFileOutput.Duration = 2;
                audioFileOutput.SamplingRate = 44100;

                audioFileOutput.FilePath = "Test_Synthesizer_InterpolateOperator_Sine_Input.wav";
                audioFileOutput.LinkTo(sine);
                patchCalculator = patchManager.CreateCalculator(sine, DEFAULT_SAMPLING_RATE, DEFAULT_CHANNEL_COUNT, DEFAULT_CHANNEL_INDEX, new CalculatorCache());
                audioFileOutputManager.WriteFile(audioFileOutput, patchCalculator);

                audioFileOutput.FilePath = "Test_Synthesizer_InterpolateOperator_Sine_Interpolated.wav";
                audioFileOutput.LinkTo(interpolated);
                patchCalculator = patchManager.CreateCalculator(
                    interpolated,
                    DEFAULT_SAMPLING_RATE,
                    DEFAULT_CHANNEL_COUNT,
                    DEFAULT_CHANNEL_INDEX,
                    new CalculatorCache());
                audioFileOutputManager.WriteFile(audioFileOutput, patchCalculator);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_SawUp()
        {
            var x = new PatchApi();
            var saw = x.SawUp(x.Number(0.5));

            IPatchCalculator patchCalculator = x.CreateCalculator(
                saw,
                DEFAULT_SAMPLING_RATE,
                DEFAULT_CHANNEL_COUNT,
                DEFAULT_CHANNEL_INDEX,
                new CalculatorCache());

            var times = new[]
            {
                0.00,
                0.25,
                0.50,
                0.75,
                1.00,
                1.25,
                1.50,
                1.75,
                2.00
            };

            var values = new List<double>(times.Length);

            foreach (double time in times)
            {
                double value = TestHelper.CalculateOneValue(patchCalculator);
                values.Add(value);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_Triangle()
        {
            var patcher = new PatchApi();
            var outlet = patcher.Triangle(patcher.Number(1));

            IPatchCalculator patchCalculator = patcher.CreateCalculator(
                outlet,
                DEFAULT_SAMPLING_RATE,
                DEFAULT_CHANNEL_COUNT,
                DEFAULT_CHANNEL_INDEX,
                new CalculatorCache());

            double[] times =
            {
                0.000,
                0.125,
                0.250,
                0.375,
                0.500,
                0.625,
                0.750,
                0.875,
                1.000,
                1.125,
                1.250,
                1.375,
                1.500,
                1.625,
                1.750,
                1.875,
                2.000
            };

            var values = new List<double>(times.Length);
            foreach (double time in times)
            {
                double value = TestHelper.CalculateOneValue(patchCalculator, time);
                values.Add(value);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_ValidateAllRootDocuments()
        {
            using (IContext context = PersistenceHelper.CreateDatabaseContext())
            {
                var repositories = PersistenceHelper.CreateRepositories(context);
                var documentManager = new DocumentManager(repositories);

                IList<string> messages = new List<string>();

                IEnumerable<Document> rootDocuments = repositories.DocumentRepository.GetAll();
                foreach (Document rootDocument in rootDocuments)
                {
                    IResult result = documentManager.Save(rootDocument);
                    messages.AddRange(result.Messages);
                }

                if (messages.Count > 0)
                {
                    string formattedMessages = string.Join(" ", messages);
                    throw new Exception(formattedMessages);
                }
            }
        }

        [TestMethod]
        public void Test_Synthesizer_OperatorFactory_GenericMethods()
        {
            using (IContext context = PersistenceHelper.CreateDatabaseContext())
            {
                var repositories = PersistenceHelper.CreateRepositories(context);
                var patchManager = new PatchManager(repositories);

                Patch patch = patchManager.CreatePatch();

                var x = new OperatorFactory(patch, repositories);
                x.New("DivideWithOrigin");
            }
        }
    }
}