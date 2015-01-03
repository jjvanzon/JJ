//
//  Circle.Diagram.Engine.IScaleTextHoldersOriginal
//
//      Author: Jan-Joost van Zon
//      Date: 2011-02-07 - 2011-02-07
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Diagram.Entities;

namespace Circle.Diagram.Engine
{
    public interface IScaleTextHoldersOriginal
    {
        List<TextHolder> TextHolders { get; set; }
        bool Active { get; set; }
        void Execute();
    }
}
