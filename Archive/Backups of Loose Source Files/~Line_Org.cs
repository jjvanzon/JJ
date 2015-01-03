//
//  Circle.Graphics.Objects.Line
//
//      Author: Jan-Joost van Zon
//      Date: 23-05-2011 - 01-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Conditions;
using Circle.Code.Objects;

namespace Circle.Graphics.Objects
{
    public class Line_Org
    {
        private NotNullable<PointD> _pointA = new NotNullable<PointD>();
        /// <summary> Not nullable </summary>
        public PointD PointA
        {
            get { return _pointA.Value; }
            set { _pointA.Value = value; }
        }

        private NotNullable<PointD> _pointB = new NotNullable<PointD>();
        /// <summary> Not nullable </summary>
        public PointD PointB
        {
            get { return _pointB.Value; }
            set { _pointB.Value = value; }
        }

        private NotNullable<LineStyle> _style = new NotNullable<LineStyle>();
        /// <summary> Not nullable </summary>
        public LineStyle Style
        {
            get { return _style.Value; }
            set { _style.Value = value; }
        }
    }
}
