using JJ.Framework.Common;
using JJ.Framework.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex
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

                // Make sure <br /> comes back as whitespace of InnerText.
                xml = xml.Replace("<br />", " ");

                // Trick to keep 'plural' properties separated.
                xml = xml.Replace("<code>", "<code> ");

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
            string xpath = "//table[@class='proptable']/tbody/tr";

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
                Name = GetName(node),
                Values = GetValues(node),
                InitialValue = GetInitialValue(node),
                AppliesTo = GetAppliesTo(node),
                Inherited = GetInherited(node),
                Percentages = GetPercentages(node),
                Media = GetMedia(node),
                
                InitialValueLinks = GetInitialValueLinks(node).ToList(),
                AppliesToLinks = GetAppliesToLinks(node).ToList(),
                InheritedLinks = GetInheritedLinks(node).ToList(),
                PercentagesLinks = GetPercentagesLinks(node).ToList(),
                MediaLinks = GetMediaLinks(node).ToList()
            };

            model.ValuesLinks = GetValuesLinks(node).ToList();
            model.NameLinks = GetNameLinks(node).ToList();

            return model;
        }

        private string GetName(XmlNode node)
        {
            string xpath = "th";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetValues(XmlNode node)
        {
            string xpath = "td[1]";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetInitialValue(XmlNode node)
        {
            string xpath = "td[2]";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetAppliesTo(XmlNode node)
        {
            string xpath = "td[3]";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetInherited(XmlNode node)
        {
            string xpath = "td[4]";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetPercentages(XmlNode node)
        {
            string xpath = "td[5]";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetMedia(XmlNode node)
        {
            string xpath = "td[6]";
            string value = SelectText(node, xpath);
            return value;
        }

        private IEnumerable<LinkModel> GetNameLinks(XmlNode node)
        {
            return GetLinks(node, "th/a");
        }

        private IEnumerable<LinkModel> GetValuesLinks(XmlNode node)
        {
            return GetLinks(node, "td[1]/a");
        }

        private IEnumerable<LinkModel> GetInitialValueLinks(XmlNode node)
        {
            return GetLinks(node, "td[2]/a");
        }

        private IEnumerable<LinkModel> GetAppliesToLinks(XmlNode node)
        {
            return GetLinks(node, "td[3]/a");
        }

        private IEnumerable<LinkModel> GetInheritedLinks(XmlNode node)
        {
            return GetLinks(node, "td[4]/a");
        }

        private IEnumerable<LinkModel> GetPercentagesLinks(XmlNode node)
        {
            return GetLinks(node, "td[5]/a");
        }

        private IEnumerable<LinkModel> GetMediaLinks(XmlNode node)
        {
            return GetLinks(node, "td[6]/a");
        }

        private IEnumerable<LinkModel> GetLinks(XmlNode node, string xpath)
        {
            foreach (XmlNode node2 in node.SelectNodes(xpath))
            {
                LinkModel model = GetLink(node2);
                yield return model;
            }
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
