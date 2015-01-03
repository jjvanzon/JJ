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
    public class PatchTypedOperatorReferenceXml<T>
        where T : Operator
    {
        public string Patch;

        public string Operator;

        public void SerializeFrom(T op, Document document)
        {
            if (op == null)
            {
                return;
            }

            if (op.Patch == null) throw new Exception(String.Format("{0} named '{1}' is not part of a Patch.", typeof(T).Name, op.Name));
            if (op.Patch.Document != document) throw new Exception(String.Format("Patch named '{0}' is not part of the Document.", op.Patch.Name));

            Patch = ReferenceHelper.WriteReference(op.Patch, () => op.Patch.Document.Patches);
            Operator = ReferenceHelper.WriteReference(op, () => op.Patch.Operators.OfType<T>());
        }

        public T Deserialize(Document document)
        {
            Patch patch = ReferenceHelper.ReadReference(Patch, () => document.Patches);
            return ReferenceHelper.ReadReference(Operator, () => patch.Operators.OfType<T>());
        }
    }
}
