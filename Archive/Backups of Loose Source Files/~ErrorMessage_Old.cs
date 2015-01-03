//
//  Circle.Diagram.Entities.ErrorMessage
//
//      Author: Jan-Joost van Zon
//      Date: 2011-05-22 - 2011-05-22
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Graphics.Objects;

namespace Circle.Diagram.Entities
{
    public class ErrorMessage
    {
        public Symbol Symbol;

        public PointD TopLeft = new PointD();
        public PointD BottomRight = new PointD();

        public PointD Center
        {
            get 
            {
                return new PointD()
                {
                    X = (TopLeft.X + BottomRight.X) / 2,
                    Y = (TopLeft.Y + BottomRight.Y) / 2
                };
            } 
        }

        public double Top
        {
            get { return TopLeft.Y; }
            set { TopLeft.Y = value; }
        }

        public double Left
        {
            get { return TopLeft.X; }
            set { TopLeft.X = value; }
        }

        public double Bottom
        {
            get { return BottomRight.Y; }
            set { BottomRight.Y = value; }
        }

        public double Right
        {
            get { return BottomRight.X; }
            set { BottomRight.X = value; }
        }

        public double Width { get { return Right - Left; } }
        public double Height { get { return Bottom - Top; } }

        public string Text;
    }
}
