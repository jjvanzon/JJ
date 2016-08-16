﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace JJ.Analysis.Names
{
    internal static class DiagnosticsIDs
    {
        public const string ConstantNameUpperCaseOrSameAsValue = "ConstantNameUpperCaseOrSameAsValue";
        public const string FieldNameUnderscoredCamelCase = "FieldNameUnderscoredCamelCase";
        public const string FieldNameAbbreviationCasing = "FieldNameAbbreviationCasing";
        public const string LocalVariableNameAbbreviationCasing = "LocalVariableNameAbbreviationCasing";
        public const string LocalVariableNameStartWithLowerCase = "LocalVariableNameStartWithLowerCase";
        public const string MethodNameAbbreviationCasing = "MethodNameAbbreviationCasing";
        public const string MethodNameStartWithUpperCase = "MethodNameStartWithUpperCase";
        public const string ParameterNameAbbreviationCasing = "ParameterNameAbbreviationCasing";
        public const string ParameterNameStartWithLowerCase = "ParameterNameStartWithLowerCase";
        public const string PropertyNameStartWithUpperCase = "PropertyNameStartWithUpperCase";
        public const string PropertyNameAbbreviationCasing = "PropertyNameAbbreviationCasing";
        public const string PublicMethodParameterRequiresNullCheck = "PublicMethodParameterRequiresNullCheck";
        public const string TypeNameAbreviationCasing = "TypeNameAbreviationCasing";
        public const string TypeNameStartWithUpperCase = "TypeNameStartWithUpperCase";
    }
}