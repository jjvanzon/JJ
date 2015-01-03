//
//  Circle.Controls.Classes.ButtonList_PrivateObjects
//
//      Author: Jan-Joost van Zon
//      Date: 23-05-2011 - 05-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Objects;
using Circle.Controls.Positioning;
using Circle.Graphics.Objects;
using Circle.Controls.Style;

namespace Circle.Controls.Classes
{
    /// <summary>
    /// Orderly positions label controls.
    /// 
    /// Some private sub-objects of ButtonList share Styling objects
    /// that need to be kept in sync.
    /// This version of the ButtonList does this by keeping the sub-objects private
    /// and exposing properties that set the objects' members.
    /// 
    /// At startup, the properties are assigned to themselves,
    /// so that the styling objects are synced.
    /// 
    /// This does create more clarity for consumers of this class,
    /// but it does create horrible overhead and unclarity in the code of this class.
    /// </summary>
    public class ButtonList_PrivateObjects : Control
    {
        public ButtonList_PrivateObjects()
        {
            Rectangle = Rectangle;
            ItemPositioning = ItemPositioning;
        }

        private readonly PositionerHorizontalEquallySpread Positioner = new PositionerHorizontalEquallySpread();

        private NotNullable<BoxStyle> _itemStyle = new NotNullable<BoxStyle>();
        /// <summary> Private not nullable </summary>
        private BoxStyle ItemStyle
        {
            get { return _itemStyle; }
            set { _itemStyle = value; }
        }

        public new RectangleD Rectangle
        {
            get
            {
                return base.Rectangle;
            }
            set
            {
                Positioner.Rectangle = value;
            }
        }

        public BoxPositioning ItemPositioning
        {
            get 
            {
                return ItemStyle.Positioning ?? Positioner.ItemPositioning;
            }
            set
            {
                ItemStyle.Positioning = value;
                Positioner.ItemPositioning = value;
            }
        }

        public LineStyle LineStyle
        {
            get { return ItemStyle.LineStyle; }
            set { ItemStyle.LineStyle = value; }
        }

        public BackStyle BackStyle
        {
            get { return ItemStyle.BackStyle; }
            set { ItemStyle.BackStyle = value; }
        }

        public TextStyle TextStyle
        {
            get { return ItemStyle.TextStyle; }
            set { ItemStyle.TextStyle = value; }
        }

        public int ItemsPerLine
        {
            get { return Positioner.ItemsPerLine; }
            set { Positioner.ItemsPerLine = value; }
        }

        public double LineHeight
        {
            get { return Positioner.LineHeight; }
            set { Positioner.LineHeight = value; }
        }

        public int ItemCount
        {
            get { return Positioner.ItemCount; }
            set { Positioner.ItemCount = value; }
        }

        public override void Draw()
        {
            Children.Clear();

            Rectangle = Rectangle;
            ItemPositioning = ItemPositioning;

            Positioner.Calculate();

            foreach (RectangleD rectangle in Positioner.Rectangles)
            {
                Label label = new Label()
                {
                    Screen = Screen,
                    Style = ItemStyle
                };
                
                label.Rectangle.X1 = rectangle.X1;
                label.Rectangle.Y1 = rectangle.Y1;
                label.Rectangle.X2 = rectangle.X2;
                label.Rectangle.Y2 = rectangle.Y2;

                Children.Add(label);
            }
        }
    }
}
