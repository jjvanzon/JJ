//
//  Circle.Code.Events.Events<T>
//
//      Author: Jan-Joost van Zon
//      Date: 2011-06-24 - 2011-06-24
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circle.Code.Events
{
    /// <summary>
    /// Wraps a non-generic Events class and exposes it as a generic one.
    /// </summary>
    public class Events_Old<T> : IHear<T>
    {
        public event Getting<T> Getting;
        public event Gotten<T> Gotten;
        public event Assigning<T> Assigning;
        public event Changing<T> Changing;
        public event Changed<T> Changed;
        public event Creating<T> Creating;
        public event Created<T> Created;
        public event Annulling<T> Annulling;
        public event Annulled<T> Annulled;

        private Events Base;

        public Events_Old()
            : this(new Events())
        { }

        public Events_Old(Events base_)
        {
            Base = base_;
            Base.Value = default(T);
            Base.Getting += Base_Getting;
            Base.Gotten += Base_Gotten;
            Base.Assigning += Base_Assigning;
            Base.Changing += Base_Changing;
            Base.Changed += Base_Changed;
            Base.Creating += Base_Creating;
            Base.Created += Base_Created;
            Base.Annulling += Base_Annulling;
            Base.Annulled += Base_Annulled;
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

        public static implicit operator Events(Events_Old<T> eventsT)
        {
            return eventsT.Base;
        }

        public static explicit operator Events_Old<T>(Events events)
        {
            return new Events_Old<T>(events);
        }

        private bool Base_Getting(object value)
        {
            if (Getting != null) return Getting((T)value);
            return true;
        }

        private void Base_Gotten(object value)
        {
            if (Gotten != null) Gotten((T)value);
        }

        private void Base_Assigning(object value)
        {
            if (Assigning != null) Assigning((T)value);
        }

        private bool Base_Changing(object old, object value)
        {
            if (Changing != null) return Changing((T)old, (T)value);
            return true;
        }

        private void Base_Changed(object old, object value)
        {
            if (Changed != null) Changed((T)old, (T)value);
        }

        private bool Base_Creating(object value)
        {
            if (Creating != null) return Creating((T)value);
            return true;
        }

        private void Base_Created(object value)
        {
            if (Created != null) Created((T)value);
        }

        private bool Base_Annulling(object value)
        {
            if (Annulling != null) return Annulling((T)value);
            return true;
        }

        private void Base_Annulled(object old)
        {
            if (Annulled != null) Annulled((T)old);
        }
    }
}
