//
//  Circle.Code.Events.HearNotNullable<T>
//
//      Author: Jan-Joost van Zon
//      Date: 02-06-2011 - 24-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circle.Code.Events
{
    /// <summary>
    /// Wraps a non-generic HearNotNullable class and exposes it as a generic one.
    /// </summary>
    public class HearNotNullable<T> : IHear<T>
    {
        public event Changed<T> Changed;

        private HearNotNullable Base;

        public HearNotNullable()
            : this(new HearNotNullable())
        { }

        public HearNotNullable(HearNotNullable base_)
        {
            Base = base_;
            Base.Changed += Base_Changed;
        }

        public T Value
        {
            get { return (T)Base.Value; }
            set { Base.Value = value; }
        }

        public bool IsCreated
        {
            get { return Base.IsCreated; }
        }

        private void Base_Changed(object old, object value)
        {
            if (Changed != null) Changed((T)old, (T)value);
        }

        public static implicit operator HearNotNullable(HearNotNullable<T> value)
        {
            return value.Base;
        }

        public static explicit operator HearNotNullable<T>(HearNotNullable value)
        {
            return new HearNotNullable<T>(value);
        }
    }
}
