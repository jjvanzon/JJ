//
//  Circle.Diagram.Engine.BorderScalingOptions
//
//      Author: Jan-Joost van Zon
//      Date: 2010-08-27 - 2010-09-03
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circle.Diagram.Engine
{
    public class BorderScalingOptions
    {
        public static BorderScalingMethod DefaultMethod = BorderScalingMethod.Moderated;
        public BorderScalingMethod Method = DefaultMethod;
        public ScalingOptionsFixed Fixed = new ScalingOptionsFixed();
        public ScalingOptionsLinear Linear = new ScalingOptionsLinear();
        public ScalingOptionsModerated Moderated = new ScalingOptionsModerated();
        public ByLayerScalingOptions ByLayer = new ByLayerScalingOptions();
    }
}
