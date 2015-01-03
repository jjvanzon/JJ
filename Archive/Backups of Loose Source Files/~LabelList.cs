//
//  Circle.Controls.Classes.LabelList
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
    public class LabelList<LabelType> : ControlList<LabelType>
        where LabelType : Label, new()
    {
        // Constructor

        public LabelList()
        {
            InitializeItemStyle();
            InitializeSyncs();
        }

        // Style

        private NotNull<BoxStyle> ItemStyleNotNull;

        public readonly Events<BoxStyle> ItemStyleEvents = new Events<BoxStyle>();

        /// <summary> Not nullable </summary>
        public BoxStyle ItemStyle
        {
            get { return ItemStyleEvents.Value; }
            set { ItemStyleEvents.Value = value; }
        }

        private void InitializeItemStyle()
        {
            ItemStyleNotNull = new NotNull<BoxStyle>(ItemStyleEvents);
        }

        // Sync

        private Sync<BoxPositioning> SyncPositioning = new Sync<BoxPositioning>();
        private Sync<BoxStyle> SyncItemStyle = new Sync<BoxStyle>();

        private void InitializeSyncs()
        {
            SyncPositioning = new Sync<BoxPositioning>(
                Positioner.ItemPositioningEvents,
                ItemStyle.PositioningEvents
            );

            SyncItemStyle = new Sync<BoxStyle>(
                ItemStyleEvents
            );

            base.Controls.Changed += Controls_Changed;
            base.Controls.Removed += Controls_Removed;
        }

        private void Controls_Changed(LabelType old, LabelType item, int index)
        {
            if (old == null)
            {                
                SyncItemStyle.Items.Add(item.StyleEvents);
            }
        }

        private void Controls_Removed(LabelType item, int index)
        {
            SyncItemStyle.Items.Remove(item.StyleEvents);
        }
    }
}
