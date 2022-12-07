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
        public IEnumerable<DefinitionModel> GetSelection(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            var streamReader = new StreamReader(stream);

            string html = streamReader.ReadToEnd();
            string xml = HtmlToXmlConverter.Convert(html);

            var doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in GetRecords(doc))
            {
                DefinitionModel importModel = CreateDefinitionModel(node);
                yield return importModel;
            }
        }

        private IEnumerable<XmlNode> GetRecords(XmlDocument doc)
        {
            string xpath = "//dl[not(ancestor::div[@class='propdef'])]/dt";
            XmlNodeList nodes = doc.SelectNodes(xpath);
            return nodes.OfType<XmlNode>();
        }

        private DefinitionModel CreateDefinitionModel(XmlNode node)
        {
            return new DefinitionModel
            {
                HashTag = GetHashTag(node),
                Context = GetContext(node),
                Term = GetTerm(node),
                Meaning = GetMeaning(node),
                ContextLinks = GetContextLinks(node).ToList(),
                TermLinks = GetTermLinks(node).ToList(),
                MeaningLinks = GetMeaningLinks(node).ToList()
            };
        }

        private string GetHashTag(XmlNode node)
        {
            string xpath = "parent::dl/preceding-sibling::h2[1]/descendant::a[1]/@name";
            XmlNode node2 = XmlHelper.SelectNode(node, xpath);
            return node2.Value;
        }

        private string GetContext(XmlNode node)
        {
            string xpath = "parent::dl/preceding-sibling::h2[1]";
            string value = SelectText(node, xpath);

            Regex regex = new Regex(@"([^0-9\. ]\w.*)");
            Match match = regex.Match(value);
            if (match == null)
            {
                throw new Exception(String.Format("Preceding non-word characters could not be cut off from the following text: '{0}'.", value));
            }

            return match.Value;
        }

        private string GetTerm(XmlNode node)
        {
            string xpath = "dt";
            string value = FormatText(node.InnerText);
            return value;
        }

        private string GetMeaning(XmlNode node)
        {
            string xpath = "following-sibling::dd[1]";
            string value = SelectText(node, xpath);
            return value;
        }

        private IEnumerable<LinkModel> GetContextLinks(XmlNode node)
        {
            return GetLinks(node, "parent::dl/preceding-sibling::h2/descendant::a[@href]");
        }

        private IEnumerable<LinkModel> GetTermLinks(XmlNode node)
        {
            return GetLinks(node, "descendant::a[@href]");
        }

        private IEnumerable<LinkModel> GetMeaningLinks(XmlNode node)
        {
            return GetLinks(node, "following-sibling::dd/descendant::a[@href]");
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

        private string SelectText(XmlNode node, string xpath)
        {
            XmlNode node2 = XmlHelper.SelectNode(node, xpath);
            string text = FormatText(node2.InnerText);
            return text;
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
