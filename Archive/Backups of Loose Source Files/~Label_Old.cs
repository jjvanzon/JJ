//
//  Circle.Controls.Classes.Label
//
//      Author: Jan-Joost van Zon
//      Date: 23-05-2011 - 06-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Circle.Code.Conditions;
using Circle.Code.Encapsulation;
using Circle.Code.Events;
using Circle.Code.Helpers;
using Circle.Code.Objects;
using Circle.Controls.Positioning;
using Circle.Controls.Style;
using Circle.Graphics.Objects;

namespace Circle.Controls.Classes
{
    /// <summary>
    /// This version of Label will keep properties of sibbling sub-objects
    /// in sync, so that e.g. Style objects that need to keep pointing
    /// to the same object, keep pointing to the same object.
    /// It does this by using Sync&gt;&lt; objects.
    /// </summary>
    public class Label : Control
    {
        // Style
        
        /*public readonly HearNotNullable<BoxStyle> StyleEvents = new HearNotNullable<BoxStyle>();
        /// <summary> Not nullable </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BoxStyle Style
        {
            get { return StyleEvents.Value; }
            set { StyleEvents.Value = value; }
        }*/

        public HearNotNullable<BoxStyle> StyleEvents { get { return Positioner.StyleEvents; } }
        /// <summary> Not nullable </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BoxStyle Style
        {
            get { return Positioner.Style; }
            set { Positioner.Style = value; }
        }

        // Text

        private readonly Text _text = new Text();
        /// <summary> Readonly, not nullable. Position properties will be overwritten. </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Text Text
        {
            get { return _text; }
        }

        // Positioner

        private readonly BoxPositioner Positioner = new BoxPositioner();

        // Syncs

        //private readonly Sync<BoxStyle> SyncStyle;
        private readonly Sync<BackStyle> SyncBackStyle;
        private readonly Sync<TextStyle> SyncTextStyle;

        public Label()
        {
            /*SyncStyle = new Sync<BoxStyle>(
                StyleEvents,
                Positioner.StyleEvents
            );*/

            SyncBackStyle = new Sync<BackStyle>(
                Style.BackStyleEvents,
                Rectangle.BackStyleEvents
            );

            SyncTextStyle = new Sync<TextStyle>(
                Text.StyleEvents,
                Style.TextStyleEvents
            );

            StyleEvents.Changed += (old, value) => SetStyle(value);

            Text.ValueEvents.Changed += (x, y) => Screen.RequestDraw();
            //StyleEvents.Changed += (x, y) => Screen.RequestDraw();

            // Disallow changing Rectangle.LineStyle.
            Rectangle.LineStyleEvents.Changing += (x, y) => false;

            // Only allow getting null for Rectangle.LineStyle.
            // TODO: figure out what this was for.
            Rectangle.LineStyleEvents.Getting += (x) => x == null;
        }

        private void SetStyle(BoxStyle value)
        {
            SyncBackStyle.Value = value.BackStyle;
            SyncTextStyle.Value = value.TextStyle;
        }

        // Draw

        public override void Draw()
        {
            Condition.NotNull(Screen, "Screen");
            Condition.NotNull(Screen.Draw, "Screen.Draw");

            Positioner.Rectangle = RectangleInScreen;
            Positioner.Calculate();

            // Back
            Positioner.BackRectangle.LineStyle = null;
            Screen.Draw.Rectangle(Positioner.BackRectangle);
            
            // Lines
            foreach (var x in Positioner.Lines)
            {
                Screen.Draw.Line(x);
            }

            // Text 
            Text.Rectangle = Positioner.TextRectangle;
            Screen.Draw.Text(Text);
        }
    }
}

