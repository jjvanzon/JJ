﻿using System;
using JJ.Business.Synthesizer.Enums;
using JJ.Framework.Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable
// ReSharper disable AccessToModifiedClosure
// ReSharper disable SuggestVarOrType_SimpleTypes

namespace JJ.Business.Synthesizer.Tests
{
    [TestClass]
    public class Synthesizer_Interpolate_Hermite_Tests : Synthesizer_Interpolate_Tests_Base
    {
        // LookAhead Forward

        [TestMethod]
        public void Test_Synthesizer_Interpolate_Hermite_LookAhead_Forward_WithCalculatorClasses()
            => Test_Synthesizer_Interpolate_Hermite_LookAhead_Forward(CalculationEngineEnum.CalculatorClasses);

        private void Test_Synthesizer_Interpolate_Hermite_LookAhead_Forward(CalculationEngineEnum calculationEngineEnum)
        {
            const double xMinus1 = Math.PI * -12 / 12;
            const double x0 = Math.PI * -09 / 12;
            const double x1 = Math.PI * -06 / 12;
            const double x2 = Math.PI * -03 / 12;
            const double x3 = Math.PI * 00 / 12;
            const double x4 = Math.PI * 03 / 12;
            const double x5 = Math.PI * 06 / 12;
            const double x6 = Math.PI * 09 / 12;
            const double x7 = Math.PI * 12 / 12;
            const double x8 = Math.PI * 15 / 12;
            const double x9 = Math.PI * 18 / 12;

            double yMinus1 = Math.Sin(xMinus1);
            double y0 = Math.Sin(x0);
            double y1 = Math.Sin(x1);
            double y2 = Math.Sin(x2);
            double y3 = Math.Sin(x3);
            double y4 = Math.Sin(x4);
            double y5 = Math.Sin(x5);
            double y6 = Math.Sin(x6);
            double y7 = Math.Sin(x7);
            double y8 = Math.Sin(x8);
            double y9 = Math.Sin(x9);

            Test_Synthesizer_Interpolate_Base(
                calculationEngineEnum,
                InterpolationTypeEnum.Hermite,
                FollowingModeEnum.LookAhead,
                slowRate: 4.0 / Math.PI,
                new[]
                {
                    (x0, y0),
                    (Math.PI * -08 / 12, Interpolator.Hermite4pt3oX(yMinus1, y0, y1, y2, 1.0 / 3.0)),
                    (Math.PI * -07 / 12, Interpolator.Hermite4pt3oX(yMinus1, y0, y1, y2, 2.0 / 3.0)),
                    (x1, y1),
                    (Math.PI * -05 / 12, Interpolator.Hermite4pt3oX(y0, y1, y2, y3, 1.0 / 3.0)),
                    (Math.PI * -04 / 12, Interpolator.Hermite4pt3oX(y0, y1, y2, y3, 2.0 / 3.0)),
                    (x2, y2),
                    (Math.PI * -02 / 12, Interpolator.Hermite4pt3oX(y1, y2, y3, y4, 1.0 / 3.0)),
                    (Math.PI * -01 / 12, Interpolator.Hermite4pt3oX(y1, y2, y3, y4, 2.0 / 3.0)),
                    (x3, y3),
                    (Math.PI * 01 / 12, Interpolator.Hermite4pt3oX(y2, y3, y4, y5, 1.0 / 3.0)),
                    (Math.PI * 02 / 12, Interpolator.Hermite4pt3oX(y2, y3, y4, y5, 2.0 / 3.0)),
                    (x4, y4),
                    (Math.PI * 04 / 12, Interpolator.Hermite4pt3oX(y3, y4, y5, y6, 1.0 / 3.0)),
                    (Math.PI * 05 / 12, Interpolator.Hermite4pt3oX(y3, y4, y5, y6, 2.0 / 3.0)),
                    (x5, y5),
                    (Math.PI * 07 / 12, Interpolator.Hermite4pt3oX(y4, y5, y6, y7, 1.0 / 3.0)),
                    (Math.PI * 08 / 12, Interpolator.Hermite4pt3oX(y4, y5, y6, y7, 2.0 / 3.0)),
                    (x6, y6),
                    (Math.PI * 10 / 12, Interpolator.Hermite4pt3oX(y5, y6, y7, y8, 1.0 / 3.0)),
                    (Math.PI * 11 / 12, Interpolator.Hermite4pt3oX(y5, y6, y7, y8, 2.0 / 3.0)),
                    (x7, y7),
                    (Math.PI * 13 / 12, Interpolator.Hermite4pt3oX(y6, y7, y8, y9, 1.0 / 3.0)),
                    (Math.PI * 14 / 12, Interpolator.Hermite4pt3oX(y6, y7, y8, y9, 2.0 / 3.0)),
                    (x8, y8)
                },
                plotLineCount: 29);
        }

        // LookAhead Backward

        [TestMethod]
        public void Test_Synthesizer_Interpolate_Hermite_LookAhead_Backward_WithCalculatorClasses()
            => Test_Synthesizer_Interpolate_Hermite_LookAhead_Backward(CalculationEngineEnum.CalculatorClasses);

        private void Test_Synthesizer_Interpolate_Hermite_LookAhead_Backward(CalculationEngineEnum calculationEngineEnum)
        {
            const double xMinus1 = Math.PI * 12 / 12;
            const double x0 = Math.PI * 09 / 12;
            const double x1 = Math.PI * 06 / 12;
            const double x2 = Math.PI * 03 / 12;
            const double x3 = Math.PI * 00 / 12;
            const double x4 = Math.PI * -03 / 12;
            const double x5 = Math.PI * -06 / 12;
            const double x6 = Math.PI * -09 / 12;
            const double x7 = Math.PI * -12 / 12;
            const double x8 = Math.PI * -15 / 12;
            const double x9 = Math.PI * -18 / 12;

            double yMinus1 = Math.Sin(xMinus1);
            double y0 = Math.Sin(x0);
            double y1 = Math.Sin(x1);
            double y2 = Math.Sin(x2);
            double y3 = Math.Sin(x3);
            double y4 = Math.Sin(x4);
            double y5 = Math.Sin(x5);
            double y6 = Math.Sin(x6);
            double y7 = Math.Sin(x7);
            double y8 = Math.Sin(x8);
            double y9 = Math.Sin(x9);

            Test_Synthesizer_Interpolate_Base(
                calculationEngineEnum,
                InterpolationTypeEnum.Hermite,
                FollowingModeEnum.LookAhead,
                slowRate: 4.0 / Math.PI,
                new[]
                {
                    (x0, y0),
                    (Math.PI * 08 / 12, Interpolator.Hermite4pt3oX(yMinus1, y0, y1, y2, 1.0 / 3.0)),
                    (Math.PI * 07 / 12, Interpolator.Hermite4pt3oX(yMinus1, y0, y1, y2, 2.0 / 3.0)),
                    (x1, y1),
                    (Math.PI * 05 / 12, Interpolator.Hermite4pt3oX(y0, y1, y2, y3, 1.0 / 3.0)),
                    (Math.PI * 04 / 12, Interpolator.Hermite4pt3oX(y0, y1, y2, y3, 2.0 / 3.0)),
                    (x2, y2),
                    (Math.PI * 02 / 12, Interpolator.Hermite4pt3oX(y1, y2, y3, y4, 1.0 / 3.0)),
                    (Math.PI * 01 / 12, Interpolator.Hermite4pt3oX(y1, y2, y3, y4, 2.0 / 3.0)),
                    (x3, y3),
                    (Math.PI * -01 / 12, Interpolator.Hermite4pt3oX(y2, y3, y4, y5, 1.0 / 3.0)),
                    (Math.PI * -02 / 12, Interpolator.Hermite4pt3oX(y2, y3, y4, y5, 2.0 / 3.0)),
                    (x4, y4),
                    (Math.PI * -04 / 12, Interpolator.Hermite4pt3oX(y3, y4, y5, y6, 1.0 / 3.0)),
                    (Math.PI * -05 / 12, Interpolator.Hermite4pt3oX(y3, y4, y5, y6, 2.0 / 3.0)),
                    (x5, y5),
                    (Math.PI * -07 / 12, Interpolator.Hermite4pt3oX(y4, y5, y6, y7, 1.0 / 3.0)),
                    (Math.PI * -08 / 12, Interpolator.Hermite4pt3oX(y4, y5, y6, y7, 2.0 / 3.0)),
                    (x6, y6),
                    (Math.PI * -10 / 12, Interpolator.Hermite4pt3oX(y5, y6, y7, y8, 1.0 / 3.0)),
                    (Math.PI * -11 / 12, Interpolator.Hermite4pt3oX(y5, y6, y7, y8, 2.0 / 3.0)),
                    (x7, y7),
                    (Math.PI * -13 / 12, Interpolator.Hermite4pt3oX(y6, y7, y8, y9, 1.0 / 3.0)),
                    (Math.PI * -14 / 12, Interpolator.Hermite4pt3oX(y6, y7, y8, y9, 2.0 / 3.0)),
                    (x8, y8)
                }, plotLineCount: 29);
        }

        // LagBehind Forward

        [TestMethod]
        public void Test_Synthesizer_Interpolate_Hermite_LagBehind_Forward_WithCalculatorClasses()
            => Test_Synthesizer_Interpolate_Hermite_LagBehind_Forward(CalculationEngineEnum.CalculatorClasses);

        private void Test_Synthesizer_Interpolate_Hermite_LagBehind_Forward(CalculationEngineEnum calculationEngineEnum)
        {
            const double xMinus1 = Math.PI * -12 / 12;
            const double x0 = Math.PI * -09 / 12;
            const double x1 = Math.PI * -06 / 12;
            const double x2 = Math.PI * -03 / 12;
            const double x3 = Math.PI * 00 / 12;
            const double x4 = Math.PI * 03 / 12;
            const double x5 = Math.PI * 06 / 12;
            const double x6 = Math.PI * 09 / 12;
            const double x7 = Math.PI * 12 / 12;
            const double x8 = Math.PI * 15 / 12;
            const double x9 = Math.PI * 18 / 12;

            double yMinus1 = Math.Sin(Math.PI * -09 / 12);
            double y0 = Math.Sin(Math.PI * -09 / 12);
            double y1 = Math.Sin(Math.PI * -09 / 12);
            double y2 = Math.Sin(Math.PI * -09 / 12);
            double y3 = Math.Sin(Math.PI * -05 / 12);
            double y4 = Math.Sin(Math.PI * -02 / 12);
            double y5 = Math.Sin(Math.PI * 01 / 12);
            double y6 = Math.Sin(Math.PI * 04 / 12);
            double y7 = Math.Sin(Math.PI * 07 / 12);
            double y8 = Math.Sin(Math.PI * 10 / 12);
            double y9 = Math.Sin(Math.PI * 13 / 12);

            Test_Synthesizer_Interpolate_Base(
                calculationEngineEnum,
                InterpolationTypeEnum.Hermite,
                FollowingModeEnum.LagBehind,
                slowRate: 4.0 / Math.PI,
                new[]
                {
                    (x0, y0),
                    (Math.PI * -08 / 12, Interpolator.Hermite4pt3oX(yMinus1, y0, y1, y2, 1.0 / 3.0)),
                    (Math.PI * -07 / 12, Interpolator.Hermite4pt3oX(yMinus1, y0, y1, y2, 2.0 / 3.0)),
                    (x1, y1),
                    (Math.PI * -05 / 12, Interpolator.Hermite4pt3oX(y0, y1, y2, y3, 1.0 / 3.0)),
                    (Math.PI * -04 / 12, Interpolator.Hermite4pt3oX(y0, y1, y2, y3, 2.0 / 3.0)),
                    (x2, y2),
                    (Math.PI * -02 / 12, Interpolator.Hermite4pt3oX(y1, y2, y3, y4, 1.0 / 3.0)),
                    (Math.PI * -01 / 12, Interpolator.Hermite4pt3oX(y1, y2, y3, y4, 2.0 / 3.0)),
                    (x3, y3),
                    (Math.PI * 01 / 12, Interpolator.Hermite4pt3oX(y2, y3, y4, y5, 1.0 / 3.0)),
                    (Math.PI * 02 / 12, Interpolator.Hermite4pt3oX(y2, y3, y4, y5, 2.0 / 3.0)),
                    (x4, y4),
                    (Math.PI * 04 / 12, Interpolator.Hermite4pt3oX(y3, y4, y5, y6, 1.0 / 3.0)),
                    (Math.PI * 05 / 12, Interpolator.Hermite4pt3oX(y3, y4, y5, y6, 2.0 / 3.0)),
                    (x5, y5),
                    (Math.PI * 07 / 12, Interpolator.Hermite4pt3oX(y4, y5, y6, y7, 1.0 / 3.0)),
                    (Math.PI * 08 / 12, Interpolator.Hermite4pt3oX(y4, y5, y6, y7, 2.0 / 3.0)),
                    (x6, y6),
                    (Math.PI * 10 / 12, Interpolator.Hermite4pt3oX(y5, y6, y7, y8, 1.0 / 3.0)),
                    (Math.PI * 11 / 12, Interpolator.Hermite4pt3oX(y5, y6, y7, y8, 2.0 / 3.0)),
                    (x7, y7),
                    (Math.PI * 13 / 12, Interpolator.Hermite4pt3oX(y6, y7, y8, y9, 1.0 / 3.0)),
                    (Math.PI * 14 / 12, Interpolator.Hermite4pt3oX(y6, y7, y8, y9, 2.0 / 3.0)),
                    (x8, y8)
                }, plotLineCount: 29);
        }

        // LagBehind Backward

        [TestMethod]
        public void Test_Synthesizer_Interpolate_Hermite_LagBehind_Backward_WithCalculatorClasses()
            => Test_Synthesizer_Interpolate_Hermite_LagBehind_Backward(CalculationEngineEnum.CalculatorClasses);

        private void Test_Synthesizer_Interpolate_Hermite_LagBehind_Backward(CalculationEngineEnum calculationEngineEnum)
        {
            const double xMinus1 = Math.PI * 12 / 12;
            const double x0 = Math.PI * 09 / 12;
            const double x1 = Math.PI * 06 / 12;
            const double x2 = Math.PI * 03 / 12;
            const double x3 = Math.PI * 00 / 12;
            const double x4 = Math.PI * -03 / 12;
            const double x5 = Math.PI * -06 / 12;
            const double x6 = Math.PI * -09 / 12;
            const double x7 = Math.PI * -12 / 12;
            const double x8 = Math.PI * -15 / 12;
            const double x9 = Math.PI * -18 / 12;

            double yMinus1 = Math.Sin(Math.PI * 09 / 12);
            double y0 = Math.Sin(Math.PI * 09 / 12);
            double y1 = Math.Sin(Math.PI * 09 / 12);
            double y2 = Math.Sin(Math.PI * 08 / 12);
            double y3 = Math.Sin(Math.PI * 05 / 12);
            double y4 = Math.Sin(Math.PI * 02 / 12);
            double y5 = Math.Sin(Math.PI * -01 / 12);
            double y6 = Math.Sin(Math.PI * -04 / 12);
            double y7 = Math.Sin(Math.PI * -07 / 12);
            double y8 = Math.Sin(Math.PI * -10 / 12);
            double y9 = Math.Sin(Math.PI * -13 / 12);

            Test_Synthesizer_Interpolate_Base(
                calculationEngineEnum,
                InterpolationTypeEnum.Hermite,
                FollowingModeEnum.LagBehind,
                slowRate: 4.0 / Math.PI,
                new[]
                {
                    (x0, y0),
                    (Math.PI * 08 / 12, Interpolator.Hermite4pt3oX(yMinus1, y0, y1, y2, 1.0 / 3.0)),
                    (Math.PI * 07 / 12, Interpolator.Hermite4pt3oX(yMinus1, y0, y1, y2, 2.0 / 3.0)),
                    (x1, y1),
                    (Math.PI * 05 / 12, Interpolator.Hermite4pt3oX(y0, y1, y2, y3, 1.0 / 3.0)),
                    (Math.PI * 04 / 12, Interpolator.Hermite4pt3oX(y0, y1, y2, y3, 2.0 / 3.0)),
                    (x2, y2),
                    (Math.PI * 02 / 12, Interpolator.Hermite4pt3oX(y1, y2, y3, y4, 1.0 / 3.0)),
                    (Math.PI * 01 / 12, Interpolator.Hermite4pt3oX(y1, y2, y3, y4, 2.0 / 3.0)),
                    (x3, y3),
                    (Math.PI * -01 / 12, Interpolator.Hermite4pt3oX(y2, y3, y4, y5, 1.0 / 3.0)),
                    (Math.PI * -02 / 12, Interpolator.Hermite4pt3oX(y2, y3, y4, y5, 2.0 / 3.0)),
                    (x4, y4),
                    (Math.PI * -04 / 12, Interpolator.Hermite4pt3oX(y3, y4, y5, y6, 1.0 / 3.0)),
                    (Math.PI * -05 / 12, Interpolator.Hermite4pt3oX(y3, y4, y5, y6, 2.0 / 3.0)),
                    (x5, y5),
                    (Math.PI * -07 / 12, Interpolator.Hermite4pt3oX(y4, y5, y6, y7, 1.0 / 3.0)),
                    (Math.PI * -08 / 12, Interpolator.Hermite4pt3oX(y4, y5, y6, y7, 2.0 / 3.0)),
                    (x6, y6),
                    (Math.PI * -10 / 12, Interpolator.Hermite4pt3oX(y5, y6, y7, y8, 1.0 / 3.0)),
                    (Math.PI * -11 / 12, Interpolator.Hermite4pt3oX(y5, y6, y7, y8, 2.0 / 3.0)),
                    (x7, y7),
                    (Math.PI * -13 / 12, Interpolator.Hermite4pt3oX(y6, y7, y8, y9, 1.0 / 3.0)),
                    (Math.PI * -14 / 12, Interpolator.Hermite4pt3oX(y6, y7, y8, y9, 2.0 / 3.0)),
                    (x8, y8)
                }, plotLineCount: 29);
        }
    }
}