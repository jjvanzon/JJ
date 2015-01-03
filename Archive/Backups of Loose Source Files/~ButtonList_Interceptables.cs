//
//  Circle.Controls.Classes.ButtonList_Interceptables
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
using Circle.Code.Events;

namespace Circle.Controls.Classes
{
    /// <summary>
    /// Orderly positions label controls.
    /// 
    /// Some sub-objects of ButtonList share Styling objects
    /// They need to be kept in sync.
    /// 
    /// This version of the ButtonList does this by 
    /// responding to Changed event associated with the properties.
    /// 
    /// The downside is the overhead of making properties interceptable.
    /// Another downside may be having to provide some event wiring.
    /// Another downside may be that things can become extreme difficult to debug.
    /// The upside is an intuitive experience in the consumer code,
    /// because it does not matter which objec reference you use,
    /// they will always stay the same object.
    /// 
    /// This is certainly the coolest solution.
    /// </summary>
    public class ButtonList_Interceptables : Control
    {
        public ButtonList_Interceptables()
        {
            SetRectangle();
            SetPositioning(new BoxPositioning());

            ItemStyleEvents.Changed += (old, value) => SetItemStyle(value);
            Positioner.RectangleEvents.Changed += (old, value) => SetRectangle();
            Positioner.ItemPositioningEvents.Changed += (old, value) => SetPositioning(value);
            ItemStyle.PositioningEvents.Changed += (old, value) => SetPositioning(value);
        }

        private void SetRectangle()
        {
            Positioner.Rectangle = Rectangle;
        }

        private void SetPositioning(BoxPositioning value)
        {
            ItemStyle.Positioning = value;
            Positioner.ItemPositioning = value;
        }

        private void SetItemStyle(BoxStyle value)
        {
            ItemStyle = value;

            foreach (Label label in Children)
            {
                label.Style = value;
            }
        }

        public readonly HearNotNullable<BoxStyle> ItemStyleEvents = new HearNotNullable<BoxStyle>();
        /// <summary> Not nullable </summary>
        public BoxStyle ItemStyle
        {
            get { return ItemStyleEvents.Value; }
            set { ItemStyleEvents.Value = value; }
        }

        public readonly PositionerHorizontalEquallySpread Positioner = new PositionerHorizontalEquallySpread();

        public override void Setup()
        {
            Condition.NotNull(ItemStyle.Positioning, "ItemStyle.Positioning");

            Positioner.Calculate();

            for (int i = Children.Count; i < Positioner.Rectangles.Count; i++)
            {
                AddLabel();
            }

            for (int i = Positioner.Rectangles.Count; i < Children.Count; i++)
            {
                RemoveLabel(i);
            }

            for (int i = 0; i < Children.Count; i++)
            {
                UpdateLabel(i);
            }
        }

        // Private methods

        private void AddLabel()
        {
            Children.Add(new Label() { Screen = Screen, Style = ItemStyle });
        }

        private void RemoveLabel(int i)
        {
            IControl label = Children[i];
            Screen.Controls.Remove(label);
            Children.Remove(label);
        }

        private void UpdateLabel(int i)
        {
            RectangleD rectA = Children[i].Rectangle;
            RectangleD rectB = Positioner.Rectangles[i];
            rectA.X1 = rectB.X1;
            rectA.Y1 = rectB.Y1;
            rectA.X2 = rectB.X2;
            rectA.Y2 = rectB.Y2;
        }
    }
}
