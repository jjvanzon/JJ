//
//  Circle.Integration.DotNet.RepositoryTypeRepository
//
//      Author: Jan-Joost van Zon
//      Date: 2011-03-01 - 2011-03-01
//
//  -----
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circle.Integration.DotNet
{
    // TODO: might use again when giving user better ability to search a type by name.
    /*public class RepositoryTypeRepository : TypeRepository
    {
        public override void Add(TypeAdapter typeAdapter)
        {
            base.Add(typeAdapter);
            typeAdapter.DotNet = DotNet;
        }

        public override TypeAdapter Get(string fullName)
        {
            TypeAdapter typeAdapter;

            // Try to get Type from dictionary
            typeAdapter = base.Get(fullName);
            if (typeAdapter != null) return typeAdapter;

            // Otherwise, get Type from assemblies, add it and return it.
            foreach (AssemblyAdapter assemblyAdapter in DotNet.Repository.Assemblies.References)
            {
                typeAdapter = assemblyAdapter.Types.Get(fullName);
                if (typeAdapter != null)
                {
                    base.Add(typeAdapter);
                    return typeAdapter;
                }
            }

            return null;
        }
    }*/
}
