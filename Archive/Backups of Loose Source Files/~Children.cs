//
//  Circle.Controls.Classes.Children
//
//      Author: Jan-Joost van Zon
//      Date: 24-05-2011 - 24-05-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Conditions;
using System.Collections;

namespace Circle.Controls.Classes
{
    public class Children : IEnumerable<IControl>
    {
        internal Children(IControl parent)
        {
            Condition.NotNull(parent, "parent");
            Parent = parent;
        }

        private readonly IControl Parent;

        private readonly List<IControl> List = new List<IControl>();

        public bool Contains(IControl control)
        {
            return List.Contains(control);
        }

        public void Add(IControl child)
        {
            Condition.NotNull(child, "child");
            if (List.Contains(child)) return;
            List.Add(child);
            child.Parent = Parent;
        }

        public void Remove(IControl child)
        {
            Condition.NotNull(child, "control");
            if (!List.Contains(child)) return;
            List.Remove(child);
            child.Parent = null;
        }

        public IEnumerator<IControl> GetEnumerator()
        {
            foreach (var x in List)
            {
                yield return x;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
