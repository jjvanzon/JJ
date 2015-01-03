//
//  Circle.Integration.DotNet.TypeFieldAdapter
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
    [DebuggerDisplay("{Field.Name}")]
    public class TypeFieldAdapter_Org : TypeMemberAdapter
    {
        public FieldInfo Field;

        public override void Convert()
        {
            Type = Field.FieldType;

            base.Convert();

            Reference.Name = Field.Name;
            Reference.IsPublic = Field.IsPublic;
        }
    }
}
