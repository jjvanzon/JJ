//
//  Circle.Graphics.Objects.RectangleD_Org
//
//      Author: Jan-Joost van Zon
//      Date: 23-05-2011 - 23-05-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Circle.Code.Events;
using System.Diagnostics;

namespace Circle.Graphics.Objects
{
    [DebuggerDisplay("({X1}, {Y1}) - ({X2}, {Y2})")]
    public class RectangleD_Org
    {
        // TODO: materialize PointA and PointB, instead of the 4 doubles,
        // because that way you can give graphics objects references to the same point.
        // perhaps this is too much of a hassle, because you would have to keep the points
        // in sync, so they stay a rectangle.

        // Actually, you could now do that using Syncs.

        public readonly Hear<double> X1Events = new Hear<double>();
        public double X1
        {
            get { return X1Events.Value; }
            set { X1Events.Value = value; }
        }

        public readonly Hear<double> Y1Events = new Hear<double>();
        public double Y1
        {
            get { return Y1Events.Value; }
            set { Y1Events.Value = value; }
        }

        public readonly Hear<double> X2Events = new Hear<double>();
        public double X2
        {
            get { return X2Events.Value; }
            set { X2Events.Value = value; }
        }

        public readonly Hear<double> Y2Events = new Hear<double>();
        public double Y2
        {
            get { return Y2Events.Value; }
            set { Y2Events.Value = value; }
        }
   
        public double Width
        {
            get { return X2 - X1; }
            set { X2 = X1 + value; }
        }

        public double Height
        {
            get { return Y2 - Y1; }
            set { Y2 = Y1 + value; }
        }

        public void Move(double x, double y)
        {
            double width = Width;
            double height = Height;
            X1 = x;
            Y1 = y;
            Width = width;
            Height = height;
        }

        public void Offset(double x, double y)
        {
            Move(X1 + x, Y1 + y);
        }

        // TODO: offer the ability to control all four sides' border style separately.

        public readonly Events<LineStyle> LineStyleEvents = new Events<LineStyle>();
        public LineStyle LineStyle
        {
            get { return LineStyleEvents.Value; }
            set { LineStyleEvents.Value = value; }
        }

        public readonly Hear<BackStyle> BackStyleEvents = new Hear<BackStyle>();
        public BackStyle BackStyle
        {
            get { return BackStyleEvents.Value; }
            set { BackStyleEvents.Value = value; }
        }

        /// <summary> Top-left corner </summary>
        public PointD PointA { get { return new PointD(X1, Y1); } }
        /// <summary> Top-right corner </summary>
        public PointD PointB { get { return new PointD(X2, Y1); } }
        /// <summary> Bottom-right corner </summary>
        public PointD PointC { get { return new PointD(X2, Y2); } }
        /// <summary> Bottom-left corner </summary>
        public PointD PointD { get { return new PointD(X1, Y2); } }

        public Line LineA { get { return new Line() { PointA = PointA, PointB = PointB, Style = LineStyle }; } }
        public Line LineB { get { return new Line() { PointA = PointB, PointB = PointC, Style = LineStyle }; } }
        public Line LineC { get { return new Line() { PointA = PointC, PointB = PointD, Style = LineStyle }; } }
        public Line LineD { get { return new Line() { PointA = PointD, PointB = PointA, Style = LineStyle }; } }

        public RectangleD Clone()
        {
            return new RectangleD()
            {
                X1 = X1,
                Y1 = Y1,
                X2 = X2,
                Y2 = Y2,
                LineStyle = LineStyle,
                BackStyle = BackStyle
            };
        }
    }
}
