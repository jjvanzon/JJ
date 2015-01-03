//
//  Circle.Code.Helpers.OfT
//
//      Author: Jan-Joost van Zon
//      Date: 02-06-2011 - 02-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circle.Code.Helpers
{
    /// <summary>
    /// This class contains basic functionality for generic types
    /// with a single generic argument.
    /// The generic type should be a wrapper for an object of T.
    /// OfT&lt;&gt; contains basic functions for converting between T and U&lt;T&gt;,
    /// and comparing T and U&lt;T&gt;.
    /// The implementation code is quite abstract, unfortunately.
    /// TODO: test the Equals functionality.
    /// </summary>
    /// <typeparam name="T">The generic argument (e.g. T)</typeparam>
    /// <typeparam name="U">The generic type (e.g. MyGeneric&lt;T&gt;</typeparam>
    /*public class OfT<T, U>
        where U : OfT<T, U>//, new()
    {
        protected T _value;
        public virtual T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public static implicit operator T(OfT<T, U> value)
        {
            return value.Value;
        }

        //public static implicit operator OfT<T, U>(T value)
        //{
        //    return new U() { Value = value };
        //}

        public static bool operator ==(OfT<T, U> A, object B)
        {
            return Equals(A, B);
        }

        public static bool operator !=(OfT<T, U> A, object B)
        {
            return !Equals(A, B);
        }

        public override bool Equals(object other)
        {
            // Compare values of U types
            if (other is U)
            {
                U oth = (U)other;
                if (object.Equals(this.Value, oth.Value))
                {
                    return true;
                }
            }

            // Compare object of T to U's value
            else if (other is T)
            {
                T oth = (T)other;
                if (object.Equals(this.Value, oth))
                {
                    return true;
                }
            }

            // Compare any other type to U's value
            else
            {
                if (object.Equals(this.Value, other))
                {
                    return true;
                }
            }

            // No match at all: return false
            return false;
        }

        public override int GetHashCode()
        {
            if (Value != null) return Value.GetHashCode();
            else return 0;
        }
    }*/
}
