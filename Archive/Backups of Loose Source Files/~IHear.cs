//
//  Circle.Code.Events.IHear
//
//      Author: Jan-Joost van Zon
//      Date: 30-06-2011 - 30-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circle.Code.Events
{
    public interface IHear
    {
        event Changed<object> Changed;
        object Value { get; set; }
    }
}
