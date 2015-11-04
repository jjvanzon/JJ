﻿namespace JJ.Business.Synthesizer.Calculation.Operators
{
    /// <summary>
    /// Slight performace gain compared to the Number_OperatorCalculator.
    /// It has Number_OperatorCalculator as a base class to make it participate in the 
    /// optimization mechanisms in OptimizedPatchCalculatorVisitor.
    /// </summary>
    internal class Zero_OperatorCalculator : Number_OperatorCalculator
    {
        public override double Calculate(double time, int channelIndex)
        {
            return 0;
        }
    }
}
