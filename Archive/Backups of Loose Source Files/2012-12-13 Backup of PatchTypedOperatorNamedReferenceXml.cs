//
//  Circle.AppsAndMedia.Sound.Xml.PatchTypedOperatorNamedReferenceXml
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
using Circle.Framework.Code.Conditions;

namespace Circle.AppsAndMedia.Sound.Xml
{
    public class PatchTypedOperatorNamedReferenceXml<T> : PatchTypedOperatorReferenceXml<T>
        where T : Operator
    {
        [XmlAttribute("name")]
        public string Name;

        public string Patch;

        public string Operator;

        public void SerializeFrom(string name, T op, Document document)
        {
            Name = name;
            base.SerializeFrom(op, document);
        }

        public void Deserialize(Document document, out string name, out T op)
        {
            Condition.NotNullOrEmpty(Name, "name");
            name = Name;
            base.Deserialize(document, out op);
        }
    }
}
