//
//  Circle.Code.Events.Sync_Old
//
//      Author: Jan-Joost van Zon
//      Date: 07-06-2011 - 07-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Conditions;

namespace Circle.Code.Events
{
    public class Sync_Old<T> : IHear<T>
    {
        public event Changed<T> Changed;

        public Sync_Old() { }

        public Sync_Old(params IHear<T>[] events)
        {
            foreach (IHear<T> x in events)
            {
                Events.Add(x);
            }

            Bind();
        }

        public readonly List<IHear<T>> Events = new List<IHear<T>>();

        private void Bind()
        {
            foreach (var e in Events)
            {
                e.Changed += (old, value) => Value = value;
            }
        }

        public T Value
        {
            get
            {
                Condition.NotEmpty(Events.Count, "Sync.Events");
                return Events[0].Value;
            }
            set
            {
                foreach (var e in Events)
                {
                    e.Value = value;
                }

                if (Changed != null) Changed(default(T), value);
            }
        }
    }
}
