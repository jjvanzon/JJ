//
//  Circle.Integration.DotNet.TypePropertyAdapter_Org
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
using System.Diagnostics;

namespace Circle.Integration.DotNet
{
    [DebuggerDisplay("{Property.Name}")]
    public class TypePropertyAdapter_Org : TypeMemberAdapter
    {
        public PropertyInfo Property;

        public override void Convert()
        {
            Type = Property.PropertyType;

            base.Convert();

            Reference.Name = Property.Name;
            Reference.IsPublic = Property.CanRead || Property.CanWrite;
        }
    }
}
