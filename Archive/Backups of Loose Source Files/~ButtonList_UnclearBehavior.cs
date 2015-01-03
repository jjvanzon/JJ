//
//  Circle.Controls.Classes.ButtonList_UnclearBehavior
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
using Circle.Code.Conditions;

namespace Circle.Controls.Classes
{
    /// <summary>
    /// Orderly positions label controls.
    /// 
    /// ButtonList.Rectangle styles the background.
    /// ButtonList.ItemStyle controls the styling of the labels.
    /// ButtonList.Positioner controls the positioning parameters.
    /// 
    /// Do not use Positioner.BoxPositioning or Positioner.Rectangle,
    /// because they will be overwritten.
    /// 
    /// That is the way this version of the ButtonList resolves overlapping styling objects.
    /// This can create confusion with consumers of this class unless they read this description.
    /// </summary>
    public class ButtonList_UnclearBehavior : Control
    {
        private NotNullable<BoxStyle> _itemStyle = new NotNullable<BoxStyle>();
        /// <summary> Not nullable </summary>
        public BoxStyle ItemStyle
        {
            get { return _itemStyle; }
            set { _itemStyle = value; }
        }

        public readonly PositionerHorizontalEquallySpread Positioner = new PositionerHorizontalEquallySpread();

        public override void Draw()
        {
            Condition.NotNull(ItemStyle.Positioning, "ItemStyle.Positioning");

            Children.Clear();

            Positioner.Rectangle = Rectangle;
            Positioner.ItemPositioning = ItemStyle.Positioning;
            Positioner.Calculate();

            foreach (RectangleD rect in Positioner.Rectangles)
            {
                Label label = new Label()
                {
                    Screen = Screen,
                    Style = ItemStyle
                };
                
                label.Rectangle.X1 = rect.X1;
                label.Rectangle.Y1 = rect.Y1;
                label.Rectangle.X2 = rect.X2;
                label.Rectangle.Y2 = rect.Y2;

                Children.Add(label);
            }
        }
    }
}
