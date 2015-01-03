//
//  Circle.Integration.DotNet.AnonymousMethodAdapter
//
//      Author: Jan-Joost van Zon
//      Date: 2011-03-03 - 2011-03-03
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Circle.CodeBase;

namespace Circle.Integration.DotNet
{
    class AnonymousMethodAdapter : Adapter
    {
        public Item Item;
        public MethodInfo Method;

        public void Convert()
        {
            Item = new Item();
            Item.IsCommand = true;
            Item.Name = Method.Name;
        }
    }
}
