//
//  Circle.Controls.Classes.DrawTrigger
//
//      Author: Jan-Joost van Zon
//      Date: 28-06-2011 - 28-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Concepts;
using Circle.Code.Conditions;
using Circle.Controls.Style;
using Circle.Graphics.Objects;

namespace Circle.Controls.Classes
{
    public class DrawResponse
    {
        private Response Response = new Response();

        private IControl Control;

        public DrawResponse(IControl control)
        {
            Condition.NotNull(control, "Control");

            Control = control;

            Response.Root = Control;

            Response.Changed += (x, y) => Control.Screen.RequestDraw();  // TODO: again a point at which Control.Screen is considered not nullable.

            Response.Types.Add
            (
                typeof(Label),
                typeof(BoxStyle),
                typeof(BoxPositioning),
                typeof(BoxAspect),
                typeof(BackStyle),
                typeof(LineStyle),
                typeof(TextStyle)
            );

            Response.Members.Add
            (
                "Value*",
                "*Positioning*",
                "*Style*",
                "Top*",
                "Right*",
                "Bottom*",
                "Left*",
                "Padding*",
                "Color*",
                "Dash*",
                "Width*",
                "ZIndex*",
                "Abbreviate*",
                "Font*",
                "*Alignment*",
                "Wrap*"
            );

            Response.Set();
        }

        ~DrawResponse()
        {
            Response.Annull();
        }
    }
}
