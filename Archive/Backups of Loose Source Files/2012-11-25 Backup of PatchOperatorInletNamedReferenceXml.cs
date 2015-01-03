//
//  Circle.AppsAndMedia.Sound.Xml.PatchOperatorInletNamedReferenceXml
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
    /// Used by DocumentXml so that its Inlets can reference to an Operator out of a Patch.
    /// </summary>
    public class PatchOperatorInletNamedReferenceXml
    {
        private PatchOperatorInletReferenceXml Base = new PatchOperatorInletReferenceXml();

        [XmlAttribute("name")]
        public string Name;

        public string Inlet
        {
            get { return Base.Inlet; }
            set { Base.Inlet = value; }
        }

        public string Patch
        {
            get { return Base.Patch; }
            set { Base.Patch = value; }
        }

        public void SerializeFrom(KeyValuePair<string, Inlet> entry, Document document)
        {
            Name = entry.Key;
            Base.SerializeFrom(entry.Value, document);
        }

        public KeyValuePair<string, Inlet> Deserialize(Document document)
        {
            Inlet inlet = Base.Deserialize(document);
            var entry = new KeyValuePair<string, Inlet>(Name, inlet);
            return entry;
        }
    }
}
