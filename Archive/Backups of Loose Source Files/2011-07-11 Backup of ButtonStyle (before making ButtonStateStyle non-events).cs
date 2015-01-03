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
    public class ButtonStyle : IEnumerable<ButtonStateStyle>
    {
        // TODO: ButtonStateStyle should not be written, so it might not need to be Events.

        // Indexer

        public ButtonStateStyle this[ButtonState state, bool focus = true]
        {
            get { return Events[state, focus]; }
        }
        
        public readonly EventsIndexer Events = new EventsIndexer();
        public class EventsIndexer
        {
            private Dictionary<ButtonState, Dictionary<bool, ButtonStateStyle>> _this = new Dictionary<ButtonState, Dictionary<bool, ButtonStateStyle>>();

            internal EventsIndexer()
            { 
                _this[ButtonState.Disabled] = new Dictionary<bool, ButtonStateStyle>(); 
                _this[ButtonState.Disabled][false] = new ButtonStateStyle();
                _this[ButtonState.Disabled][false].State = ButtonState.Disabled;
                _this[ButtonState.Disabled][false].Focus = false;
                _this[ButtonState.Disabled][true] = new ButtonStateStyle();
                _this[ButtonState.Disabled][true].State = ButtonState.Disabled;
                _this[ButtonState.Disabled][true].Focus = true;

                _this[ButtonState.Down] = new Dictionary<bool, ButtonStateStyle>(); 
                _this[ButtonState.Down][false] = new ButtonStateStyle();
                _this[ButtonState.Down][false].State = ButtonState.Down;
                _this[ButtonState.Down][false].Focus = false;
                _this[ButtonState.Down][true] = new ButtonStateStyle();
                _this[ButtonState.Down][true].State = ButtonState.Down;
                _this[ButtonState.Down][true].Focus = true;

                _this[ButtonState.Up] = new Dictionary<bool, ButtonStateStyle>(); 
                _this[ButtonState.Up][false] = new ButtonStateStyle();
                _this[ButtonState.Up][false].State = ButtonState.Up;
                _this[ButtonState.Up][false].Focus = false;
                _this[ButtonState.Up][true] = new ButtonStateStyle();
                _this[ButtonState.Up][true].State = ButtonState.Up;
                _this[ButtonState.Up][true].Focus = true;
           }

            public ButtonStateStyle this[ButtonState state, bool focus]
            {
                get { return _this[state][focus]; }
                private set { _this[state][focus] = value; }
            }
        }

        // IEnumerable

        public IEnumerator<ButtonStateStyle> GetEnumerator()
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
