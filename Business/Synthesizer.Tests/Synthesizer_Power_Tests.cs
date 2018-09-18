﻿using System;
using JJ.Business.Synthesizer.Configuration;
using JJ.Business.Synthesizer.Enums;
using JJ.Business.Synthesizer.Helpers;
using JJ.Business.Synthesizer.Tests.Helpers;
using JJ.Framework.Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JJ.Business.Synthesizer.Tests
{
    [TestClass]
    public class Synthesizer_Power_Tests
    {
        private readonly double[] _bases = MathHelper.SpreadDoubles(-10, 10, 21);
        private readonly double[] _exponents = { -1, 0, 2, Math.E, 12 };

        [TestMethod]
        public void Test_Synthesizer_Power_WithRoslyn() => Test_Synthesizer_Power(CalculationMethodEnum.Roslyn);

        [TestMethod]
        public void Test_Synthesizer_Power_WithCalculatorClasses() => Test_Synthesizer_Power(CalculationMethodEnum.CalculatorClasses);

        private void Test_Synthesizer_Power(CalculationMethodEnum calculationMethodEnum)
            => TestExecutor.ExecuteTest(
                x => x.New(nameof(SystemPatchNames.Power), x.PatchInlet(DimensionEnum.Base), x.PatchInlet(DimensionEnum.Exponent)),
                Math.Pow,
                DimensionEnum.Base,
                _bases,
                DimensionEnum.Exponent,
                _exponents,
                calculationMethodEnum);
    }
}