//
//  Circle.Controls.Classes.ButtonList
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
    /// ButtonList that uses any Label-derived class.
    /// </summary>
    /// <typeparam name="LabelType"></typeparam>
    public class ButtonList<LabelType> : Control
        where LabelType : Label, new()
    {
        // Constructor

        public ButtonList()
        {
            InitializeSyncs();

            SyncPositioning.Value = new BoxPositioning();
            ItemStyle.TextStyle = new TextStyle();
        }

        // Positioner

        public readonly PositionerHorizontalEquallySpread Positioner = new PositionerHorizontalEquallySpread();

        // Item Style

        public readonly HearNotNullable<BoxStyle> ItemStyleEvents = new HearNotNullable<BoxStyle>();
        /// <summary> Not nullable </summary>
        public BoxStyle ItemStyle
        {
            get { return ItemStyleEvents.Value; }
            set { ItemStyleEvents.Value = value; }
        }

        // Item Count

        public readonly Hear<int> CountEvents = new Hear<int>();
        public int Count
        {
            get { return CountEvents.Value; }
            set
            {
                Condition.NotBelowZero(value, "Count");
                CountEvents.Value = value;

                Setup();
            }
        }

        // Labels

        public readonly List<LabelType> Labels = new List<LabelType>();

        // Syncs

        private Sync<BoxPositioning> SyncPositioning = new Sync<BoxPositioning>();
        private Sync<BoxStyle> SyncItemStyle = new Sync<BoxStyle>();
        private Sync<int> SyncCount = new Sync<int>();

        public void InitializeSyncs()
        {
            SyncPositioning = new Sync<BoxPositioning>(
                ItemStyle.PositioningEvents,
                Positioner.ItemPositioningEvents
            );

            SyncItemStyle = new Sync<BoxStyle>(
                ItemStyleEvents
            );

            SyncCount = new Sync<int>(
                CountEvents,
                Positioner.ItemCountEvents
            );

            Positioner.Rectangle = Rectangle;
            Positioner.RectangleEvents.Changed += (x, y) => Positioner.Rectangle = Rectangle;

            CountEvents.Changed += (x, y) =>
            {
                if (Screen == null) return;
                Screen.RequestDraw();
            };
        }

        // Setup Labels

        // TODO: addition and removal of labels should be split off 
        // the updating of their positions in order to also supply
        // public Add and Remove methods.

        public override void Setup()
        {
            Condition.NotNull(ItemStyle.Positioning, "ItemStyle.Positioning");

            Positioner.Calculate();

            for (int i = Labels.Count; i < Positioner.Rectangles.Count; i++)
            {
                AddLabel();
            }

            for (int i = Labels.Count - 1 ; i >= Positioner.Rectangles.Count; i--)
            {
                RemoveLabel(i);
            }

            for (int i = 0; i < Labels.Count; i++)
            {
                UpdateLabel(i);
            }
        }

        private void AddLabel()
        {
            LabelType label = new LabelType() { Screen = Screen, Style = ItemStyle };
            Labels.Add(label);
            Children.Add(label);
            label.Style = ItemStyle; // TODO: Let SyncItemStyle respond to Sync.Items.Added and let Sync set the ItemStyle instead of doing it manually here.
            SyncItemStyle.Items.Add(label.StyleEvents);
        }

        private void RemoveLabel(int i)
        {
            LabelType label = Labels[i];
            Labels.Remove(label);
            Children.Remove(label);
            label.Screen = null;
            SyncItemStyle.Items.Remove(label.StyleEvents);
        }

        private void UpdateLabel(int i)
        {
            RectangleD rectA = Labels[i].Rectangle;
            RectangleD rectB = Positioner.Rectangles[i];
            rectA.X1 = rectB.X1;
            rectA.Y1 = rectB.Y1;
            rectA.X2 = rectB.X2;
            rectA.Y2 = rectB.Y2;
        }
    }
}
