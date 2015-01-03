//
//  Circle.Controls.Classes.Draw
//
//      Author: Jan-Joost van Zon
//      Date: 24-05-2011 - 24-05-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circle.Controls.Classes
{
    public class Draw
    {
        internal Draw(Screen screen)
        {
            Screen = screen;
        }

        public Screen Screen { get; private set; }
    }
}
