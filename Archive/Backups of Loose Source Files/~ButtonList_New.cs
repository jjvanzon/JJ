//
//  Circle.Controls.Classes.ButtonList
//
//      Author: Jan-Joost van Zon
//      Date: 23-05-2011 - 13-07-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Controls.Positioners;
using Circle.Graphics.Objects;
using Circle.Controls.Style;
using Circle.Code.Conditions;
using Circle.Code.Events;
using Circle.Code.Concepts;
using Circle.Code.Objects;

namespace Circle.Controls.Classes
{
    /// <summary>
    /// ButtonList that uses any Label-derived class.
    /// </summary>
    /// <typeparam name="LabelType"></typeparam>
    public class ButtonList_New<ButtonType> : ControlList<ButtonType>
        where ButtonType : Button, new()
    {
        // Constructor

        public ButtonList_New()
        {
            InitializeButtonStyle();
            InitializeSyncs();
        }

        // Style

        private NotNull<ButtonStyle> ButtonStyleNotNull;

        public readonly Events<ButtonStyle> ButtonStyleEvents = new Events<ButtonStyle>();

        /// <summary> Not nullable </summary>
        public ButtonStyle ButtonStyle
        {
            get { return ButtonStyleEvents.Value; }
            set { ButtonStyleEvents.Value = value; }
        }

        private void InitializeButtonStyle()
        {
            ButtonStyleNotNull = new NotNull<ButtonStyle>(ButtonStyleEvents);
        }

        // Sync

        private Sync<BoxPositioning> SyncPositioning = new Sync<BoxPositioning>();
        private Sync<ButtonStyle> SyncButtonStyle = new Sync<ButtonStyle>();

        private new void InitializeSyncs()
        {
            SyncPositioning = new Sync<BoxPositioning>(
                Positioner.ItemPositioningEvents,
                ButtonStyle[ButtonState.Up, false].Style.PositioningEvents
            );

            SyncButtonStyle = new Sync<ButtonStyle>(
                ButtonStyleEvents
            );

            base.Controls.Changed += Controls_Changed;
            base.Controls.Removed += Controls_Removed;

        }

        private void Controls_Changed(ButtonType old, ButtonType item, int index)
        {
            if (old == null)
            {                
                SyncButtonStyle.Items.Add(item.StyleEvents);
            }
        }

        private void Controls_Removed(ButtonType item, int index)
        {
            SyncButtonStyle.Items.Remove(item.StyleEvents);
        }
    }
}
