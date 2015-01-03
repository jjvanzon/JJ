//
//  Circle.Code.Relations.OneToMany_Try1
//
//      Author: Jan-Joost van Zon
//      Date: 25-05-2011 - 04-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Conditions;
using System.Collections;

namespace Circle.Code.Relations
{
    // TODO: I just can't seem to hack this.
    public class OneToMany_Try1<P, C> : IEnumerable<C>
    {
        public OneToMany_Try1(P parent)
        {
            Condition.NotNull(parent, "parent");
            Parent = parent;
        }

        internal P Parent;

        private readonly List<ManyToOne_Try1<C, P>> Children = new List<ManyToOne_Try1<C, P>>();

        public void Add(ManyToOne_Try1<C, P> child)
        {
            Condition.NotNull(child, "child");

            if (!Children.Contains(child))
            {
                if (child.Parent != null)
                {
                    child.Parent.Children.Remove(child);
                }

                Children.Add(child);
            }
        }

        public void Remove(ManyToOne_Try1<C, P> child)
        {
            Condition.NotNull(child, "child");
            Children.Remove(child);
        }

        public IEnumerator<C> GetEnumerator()
        {
            foreach (var x in Children)
            {
                yield return x.Child;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
