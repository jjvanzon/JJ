﻿using JJ.Framework.Presentation.Svg.Helpers;

namespace JJ.Framework.Presentation.Svg.Models.Styling
{
    public class BackStyle
    {
        public BackStyle()
        {
            Visible = true;
            Color = ColorHelper.White;
        }

        public bool Visible { get; set; }
        public int Color { get; set; }
    }
}
