//
//  Circle.Controls.Positioning.BoxPositioner_Org
//
//      Author: Jan-Joost van Zon
//      Date: 13-06-2011 - 14-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Graphics.Objects;
using Circle.Code.Objects;
using Circle.Code.Events;
using Circle.Controls.Style;

namespace Circle.Controls.Positioning
{
    public class BoxPositioner_Org
    {
        public BoxPositioner_Org()
        {
            StyleNotNull = new NotNull<BoxStyle>(StyleEvents);
        }

        // Rectangle

        private NotNullable<RectangleD> _rectangle = new NotNullable<RectangleD>();

        /// <summary> Not nullable. Input. </summary>
        public RectangleD Rectangle
        {
            get { return _rectangle.Value; }
            set { _rectangle.Value = value; }
        }

        // Style

        private readonly NotNull<BoxStyle> StyleNotNull;

        public readonly Events<BoxStyle> StyleEvents = new Events<BoxStyle>();

        /// <summary> Not nullable. Input. </summary>
        public BoxStyle Style
        {
            get { return StyleEvents.Value; }
            set { StyleEvents.Value = value; }
        }

        // Output

        public RectangleD BorderRectangle { get; private set; }
        public RectangleD BackRectangle { get; private set; }
        public RectangleD TextRectangle { get; private set; }
        public Line TopLine { get; private set; }
        public Line RightLine { get; private set; }
        public Line BottomLine { get; private set; }
        public Line LeftLine { get; private set; }
        public readonly List<Line> Lines = new List<Line>();

        // Procedure

        public void Calculate()
        {
            CalculateBorderRectangle();
            CalculateBackRectangle();
            CalculateTextRectangle();
            CalculateBorderLines();
        }

        private void CalculateBorderRectangle()
        {
            BorderRectangle = Rectangle.Clone();

            // Substract 1/2 border width,
            // so border will be on the inside of the box.
            if (Style.LineStyles.Left != null) BorderRectangle.X1 += Style.LineStyles.Left.Width / 2;
            if (Style.LineStyles.Top != null) BorderRectangle.Y1 += Style.LineStyles.Top.Width / 2;
            if (Style.LineStyles.Right != null) BorderRectangle.X2 -= Style.LineStyles.Right.Width / 2;
            if (Style.LineStyles.Bottom != null) BorderRectangle.Y2 -= Style.LineStyles.Bottom.Width / 2;
        }

        private void CalculateBackRectangle()
        {
            BackRectangle = Rectangle.Clone();

            if (Style.Positioning != null &&
                Style.Positioning.BackPositioning == BackPositioning.Inside)
            {
                // Substract Border

                if (Style.LineStyles.Left != null) BackRectangle.X1 += Style.LineStyles.Left.Width;
                if (Style.LineStyles.Top != null) BackRectangle.Y1 += Style.LineStyles.Top.Width;
                if (Style.LineStyles.Right != null) BackRectangle.X2 -= Style.LineStyles.Right.Width;
                if (Style.LineStyles.Bottom != null) BackRectangle.Y2 -= Style.LineStyles.Bottom.Width;
            }

            if (Style.Positioning != null &&
                Style.Positioning.BackPositioning == BackPositioning.OnLine)
            {
                // Add Border

                if (Style.LineStyles.Left != null) BackRectangle.X1 += Style.LineStyles.Left.Width / 2;
                if (Style.LineStyles.Top != null) BackRectangle.Y1 += Style.LineStyles.Top.Width / 2;
                if (Style.LineStyles.Right != null) BackRectangle.X2 -= Style.LineStyles.Right.Width / 2;
                if (Style.LineStyles.Bottom != null) BackRectangle.Y2 -= Style.LineStyles.Bottom.Width / 2;
            }
        }

        private void CalculateTextRectangle()
        {
            TextRectangle = Rectangle.Clone();

            // Substract Border

            if (Style.LineStyles.Left != null) TextRectangle.X1 += Style.LineStyles.Left.Width;
            if (Style.LineStyles.Top != null) TextRectangle.Y1 += Style.LineStyles.Top.Width;
            if (Style.LineStyles.Right != null) TextRectangle.X2 -= Style.LineStyles.Right.Width;
            if (Style.LineStyles.Bottom != null) TextRectangle.Y2 -= Style.LineStyles.Bottom.Width;

            // Substract Padding

            if (Style.Positioning != null)
            {
                TextRectangle.X1 += Style.Positioning.Padding.Left;
                TextRectangle.Y1 += Style.Positioning.Padding.Top;
                TextRectangle.X2 -= Style.Positioning.Padding.Right;
                TextRectangle.Y2 -= Style.Positioning.Padding.Bottom;
            }
        }

        private void CalculateBorderLines()
        {
            Lines.Clear();

            TopLine = new Line();
            RightLine = new Line();
            BottomLine = new Line();
            LeftLine = new Line();

            Lines.Add(TopLine);
            Lines.Add(RightLine);
            Lines.Add(BottomLine);
            Lines.Add(LeftLine);

            if (Style.LineStyles.Top != null)
            {
                TopLine.PointA = BorderRectangle.PointA;
                TopLine.PointB = BorderRectangle.PointB;
                TopLine.Style = Style.LineStyles.Top;
            }

            if (Style.LineStyles.Right != null)
            {
                RightLine.PointA = BorderRectangle.PointB;
                RightLine.PointB = BorderRectangle.PointC;
                RightLine.Style = Style.LineStyles.Right;
            }

            if (Style.LineStyles.Bottom != null)
            {
                BottomLine.PointA = BorderRectangle.PointC;
                BottomLine.PointB = BorderRectangle.PointD;
                BottomLine.Style = Style.LineStyles.Bottom;
            }

            if (Style.LineStyles.Left != null)
            {
                LeftLine.PointA = BorderRectangle.PointD;
                LeftLine.PointB = BorderRectangle.PointA;
                LeftLine.Style = Style.LineStyles.Left;
            }

            if (Style.Positioning != null && 
                Style.Positioning.LineEndPositioning == LineEndPositioning.Outside)
            {
                if (Style.LineStyles.Left != null)
                {
                    TopLine.PointA.X -= Style.LineStyles.Left.Width / 2;
                    BottomLine.PointB.X -= Style.LineStyles.Left.Width / 2;
                }

                if (Style.LineStyles.Right != null)
                {
                    TopLine.PointB.X += Style.LineStyles.Right.Width / 2;
                    BottomLine.PointA.X += Style.LineStyles.Right.Width / 2;
                }

                if (Style.LineStyles.Top != null)
                {
                    RightLine.PointA.Y -= Style.LineStyles.Top.Width / 2;
                    LeftLine.PointB.Y -= Style.LineStyles.Top.Width / 2;
                }

                if (Style.LineStyles.Bottom != null)
                {
                    RightLine.PointB.Y += Style.LineStyles.Bottom.Width / 2;
                    LeftLine.PointA.Y += Style.LineStyles.Bottom.Width / 2;
                }
            }

            Lines.OrderBy(x => x.Style.ZIndex);
        }
    }
}
