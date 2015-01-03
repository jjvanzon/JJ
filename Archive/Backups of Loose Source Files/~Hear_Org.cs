//
//  Circle.Code.Events.Hear
//
//      Author: Jan-Joost van Zon
//      Date: 02-06-2011 - 02-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Helpers;
using System.ComponentModel;

namespace Circle.Code.Events
{
    /// <summary>
    /// A lightweight version of Events&lt;T&gt;,
    /// that can pick up a change of value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DefaultProperty("Value")]
    public class Hear_Org<T> : IHear<T>
    {
        public event Changed<T> Changed;

        private T _value;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (Object.Equals(_value, value)) return;

                T old = _value;

                _value = value;

                if (Changed != null) Changed(old, _value);
            }
        }
    }
}
