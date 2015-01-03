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
using Circle.Code.Events;
using Circle.Code.Helpers;
using Circle.Code.Objects;
using Circle.Controls.Positioners;
using Circle.Controls.Style;
using Circle.Graphics.Objects;
using Circle.Code.Concepts;

namespace Circle.Controls.Classes
{
    /// <summary>
    /// This version of Label will keep properties of sibbling sub-objects
    /// in sync, so that e.g. Style objects that need to keep pointing
    /// to the same object, keep pointing to the same object.
    /// It does this by using Sync&gt;&lt; objects.
    /// </summary>
    public class Label_Org : Control
    {
        // Constructor

        public Label_Org()
        {
            InitializeSyncs();
            InitializeDrawResponse();
        }

        ~Label_Org()
        {
            FinalizeDrawResponse();
        }

        // Style

        [Browsable(false)]
        public Events<BoxStyle> StyleEvents { get { return Positioner.StyleEvents; } }

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

        private Sync<BackStyle> SyncBackStyle;
        private Sync<TextStyle> SyncTextStyle;

        private void InitializeSyncs()
        {
            SyncBackStyle = new Sync<BackStyle>(
                Style.BackStyleEvents,
                Rectangle.BackStyleEvents
            );

            SyncTextStyle = new Sync<TextStyle>(
                Text.StyleEvents,
                Style.TextStyleEvents
            );

            StyleEvents.Changed += (old, value) =>
            {
                SyncBackStyle.Value = value.BackStyle;
                SyncTextStyle.Value = value.TextStyle;
            };
        }

        // Draw Response

        private Response DrawResponse = new Response();

        private void InitializeDrawResponse()
        {
            DrawResponse.Root = this;

            DrawResponse.Changed += (x, y) => 
            {
                if (Screen != null) Screen.RequestDraw(); 
            };

            DrawResponse.Types.Add
            (
                typeof(Label),
                typeof(Text),
                typeof(Line),
                typeof(BoxStyle),
                typeof(BoxPositioning),
                typeof(BoxAspect),
                typeof(BackStyle),
                typeof(LineStyles),
                typeof(LineStyle),
                typeof(TextStyle),
                typeof(CornerStyles),
                typeof(CornerStyle)
            );

            DrawResponse.Members.Add
            (
                "Value*",
                "*Positioning*",
                "*Style*",
                "Top*",
                "Right*",
                "Bottom*",
                "Left*",
                "Padding*",
                "Color*",
                "Dash*",
                "Width*",
                "ZIndex*",
                "Abbreviate*",
                "Font*",
                "*Alignment*",
                "Wrap*"
            );

            DrawResponse.Set();
        }

        private void FinalizeDrawResponse()
        {
            DrawResponse.Annull();
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

