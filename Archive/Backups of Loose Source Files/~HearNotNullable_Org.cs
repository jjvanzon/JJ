//
//  Circle.Code.Events.HearNotNullable
//
//      Author: Jan-Joost van Zon
//      Date: 31-05-2011 - 02-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Conditions;
using Circle.Code.Helpers;
using System.ComponentModel;

namespace Circle.Code.Events
{
    [DefaultProperty("Value")]
    public class HearNotNullable_Org<T> : IHear<T>
    {
        public event Changed<T> Changed;

        private T _value;

        public T Value
        {
            get
            {
                if (_value == null)
                {
                    // Make sure the setter goes off, so a changed event is fired.
                    Value = Activator.CreateInstance<T>();
                }

                return _value;
            }
            set
            {
                Condition.NotNull(value, typeof(T).Name);

                if (Object.Equals(_value, value)) return;

                T old = _value;

                _value = value;

                if (Changed != null) Changed(old, _value);
            }
        }

        /*public static implicit operator HearNotNullable<T>(T value)
        {
            return new HearNotNullable<T>() { Value = value };
        }*/
    }
}
