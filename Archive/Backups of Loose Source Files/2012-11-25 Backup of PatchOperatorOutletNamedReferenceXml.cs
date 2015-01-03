//
//  Circle.AppsAndMedia.Sound.Xml.PatchOperatorOutletNamedReferenceXml
//
//      Author: Jan-Joost van Zon
//      Date: 2012-03-12 - 2012-03-12
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Circle.Framework.Code.Conditions;

namespace Circle.AppsAndMedia.Sound.Xml
{
    /// <summary>
    /// Used by DocumentXml so that its Outlets can reference an Operator Outlet out of a Patch.
    /// </summary>
    public class PatchOperatorOutletNamedReferenceXml
    {
        private PatchOperatorOutletReferenceXml Base = new PatchOperatorOutletReferenceXml();

        [XmlAttribute("name")]
        public string Name;

        public string Outlet
        {
            get { return Base.Outlet; }
            set { Base.Outlet = value; }
        }

        public string Patch
        {
            get { return Base.Patch; }
            set { Base.Patch = value; }
        }

        public void SerializeFrom(KeyValuePair<string, Outlet> entry, Document document)
        {
            Name = entry.Key;
            Base.SerializeFrom(entry.Value, document);
        }

        public KeyValuePair<string, Outlet> Deserialize(Document document)
        {
            Outlet outlet = Base.Deserialize(document);
            var entry = new KeyValuePair<string, Outlet>(Name, outlet);
            return entry;
        }
    }
}
