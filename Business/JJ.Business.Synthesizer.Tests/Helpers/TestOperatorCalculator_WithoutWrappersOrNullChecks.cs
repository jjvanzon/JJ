﻿using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Names;
using JJ.Persistence.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.Tests.Helpers
{
    /// <summary>
    /// Variation with no null checks.
    /// Trying out a few things that might optimize things.
    /// </summary>
    internal class TestOperatorCalculator_WithoutWrappersOrNullChecks : ITestOperatorCalculator
    {
        private IDictionary<string, Func<Operator, double, double>> _funcDictionary;

        public TestOperatorCalculator_WithoutWrappersOrNullChecks()
        {
            _funcDictionary = new Dictionary<string, Func<Operator, double, double>>
            {
                { PropertyNames.ValueOperator, CalculateValueOperator },
                { PropertyNames.Add, CalculateAdd },
                { PropertyNames.Substract, CalculateSubstract },
                { PropertyNames.Multiply, CalculateMultiply },
            };
        }

        public double CalculateValue(Outlet outlet, double time)
        {
            Func<Operator, double, double> func = _funcDictionary[outlet.Operator.OperatorTypeName];
            double value = func(outlet.Operator, time);
            return value;
        }

        private double CalculateValueOperator(Operator op, double time)
        {
            return op.AsValueOperator.Value;
        }

        private double CalculateAdd(Operator op, double time)
        {
            double a = CalculateValue(op.Inlets[0].InputOutlet, time);
            double b = CalculateValue(op.Inlets[1].InputOutlet, time);
            return a + b;
        }

        private double CalculateSubstract(Operator op, double time)
        {
            double a = CalculateValue(op.Inlets[0].InputOutlet, time);
            double b = CalculateValue(op.Inlets[1].InputOutlet, time);
            return a - b;
        }

        private double CalculateMultiply(Operator op, double time)
        {
            if (op.Inlets[2].InputOutlet == null)
            {
                double a = CalculateValue(op.Inlets[0].InputOutlet, time);
                double b = CalculateValue(op.Inlets[1].InputOutlet, time);
                return a * b;
            }
            else
            {
                double origin = CalculateValue(op.Inlets[2].InputOutlet, time);
                double a = CalculateValue(op.Inlets[0].InputOutlet, time);
                double b = CalculateValue(op.Inlets[1].InputOutlet, time);
                return (a - origin) * b + origin;
            }
        }
    }
}
