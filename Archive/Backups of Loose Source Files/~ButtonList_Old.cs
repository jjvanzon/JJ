//
//  Circle.Controls.Classes.ButtonList
//
//      Author: Jan-Joost van Zon
//      Date: 23-05-2011 - 19-06-2011
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
            BindButtonsEvents();

            ItemStyle.TextStyle = new TextStyle();
        }

        ~ButtonList()
        {
            UnBindButtonsEvents();
        }

        // Sync

        private Sync<BoxPositioning> SyncPositioning = new Sync<BoxPositioning>();
        private Sync<BoxStyle> SyncItemStyle = new Sync<BoxStyle>();
        private Sync<int> SyncCount = new Sync<int>();

        public void InitializeSyncs()
        {
            SyncPositioning = new Sync<BoxPositioning>(
                Positioner.ItemPositioningEvents,
                ItemStyle.PositioningEvents
            );

            SyncItemStyle = new Sync<BoxStyle>(
                ItemStyleEvents
            );

            SyncCount = new Sync<int>(
                Buttons.CountEvents,
                Positioner.ItemCountEvents
            );

            Positioner.Rectangle = Rectangle;
            Positioner.RectangleEvents.Changed += (x, y) => Positioner.Rectangle = Rectangle;

            Buttons.CountEvents.Changed += (x, y) =>
            {
                if (Screen == null) return;
                Screen.RequestDraw();
            };
        }

        // Positioner

        public readonly PositionerHorizontalEquallySpread Positioner = new PositionerHorizontalEquallySpread();

        // Style

        public readonly HearNotNullable<BoxStyle> ItemStyleEvents = new HearNotNullable<BoxStyle>();
        /// <summary> Not nullable </summary>
        public BoxStyle ItemStyle
        {
            get { return ItemStyleEvents.Value; }
            set { ItemStyleEvents.Value = value; }
        }

        // List

        public readonly ListWithEvents<LabelType> Buttons = new ListWithEvents<LabelType>(); 

        private void BindButtonsEvents()
        {
            Buttons.Added += new Added<LabelType>(Buttons_Added);
            Buttons.Removed += new Removed<LabelType>(Buttons_Removed);
        }

        private void UnBindButtonsEvents()
        {
            Buttons.Added -= new Added<LabelType>(Buttons_Added);
            Buttons.Removed -= new Removed<LabelType>(Buttons_Removed);
        }

        private void Buttons_Added(LabelType item, int index)
        {
            if (item == null) item = new LabelType();
            Buttons[index] = item;
            item.Name = "Item" + Buttons.Count;
            item.Screen = Screen;
            Children.Add(item, index);
            SyncItemStyle.Items.Add(item.StyleEvents);
        }

        private bool Buttons_Removed(LabelType item, int index)
        {
            Children.Remove(item);
            item.Screen = null;
            SyncItemStyle.Items.Remove(item.StyleEvents);
            return true;
        }

        // Draw

        public override void Draw()
        {
            Condition.NotNull(ItemStyle.Positioning, "ItemStyle.Positioning");

            Positioner.Calculate();

            for (int i = 0; i < Buttons.Count; i++)
            {
                PositionItem(i);
            }
        }

        private void PositionItem(int i)
        {
            RectangleD rectA = Buttons[i].Rectangle;
            RectangleD rectB = Positioner.Rectangles[i];
            rectA.X1 = rectB.X1;
            rectA.Y1 = rectB.Y1;
            rectA.X2 = rectB.X2;
            rectA.Y2 = rectB.Y2;
        }
    }
}
