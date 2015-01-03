//
//  Circle.Integration.DotNet.AssemblyTypeDictionary
//
//      Author: Jan-Joost van Zon
//      Date: 2011-03-01 - 2011-03-01
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Circle.Integration.DotNet
{
    // TODO: might use again when giving user better ability to search a type by name.
    /*public class AssemblyTypeRepository : TypeRepository
    {
        public AssemblyAdapter Assembly { get; internal set; }

        public override TypeAdapter Get(string fullName)
        {
            TypeAdapter typeAdapter;

            // Try to get Type from dictionary
            typeAdapter = base.Get(fullName);
            if (typeAdapter != null) return typeAdapter;

            // Otherwise, get Type from assembly, add it and return it.
            Assembly assembly = Assembly.Assembly;
            if (assembly != null)
            {
                Type type = assembly.GetType(fullName);
                if (type != null)
                {
                    typeAdapter = base.Add(type);
                    return typeAdapter;
                }
            }

            return null;
        }
    }*/
}
