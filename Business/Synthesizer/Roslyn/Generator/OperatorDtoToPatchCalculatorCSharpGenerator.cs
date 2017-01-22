﻿using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Dto;
using JJ.Framework.Common;
using JJ.Business.Synthesizer.Calculation;
using JJ.Business.Synthesizer.Roslyn.Helpers;
using JJ.Business.Synthesizer.Roslyn.Visitors;
using JJ.Business.Synthesizer.Calculation.Patches;
using JJ.Business.Synthesizer.Enums;
using JJ.Framework.Collections;
using JJ.Framework.Exceptions;
using JJ.Business.Synthesizer.Helpers;
using System;

namespace JJ.Business.Synthesizer.Roslyn.Generator
{
    internal class OperatorDtoToPatchCalculatorCSharpGenerator
    {
        private const int RAW_CALCULATION_INDENT_LEVEL = 4;
        private const string TAB_STRING = "    ";

        public string Execute(OperatorDtoBase dto, string generatedNameSpace, string generatedClassName)
        {
            if (string.IsNullOrEmpty(generatedNameSpace)) throw new NullOrEmptyException(() => generatedNameSpace);
            if (string.IsNullOrEmpty(generatedClassName)) throw new NullOrEmptyException(() => generatedClassName);

            // Build up Method Body
            var visitor = new OperatorDtoToCSharpVisitor();
            OperatorDtoToCSharpVisitorResult visitorResult = visitor.Execute(dto, RAW_CALCULATION_INDENT_LEVEL);

            // Build up Code File
            var sb = new StringBuilderWithIndentation(TAB_STRING);

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Runtime.CompilerServices;");
            sb.AppendLine("using " + typeof(PatchCalculatorBase).Namespace + ";");
            sb.AppendLine("using " + typeof(SineCalculator).Namespace + ";");
            sb.AppendLine("using " + typeof(DimensionEnum).Namespace + ";");
            sb.AppendLine("using " + typeof(NameHelper).Namespace + ";");
            sb.AppendLine();
            sb.AppendLine($"namespace {generatedNameSpace}");
            sb.AppendLine("{");
            sb.Indent();
            {
                sb.AppendLine($"public class {generatedClassName} : PatchCalculatorBase");
                sb.AppendLine("{");
                sb.Indent();
                {
                    // Fields
                    sb.AppendLine("// Fields");
                    sb.AppendLine();
                    WriteFields(sb, visitorResult);
                    sb.AppendLine();

                    // Constructor
                    sb.AppendLine("// Constructor");
                    sb.AppendLine();
                    WriteConstructor(sb, visitorResult, generatedClassName);
                    sb.AppendLine();

                    // Calculate Method
                    sb.AppendLine("// Calculate");
                    sb.AppendLine();
                    WriteCalculateMethod(sb, visitorResult);
                    sb.AppendLine();

                    // Values
                    sb.AppendLine("// Values");
                    sb.AppendLine();
                    WriteSetValueByListIndex(sb, visitorResult.InputVariableInfos);
                    sb.AppendLine();
                    WriteSetValueByDimensionEnum(sb, visitorResult.InputVariableInfos, visitorResult.LongLivedDimensionVariableInfos);
                    sb.AppendLine();
                    WriteSetValueByName(sb, visitorResult.InputVariableInfos, visitorResult.LongLivedDimensionVariableInfos);
                    sb.AppendLine();
                    WriteSetValueByDimensionEnumAndListIndex(sb, visitorResult.InputVariableInfos, visitorResult.LongLivedDimensionVariableInfos);
                    sb.AppendLine();
                    WriteSetValueByNameAndListIndex(sb, visitorResult.InputVariableInfos, visitorResult.LongLivedDimensionVariableInfos);
                    sb.AppendLine();

                    // Reset
                    sb.AppendLine("// Reset");
                    sb.AppendLine();
                    WriteResetMethod(sb, visitorResult);
                    sb.Unindent();
                }
                sb.AppendLine("}");
                sb.Unindent();
            }
            sb.AppendLine("}");

            string generatedCode = sb.ToString();
            return generatedCode;
        }

        private void WriteFields(StringBuilderWithIndentation sb, OperatorDtoToCSharpVisitorResult visitorResult)
        {
            IList<string> instanceVariableNamesCamelCase = GetInstanceVariableNamesCamelCase(visitorResult);

            foreach (string variableName in instanceVariableNamesCamelCase)
            {
                sb.AppendLine($"private double _{variableName};");
            }
        }

        private void WriteConstructor(StringBuilderWithIndentation sb, OperatorDtoToCSharpVisitorResult visitorResult, string generatedClassName)
        {
            sb.AppendLine($"public {generatedClassName}(int samplingRate, int channelCount, int channelIndex)");
            sb.Indent();
            {
                sb.AppendLine($": base(samplingRate, channelCount, channelIndex)");
                sb.Unindent();
            }
            sb.AppendLine("{");
            sb.Indent();
            {
                if (visitorResult.InputVariableInfos.Any())
                {
                    foreach (ExtendedVariableInfo inputVariableInfo in visitorResult.InputVariableInfos)
                    {
                        sb.AppendLine($"_{inputVariableInfo.VariableNameCamelCase} = {CompilationHelper.FormatValue(inputVariableInfo.Value ?? 0.0)};");
                    }
                    sb.AppendLine("");
                }

                sb.AppendLine("Reset(time: 0.0);");
                sb.Unindent();
            }
            sb.AppendLine("}");
        }

        private void WriteCalculateMethod(StringBuilderWithIndentation sb, OperatorDtoToCSharpVisitorResult visitorResult)
        {
            IList<string> instanceVariableNamesCamelCase = GetInstanceVariableNamesCamelCase(visitorResult);

            sb.AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
            sb.AppendLine("public override void Calculate(float[] buffer, int frameCount, double startTime)");
            sb.AppendLine("{");
            sb.Indent();
            {
                // Copy Fields to Local
                sb.AppendLine("double frameDuration = _frameDuration;");
                sb.AppendLine("int channelCount = _channelCount;");
                sb.AppendLine("int channelIndex = _channelIndex;");

                sb.AppendLine();
                foreach (string variableName in instanceVariableNamesCamelCase)
                {
                    sb.AppendLine($"double {variableName} = _{variableName};");
                }
                sb.AppendLine();

                // Declare Locally Reused Variables
                foreach (string positionVariableName in visitorResult.LocalDimensionVariableNamesCamelCase)
                {
                    sb.AppendLine($"double {positionVariableName};"); 
                }
                sb.AppendLine();

                // Initialize ValueCount variable
                sb.AppendLine("int valueCount = frameCount * channelCount;");

                // Initialize First Time Variable
                sb.AppendLine($"{visitorResult.FirstTimeVariableNameCamelCase} = startTime;");
                sb.AppendLine();

                // Loop
                sb.AppendLine("for (int i = channelIndex; i < valueCount; i += channelCount)"); // Writes values in an interleaved way to the buffer."
                sb.AppendLine("{");
                sb.Indent();
                {
                    // Raw Calculation
                    sb.Append(visitorResult.RawCalculationCode);

                    // Accumulate
                    sb.AppendLine("// Accumulate");
                    sb.AppendLine($"double value = {visitorResult.ReturnValueLiteral};");
                    sb.AppendLine();
                    sb.AppendLine("if (double.IsNaN(value))"); // winmm will trip over NaN.
                    sb.AppendLine("{");
                    sb.Indent();
                    {
                        sb.AppendLine("value = 0;");
                        sb.Unindent();
                    }
                    sb.AppendLine("}");
                    sb.AppendLine();
                    sb.AppendLine("float floatValue = (float)value;"); // TODO: This seems unsafe. What happens if the cast to float is invalid?
                    sb.AppendLine();
                    sb.AppendLine("PatchCalculatorHelper.InterlockedAdd(ref buffer[i], floatValue);");

                    sb.AppendLine();
                    sb.AppendLine($"{visitorResult.FirstTimeVariableNameCamelCase} += frameDuration;");

                    sb.Unindent();
                }
                sb.AppendLine("}");
                sb.AppendLine();

                // Copy Local to Fields
                foreach (string variableName in instanceVariableNamesCamelCase)
                {
                    sb.AppendLine($"_{variableName} = {variableName};");
                }

                sb.Unindent();
            }
            sb.AppendLine("}");
        }

        private void WriteSetValueByListIndex(StringBuilderWithIndentation sb, IList<ExtendedVariableInfo> inputVariableInfos)
        {
            sb.AppendLine("public override void SetValue(int listIndex, double value)");
            sb.AppendLine("{");
            sb.Indent();
            {
                sb.AppendLine("base.SetValue(listIndex, value);");
                sb.AppendLine();

                if (inputVariableInfos.Any())
                {
                    sb.AppendLine("switch (listIndex)");
                    sb.AppendLine("{");
                    sb.Indent();
                    {
                        int i = 0;
                        foreach (string inputVariableName in inputVariableInfos.Select(x => x.VariableNameCamelCase))
                        {
                            sb.AppendLine($"case {i}:");
                            sb.Indent();
                            {
                                sb.AppendLine($"_{inputVariableName} = value;");
                                sb.AppendLine("break;");
                                sb.AppendLine();
                                sb.Unindent();
                            }
                            i++;
                        }

                        sb.Unindent();
                    }
                    sb.AppendLine("}");
                }
                sb.Unindent();
            }
            sb.AppendLine("}");
        }

        private void WriteSetValueByDimensionEnum(
            StringBuilderWithIndentation sb, 
            IList<ExtendedVariableInfo> inputVariableInfos,
            IList<ExtendedVariableInfo> firstLevelDimensionVariableInfos)
        {
            IList<ExtendedVariableInfo> variableInfos = firstLevelDimensionVariableInfos.Union(inputVariableInfos).ToArray();

            sb.AppendLine("public override void SetValue(DimensionEnum dimensionEnum, double value)");
            sb.AppendLine("{");
            sb.Indent();
            {
                sb.AppendLine("base.SetValue(dimensionEnum, value);");
                sb.AppendLine();

                WriteFieldAssignmentsByDimensionEnum(sb, variableInfos);

                sb.Unindent();
            }
            sb.AppendLine("}");
        }

        private void WriteSetValueByName(
            StringBuilderWithIndentation sb, 
            IList<ExtendedVariableInfo> inputVariableInfos,
            IList<ExtendedVariableInfo> firstLevelDimensionVariableInfos)
        {
            IList<ExtendedVariableInfo> variableInfos = inputVariableInfos.Union(firstLevelDimensionVariableInfos).ToArray();

            sb.AppendLine("public override void SetValue(string name, double value)");
            sb.AppendLine("{");
            sb.Indent();
            {
                sb.AppendLine("base.SetValue(name, value);");
                sb.AppendLine();

                sb.AppendLine("string canonicalName = NameHelper.ToCanonical(name);");
                sb.AppendLine();

                WriteFieldAssignmentsByCanonicalName(sb, variableInfos);

                sb.Unindent();
            }
            sb.AppendLine("}");
        }

        private void WriteSetValueByDimensionEnumAndListIndex(
            StringBuilderWithIndentation sb, 
            IList<ExtendedVariableInfo> inputVariableInfos,
            IList<ExtendedVariableInfo> firstLevelDimensionVariableInfos)
        {
            sb.AppendLine("public override void SetValue(DimensionEnum dimensionEnum, int listIndex, double value)");
            sb.AppendLine("{");
            sb.Indent();
            {
                sb.AppendLine("base.SetValue(dimensionEnum, listIndex, value);");
                sb.AppendLine();

                // Dimension Variables
                WriteFieldAssignmentsByDimensionEnum(sb, firstLevelDimensionVariableInfos);
                sb.AppendLine();

                // Input Variables
                var groups = inputVariableInfos.GroupBy(x => x.DimensionEnum);
                foreach (var group in groups)
                {
                    int i = 0;
                    foreach (ExtendedVariableInfo inputVariableInfo in group)
                    {
                        sb.AppendLine($"if (dimensionEnum == {nameof(DimensionEnum)}.{group.Key} && listIndex == {i})");
                        sb.AppendLine("{");
                        sb.Indent();
                        {
                            sb.AppendLine($"_{inputVariableInfo.VariableNameCamelCase} = value;");
                            sb.Unindent();
                        }
                        sb.AppendLine("}");
                        sb.AppendLine();

                        i++;
                    }
                }

                sb.Unindent();
            }
            sb.AppendLine("}");
        }

        private void WriteSetValueByNameAndListIndex(
            StringBuilderWithIndentation sb, 
            IList<ExtendedVariableInfo> inputVariableInfos,
            IList<ExtendedVariableInfo> firstLevelDimensionVariableInfos)
        {
            sb.AppendLine("public override void SetValue(string name, int listIndex, double value)");
            sb.AppendLine("{");
            sb.Indent();
            {
                sb.AppendLine("base.SetValue(name, listIndex, value);");
                sb.AppendLine();
                sb.AppendLine("string canonicalName = NameHelper.ToCanonical(name);");
                sb.AppendLine();

                // Dimension Variables
                WriteFieldAssignmentsByCanonicalName(sb, firstLevelDimensionVariableInfos);

                // Input Variables
                var groups = inputVariableInfos.GroupBy(x => x.CanonicalName);
                foreach (var group in groups)
                {
                    int i = 0;
                    foreach (ExtendedVariableInfo inputVariableInfo in group)
                    {
                        sb.AppendLine($"if (String.Equals(canonicalName, \"{group.Key}\") && listIndex == {i})");
                        sb.AppendLine("{");
                        sb.Indent();
                        {
                            sb.AppendLine($"_{inputVariableInfo.VariableNameCamelCase} = value;");
                            sb.Unindent();
                        }
                        sb.AppendLine("}");
                        sb.AppendLine();

                        i++;
                    }
                }

                sb.Unindent();
            }
            sb.AppendLine("}");
        }

        /// <summary> Assumes that the variable dimensionEnum is already declared. </summary>
        private static void WriteFieldAssignmentsByDimensionEnum(
            StringBuilderWithIndentation sb,
            IList<ExtendedVariableInfo> variableInfos)
        {
            var groups = variableInfos.GroupBy(x => x.DimensionEnum);
            if (groups.Any())
            {
                sb.AppendLine("switch (dimensionEnum)");
                sb.AppendLine("{");
                sb.Indent();
                {
                    foreach (var group in groups)
                    {
                        sb.AppendLine($"case {nameof(DimensionEnum)}.{group.Key}:");
                        sb.Indent();
                        {
                            foreach (ExtendedVariableInfo variableInfo in group)
                            {
                                sb.AppendLine($"_{variableInfo.VariableNameCamelCase} = value;");
                            }
                            sb.AppendLine("break;");
                            sb.AppendLine();
                            sb.Unindent();
                        }
                    }

                    sb.Unindent();
                }
                sb.AppendLine("}");
            }
        }

        /// <summary> Assumes that the variable canonicalName is already declared. </summary>
        private static void WriteFieldAssignmentsByCanonicalName(
            StringBuilderWithIndentation sb, 
            IList<ExtendedVariableInfo> variableInfos)
        {
            var groups = variableInfos.GroupBy(x => x.CanonicalName);
            if (groups.Any())
            {
                foreach (var group in groups)
                {
                    sb.AppendLine($"if (String.Equals(canonicalName, \"{group.Key}\", StringComparison.Ordinal))");
                    sb.AppendLine("{");
                    sb.Indent();
                    {
                        foreach (ExtendedVariableInfo variableInfo in group)
                        {
                            sb.AppendLine($"_{variableInfo.VariableNameCamelCase} = value;");
                        }
                        sb.Unindent();
                    }
                    sb.AppendLine("}");
                    sb.AppendLine();
                }
            }
        }

        private void WriteResetMethod(StringBuilderWithIndentation sb, OperatorDtoToCSharpVisitorResult visitorResult)
        {
            sb.AppendLine("public override void Reset(double time)");
            sb.AppendLine("{");
            sb.Indent();
            {
                foreach (string variableName in visitorResult.LongLivedPhaseVariableNamesCamelCase)
                {
                    sb.AppendLine($"_{variableName} = 0.0;");
                }

                foreach (string variableName in visitorResult.LongLivedPreviousPositionVariableNamesCamelCase)
                {
                    // DIRTY: Phase of a partial does not have to be related to the time-dimension!
                    sb.AppendLine($"_{variableName} = time;");
                }

                sb.Unindent();
            }
            sb.AppendLine("}");
        }

        private static IList<string> GetInstanceVariableNamesCamelCase(OperatorDtoToCSharpVisitorResult visitorResult)
        {
            return visitorResult.LongLivedPhaseVariableNamesCamelCase.Union(visitorResult.LongLivedPreviousPositionVariableNamesCamelCase)
                                                                     .Union(visitorResult.LongLivedOriginVariableNamesCamelCase)
                                                                     .Union(visitorResult.LongLivedDimensionVariableInfos.Select(x => x.VariableNameCamelCase))
                                                                     .Union(visitorResult.InputVariableInfos.Select(x => x.VariableNameCamelCase))
                                                                     .ToArray();
        }
    }
}
