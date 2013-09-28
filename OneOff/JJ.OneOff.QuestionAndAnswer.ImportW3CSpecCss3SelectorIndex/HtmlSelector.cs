using JJ.Framework.Common;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex
{
    public class HtmlSelector : ISelector
    {
        public IEnumerable<ImportModel> GetSelection(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            using (var streamReader = new StreamReader(stream))
            {
                string html = streamReader.ReadToEnd();
                string xml = HtmlToXmlConverter.Convert(html);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                foreach (XmlNode node in GetRecords(doc))
                {
                    ImportModel importModel = CreateImportModel(node);
                    yield return importModel;
                }
            }
        }

        private IEnumerable<XmlNode> GetRecords(XmlDocument doc)
        {
            string xpath = "//table[@class='data']/tbody/tr";

            XmlNodeList list = doc.SelectNodes(xpath);
            foreach (XmlNode node in list)
            {
                yield return node;
            }
        }

        private ImportModel CreateImportModel(XmlNode node)
        {
            var model = new ImportModel
            {
                Pattern = GetPattern(node),
                Meaning = GetMeaning(node),
                DescribedInSection = GetDescribedInSection(node),
                FirstDefinedInLevel = GetFirstDefinedInLevel(node),
                DescribedInSectionLink = GetDescribedInSectionLink(node)
            };

            return model;
        }

        private string GetPattern(XmlNode node)
        {
            string xpath = "th";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetMeaning(XmlNode node)
        {
            string xpath = "td[1]";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetDescribedInSection(XmlNode node)
        {
            string xpath = "td[2]";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetFirstDefinedInLevel(XmlNode node)
        {
            string xpath = "td[3]";
            string value = SelectText(node, xpath);
            return value;
        }

        private LinkModel GetDescribedInSectionLink(XmlNode node)
        {
            string xpath = "td[2]/a";
            XmlNode node2 = SelectNode(node, xpath);
            LinkModel model = GetLink(node2);
            return model;
        }

        private LinkModel GetLink(XmlNode node)
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
            XmlNode node2 = SelectNode(node, xpath);
            return node2.Value;
        }

        // Helpers

        private string SelectText(XmlNode node, string xpath)
        {
            XmlNode node2 = SelectNode(node, xpath);
            string text = FormatText(node2.InnerText);
            return text;
        }

        private XmlNode SelectNode(XmlNode node, string xpath)
        {
            XmlNode node2 = TrySelectNode(node, xpath);

            if (node2 == null)
            {
                throw new Exception(String.Format("{0} not found.", xpath));
            }

            return node2;
        }

        private XmlNode TrySelectNode(XmlNode node, string xpath)
        {
            XmlNodeList nodes = node.SelectNodes(xpath);

            if (nodes.Count > 1)
            {
                throw new Exception(String.Format("{0} was found multiple times.", xpath));
            }

            if (nodes.Count == 0)
            {
                return null;
            }

            XmlNode node2 = nodes[0];

            return node2;
        }

        private string FormatText(string text)
        {
            // TODO: Remove accessive whitespace between words.

            return HttpUtility.HtmlDecode(text);
        }
    }
}
