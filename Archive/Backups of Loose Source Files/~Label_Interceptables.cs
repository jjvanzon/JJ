//
//  Circle.Controls.Classes.Label_Interceptables
//
//      Author: Jan-Joost van Zon
//      Date: 23-05-2011 - 06-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Controls.Style;
using Circle.Controls.Positioning;
using Circle.Graphics.Objects;
using Circle.Code.Conditions;
using Circle.Code.Events;
using Circle.Code.Objects;

namespace Circle.Controls.Classes
{
    /// <summary>
    /// This version of Label will keep properties of sibbling sub-objects
    /// in sync, so that e.g. Style objects that need to keep pointing
    /// to the same object, keep pointing to the same object.
    /// </summary>
    public class Label_Interceptables : Control
    {
        public Label_Interceptables()
        {
            Text.ValueEvents.Changed += (x, y) => Screen.RequestDraw();

            StyleEvents.Changed += (old, value) => SetStyle(value);
            Style.BackStyleEvents.Changed += (old, value) => SetBackStyle(value);
            Style.LineStyleEvents.Changed += (old, value) => SetLineStyle(value);
            Style.TextStyleEvents.Changed += (old, value) => SetTextStyle(value);
            Text.StyleEvents.Changed += (old, value) => SetTextStyle(value);
            Rectangle.LineStyleEvents.Changed += (old, value) => SetLineStyle(value);
            Rectangle.BackStyleEvents.Changed += (old, value) => SetBackStyle(value);
        }

        private void SetStyle(BoxStyle value)
        {
            Style = value;
            SetLineStyle(value.LineStyle);
            SetBackStyle(value.BackStyle);
        }

        private void SetLineStyle(LineStyle value)
        {
            Style.LineStyle = value;
            Rectangle.LineStyle = value;
        }

        private void SetBackStyle(BackStyle value)
        {
            Style.BackStyle = value;
            Rectangle.BackStyle = value;
        }

        private void SetTextStyle(TextStyle value)
        {
            Style.TextStyle = value;
            Text.Style = value;
        }

        private readonly HearNotNullable<BoxStyle> StyleEvents = new HearNotNullable<BoxStyle>();
        /// <summary> Not nullable </summary>
        public BoxStyle Style
        {
            get { return StyleEvents.Value; }
            set { StyleEvents.Value = value; }
        }

        /// <summary> Position properties will be overwritten. </summary>
        public readonly Text Text = new Text();

        public override void Draw()
        {
            Condition.NotNull(Screen, "Screen");
            Condition.NotNull(Screen.Draw, "Screen.Draw");

            RectangleD rectangle = RectangleInScreen;

            Text.Rectangle = rectangle;

            Screen.Draw.Rectangle(rectangle);
            Screen.Draw.Text(Text);
        }
    }
}

// TODO: start using BoxPositioning, which should get more styling capabilities.
//private BoxPositioning BoxPositioning = new BoxPositioning();

// TODO: use BoxPositioning.
//BoxPositioning.Outlining = Style.Outlining;
//BoxPositioning.Position = Position;
