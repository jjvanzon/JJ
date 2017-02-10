﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using JJ.Business.Synthesizer.Calculation;
using JJ.Business.Synthesizer.Calculation.Arrays;
using JJ.Business.Synthesizer.Calculation.Patches;
using JJ.Business.Synthesizer.Configuration;
using JJ.Business.Synthesizer.CopiedCode.FromFramework;
using JJ.Business.Synthesizer.Dto;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Roslyn.Helpers;
using JJ.Business.Synthesizer.Visitors;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Collections;
using JJ.Framework.Common;
using JJ.Framework.Exceptions;
using JJ.Framework.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace JJ.Business.Synthesizer.Roslyn
{
    internal class OperatorDtoCompiler
    {
        private const string GENERATED_NAME_SPACE = "GeneratedCSharp";
        private const string GENERATED_CLASS_NAME = "GeneratedPatchCalculator";
        private const string GENERATED_CLASS_FULL_NAME = GENERATED_NAME_SPACE + "." + GENERATED_CLASS_NAME;

        private static readonly bool _includeSymbols = ConfigurationHelper.GetSection<ConfigurationSection>().IncludeSymbolsWithCompilation;
        private static readonly CSharpCompilationOptions _csharpCompilationOptions = GetCSharpCompilationOptions();

        private static readonly SyntaxTree[] _includedSyntaxTrees = CreateIncludedSyntaxTrees(
            $"Calculation\\{nameof(SineCalculator)}.cs",
            $"Calculation\\{nameof(BiQuadFilterWithoutFields)}.cs",
            $"Calculation\\Arrays\\{nameof(ArrayCalculatorBase)}.cs",
            $"Calculation\\Arrays\\{nameof(ArrayCalculatorBase_Block)}.cs",
            $"Calculation\\Arrays\\{nameof(ArrayCalculatorBase_Line)}.cs",
            $"Calculation\\Arrays\\{nameof(ArrayCalculator_MinPosition_Line)}.cs",
            $"Calculation\\Arrays\\{nameof(ArrayCalculator_MinPositionZero_Line)}.cs",
            $"Calculation\\Arrays\\{nameof(ArrayCalculator_RotatePosition_Block)}.cs",
            $"Helpers\\{nameof(ConversionHelper)}.cs",
            $"Calculation\\Patches\\{nameof(PatchCalculatorHelper)}.cs",
            $"CopiedCode\\FromFramework\\{nameof(MathHelper)}.cs",
            $"CopiedCode\\FromFramework\\{nameof(Geometry)}.cs");

        private static readonly IList<MetadataReference> _metaDataReferences = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IPatchCalculator).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(LessThanException).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Expression).Assembly.Location)
        };

        [Obsolete("Consider using CompileToPatchCalculatorActivationInfo instead, to compile once and instantiate multiple times.")]
        public IPatchCalculator CompileToPatchCalculator(
            OperatorDtoBase dto, 
            int samplingRate, 
            int channelCount, 
            int channelIndex, 
            CalculatorCache calculatorCache, 
            ICurveRepository curveRepository,
            IOperatorRepository operatorRepository,
            ISampleRepository sampleRepository)
        {
            ActivationInfo activationInfo = CompileToPatchCalculatorActivationInfo(
                dto,
                samplingRate,
                channelCount,
                channelIndex,
                calculatorCache,
                curveRepository,
                operatorRepository,
                sampleRepository);

            var calculator = (IPatchCalculator)Activator.CreateInstance(activationInfo.Type, activationInfo.Args);

            return calculator;
        }

        public ActivationInfo CompileToPatchCalculatorActivationInfo(
            OperatorDtoBase dto,
            int samplingRate,
            int channelCount,
            int channelIndex,
            CalculatorCache calculatorCache,
            ICurveRepository curveRepository,
            IOperatorRepository operatorRepository,
            ISampleRepository sampleRepository)
        {
            if (dto == null) throw new NullException(() => dto);

            var preProcessingVisitor = new OperatorDtoPreProcessingExecutor(samplingRate, channelCount);
            dto = preProcessingVisitor.Execute(dto);

            var codeGenerationSimplificationVisitor = new OperatorDtoVisitor_CodeGenerationSimplification();
            dto = codeGenerationSimplificationVisitor.Execute(dto);

            var codeGenerator = new OperatorDtoToPatchCalculatorCSharpGenerator(channelCount, channelIndex, calculatorCache, curveRepository, sampleRepository);
            OperatorDtoToPatchCalculatorCSharpGeneratorResult codeGeneratorResult = codeGenerator.Execute(dto, GENERATED_NAME_SPACE, GENERATED_CLASS_NAME);

            Type type = Compile(codeGeneratorResult.GeneratedCode);

            Dictionary<string, double[]> arrays = codeGeneratorResult.CalculatorVariableInfos.ToDictionary(x => x.NameCamelCase, x => x.Calculator.UnderlyingArray);
            Dictionary<string, double> arrayRates = codeGeneratorResult.CalculatorVariableInfos.ToDictionary(x => x.NameCamelCase, x => x.Calculator.Rate);
            Dictionary<string, double> arrayValuesBefore = codeGeneratorResult.CalculatorVariableInfos.ToDictionary(x => x.NameCamelCase, x => x.Calculator.ValueBefore);
            Dictionary<string, double> arrayValuesAfter = codeGeneratorResult.CalculatorVariableInfos.ToDictionary(x => x.NameCamelCase, x => x.Calculator.ValueAfter);

            var args = new object[] { samplingRate, channelCount, channelIndex, arrays, arrayRates, arrayValuesBefore, arrayValuesAfter };

            return new ActivationInfo(type, args);
        }

        private Type Compile(string generatedCode)
        {
            string generatedCodeFileName = "";
            if (_includeSymbols)
            {
                generatedCodeFileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".cs";
                File.WriteAllText(generatedCodeFileName, generatedCode, Encoding.UTF8);
            }

            SyntaxTree generatedSyntaxTree = CSharpSyntaxTree.ParseText(generatedCode, path: generatedCodeFileName, encoding: Encoding.UTF8);
            IList<SyntaxTree> syntaxTrees = generatedSyntaxTree.Union(_includedSyntaxTrees).ToArray();

            string assemblyName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees,
                _metaDataReferences,
                _csharpCompilationOptions);

            var assemblyStream = new MemoryStream();
            MemoryStream pdbStream = null;
            if (_includeSymbols)
            {
                pdbStream = new MemoryStream();
            }

            EmitResult emitResult = compilation.Emit(assemblyStream, pdbStream);
            if (!emitResult.Success)
            {
                IEnumerable<Diagnostic> failureDiagnostics = emitResult.Diagnostics.Where(x =>
                    x.IsWarningAsError ||
                    x.Severity == DiagnosticSeverity.Error);

                string concatinatedFailureDiagnostics = string.Join(Environment.NewLine, failureDiagnostics);
                throw new Exception("CSharpCompilation.Emit failed. " + concatinatedFailureDiagnostics);
            }

            assemblyStream.Position = 0;
            byte[] assemblyBytes = StreamHelper.StreamToBytes(assemblyStream);

            byte[] pdbBytes = null;
            if (_includeSymbols)
            {
                // ReSharper disable once PossibleNullReferenceException
                pdbStream.Position = 0;
                pdbBytes = StreamHelper.StreamToBytes(pdbStream);
            }

            Assembly assembly = Assembly.Load(assemblyBytes, pdbBytes);

            Type type = assembly.GetType(GENERATED_CLASS_FULL_NAME);

            return type;
        }

        private static CSharpCompilationOptions GetCSharpCompilationOptions()
        {
#if DEBUG
            const OptimizationLevel optimizationLevel = OptimizationLevel.Debug;
#else
            const OptimizationLevel optimizationLevel = OptimizationLevel.Release;
#endif
            // ReSharper disable once RedundantArgumentDefaultValue
            return new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: optimizationLevel);
        }

        private static SyntaxTree[] CreateIncludedSyntaxTrees(params string[] codeFilesNames)
        {
            return codeFilesNames.Select(x => CreateSyntaxTree(x)).ToArray();
        }

        private static SyntaxTree CreateSyntaxTree(string codeFileName)
        {
            string codeFileCSharp = File.ReadAllText(codeFileName);

            return CSharpSyntaxTree.ParseText(codeFileCSharp, path: codeFileName, encoding: Encoding.UTF8);
        }
    }
}