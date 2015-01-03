//
//  Circle.Controls.Classes.Label_UnclearBehavior
//
//      Author: Jan-Joost van Zon
//      Date: 23-05-2011 - 03-06-2011
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
    /// This version of Label will overwrite properties of sibbling subobjects
    /// as soon as it is redrawn. This can create confusion with consumers of this class.
    /// </summary>
    public class Label_UnclearBehavior : Control
    {
        public Label_UnclearBehavior()
        {
            Text.ValueEvents.Changed += (x, y) => Screen.RequestDraw();
        }

        private NotNullable<BoxStyle> _style = new NotNullable<BoxStyle>();
        /// <summary> Not nullable </summary>
        public BoxStyle Style
        {
            get { return _style; }
            set { _style = value; }
        }

        /// <summary> Do not set the Style or Position properties, because these will be overwritten. </summary>
        public readonly Text Text = new Text();

        public override void Draw()
        {
            Condition.NotNull(Screen, "Screen");
            Condition.NotNull(Screen.Draw, "Screen.Draw");

            RectangleD rectangle = RectangleInScreen;

            rectangle.LineStyle = Style.LineStyle ?? rectangle.LineStyle;
            rectangle.BackStyle = Style.BackStyle ?? rectangle.BackStyle;

            Text.Rectangle = rectangle;
            Text.Style = Style.TextStyle ?? Text.Style;

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
