//
//  Circle.Integration.DotNet.NestedTypeAdapter
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
using System.Reflection;

namespace Circle.Integration.DotNet
{
    public class NestedTypeAdapter : MemberAdapter
    {
        public override void Convert()
        {
            base.Convert();

            RelatedItem.Name = Type.Name;
            RelatedItem.IsPublic = Type.IsPublic;
        }
    }
}
