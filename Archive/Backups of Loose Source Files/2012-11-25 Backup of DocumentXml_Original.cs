////
////  Circle.AppsAndMedia.Sound.xml.DocumentXml_Original
////
////      Author: Jan-Joost van Zon
////      Date: 2011-10-31 - 2011-11-09
////
////  -----

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml.Serialization;
//using Circle.Framework.Code.Conditions;

//namespace Circle.AppsAndMedia.Sound.Xml
//{
//    public class DocumentXml_Original
//    {
//        [XmlAttribute("name")]
//        public string Name;

//        public bool NameSpecified
//        {
//            get { return !String.IsNullOrEmpty(Name); }
//        }

//        [XmlElement("Inlet")]
//        public List<PatchOperatorNamedReferenceXml<InletOperator>> InletReferenceXmls = new List<PatchOperatorNamedReferenceXml<InletOperator>>();

//        [XmlElement("Curve")]
//        public List<CurveXml> CurveXmls = new List<CurveXml>();

//        [XmlElement("Patch")]
//        public List<PatchXml> PatchXmls = new List<PatchXml>();

//        [XmlElement("Outlet")]
//        public List<PatchOperatorNamedReferenceXml<Outlet>> OutletReferenceXmls = new List<PatchOperatorNamedReferenceXml<Outlet>>();

//        /// <summary> Required </summary>
//        [XmlElement("Transport")]
//        public TransportXml TransportXml = new TransportXml();

//        public bool TransportXmlSpecified
//        {
//            get { return TransportXml.Count > 0; }
//        }

//        public void SerializeFrom(Document document)
//        {
//            Name = document.Name;

//            foreach (KeyValuePair<string, InletOperator> entry in document.Inlets)
//            {
//                var inletXml = new PatchOperatorNamedReferenceXml<InletOperator>();
//                inletXml.SerializeFrom(entry, document);
//                InletReferenceXmls.Add(inletXml);
//            }

//            foreach (Curve curve in document.Curves)
//            {
//                var curveXml = new CurveXml();
//                curveXml.SerializeFrom(curve);
//                CurveXmls.Add(curveXml);
//            }

//            foreach (Patch patch in document.Patches)
//            {
//                var patchXml = new PatchXml();
//                patchXml.SerializeFrom(patch);
//                PatchXmls.Add(patchXml);
//            }

//            foreach (KeyValuePair<string, Outlet> entry in document.Outlets)
//            {
//                var outletXml = new PatchOperatorNamedReferenceXml<Outlet>();
//                outletXml.SerializeFrom(entry, document);
//                OutletReferenceXmls.Add(outletXml);
//            }

//            TransportXml = new TransportXml();
//            TransportXml.SerializeFrom(document.Transport);
//        }

//        public Document Deserialize()
//        {
//            var document = new Document();

//            document.Name = Name;

//            foreach (CurveXml curveXml in CurveXmls)
//            {
//                Curve curve = curveXml.Deserialize();
//                document.Curves.Add(curve);
//            }

//            foreach (PatchXml patchXml in PatchXmls)
//            {
//                Patch patch = patchXml.Deserialize();
//                document.Patches.Add(patch);
//            }

//            TransportXml.DeserializeTo(document.Transport);

//            return document;
//        }

//        public void DeserializeReferencesTo(Document document)
//        {
//            Condition.NotNull(document, "document");

//            foreach (PatchOperatorNamedReferenceXml<InletOperator> inletReferenceXml in InletReferenceXmls)
//            {
//                KeyValuePair<string, InletOperator> entry = inletReferenceXml.Deserialize(document);
//                Condition.IsOfType<InletOperator>(entry.Value, String.Format("Document inlet operator '{0}'", entry.Key));
//                document.Inlets[entry.Key] = entry.Value;
//            }

//            for (int i = 0; i < PatchXmls.Count; i++)
//            {
//                PatchXml patchXml = PatchXmls[i];
//                Patch patch = document.Patches[i];
//                patchXml.DeserializeReferencesTo(patch);
//            }

//            TransportXml.DeserializeReferencesTo(document.Transport);

//            foreach (PatchOperatorNamedReferenceXml<Outlet> outletReferenceXml in OutletReferenceXmls)
//            {
//                KeyValuePair<string, Outlet> entry = outletReferenceXml.Deserialize(document);
//                Condition.IsOfType<Outlet>(entry.Value, String.Format("Document outlet operator '{0}'", entry.Key));
//                document.Outlets[entry.Key] = entry.Value;
//            }
//        }
//    }
//}
