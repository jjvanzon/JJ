//
//  Circle.Controls.Style.StyleFourSides
//
//      Author: Jan-Joost van Zon
//      Date: 15-06-2011 - 15-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circle.Controls.Style
{
    // TODO: this geneirc class might not be required.
    public class FourSides<T>
    {
        public FourSides()
            : this(default(T))
        { 
        }

        public FourSides(T default_)
        {
            _top = default_;
            _right = default_;
            _bottom = default_;
            _left = default_;
        }

        private T _top;
        public T Top { get { return _top; } set { _top = value; } }

        private T _right;
        public T Right { get { return _right; } set { _right = value; } }

        private T _bottom;
        public T Bottom { get { return _bottom; } set { _bottom = value; } }

        private T _left;
        public T Left { get { return _left; } set { _left = value; } }

        public void Set(T top, T right, T bottom, T left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public void Set(T topBottom, T rightLeft)
        {
            Top = topBottom;
            Right = rightLeft;
            Bottom = topBottom;
            Left = rightLeft;
        }

        public void Set(T top, T rightLeft, T bottom)
        {
            Top = top;
            Right = rightLeft;
            Bottom = bottom;
            Left = rightLeft;
        }

        public T All
        {
            get
            {
                if (Equals(Top, Right) &&
                    Equals(Right, Bottom) &&
                    Equals(Bottom, Left) &&
                    Equals(Left, Top))
                {
                    return Left;
                }
                else
                {
                    return default(T);
                }
            }
            set 
            {
                Top = value;
                Right = value;
                Bottom = value;
                Left = value;
            }
        }

        public T Horizontal
        {
            get
            {
                IComparable LeftComparable = Left as IComparable;
                IComparable RightComparable = Right as IComparable;

                if (Left != null && Right != null)
                {
                    return LeftComparable.CompareTo(RightComparable) > 0 ? Left : Right;
                }
                else
                {
                    return default(T);
                }
            }
            set 
            {
                Right = value;
                Left = value;
            }
        }

        public T Vertical
        {
            get
            {
                IComparable TopComparable = Top as IComparable;
                IComparable BottomComparable = Bottom as IComparable;

                if (Top != null && Bottom != null)
                {
                    return TopComparable.CompareTo(BottomComparable) > 0 ? Top : Bottom;
                }
                else
                {
                    return default(T);
                }
            }
            set 
            {
                Top = value;
                Bottom = value;
            }
        }
    }
}
