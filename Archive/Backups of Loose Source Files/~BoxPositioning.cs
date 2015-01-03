//
//  Circle.Controls.Positioning.Box
//
//      Author: Jan-Joost van Zon
//      Date: 23-05-2011 - 23-05-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Conditions;
using Circle.Graphics.Objects;
using Circle.Code.Objects;

namespace Circle.Controls.Positioning
{
    /// <summary>
    /// I probably do not need this class.
    /// </summary>
    public class BoxPositioning
    {
        private NotNullable<BoxOutlining> _outlining = new NotNullable<BoxOutlining>();
        /// <summary> Not nullable </summary>
        public BoxOutlining Outlining
        {
            get { return _outlining; }
            set { _outlining = value; }
        }

        private NotNullable<RectangleD> _position = new NotNullable<RectangleD>();
        /// <summary> Not nullable </summary>
        public RectangleD Position
        {
            get { return _position; }
            set { _position = value; }
        }

        // TODO: more implementation

        // OuterWidth
        // InnerWidth

        // More settable widths?
    }
}
