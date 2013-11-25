using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using JJ.Framework.Common;
using JJ.Framework.Xml;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss21BoxModel.Models;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss21BoxModel.Selectors
{
    public class DefinitionSelector
    {
        private class Record
        {
            public XmlNode HTag { get; set; }
            public XmlNode DlTag { get; set; }
            public XmlNode DtTag { get; set; }
            public XmlNode DdTag { get; set; }
        }

        public IEnumerable<DefinitionModel> GetSelection(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            var streamReader = new StreamReader(stream);

            string html = streamReader.ReadToEnd();
            string xml = HtmlToXmlConverter.Convert(html);

            var doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (Record record in GetRecords(doc))
            {
                DefinitionModel model = CreateDefinitionModel(record);
                yield return model;
            }
        }

        private IEnumerable<Record> GetRecords(XmlDocument doc)
        {
            foreach (XmlNode dlTag in GetDlTags(doc))
            {
                XmlNode hTag = GetHTag(dlTag);
                foreach (XmlNode dtTag in GetDtTags(dlTag))
                {
                    XmlNode ddTag = GetDdTag(dtTag);

                    yield return new Record
                    {
                        HTag = hTag,
                        DlTag = dlTag,
                        DtTag = dtTag,
                        DdTag = ddTag
                    };
                }
            }
        }

        private IEnumerable<XmlNode> GetDlTags(XmlDocument doc)
        {
            string xpath = "//dl[not(ancestor::div[@class='propdef'])]";
            XmlNodeList nodes = doc.SelectNodes(xpath);
            return nodes.OfType<XmlNode>();
        }

        private XmlNode GetHTag(XmlNode dlTag)
        {
            string xpath1 = "preceding-sibling::h3[1]";
            XmlNode node1 = XmlHelper.TrySelectNode(dlTag, xpath1);
            if (node1 != null)
            {
                return node1;
            }

            string xpath2 = "preceding-sibling::h2[1]";
            XmlNode node2 = XmlHelper.SelectNode(dlTag, xpath2);
            return node2;
        }

        private IEnumerable<XmlNode> GetDtTags(XmlNode dlTag)
        {
            string xpath = "dt";
            XmlNodeList nodes = dlTag.SelectNodes(xpath);
            return nodes.OfType<XmlNode>();
        }

        private XmlNode GetDdTag(XmlNode dtTag)
        {
            string xpath = "following-sibling::dd[1]"; // [1] is required, because all dt's and dd's in a dl are sibblings.
            XmlNode ddTag = XmlHelper.SelectNode(dtTag, xpath);
            return ddTag;
        }

        private DefinitionModel CreateDefinitionModel(Record record)
        {
            return new DefinitionModel
            {
                HashTag = GetHashTag(record),
                HashTagLinkText = GetHashTagLinkText(record),
                Context = GetContext(record),
                Term = GetTerm(record),
                Meaning = GetMeaning(record),
                ContextLinks = GetContextLinks(record).ToList(),
                TermLinks = GetTermLinks(record).ToList(),
                MeaningLinks = GetMeaningLinks(record).ToList()
            };
        }

        private string GetHashTag(Record record)
        {
            string xpath = "descendant::a[1]/@name";
            XmlNode node = XmlHelper.SelectNode(record.HTag, xpath);
            return node.Value;
        }

        private string GetHashTagLinkText(Record record)
        {
            // The context happens to be the same as the hash tag link text.
            return GetContext(record);
        }

        private string GetContext(Record record)
        {
            string value = GetText(record.HTag);

            Regex regex = new Regex(@"([^0-9\. ]\w.*)");
            Match match = regex.Match(value);
            if (match == null)
            {
                throw new Exception(String.Format("Non-word characters on the left could not be cut off from the following text: '{0}'.", value));
            }

            return match.Value;
        }

        private string GetTerm(Record record)
        {
            string value = GetText(record.DtTag);
            return value;
        }

        private string GetMeaning(Record record)
        {
            string value = GetText(record.DdTag);
            return value;
        }

        private IEnumerable<LinkModel> GetContextLinks(Record record)
        {
            return GetLinks(record.HTag, "descendant::a[@href]");
        }

        private IEnumerable<LinkModel> GetTermLinks(Record record)
        {
            return GetLinks(record.DtTag, "descendant::a[@href]");
        }

        private IEnumerable<LinkModel> GetMeaningLinks(Record record)
        {
            return GetLinks(record.DdTag, "descendant::a[@href]");
        }

        private IEnumerable<LinkModel> GetLinks(XmlNode node, string xpath)
        {
            foreach (XmlNode node2 in node.SelectNodes(xpath))
            {
                LinkModel model = CreateLinkModel(node2);
                yield return model;
            }
        }

        private LinkModel CreateLinkModel(XmlNode node)
        {
            var model = new LinkModel
            {
                Description = GetLinkDescription(node),
                Url = GetLinkUrl(node)
            };

            return model;
        }

        private string GetLinkDescription(XmlNode node)
        {
            string text = FormatText(node.InnerText);
            return text;
        }

        private string GetLinkUrl(XmlNode node)
        {
            string xpath = "@href";
            XmlNode node2 = XmlHelper.SelectNode(node, xpath);
            return node2.Value;
        }

        // Helpers

        /// <summary>
        /// Gets the text from the content selected with an XPath from an XmlNode, HTML-decodes it and removes excessive whitespace.
        /// </summary>
        private string SelectText(XmlNode node, string xpath)
        {
            XmlNode node2 = XmlHelper.SelectNode(node, xpath);
            return GetText(node2);
        }

        /// <summary>
        /// Gets the text from an XmlNode, HTML-decodes and removes excessive whitespace.
        /// </summary>
        private string GetText(XmlNode node)
        {
            return FormatText(node.InnerText);
        }

        /// <summary>
        /// HTML-decodes and removes excessive whitespace.
        /// </summary>
        private string FormatText(string text)
        {
            if (text == null)
            {
                return null;
            }

            text = HttpUtility.HtmlDecode(text);
            text = text.RemoveExcessiveWhiteSpace();
            return text;
        }
    }
}
