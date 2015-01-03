//
//  Circle.Controls.Classes.ScreenControls
//
//      Author: Jan-Joost van Zon
//      Date: 04-06-2011 - 04-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Conditions;
using System.Collections;

namespace Circle.Controls.Classes
{
    public class ScreenControls : IEnumerable<IControl>
    {
        internal ScreenControls(Screen screen)
        {
            Condition.NotNull(screen, "screen");
            Screen = screen;
        }

        private readonly Screen Screen;

        private readonly List<IControl> List = new List<IControl>();

        public bool Contains(IControl control)
        {
            return List.Contains(control);
        }

        public void Add(IControl control)
        {
            Condition.NotNull(control, "control");
            if (List.Contains(control)) return;
            List.Add(control);
            control.Screen = Screen;
        }

        public void Remove(IControl control)
        {
            Condition.NotNull(control, "control");
            if (!List.Contains(control)) return;
            List.Remove(control);
            control.Screen = null;
        }

        public IEnumerator<IControl> GetEnumerator()
        {
            foreach (var x in List)
            {
                yield return x;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
