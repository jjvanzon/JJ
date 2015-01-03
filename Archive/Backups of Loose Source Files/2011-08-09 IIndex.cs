// 2011-08-09

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circle.Data.Collections
{
    public interface IIndex<T1, T2>
    {
        T2 this[T1 x] { get; set; }
        bool Contains(T1 x);
    }

    public interface IIndex<T1, T2, T3>
    {
        T3 this[T1 x, T2 y] { get; set; }
        bool Contains(T1 x, T2 y);
    }

    public interface IIndex<T1, T2, T3, T4>
    {
        T4 this[T1 a, T2 b, T3 c] { get; set; }
        bool Contains(T1 a, T2 b, T3 c);
    }

    public interface IIndex<T1, T2, T3, T4, T5>
    {
        T5 this[T1 a, T2 b, T3 c, T4 d] { get; set; }
        bool Contains(T1 a, T2 b, T3 c, T4 d);
    }

    public interface IIndex<T1, T2, T3, T4, T5, T6>
    {
        T6 this[T1 a, T2 b, T3 c, T4 d, T5 e] { get; set; }
        bool Contains(T1 a, T2 b, T3 c, T4 d, T5 e);
    }

    public interface IIndex<T1, T2, T3, T4, T5, T6, T7>
    {
        T7 this[T1 a, T2 b, T3 c, T4 d, T5 e, T6 f] { get; set; }
        bool Contains(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f);
    }

    public interface IIndex<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        T8 this[T1 a, T2 b, T3 c, T4 d, T5 e, T6 f, T7 g] { get; set; }
        bool Contains(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f, T7 g);
    }
}
