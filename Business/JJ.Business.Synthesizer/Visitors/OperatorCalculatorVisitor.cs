﻿using JJ.Business.Synthesizer.Calculation.Operators;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Framework.Reflection;
using JJ.Persistence.Synthesizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.Visitors
{
    internal class OperatorCalculatorVisitor : OperatorVisitorBase
    {
        private Stack<OperatorCalculatorBase> _stack;
        private Dictionary<int, OperatorCalculatorBase> _dictionary = new Dictionary<int, OperatorCalculatorBase>();

        public IList<OperatorCalculatorBase> Execute(IList<Outlet> channelOutlets)
        {
            if (channelOutlets == null) throw new NullException(() => channelOutlets);

            _stack = new Stack<OperatorCalculatorBase>();
            _dictionary = new Dictionary<int, OperatorCalculatorBase>();

            var list = new List<OperatorCalculatorBase>();

            for (int i = 0; i < channelOutlets.Count; i++)
            {
                Outlet channelOutlet = channelOutlets[i];

                VisitOutlet(channelOutlet);

                OperatorCalculatorBase operatorCalculator = _stack.Pop();

                list.Add(operatorCalculator);
            }

            return list;
        }

        protected override void VisitOperator(Operator op)
        {
            OperatorCalculatorBase calculator;
            if (_dictionary.TryGetValue(op.ID, out calculator))
            {
                _stack.Push(calculator);
                return;
            }

            base.VisitOperator(op);

            calculator = _stack.Peek();
            _dictionary.Add(op.ID, calculator);
        }

        protected override void VisitAdd(Operator op)
        {
            OperatorCalculatorBase operandACalculator = _stack.Pop();
            OperatorCalculatorBase operandBCalculator = _stack.Pop();

            if (operandACalculator == null || operandBCalculator == null)
            {
                var calculator = new ValueCalculator(0);
                _stack.Push(calculator);
            }
            else
            {
                var calculator = new AddCalculator(operandACalculator, operandBCalculator);
                _stack.Push(calculator);
            }
        }

        protected override void VisitValueOperator(Operator op)
        {
            double value = op.AsValueOperator.Value;

            var calculator = new ValueCalculator(value);
            _stack.Push(calculator);
        }

        protected override void VisitInlet(Inlet inlet)
        {
            if (inlet.Input == null)
            {
                _stack.Push(null);
            }

            base.VisitInlet(inlet);
        }
    }
}
