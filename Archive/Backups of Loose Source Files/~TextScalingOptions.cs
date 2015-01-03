//
//  Circle.Diagram.Engine.TextScalingOptions
//
//      Author: Jan-Joost van Zon
//      Date: 2010-08-21 - 2011-02-07
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circle.Diagram.Engine
{
    public class TextScalingOptions
    {
        private TextScalingMethod _method;
        public event ParameterlessEvent MethodChanged;
        public TextScalingMethod Method
        {
            get
            {
                return _method;
            }
            set
            {
                if (_method != value)
                {
                    _method = value;
                    if (MethodChanged != null) MethodChanged();
                }
            }
        }

        public ScalingOptionsFixed Fixed;
        public ScalingOptionsLinear Linear;
        public ScalingOptionsModerated Moderated;
    }
}
