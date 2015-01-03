//
//  Circle.AppsAndMedia.Sound.Xml.PatchTypedOperatorReferenceXml
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
    public class PatchOutletReferenceXml
    {
        private PatchTypedOperatorReferenceXml<PatchOutlet> Base = new PatchTypedOperatorReferenceXml<PatchOutlet>();

        public string Patch
        {
            get { return Base.Patch; }
            set { Base.Patch = value; }
        }

        public string Outlet
        {
            get { return Base.Operator; }
            set { Base.Operator = value; }
        }

        public virtual void SerializeFrom(PatchOutlet op, Document document)
        {
            Base.SerializeFrom(op, document);
        }

        public virtual PatchOutlet Deserialize(Document document)
        {
            return Base.Deserialize(document);
        }
    }
}
