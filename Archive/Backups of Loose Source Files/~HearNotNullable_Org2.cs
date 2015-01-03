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
    public class HearNotNullable_Org<T> : Hear<T>
    {
        public new event Changed<T> Changed;

        public HearNotNullable_Org()
        {
            base.Changed += HearNotNullable_Changed;
        }

        private void HearNotNullable_Changed(T old, T value)
        {
            if (Changed != null) Changed(old, value);
        }

        public new T Value
        {
            get
            {
                if (base.Value == null)
                {
                    Value = Activator.CreateInstance<T>(); // Make sure the setter goes off, so a changed event is fired.
                }

                return base.Value;
            }
            set
            {
                Condition.NotNull(value, typeof(T).Name);

                base.Value = value;
            }
        }
    }
}
