﻿using System;
using JJ.Business.Synthesizer.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable
// ReSharper disable AccessToModifiedClosure
// ReSharper disable SuggestVarOrType_SimpleTypes

namespace JJ.Business.Synthesizer.Tests
{
    [TestClass]
    public class Synthesizer_Interpolate_Line_Tests : Synthesizer_Interpolate_Tests_Base
    {
        // LookAhead

        [TestMethod]
        public void Test_Synthesizer_Interpolate_Line_LookAhead_Forward_WithCalculatorClasses()
            => Test_Synthesizer_Interpolate_Line_LookAhead_Forward(CalculationEngineEnum.CalculatorClasses);

        private void Test_Synthesizer_Interpolate_Line_LookAhead_Forward(CalculationEngineEnum calculationEngineEnum)
        {
            double y0 = Math.Sin(Math.PI * -12 / 12);
            double y1 = Math.Sin(Math.PI * -9 / 12);
            double y2 = Math.Sin(Math.PI * -6 / 12);
            double y3 = Math.Sin(Math.PI * -3 / 12);
            double y4 = Math.Sin(Math.PI * -0 / 12);
            double y5 = Math.Sin(Math.PI * 3 / 12);
            double y6 = Math.Sin(Math.PI * 6 / 12);
            double y7 = Math.Sin(Math.PI * 9 / 12);
            double y8 = Math.Sin(Math.PI * 12 / 12);

            Test_Synthesizer_Interpolate_Base(
                calculationEngineEnum,
                InterpolationTypeEnum.Line,
                FollowingModeEnum.LookAhead,
                slowRate: 4.0 / Math.PI,
                new[]
                {
                    (Math.PI * -12 / 12, y0),
                    (Math.PI * -11 / 12, (2 * y0 + y1) / 3),
                    (Math.PI * -10 / 12, (y0 + 2 * y1) / 3),
                    (Math.PI * -09 / 12, y1),
                    (Math.PI * -08 / 12, (2 * y1 + y2) / 3),
                    (Math.PI * -07 / 12, (y1 + 2 * y2) / 3),
                    (Math.PI * -06 / 12, y2),
                    (Math.PI * -05 / 12, (2 * y2 + y3) / 3),
                    (Math.PI * -04 / 12, (y2 + 2 * y3) / 3),
                    (Math.PI * -03 / 12, y3),
                    (Math.PI * -02 / 12, (2 * y3 + y4) / 3),
                    (Math.PI * -01 / 12, (y3 + 2 * y4) / 3),
                    (Math.PI * 00 / 12, y4),
                    (Math.PI * 01 / 12, (2 * y4 + y5) / 3),
                    (Math.PI * 02 / 12, (y4 + 2 * y5) / 3),
                    (Math.PI * 03 / 12, y5),
                    (Math.PI * 04 / 12, (2 * y5 + y6) / 3),
                    (Math.PI * 05 / 12, (y5 + 2 * y6) / 3),
                    (Math.PI * 06 / 12, y6),
                    (Math.PI * 07 / 12, (2 * y6 + y7) / 3),
                    (Math.PI * 08 / 12, (y6 + 2 * y7) / 3),
                    (Math.PI * 09 / 12, y7),
                    (Math.PI * 10 / 12, (2 * y7 + y8) / 3),
                    (Math.PI * 11 / 12, (y7 + 2 * y8) / 3),
                    (Math.PI * 12 / 12, y8)
                });
        }

        [TestMethod]
        public void Test_Synthesizer_Interpolate_Line_LookAhead_Backward_WithCalculatorClasses()
            => Test_Synthesizer_Interpolate_Line_LookAhead_Backward(CalculationEngineEnum.CalculatorClasses);

        private void Test_Synthesizer_Interpolate_Line_LookAhead_Backward(CalculationEngineEnum calculationEngineEnum)
        {
            double y0 = Math.Sin(Math.PI * 12 / 12);
            double y1 = Math.Sin(Math.PI * 9 / 12);
            double y2 = Math.Sin(Math.PI * 6 / 12);
            double y3 = Math.Sin(Math.PI * 3 / 12);
            double y4 = Math.Sin(Math.PI * 0 / 12);
            double y5 = Math.Sin(Math.PI * -3 / 12);
            double y6 = Math.Sin(Math.PI * -6 / 12);
            double y7 = Math.Sin(Math.PI * -9 / 12);
            double y8 = Math.Sin(Math.PI * -12 / 12);

            Test_Synthesizer_Interpolate_Base(
                calculationEngineEnum,
                InterpolationTypeEnum.Line,
                FollowingModeEnum.LookAhead,
                slowRate: 4.0 / Math.PI,
                new[]
                {
                    (Math.PI * 12 / 12, y0),
                    (Math.PI * 11 / 12, (2 * y0 + y1) / 3),
                    (Math.PI * 10 / 12, (y0 + 2 * y1) / 3),
                    (Math.PI * 09 / 12, y1),
                    (Math.PI * 08 / 12, (2 * y1 + y2) / 3),
                    (Math.PI * 07 / 12, (y1 + 2 * y2) / 3),
                    (Math.PI * 06 / 12, y2),
                    (Math.PI * 05 / 12, (2 * y2 + y3) / 3),
                    (Math.PI * 04 / 12, (y2 + 2 * y3) / 3),
                    (Math.PI * 03 / 12, y3),
                    (Math.PI * 02 / 12, (2 * y3 + y4) / 3),
                    (Math.PI * 01 / 12, (y3 + 2 * y4) / 3),
                    (Math.PI * 00 / 12, y4),
                    (Math.PI * -01 / 12, (2 * y4 + y5) / 3),
                    (Math.PI * -02 / 12, (y4 + 2 * y5) / 3),
                    (Math.PI * -03 / 12, y5),
                    (Math.PI * -04 / 12, (2 * y5 + y6) / 3),
                    (Math.PI * -05 / 12, (y5 + 2 * y6) / 3),
                    (Math.PI * -06 / 12, y6),
                    (Math.PI * -07 / 12, (2 * y6 + y7) / 3),
                    (Math.PI * -08 / 12, (y6 + 2 * y7) / 3),
                    (Math.PI * -09 / 12, y7),
                    (Math.PI * -10 / 12, (2 * y7 + y8) / 3),
                    (Math.PI * -11 / 12, (y7 + 2 * y8) / 3),
                    (Math.PI * -12 / 12, y8)
                });
        }

        // LagBehind

        [TestMethod]
        public void Test_Synthesizer_Interpolate_Line_LagBehind_Forward_WithCalculatorClasses()
            => Test_Synthesizer_Interpolate_Line_LagBehind_Forward(CalculationEngineEnum.CalculatorClasses);

        private void Test_Synthesizer_Interpolate_Line_LagBehind_Forward(
            CalculationEngineEnum calculationEngineEnum)
        {
            double y0 = Math.Sin(Math.PI * -12 / 12);
            double y1 = Math.Sin(Math.PI * -11 / 12);
            double y2 = Math.Sin(Math.PI * -8 / 12);
            double y3 = Math.Sin(Math.PI * -5 / 12);
            double y4 = Math.Sin(Math.PI * -2 / 12);
            double y5 = Math.Sin(Math.PI * 1 / 12);
            double y6 = Math.Sin(Math.PI * 4 / 12);
            double y7 = Math.Sin(Math.PI * 7 / 12);
            double y8 = Math.Sin(Math.PI * 10 / 12);

            Test_Synthesizer_Interpolate_Base(
                calculationEngineEnum,
                InterpolationTypeEnum.Line,
                FollowingModeEnum.LagBehind,
                slowRate: 4.0 / Math.PI,
                new[]
                {
                    (Math.PI * -12 / 12, y0),
                    (Math.PI * -11 / 12, (2 * y0 + y1) / 3),
                    (Math.PI * -10 / 12, (y0 + 2 * y1) / 3),
                    (Math.PI * -09 / 12, y1),
                    (Math.PI * -08 / 12, (2 * y1 + y2) / 3),
                    (Math.PI * -07 / 12, (y1 + 2 * y2) / 3),
                    (Math.PI * -06 / 12, y2),
                    (Math.PI * -05 / 12, (2 * y2 + y3) / 3),
                    (Math.PI * -04 / 12, (y2 + 2 * y3) / 3),
                    (Math.PI * -03 / 12, y3),
                    (Math.PI * -02 / 12, (2 * y3 + y4) / 3),
                    (Math.PI * -01 / 12, (y3 + 2 * y4) / 3),
                    (Math.PI * 00 / 12, y4),
                    (Math.PI * 01 / 12, (2 * y4 + y5) / 3),
                    (Math.PI * 02 / 12, (y4 + 2 * y5) / 3),
                    (Math.PI * 03 / 12, y5),
                    (Math.PI * 04 / 12, (2 * y5 + y6) / 3),
                    (Math.PI * 05 / 12, (y5 + 2 * y6) / 3),
                    (Math.PI * 06 / 12, y6),
                    (Math.PI * 07 / 12, (2 * y6 + y7) / 3),
                    (Math.PI * 08 / 12, (y6 + 2 * y7) / 3),
                    (Math.PI * 09 / 12, y7),
                    (Math.PI * 10 / 12, (2 * y7 + y8) / 3),
                    (Math.PI * 11 / 12, (y7 + 2 * y8) / 3),
                    (Math.PI * 12 / 12, y8)
                }, plotLineCount: 25);
        }
    }
}