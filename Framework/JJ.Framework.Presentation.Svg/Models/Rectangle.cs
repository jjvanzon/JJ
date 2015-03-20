﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Framework.Presentation.Svg.Models
{
    public class Rectangle : ElementBase
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public override float CenterX
        {
            get { return X + Width / 2; }
        }

        public override float CenterY
        {
            get { return Y + Height / 2; }
        }
    }
}
