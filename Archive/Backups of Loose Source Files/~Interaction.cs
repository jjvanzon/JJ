//
//  Circle.Controls.Classes.Interaction
//
//      Author: Jan-Joost van Zon
//      Date: 24-05-2011 - 24-05-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Controls.Positioning;
using System.Windows.Forms;

namespace Circle.Controls.Classes
{
    /// <summary>
    /// This class manages user interface events.
    /// It is reponsible for figuring out the right response to user interaction.
    /// Depending on the locations, focus and z-order it is figured out what object
    /// should repond to mouse and keyboard events.
    /// It also takes mouse capture into consideration.
    /// </summary>
    public class Interaction
    {
        // Constructor

        internal Interaction(Screen screen)
        {
            Screen = screen;
        }

        // Variables

        public Screen Screen { get; private set; }
        public IControl MouseCapture;

        // Mouse Aids

        private IControl MouseDownControl;
        private IControl MouseMoveControl;
        private IControl MouseUpControl;
        private IControl PreviousMouseUpControl;

        // Methods

        public void MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownControl = HitControl(e.X, e.Y);

            if (MouseDownControl == null) return;

            MouseDownControl.MouseDown(MouseDownControl, e);
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            MouseMoveControl = HitControl(e.X, e.Y);

            if (MouseMoveControl == null) return;

            MouseMoveControl.MouseMove(MouseMoveControl, e);
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            PreviousMouseUpControl = MouseUpControl;

            MouseUpControl = HitControl(e.X, e.Y);

            if (MouseUpControl == null) return;

            MouseUpControl.MouseUp(MouseUpControl, e);
        }

        // TODO: manage Click Timer

        public void Click(object sender, EventArgs e)
        {
            if (MouseUpControl == null) return;

            if (MouseDownControl == MouseUpControl)
            {
                MouseUpControl.Click(MouseDownControl, e);
            }
        }

        public void DoubleClick(object sender, EventArgs e)
        {
            if (MouseUpControl == null) return;

            if (MouseDownControl == PreviousMouseUpControl && PreviousMouseUpControl == MouseUpControl)
            {
                MouseUpControl.DoubleClick(MouseDownControl, e);
            }
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            Screen.Focus.Controls.ForEach(x => x.KeyDown(x, e));
        }

        public void KeyUp(object sender, KeyEventArgs e)
        {
            Screen.Focus.Controls.ForEach(x => x.KeyUp(x, e));
        }

        public IControl HitControl(int x, int y)
        {
            if (MouseCapture != null) return MouseCapture;

            var q = from c in Screen.Controls
                    where c.Enabled && c.Hit(x, y)
                    orderby c.ZOrder
                    select c;

            return q.FirstOrDefault();
        }
    }
}
