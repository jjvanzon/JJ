//
//  Circle.Code.Events.MethodWithEvents
//
//      Author: Jan-Joost van Zon
//      Date: 12-06-2011 - 12-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circle.Code.Events
{
    /// <summary>
    /// This class is not finished.
    /// I do not know how to have a generic type argument, that is a delegate type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MethodWithEvents_Old<T>
    {
        public MethodWithEvents_Old(Action<T> action)
        {
            Action = action;
        }

        public event Before<T> Before;
        public event After<T> After;

        private Action<T> Action;

        public void Execute(T args)
        {
            if (Before != null)
                Before(args);

            Action(args);

            if (After != null)
                After(args);
        }
    }
}
