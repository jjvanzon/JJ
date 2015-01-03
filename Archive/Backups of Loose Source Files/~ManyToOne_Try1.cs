//
//  Circle.Code.Relations.ManyToOne
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

namespace Circle.Code.Relations
{
    // TODO: I just can't seem to hack this.
    public class ManyToOne_Try1<C, P>
    {
        public ManyToOne_Try1(C child)
        {
            Condition.NotNull(child, "child");
            Child = child;
        }

        internal C Child;

        private OneToMany_Try1<P, C> _parent;
        public OneToMany_Try1<P, C> Parent
        {
            get { return _parent; }
            set
            {
                if (_parent == value) return;
                if (_parent != null) _parent.Remove(this);
                _parent = value;
                if (_parent != null) _parent.Add(this);
            }
        }
    }
}
