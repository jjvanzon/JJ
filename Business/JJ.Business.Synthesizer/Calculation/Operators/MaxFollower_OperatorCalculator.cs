﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Collections;

namespace JJ.Business.Synthesizer.Calculation.Operators
{
    internal class MaxFollower_OperatorCalculator : MaxOrMinFollower_OperatorCalculatorBase
    {
        public MaxFollower_OperatorCalculator(
            OperatorCalculatorBase signalCalculator,
            OperatorCalculatorBase sliceLengthCalculator,
            OperatorCalculatorBase sampleCountCalculator,
            DimensionStack dimensionStack)
            : base(signalCalculator, sliceLengthCalculator, sampleCountCalculator, dimensionStack)
        { }

        protected override double GetMaxOrMin(RedBlackTree<double, double> redBlackTree)
        {
            return redBlackTree.GetMaximum();
        }
    }
}
