//
//  Circle.Integration.DotNet.ObjectRepositoryOlder
//
//      Author: Jan-Joost van Zon
//      Date: 2011-05-09 - 2011-05-09
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Core;

namespace Circle.Integration.DotNet
{
    /// <summary>
    /// This class will be replaced.
    /// It registers objects by just object,
    /// not by type and object.
    /// Since Circle needs the base of an object to be a separate object,
    /// You have to create a separate object adapter for the same .NET object.
    /// And register it under both object and type.
    /// </summary>
    public class ObjectRepositoryOlder : AdapterBase
    {
        private readonly Dictionary<object, IReference> Dictionary = new Dictionary<object, IReference>();

        public bool Contains(object dotNetObject)
        {
            return Dictionary.ContainsKey(dotNetObject);
        }

        public ObjectAdapter Get(object dotNetObject)
        {
            if (!Dictionary.ContainsKey(dotNetObject)) return null;

            return (ObjectAdapter)Dictionary[dotNetObject];
        }

        public virtual ObjectAdapter this[object dotNetObject]
        {
            get { return Get(dotNetObject); }
        }

        public ObjectAdapter Add(object dotNetObject)
        {
            ObjectAdapter objectAdapter = new ObjectAdapter() { DotNetObject = dotNetObject, DotNet = DotNet };
            objectAdapter.Convert(); // Added 2011-05-10
            Add(objectAdapter);
            return objectAdapter;
        }

        public void Add(ObjectAdapter objectAdapter)
        {
            Dictionary.Add(objectAdapter.DotNetObject, objectAdapter);
        }

        // Core

        public override string Name
        {
            get { return "Objects"; }
            set { throw new NotSupportedException(); }
        }

        public override bool IsList
        {
            get { return true; }
            set { throw new NotSupportedException(); }
        }

        public override List<IReference> References
        {
            get { return Dictionary.Values.ToList(); }
        }
    }
}
