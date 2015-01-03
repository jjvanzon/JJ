////
////  Circle.AppsAndMedia.Sound.Xml.DocumentOperatorXml_Original
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

//namespace Circle.AppsAndMedia.Sound.Xml
//{
//    public class DocumentOperatorXml_Original : OperatorXml
//    {
//        [XmlElement("Document")]
//        public DocumentReferenceXml DocumentReferenceXml;

//        [XmlElement("Operand")]
//        public List<OperatorNamedReferenceXml> OperandXmls = new List<OperatorNamedReferenceXml>();

//        public override void SerializeFrom(Operator op)
//        {
//            base.SerializeFrom(op);

//            var documentOperator = (DocumentOperator)op;

//            DocumentReferenceXml = new DocumentReferenceXml();
//            DocumentReferenceXml.SerializeFrom(documentOperator.Document, op.Patch.Document.Workspace);

//            foreach (KeyValuePair<string, Operator> operandEntry in documentOperator.Operands)
//            {
//                var operandXml = new \();
//                OperandXmls.Add(operandXml);
//                operandXml.SerializeFrom(operandEntry, op.Patch);
//            }
//        }

//        public override void DeserializeReferencesTo(Operator op)
//        {
//            var documentOperator = (DocumentOperator)op;

//            if (DocumentReferenceXml != null)
//            {
//                documentOperator.Document = DocumentReferenceXml.Deserialize(documentOperator.Patch.Document);
//            }

//            foreach (OperatorNamedReferenceXml operandXml in OperandXmls)
//            {
//                KeyValuePair<string, Operator> entry = operandXml.Deserialize(op.Patch);
//                documentOperator.Operands.Add(entry.Key, entry.Value);
//            }
//        }
//    }
//}
