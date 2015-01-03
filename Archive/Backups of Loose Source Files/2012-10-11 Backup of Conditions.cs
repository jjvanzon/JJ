//
//  Circle.Framework.Code.Conditions.Conditions
//
//      Author: Jan-Joost van Zon
//      Date: 2012-10-10 - 2012-10-10
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Circle.Framework.Code.Conditions
{
    public class Conditions : IEnumerable<ICondition>
    {
        // Constuctor

        public Conditions(params ICondition[] list)
        {
            InitializeList(list);
        }

        // List

        private ICondition[] List;

        private void InitializeList(ICondition[] list)
        {
            Condition.NotNull(list, "list");

            foreach (ICondition item in list)
            {
                Condition.NotNull(item, "item");
            }

            List = list;
        }

        // Evaluate

        public void Evaluate()
        {
            foreach (ICondition x in List)
            {
                x.Evaluate();
            }
        }

        // IEnumerator

        public IEnumerator<ICondition> GetEnumerator()
        {
            foreach (ICondition x in List)
            {
                yield return x;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (ICondition x in List)
            {
                yield return x;
            }
        }
    }
}
