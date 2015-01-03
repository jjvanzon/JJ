//
//  Circle.Integration.DotNet.ObjectMemberAdapter
//
//      Author: Jan-Joost van Zon
//      Date: 2011-03-12 - 2011-03-12
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.CodeBase;

namespace Circle.Integration.DotNet
{
    public class ObjectMemberAdapter : AdapterBase
    {
        // Input
        public object ParentDotNetObject; 
        public IObj ParentObject; // TODO: anything done with this? If not: remove.

        // Throughput
        public TypeMemberAdapter TypeMemberAdapter;
        //public object Value; // Used for displaying default values for uninstantiated members

        // Output
        public IReference Reference;

        public IObj Object;

        // Command
        public override void Convert()
        {
            Reference.ClassMember = TypeMemberAdapter.Reference;
            ParentObject.References.Add(Reference);

            if (TypeMemberAdapter.Type.IsValueType || TypeMemberAdapter.Type == typeof(string)) // TODO: replace by something that looks at Circle data, instead of .NET data.
            {
                Object = new Obj();
            }
        }
    }
}
