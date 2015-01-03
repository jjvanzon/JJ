//
//  Circle.Controls.Classes.ControlList
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
    /// Visual list of controls.
    /// </summary>
    /// <typeparam name="ControlType"></typeparam>
    public class ControlList<ControlType> : Control
        where ControlType : Control, new()
    {
        // Constructor

        public ControlList()
        {
            InitializeSyncs();
            BindControlsEvents();
        }

        ~ControlList()
        {
            UnBindControlsEvents();
        }

        // Positioner

        public readonly HorizontalEquallySpread Positioner = new HorizontalEquallySpread();

        // List

        /// <summary> Items not nullable </summary>
        public readonly ListWithEvents<ControlType> Controls = new ListWithEvents<ControlType>(); 

        private void BindControlsEvents()
        {
            Controls.Added += Controls_Added;
            Controls.Changed += Controls_Changed;
            Controls.Annulling += Controls_Annulling;
            Controls.Removed += Controls_Removed;
        }

        private void UnBindControlsEvents()
        {
            Controls.Added -= Controls_Added;
            Controls.Changed -= Controls_Changed;
            Controls.Annulling -= Controls_Annulling;
            Controls.Removed -= Controls_Removed;
        }

        // Auto-instantiate

        private void Controls_Added(AddedEventArgs<ControlType> e)
        {
            e.Item = e.Item ?? new ControlType();
        }

        // Auto-configure

        private int NewNumber = 1;

        private void Controls_Changed(ControlType old, ControlType item, int index)
        {
            if (old == null)
            {
                item.Name = "Item" + NewNumber++;
                item.Screen = Screen;
                Children.Add(item, index);
            }
        }

        // Not null

        private bool Controls_Annulling(ControlType item, int index)
        {
            return false;
        }

        // Remove

        private void Controls_Removed(ControlType item, int index)
        {
            Children.Remove(item);
            item.Screen = null;
        }

        // Sync

        private Sync<int> SyncCount = new Sync<int>();

        private void InitializeSyncs()
        {
            SyncCount = new Sync<int>(
                Controls.CountEvents,
                Positioner.ItemCountEvents
            );

            Positioner.Rectangle = Rectangle;
            Positioner.RectangleEvents.Changed += (x, y) => Positioner.Rectangle = Rectangle;

            Controls.CountEvents.Changed += (x, y) =>
            {
                if (Screen == null) return;
                Screen.RequestDraw();
            };
        }

        // Draw

        public override void Draw()
        {
            Condition.NotNull(Positioner.ItemPositioning, "Positioner.ItemPositioning");

            Positioner.Calculate();

            for (int i = 0; i < Controls.Count; i++)
            {
                PositionItem(i);
            }
        }

        private void PositionItem(int i)
        {
            RectangleD rectA = Controls[i].Rectangle;
            RectangleD rectB = Positioner.Rectangles[i];
            rectA.X1 = rectB.X1;
            rectA.Y1 = rectB.Y1;
            rectA.X2 = rectB.X2;
            rectA.Y2 = rectB.Y2;
        }
    }
}
