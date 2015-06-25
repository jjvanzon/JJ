﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JJ.Framework.Common;
using JJ.Framework.Data;
using JJ.Framework.Validation;
using JJ.Framework.IO;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Business.Synthesizer.Extensions;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Validation;
using JJ.Business.Synthesizer.Warnings;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Factories;
using JJ.Business.Synthesizer.Calculation;
using JJ.Business.Synthesizer.Tests.Helpers;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Managers;
using System.IO;
using JJ.Business.Synthesizer.Calculation.AudioFileOutputs;
using JJ.Business.Synthesizer.Calculation.Operators;
using JJ.Framework.Testing;
using JJ.Business.Synthesizer.Tests.Accessors;
using JJ.Business.Synthesizer.Structs;
using JJ.Business.Synthesizer.Infos;

namespace JJ.Business.Synthesizer.Tests
{
    [TestClass]
    public class SynthesizerTests
    {
        [TestMethod]
        public void Test_Synthesizer()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                ICurveRepository curveRepository = PersistenceHelper.CreateRepository<ICurveRepository>(context);
                ISampleRepository sampleRepository = PersistenceHelper.CreateRepository<ISampleRepository>(context);

                OperatorFactory x = TestHelper.CreateOperatorFactory(context);

                Add_OperatorWrapper add = x.Add(x.Value(2), x.Value(3));
                Substract_OperatorWrapper substract = x.Substract(add, x.Value(1));

                IValidator validator = new OperatorValidator_Recursive(substract.Operator, curveRepository, sampleRepository, alreadyDone: new HashSet<object>());
                validator.Verify();

                IOperatorCalculator calculator1 = new OptimizedOperatorCalculator(curveRepository, sampleRepository, add);
                double value = calculator1.Calculate(0, 0);
                Assert.AreEqual(5, value, 0.0001);

                IOperatorCalculator calculator2 = new OptimizedOperatorCalculator(curveRepository, sampleRepository, substract);
                value = calculator2.Calculate(0, 0);
                Assert.AreEqual(4, value, 0.0001);

                // Test recursive validator
                CultureHelper.SetThreadCulture("nl-NL");

                add.OperandA = null;
                var valueOperatorWrapper = new Value_OperatorWrapper(substract.OperandB.Operator);
                valueOperatorWrapper.Value = 0;
                substract.Operator.Inlets[0].Name = "134";

                IValidator validator2 = new OperatorValidator_Recursive(substract.Operator, curveRepository, sampleRepository, alreadyDone: new HashSet<object>());
                IValidator warningValidator = new OperatorWarningValidator_Recursive(substract.Operator, sampleRepository);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_AddValidator()
        {
            IValidator validator1 = new OperatorValidator_Add(new Operator
            {
                Inlets = new Inlet[]
                { 
                    new Inlet { Name = "qwer"},
                    new Inlet { Name = "asdf" },
                },
                Outlets = new Outlet[]
                {
                    new Outlet { Name = "zxcv" }
                }
            });

            IValidator validator2 = new OperatorValidator_Add(new Operator());

            bool isValid = validator1.IsValid &&
                           validator2.IsValid;
        }

        [TestMethod]
        public void Test_Synthesizer_WarningValidators()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                OperatorFactory factory = TestHelper.CreateOperatorFactory(context);

                IValidator validator1 = new OperatorWarningValidator_Add(factory.Add().Operator);
                IValidator validator2 = new OperatorWarningValidator_Value(factory.Value().Operator);

                bool isValid = validator1.IsValid &&
                               validator2.IsValid;
            }
        }

        [TestMethod]
        public void Test_Synthesizer_InterpretedOperatorCalculator_Adder()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                ICurveRepository curveRepository = PersistenceHelper.CreateRepository<ICurveRepository>(context);
                ISampleRepository sampleRepository = PersistenceHelper.CreateRepository<ISampleRepository>(context);

                OperatorFactory factory = TestHelper.CreateOperatorFactory(context);

                Value_OperatorWrapper val1 = factory.Value(1);
                Value_OperatorWrapper val2 = factory.Value(2);
                Value_OperatorWrapper val3 = factory.Value(3);
                Adder_OperatorWrapper adder = factory.Adder(val1, val2, val3);

                IValidator validator = new OperatorValidator_Adder(adder.Operator);
                validator.Verify();

                var calculator = new InterpretedOperatorCalculator(curveRepository, sampleRepository, adder);
                double value = calculator.Calculate(0, 0);

                adder.Operator.Inlets[0].Name = "qwer";
                IValidator validator2 = new OperatorValidator_Adder(adder.Operator);
                //validator2.Verify();
            }
        }

        [TestMethod]
        public void Test_Synthesizer_ShorterCodeNotation()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                OperatorFactory x = TestHelper.CreateOperatorFactory(context);

                Substract_OperatorWrapper substract = x.Substract(x.Add(x.Value(2), x.Value(3)), x.Value(1));

                Substract_OperatorWrapper substract2 = x.Substract
                (
                    x.Add
                    (
                        x.Value(2),
                        x.Value(3)
                    ),
                    x.Value(1)
                );
            }
        }

        [TestMethod]
        public void Test_Synthesizer_InterpretedOperatorCalculator_SineWithCurve_InterpretedMode()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                ICurveRepository curveRepository = PersistenceHelper.CreateRepository<ICurveRepository>(context);
                ISampleRepository sampleRepository = PersistenceHelper.CreateRepository<ISampleRepository>(context);

                CurveFactory curveFactory = TestHelper.CreateCurveFactory(context);
                Curve curve = curveFactory.CreateCurve(1, 0, 1, 0.8, null, null, 0.8, 0);

                OperatorFactory f = TestHelper.CreateOperatorFactory(context);
                Sine_OperatorWrapper sine = f.Sine(f.CurveIn(curve), f.Value(440));

                CultureHelper.SetThreadCulture("nl-NL");
                IValidator[] validators = 
                {
                    new CurveValidator(curve, alreadyDone: new HashSet<object>()), 
                    new OperatorValidator_Versatile(sine.Operator),
                    new OperatorWarningValidator_Versatile(sine.Operator)
                };
                validators.ForEach(x => x.Verify());

                var calculator = new InterpretedOperatorCalculator(curveRepository, sampleRepository, sine);
                var values = new double[]
                {
                    calculator.Calculate(sine, 0.00),
                    calculator.Calculate(sine, 0.05),
                    calculator.Calculate(sine, 0.10),
                    calculator.Calculate(sine, 0.15),
                    calculator.Calculate(sine, 0.20),
                    calculator.Calculate(sine, 0.25),
                    calculator.Calculate(sine, 0.30),
                    calculator.Calculate(sine, 0.35),
                    calculator.Calculate(sine, 0.40),
                    calculator.Calculate(sine, 0.45),
                    calculator.Calculate(sine, 0.50),
                    calculator.Calculate(sine, 0.55),
                    calculator.Calculate(sine, 0.60),
                    calculator.Calculate(sine, 0.65),
                    calculator.Calculate(sine, 0.70),
                    calculator.Calculate(sine, 0.75),
                    calculator.Calculate(sine, 0.80),
                    calculator.Calculate(sine, 0.85),
                    calculator.Calculate(sine, 0.90),
                    calculator.Calculate(sine, 0.95),
                    calculator.Calculate(sine, 1.00)
                };
            }
        }

        [TestMethod]
        public void Test_Synthesizer_TimePowerWithEcho()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                ICurveRepository curveRepository = PersistenceHelper.CreateRepository<ICurveRepository>(context);
                ISampleRepository sampleRepository = PersistenceHelper.CreateRepository<ISampleRepository>(context);

                SampleManager sampleManager = TestHelper.CreateSampleManager(context);
                AudioFileOutputManager audioFileOutputManager = TestHelper.CreateAudioFileOutputManager(context);
                OperatorFactory operatorFactory = TestHelper.CreateOperatorFactory(context);

                Stream sampleStream = TestHelper.GetViolin16BitMono44100WavStream();
                Sample sample = sampleManager.CreateSample(sampleStream);
                sample.SamplingRate = 8000;
                sample.BytesToSkip = 100;

                Outlet sampleOperatorOutlet = operatorFactory.Sample(sample);
                Outlet effect = EntityFactory.CreateTimePowerEffectWithEcho(operatorFactory, sampleOperatorOutlet);

                AudioFileOutput audioFileOutput = audioFileOutputManager.CreateWithRelatedEntities();
                audioFileOutput.AudioFileOutputChannels[0].Outlet = effect;
                audioFileOutput.FilePath = "Test_Synthesizer_TimePowerWithEcho.wav";
                audioFileOutput.Duration = 6.5;

                Stopwatch sw = Stopwatch.StartNew();
                audioFileOutputManager.Execute(audioFileOutput);
                sw.Stop();

                double ratio = sw.Elapsed.TotalSeconds / audioFileOutput.Duration;
                string message = String.Format("Ratio: {0:0.00}%, {1}ms.", ratio * 100, sw.ElapsedMilliseconds);
                Assert.Inconclusive(message);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_MultiplyWithEcho()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                ICurveRepository curveRepository = PersistenceHelper.CreateRepository<ICurveRepository>(context);
                ISampleRepository sampleRepository = PersistenceHelper.CreateRepository<ISampleRepository>(context);

                SampleManager sampleManager = TestHelper.CreateSampleManager(context);
                AudioFileOutputManager audioFileOutputManager = TestHelper.CreateAudioFileOutputManager(context);
                OperatorFactory operatorFactory = TestHelper.CreateOperatorFactory(context);

                Stream sampleStream = TestHelper.GetViolin16BitMono44100WavStream();
                Sample sample = sampleManager.CreateSample(sampleStream);
                sample.SamplingRate = 8000;
                sample.BytesToSkip = 100;

                Outlet sampleOperatorOutlet = operatorFactory.Sample(sample);
                Outlet effect = EntityFactory.CreateMultiplyWithEcho(operatorFactory, sampleOperatorOutlet);

                AudioFileOutput audioFileOutput = audioFileOutputManager.CreateWithRelatedEntities();
                audioFileOutput.AudioFileOutputChannels[0].Outlet = effect;
                audioFileOutput.FilePath = "Test_Synthesizer_MultiplyWithEcho.wav";
                audioFileOutput.Duration = 6.5;

                Stopwatch sw = Stopwatch.StartNew();
                audioFileOutputManager.Execute(audioFileOutput);
                sw.Stop();

                double ratio = sw.Elapsed.TotalSeconds / audioFileOutput.Duration;
                string message = String.Format("Ratio: {0:0.00}%, {1}ms.", ratio * 100, sw.ElapsedMilliseconds);
                Assert.Inconclusive(message);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_HardCodedTimePowerWithEcho()
        {
            const double seconds = 6.5;
            const double samplingRate = 44100.0;

            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                SampleManager sampleManager = TestHelper.CreateSampleManager(context);
                AudioFileOutputManager audioFileOutputManager = TestHelper.CreateAudioFileOutputManager(context);

                Stream sampleStream = TestHelper.GetViolin16BitMono44100WavStream();
                Sample sample = sampleManager.CreateSample(sampleStream);
                sample.SamplingRate = 8000;
                sample.BytesToSkip = 100;

                var hardCodedCalculator = new HardCodedOperatorCalculator(sample);

                Stopwatch sw = Stopwatch.StartNew();
                using (Stream destStream = new FileStream("Test_Synthesizer_HardCodedTimePowerWithEcho.wav", FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    using (BinaryWriter writer = new BinaryWriter(destStream))
                    {
                        int destSampleCount = (int)(samplingRate * seconds);

                        // Write header
                        var audioFileInfo = new AudioFileInfo
                        {
                            BytesPerValue = 2,
                            ChannelCount = 1,
                            SamplingRate = 44100,
                            SampleCount = destSampleCount
                        };
                        WavHeaderStruct wavHeaderStruct = WavHeaderManager.CreateWavHeaderStruct(audioFileInfo);
                        writer.WriteStruct(wavHeaderStruct);

                        double t = 0;
                        double dt = 1.0 / samplingRate;

                        for (int i = 0; i < destSampleCount; i++)
                        {
                            double value = hardCodedCalculator.CalculateTimePowerWithEcho(t);
                            short convertedValue = (short)value;

                            writer.Write(convertedValue);

                            t += dt;
                        }
                    }
                }
                sw.Stop();

                double ratio = sw.Elapsed.TotalSeconds / seconds;
                string message = String.Format("Ratio: {0:0.00}%, {1}ms.", ratio * 100, sw.ElapsedMilliseconds);
                Assert.Inconclusive(message);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_HardCodedMultiplyWithEcho()
        {
            const double seconds = 6.5;
            const double samplingRate = 44100.0;

            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                SampleManager sampleManager = TestHelper.CreateSampleManager(context);

                Stream sampleStream = TestHelper.GetViolin16BitMono44100WavStream();
                Sample sample = sampleManager.CreateSample(sampleStream);
                sample.SamplingRate = 8000;
                sample.BytesToSkip = 100;

                var hardCodedCalculator = new HardCodedOperatorCalculator(sample);

                Stopwatch sw = Stopwatch.StartNew();
                using (Stream destStream = new FileStream("Test_Synthesizer_HardCodedMultiplyWithEcho.wav", FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    using (BinaryWriter writer = new BinaryWriter(destStream))
                    {
                        int destSampleCount = (int)(samplingRate * seconds);

                        // Write header
                        var audioFileInfo = new AudioFileInfo
                        {
                            BytesPerValue = 2,
                            ChannelCount = 1,
                            SamplingRate = 44100,
                            SampleCount = destSampleCount
                        };
                        WavHeaderStruct wavHeaderStruct = WavHeaderManager.CreateWavHeaderStruct(audioFileInfo);
                        writer.WriteStruct(wavHeaderStruct);

                        double t = 0;
                        double dt = 1.0 / samplingRate;

                        for (int i = 0; i < destSampleCount; i++)
                        {
                            double value = hardCodedCalculator.CalculateMultiplyWithEcho(t);
                            short convertedValue = (short)value;

                            writer.Write(convertedValue);

                            t += dt;
                        }
                    }
                }
                sw.Stop();

                double ratio = sw.Elapsed.TotalSeconds / seconds;
                string message = String.Format("Ratio: {0:0.00}%, {1}ms.", ratio * 100, sw.ElapsedMilliseconds);
                Assert.Inconclusive(message);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_OptimizedOperatorCalculator()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                ICurveRepository curveRepository = PersistenceHelper.CreateRepository<ICurveRepository>(context);
                ISampleRepository sampleRepository = PersistenceHelper.CreateRepository<ISampleRepository>(context);

                OperatorFactory x = TestHelper.CreateOperatorFactory(context);
                Outlet outlet = x.Add(x.Value(1), x.Value(2));
                var calculator = new OptimizedOperatorCalculator(curveRepository, sampleRepository, outlet);
                double result = calculator.Calculate(0, 0);
                Assert.AreEqual(3.0, result, 0.0001);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_OptimizedOperatorCalculator_WithNullInlet()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                ICurveRepository curveRepository = PersistenceHelper.CreateRepository<ICurveRepository>(context);
                ISampleRepository sampleRepository = PersistenceHelper.CreateRepository<ISampleRepository>(context);

                OperatorFactory x = TestHelper.CreateOperatorFactory(context);
                Outlet outlet = x.Add(null, x.Value(2));
                var calculator = new OptimizedOperatorCalculator(curveRepository, sampleRepository, outlet);
                double result = calculator.Calculate(0, 0);
                Assert.AreEqual(2.0, result, 0.000000001);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_OptimizedOperatorCalculator_Nulls()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                ICurveRepository curveRepository = PersistenceHelper.CreateRepository<ICurveRepository>(context);
                ISampleRepository sampleRepository = PersistenceHelper.CreateRepository<ISampleRepository>(context);

                OperatorFactory x = TestHelper.CreateOperatorFactory(context);
                Outlet outlet = x.Add(x.Value(1), x.Add(x.Value(2), null));
                var calculator = new OptimizedOperatorCalculator(curveRepository, sampleRepository, outlet);
                double result = calculator.Calculate(0, 0);
                Assert.AreEqual(3.0, result, 0.000000001);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_OptimizedOperatorCalculator_NestedOperators()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                ICurveRepository curveRepository = PersistenceHelper.CreateRepository<ICurveRepository>(context);
                ISampleRepository sampleRepository = PersistenceHelper.CreateRepository<ISampleRepository>(context);

                OperatorFactory x = TestHelper.CreateOperatorFactory(context);
                Outlet outlet = x.Add(x.Add(x.Value(1), x.Value(2)), x.Value(4));
                var calculator = new OptimizedOperatorCalculator(curveRepository, sampleRepository, outlet);
                double result = calculator.Calculate(0, 0);
                Assert.AreEqual(7.0, result, 0.000000001);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_OptimizedOperatorCalculator_TwoChannels()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                ICurveRepository curveRepository = PersistenceHelper.CreateRepository<ICurveRepository>(context);
                ISampleRepository sampleRepository = PersistenceHelper.CreateRepository<ISampleRepository>(context);

                OperatorFactory x = TestHelper.CreateOperatorFactory(context);
                Outlet outlet1 = x.Add(x.Add(x.Value(1), x.Value(2)), x.Value(4));
                Outlet outlet2 = x.Add(x.Value(5), x.Value(6));
                var calculator = new OptimizedOperatorCalculator(curveRepository, sampleRepository, outlet1, outlet2);
                double result1 = calculator.Calculate(0, 0);
                double result2 = calculator.Calculate(0, 1);
                Assert.AreEqual(7.0, result1, 0.000000001);
                Assert.AreEqual(11.0, result2, 0.000000001);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_OptimizedOperatorCalculator_InstanceIntegrity()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                ICurveRepository curveRepository = PersistenceHelper.CreateRepository<ICurveRepository>(context);
                ISampleRepository sampleRepository = PersistenceHelper.CreateRepository<ISampleRepository>(context);

                OperatorFactory x = TestHelper.CreateOperatorFactory(context);
                Outlet sharedOutlet = x.Value(1);
                Outlet outlet1 = x.Add(sharedOutlet, x.Value(2));
                Outlet outlet2 = x.Add(sharedOutlet, x.Value(3));
                var calculator = new OptimizedOperatorCalculator(curveRepository, sampleRepository, outlet1, outlet2);
                double result1 = calculator.Calculate(0, 0);
                double result2 = calculator.Calculate(0, 1);
                Assert.AreEqual(3.0, result1, 0.000000001);
                Assert.AreEqual(4.0, result2, 0.000000001);
            }
        }

        [TestMethod]
        public void Test_Synthesizer_WhiteNoiseOperator()
        {
            using (IContext context = PersistenceHelper.CreateMemoryContext())
            {
                var repositoryWrapper = PersistenceHelper.CreateRepositoryWrapper(context);

                var x = new OperatorFactory(
                    repositoryWrapper.OperatorRepository,
                    repositoryWrapper.OperatorTypeRepository,
                    repositoryWrapper.InletRepository,
                    repositoryWrapper.OutletRepository,
                    repositoryWrapper.CurveRepository,
                    repositoryWrapper.SampleRepository);

                var audioFileOutputManager = new AudioFileOutputManager(
                    repositoryWrapper.AudioFileOutputRepository,
                    repositoryWrapper.AudioFileOutputChannelRepository,
                    repositoryWrapper.SampleDataTypeRepository,
                    repositoryWrapper.SpeakerSetupRepository,
                    repositoryWrapper.AudioFileFormatRepository,
                    repositoryWrapper.CurveRepository,
                    repositoryWrapper.SampleRepository);

                Outlet outlet = x.Multiply(x.WhiteNoise(), x.Value(Int16.MaxValue));

                AudioFileOutput audioFileOutput = audioFileOutputManager.CreateWithRelatedEntities();
                audioFileOutput.FilePath = "Test_Synthesizer_WhiteNoiseOperator.wav";
                audioFileOutput.Duration = 20;
                audioFileOutput.AudioFileOutputChannels[0].Outlet = outlet;

                // Execute once to fill caches.
                audioFileOutputManager.Execute(audioFileOutput);

                Stopwatch sw = Stopwatch.StartNew();
                audioFileOutputManager.Execute(audioFileOutput);
                sw.Stop();

                double ratio = sw.Elapsed.TotalSeconds / audioFileOutput.Duration;
                string message = String.Format("Ratio: {0:0.00}%, {1}ms.", ratio * 100, sw.ElapsedMilliseconds);
                Assert.Inconclusive(message);
            }
        }
    }
}
