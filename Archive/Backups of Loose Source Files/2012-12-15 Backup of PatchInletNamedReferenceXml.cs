//
//  Circle.AppsAndMedia.Sound.Xml.PatchInletNamedReferenceXml
//
//      Author: Jan-Joost van Zon
//      Date: 2012-11-24 - 2012-11-24
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Circle.AppsAndMedia.Sound.Xml
{
    /// <summary>
    /// Used by DocumentXml so that its Inlets can reference a PatchInlet of a Patch.
    /// </summary>
    public class PatchInletNamedReferenceXml
    {
        private PatchTypedOperatorNamedReferenceXml<PatchInlet> Base = new PatchTypedOperatorNamedReferenceXml<PatchInlet>();

        [XmlAttribute("name")]
        public string Name
        {
            get { return Base.Name; }
            set { Base.Name = value; }
        }

        public string Patch
        {
            get { return Base.Patch; }
            set { Base.Patch = value; }
        }

        public string Operator
        {
            get { return Base.Operator; }
            set { Base.Operator = value; }
        }

        public void SerializeFrom(string name, PatchInlet patchInlet, Document document)
        {
            Base.SerializeFrom(name, patchInlet, document);
        }

        public void Deserialize(Document document, out string name, out PatchInlet patchInlet)
        {
            Base.Deserialize(document, out name, out patchInlet);
        }
    }
}
