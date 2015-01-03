//
//  Circle.Controls.Style.ButtonStyle
//
//      Author: Jan-Joost van Zon
//      Date: 2011-07-08 - 2011-07-08
//
//  -----

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Circle.Code.Concepts;
using Circle.Code.Events;
using Circle.Controls.Classes;
using Circle.Graphics.Objects;
using Circle.Code.Helpers;

namespace Circle.Controls.Style // Cannot put it in the Circle.Controls.Style assembly, because it would create a circular reference.
{
    public class ButtonStyle : IEnumerable<BoxStyle>
    {
        // TODO: 
        //
        // Probably something smarter is possible for managing the style for multiple states at once.
        // Something that would make the following possible:
        //
        // var butonStyleUpQuery = from x in ButtonStyle where x.State == ButtonState.Up;
        // foreach (var buttonStyleUp in butonStyleUpQuery)
        // {
        //      buttonStyleUp.TextStyle = new TextStyle();
        //      buttonStyleUp.TextStyle.Font = new Font("Comic Sans", 12);
        // }

        public ButtonStyle()
        {
            InitializeAll();
        }

        ~ButtonStyle()
        {
            FinalizeAll();
        }

        // Indexer

        public BoxStyle this[ButtonState state, bool focus = true]
        {
            get { return Events[state, focus].Value; }
            set { Events[state, focus].Value = value; }
        }

        public readonly EventsIndexer Events = new EventsIndexer();
        public class EventsIndexer
        {
            private Dictionary<ButtonState, Dictionary<bool, Events<BoxStyle>>> _this = new Dictionary<ButtonState, Dictionary<bool, Events<BoxStyle>>>();

            internal EventsIndexer() 
            { 
                _this[ButtonState.Disabled] = new Dictionary<bool, Events<BoxStyle>>(); 
                _this[ButtonState.Disabled][false] = new Events<BoxStyle>();
                _this[ButtonState.Disabled][true] = new Events<BoxStyle>();

                _this[ButtonState.Down] = new Dictionary<bool, Events<BoxStyle>>(); 
                _this[ButtonState.Down][false] = new Events<BoxStyle>();
                _this[ButtonState.Down][true] = new Events<BoxStyle>();

                _this[ButtonState.Up] = new Dictionary<bool, Events<BoxStyle>>(); 
                _this[ButtonState.Up][false] = new Events<BoxStyle>();
                _this[ButtonState.Up][true] = new Events<BoxStyle>();
            }

            public Events<BoxStyle> this[ButtonState state, bool focus]
            {
                get { return _this[state][focus]; }
                private set { _this[state][focus] = value; }
            }
        }

        // Subsets

        public BoxStyle Up
        {
            get { return this[ButtonState.Up, true]; }
            set
            {
                this[ButtonState.Up, true] = value;
                this[ButtonState.Up, false] = value;
            }
        }

        public BoxStyle Down
        {
            get { return this[ButtonState.Down, true]; }
            set
            {
                this[ButtonState.Down, true] = value;
                this[ButtonState.Down, false] = value;
            }
        }

        public BoxStyle Disabled
        {
            get { return this[ButtonState.Disabled, true]; }
            set
            {
                this[ButtonState.Disabled, true] = value;
                this[ButtonState.Disabled, false] = value;
            }
        }

        // Style Sync

        public readonly BoxStyle All = new BoxStyle();

        private void InitializeAll()
        {
            All.BackStyle = new BackStyle();
            All.TextStyle = new TextStyle();
            All.Positioning = new BoxPositioning();
            All.LineStyles.Top = new LineStyle();
            All.LineStyles.Right = new LineStyle();
            All.LineStyles.Bottom = new LineStyle();
            All.LineStyles.Left = new LineStyle();
            All.CornerStyles.TopLeft = new CornerStyle();
            All.CornerStyles.TopRight = new CornerStyle();
            All.CornerStyles.BottomRight = new CornerStyle();
            All.CornerStyles.BottomLeft = new CornerStyle();

            // Prevent the objects created above from being changed.

            All.BackStyleEvents.Changing += (x, y) => false;
            All.TextStyleEvents.Changing += (x, y) => false;
            All.PositioningEvents.Changing += (x, y) => false;
            All.LineStyles.TopEvents.Changing += (x, y) => false;
            All.LineStyles.LeftEvents.Changing += (x, y) => false;
            All.LineStyles.BottomEvents.Changing += (x, y) => false;
            All.LineStyles.RightEvents.Changing += (x, y) => false;
            All.CornerStyles.BottomRightEvents.Changing += (x, y) => false;
            All.CornerStyles.BottomLeftEvents.Changing += (x, y) => false;

            // Respond to each style property, by assigning all styles at once.

            InitializeAllProperty
            (
                (x) => x.BackStyle.ColorEvents,
                (x) => x.BackStyle = new BackStyle()
            );

            InitializeAllTextStyleProperty((x) => x.TextStyle.FontEvents);
            InitializeAllTextStyleProperty((x) => x.TextStyle.ColorEvents);
            InitializeAllTextStyleProperty((x) => x.TextStyle.AbbreviateEvents);
            InitializeAllTextStyleProperty((x) => x.TextStyle.WrapEvents);
            InitializeAllTextStyleProperty((x) => x.TextStyle.HorizontalAlignmentEvents);
            InitializeAllTextStyleProperty((x) => x.TextStyle.VerticalAlignmentEvents);

            InitializeAllPositioningProperty((x) => x.Positioning.Margin.TopEvents);
            InitializeAllPositioningProperty((x) => x.Positioning.Margin.RightEvents);
            InitializeAllPositioningProperty((x) => x.Positioning.Margin.BottomEvents);
            InitializeAllPositioningProperty((x) => x.Positioning.Margin.LeftEvents);

            InitializeAllPositioningProperty((x) => x.Positioning.Padding.TopEvents);
            InitializeAllPositioningProperty((x) => x.Positioning.Padding.RightEvents);
            InitializeAllPositioningProperty((x) => x.Positioning.Padding.BottomEvents);
            InitializeAllPositioningProperty((x) => x.Positioning.Padding.LeftEvents);

            InitializeAllPositioningProperty((x) => x.Positioning.BackPositioningEvents);

            InitializeAllLineStyle((x) => x.LineStyles.TopEvents);
            InitializeAllLineStyle((x) => x.LineStyles.RightEvents);
            InitializeAllLineStyle((x) => x.LineStyles.BottomEvents);
            InitializeAllLineStyle((x) => x.LineStyles.LeftEvents);

            InitializeAllProperty((x) => x.CornerStyles.TopLeft.LineEndPositioningEvents);
            InitializeAllProperty((x) => x.CornerStyles.TopRight.LineEndPositioningEvents);
            InitializeAllProperty((x) => x.CornerStyles.BottomRight.LineEndPositioningEvents);
            InitializeAllProperty((x) => x.CornerStyles.BottomLeft.LineEndPositioningEvents);
        }

        private void InitializeAllTextStyleProperty(Func<BoxStyle, IHear<object>> property)
        {
            InitializeAllProperty
            (
                (x) => property(x),
                (x) => x.TextStyle = new TextStyle()
            );
        }

        private void InitializeAllPositioningProperty(Func<BoxStyle, IHear<object>> property)
        {
            InitializeAllProperty
            (
                (x) => property(x),
                (x) => x.Positioning = new BoxPositioning()
            );
        }

        private void InitializeAllLineStyle(Func<BoxStyle, IHear<LineStyle>> lineStyle)
        {
            InitializeAllProperty
            (
                (x) => lineStyle(x).Value.WidthEvents,
                (x) => lineStyle(x).Value = new LineStyle()
            );

            InitializeAllProperty
            (
                (x) => lineStyle(x).Value.ColorEvents,
                (x) => lineStyle(x).Value = new LineStyle()
            );

            InitializeAllProperty
            (
                (x) => lineStyle(x).Value.DashEvents,
                (x) => lineStyle(x).Value = new LineStyle()
            );

            InitializeAllProperty
            (
                (x) => lineStyle(x).Value.ZIndexEvents,
                (x) => lineStyle(x).Value = new LineStyle()
            );
        }

        private void InitializeAllProperty(Func<BoxStyle, IHear<object>> property, Action<BoxStyle> createParent = null)
        {
            property(All).Changed += (old, value) =>
            {
                foreach (var x in this)
                {
                    if (createParent != null) createParent(x);
                    property(x).Value = value;
                }
            };
        }

        private void FinalizeAll()
        {
            FinalizeAllProperty
            (
                (x) => x.BackStyle.ColorEvents,
                (x) => x.BackStyle = new BackStyle()
            );

            FinalizeAllTextStyleProperty((x) => x.TextStyle.FontEvents);
            FinalizeAllTextStyleProperty((x) => x.TextStyle.ColorEvents);
            FinalizeAllTextStyleProperty((x) => x.TextStyle.AbbreviateEvents);
            FinalizeAllTextStyleProperty((x) => x.TextStyle.WrapEvents);
            FinalizeAllTextStyleProperty((x) => x.TextStyle.HorizontalAlignmentEvents);
            FinalizeAllTextStyleProperty((x) => x.TextStyle.VerticalAlignmentEvents);

            FinalizeAllPositioningProperty((x) => x.Positioning.Margin.TopEvents);
            FinalizeAllPositioningProperty((x) => x.Positioning.Margin.RightEvents);
            FinalizeAllPositioningProperty((x) => x.Positioning.Margin.BottomEvents);
            FinalizeAllPositioningProperty((x) => x.Positioning.Margin.LeftEvents);

            FinalizeAllPositioningProperty((x) => x.Positioning.Padding.TopEvents);
            FinalizeAllPositioningProperty((x) => x.Positioning.Padding.RightEvents);
            FinalizeAllPositioningProperty((x) => x.Positioning.Padding.BottomEvents);
            FinalizeAllPositioningProperty((x) => x.Positioning.Padding.LeftEvents);

            FinalizeAllPositioningProperty((x) => x.Positioning.BackPositioningEvents);

            FinalizeAllLineStyle((x) => x.LineStyles.TopEvents);
            FinalizeAllLineStyle((x) => x.LineStyles.RightEvents);
            FinalizeAllLineStyle((x) => x.LineStyles.BottomEvents);
            FinalizeAllLineStyle((x) => x.LineStyles.LeftEvents);

            FinalizeAllProperty((x) => x.CornerStyles.TopLeft.LineEndPositioningEvents);
            FinalizeAllProperty((x) => x.CornerStyles.TopRight.LineEndPositioningEvents);
            FinalizeAllProperty((x) => x.CornerStyles.BottomRight.LineEndPositioningEvents);
            FinalizeAllProperty((x) => x.CornerStyles.BottomLeft.LineEndPositioningEvents);

            All.CornerStyles.BottomLeftEvents.Changing -= (x, y) => false;
            All.CornerStyles.BottomRightEvents.Changing -= (x, y) => false;
            All.LineStyles.RightEvents.Changing -= (x, y) => false;
            All.LineStyles.BottomEvents.Changing -= (x, y) => false;
            All.LineStyles.LeftEvents.Changing -= (x, y) => false;
            All.LineStyles.TopEvents.Changing -= (x, y) => false;
            All.PositioningEvents.Changing -= (x, y) => false;
            All.TextStyleEvents.Changing -= (x, y) => false;
            All.BackStyleEvents.Changing -= (x, y) => false;
        }

        private void FinalizeAllTextStyleProperty(Func<BoxStyle, IHear<object>> property)
        {
            FinalizeAllProperty
            (
                (x) => property(x),
                (x) => x.TextStyle = new TextStyle()
            );
        }

        private void FinalizeAllPositioningProperty(Func<BoxStyle, IHear<object>> property)
        {
            FinalizeAllProperty
            (
                (x) => property(x),
                (x) => x.Positioning = new BoxPositioning()
            );
        }

        private void FinalizeAllLineStyle(Func<BoxStyle, IHear<LineStyle>> lineStyle)
        {
            FinalizeAllProperty
            (
                (x) => lineStyle(x).Value.WidthEvents,
                (x) => lineStyle(x).Value = new LineStyle()
            );

            FinalizeAllProperty
            (
                (x) => lineStyle(x).Value.ColorEvents,
                (x) => lineStyle(x).Value = new LineStyle()
            );

            FinalizeAllProperty
            (
                (x) => lineStyle(x).Value.DashEvents,
                (x) => lineStyle(x).Value = new LineStyle()
            );

            FinalizeAllProperty
            (
                (x) => lineStyle(x).Value.ZIndexEvents,
                (x) => lineStyle(x).Value = new LineStyle()
            );
        }

        private void FinalizeAllProperty(Func<BoxStyle, IHear<object>> property, Action<BoxStyle> createParent = null)
        {
            property(All).Changed -= (old, value) =>
            {
                foreach (var x in this)
                {
                    if (createParent != null) createParent(x);
                    property(x).Value = value;
                }
            };
        }

        // IEnumerable

        public IEnumerator<BoxStyle> GetEnumerator()
        {
            yield return this[ButtonState.Disabled, false];
            yield return this[ButtonState.Disabled, true];
            yield return this[ButtonState.Up, false];
            yield return this[ButtonState.Up, true];
            yield return this[ButtonState.Down, false];
            yield return this[ButtonState.Down, true];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this[ButtonState.Disabled, false];
            yield return this[ButtonState.Disabled, true];
            yield return this[ButtonState.Up, false];
            yield return this[ButtonState.Up, true];
            yield return this[ButtonState.Down, false];
            yield return this[ButtonState.Down, true];
        }
    }
}
