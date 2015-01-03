//
//  Circle.AppsAndMedia.Sound.Xml.PatchOperatorInletReferenceXml
//
//      Author: Jan-Joost van Zon
//      Date: 2011-10-31 - 2011-11-09
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Circle.AppsAndMedia.Sound.Xml
{
    public class PatchOperatorInletReferenceXml
    {
        public string Patch;

        public string Inlet;

        public void SerializeFrom(Inlet inlet, Document document)
        {
            if (inlet == null)
            {
                return;
            }

            if (inlet.Operator == null) throw new Exception(String.Format("Inlet named '{0}' is not part of an Operator.", inlet.Name));
            if (inlet.Operator.Patch == null) throw new Exception(String.Format("Operator named '{0}' is not part of a Patch.", inlet.Operator.Name));
            if (inlet.Operator.Patch.Document != document) throw new Exception(String.Format("Patch named '{0}' is not part of the Document.", inlet.Operator.Patch.Name));

            Patch = ReferenceHelper.WriteReference(inlet.Operator.Patch, () => inlet.Operator.Patch.Document.Patches);

            string operatorReference = ReferenceHelper.WriteReference(inlet.Operator, () => inlet.Operator.Patch.Operators);

            if (inlet.Operator.Inlets.Count == 1)
            {
                // One inlet: operator reference is enough as a reference to its inlet.
                Inlet = operatorReference;
            }
            else
            {
                // More than one inlet: an inlet reference must be used, qualified with the operator reference.
                string inletReference = ReferenceHelper.WriteReference(inlet, () => inlet.Operator.Inlets);
                Inlet = operatorReference + ": " + inletReference;
            }
        }

        public Inlet Deserialize(Document document)
        {
            if (Inlet == null || Patch == null)
            {
                return null;
            }
            else
            {
                Patch patch = ReferenceHelper.ReadReference(Patch, () => document.Patches);
                Inlet inlet = TryReadInletReferenceByOperatorReference(patch);
                if (inlet == null) inlet = TryReadInletReferenceByOperatorAndInletReference(patch);
                if (inlet == null) throw new Exception(String.Format("Inlet '{0}' not found or ambiguously defined.", Inlet));
                return inlet;
            }
        }

        private Inlet TryReadInletReferenceByOperatorReference(Patch patch)
        {
            Operator op = ReferenceHelper.ReadReference(Inlet, () => patch.Operators);

            if (op.Inlets.Count == 1)
            {
                return op.Inlets[0];
            }

            return null;
        }

        private Inlet TryReadInletReferenceByOperatorAndInletReference(Patch patch)
        {
            return ReferenceHelper.TryReadQualifierReference(Inlet, () => patch.Operators, (op) => op.Inlets);
        }
    }
}
