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

            ItemStyle.TextStyle = new TextStyle();
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

        // TODO: I do not like this list to be entirely public,
        // without intercepting Add and Remove. IndexOf is used by Test.Form2 by the way.
        // TODO: Make this an interceptable list keep everyting Sync.
        public readonly List<LabelType> Labels = new List<LabelType>();

        public LabelType Add(int index = -1)
        {
            LabelType label = AddLabel(index);
            Count++; 
            return label;
        }

        public void Remove(int index)
        {
            RemoveLabel(index);
            Count--;
        }

        public void Clear()
        {
            Count = 0;
        }

        public readonly Hear<int> CountEvents = new Hear<int>();
        public int Count
        {
            get { return CountEvents.Value; }
            set
            {
                Condition.NotBelowZero(value, "Count");
                if (Count == value) return;
                CountEvents.Value = value;
                ApplyCount();
            }
        }

        private void ApplyCount()
        {
            for (int i = Labels.Count; i < Count; i++)
            {
                AddLabel();
            }

            for (int i = Labels.Count - 1 ; i >= Count; i--)
            {
                RemoveLabel(i);
            }
        }

        private LabelType AddLabel(int index = -1)
        {
            LabelType label = new LabelType() { Screen = Screen };

            if (index == -1)
            {
                Labels.Add(label);
                Children.Add(label);
            }
            else
            {
                Labels.Insert(index, label);
                Children.Add(label, index);
            }

            label.Name = "Label" + Labels.Count;

            SyncItemStyle.Items.Add(label.StyleEvents);

            return label;
        }

        private void RemoveLabel(int index)
        {
            Condition.AboveZero(Labels.Count, "Labels.Count");

            LabelType label;
            if (index != -1)
            {
                label = Labels[index];
            }
            else
            {
                label = Labels.Last();
            }

            Labels.Remove(label);
            Children.Remove(label);
            label.Screen = null;
            SyncItemStyle.Items.Remove(label.StyleEvents);
        }


        // Draw

        public override void Draw()
        {
            Condition.NotNull(ItemStyle.Positioning, "ItemStyle.Positioning");

            Positioner.Calculate();

            for (int i = 0; i < Labels.Count; i++)
            {
                PositionLabel(i);
            }
        }

        private void PositionLabel(int i)
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
