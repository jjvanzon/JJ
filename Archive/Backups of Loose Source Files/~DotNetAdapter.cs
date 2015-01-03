//
//  Circle.Integration.DotNet.DotNetAdapter
//
//      Author: Jan-Joost van Zon
//      Date: 2011-05-02 - 2011-05-02
//
//  -----

/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Circle.Core;

namespace Circle.Integration.DotNet
{
    public class DotNetAdapter : AdapterBase
    {
        public AppDomain AppDomain;

        public override string Name { get; set; }

        public override bool IsModule
        {
            get { return true; }
            set { throw new NotSupportedException(); }
        }

        public override List<IReference> References
        {
            get
            {
                ConvertChildren();
                return base.References;
            }
        }

        private bool ChildrenConverted;

        private void ConvertChildren()
        {
            if (ChildrenConverted) return;
            ChildrenConverted = true;

            if (AppDomain == null) throw new Exception("AppDomain not specified.");

            foreach (Assembly assembly in AppDomain.GetAssemblies())
            {
                base.References.Add(Domain.Assemblies.Add(assembly));
                //adapter.ConvertTypes();
            }
        }
   }
}*/