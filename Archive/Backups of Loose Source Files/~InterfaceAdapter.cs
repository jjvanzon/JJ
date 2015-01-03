//
//  Circle.Integration.DotNet.InterfaceAdapter
//
//      Author: Jan-Joost van Zon
//      Date: 2011-03-03 - 2011-03-03
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.CodeBase;

namespace Circle.Integration.DotNet
{
    class InterfaceAdapter : MemberAdapter
    {
        public void Convert()
        {
            base.Convert();

            RelatedItem.Name = Type.Name;
            RelatedItem.IsBase = true;
        }
    }
}
