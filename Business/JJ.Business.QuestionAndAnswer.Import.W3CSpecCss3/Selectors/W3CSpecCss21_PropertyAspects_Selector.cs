using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Text.RegularExpressions;
using JJ.Framework.Common;
using JJ.Framework.Xml;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Converters;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;
using JJ.Business.QuestionAndAnswer.Import;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Selectors
{
    public class W3CSpecCss21_PropertyAspects_Selector : ISelector<W3CSpecCss21_PropertyAspects_ImportModel>
    {
        public IEnumerable<W3CSpecCss21_PropertyAspects_ImportModel> GetSelection(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            var streamReader = new StreamReader(stream);

            string html = streamReader.ReadToEnd();
            string xml = HtmlToXmlConverter.Convert(html);

            var doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in GetRecords(doc))
            {
                W3CSpecCss21_PropertyAspects_ImportModel importModel = CreatePropertyDefinitionModel(node);
                yield return importModel;
            }
        }

        private IEnumerable<XmlNode> GetRecords(XmlDocument doc)
        {
            string xpath = "//div[@class='propdef']/dl";
            XmlNodeList nodes = doc.SelectNodes(xpath);
            return nodes.OfType<XmlNode>();
        }

        private W3CSpecCss21_PropertyAspects_ImportModel CreatePropertyDefinitionModel(XmlNode node)
        {
            return new W3CSpecCss21_PropertyAspects_ImportModel
            {
                HashTag = GetHashTag(node),

                Name = GetName(node),
                Value = GetValue(node),
                Initial = GetInitial(node),
                AppliesTo = GetAppliesTo(node),
                Inherited = GetInherited(node),
                Percentages = GetPercentages(node),
                Media = GetMedia(node),
                ComputedValue = GetComputedValue(node),

                NameLinks = GetNameLinks(node).ToList(),
                ValueLinks = GetValueLinks(node).ToList(),
                InitialLinks = GetInitialLinks(node).ToList(),
                AppliesToLinks = GetAppliesToLinks(node).ToList(),
                InheritedLinks = GetInheritedLinks(node).ToList(),
                PercentagesLinks = GetPercentagesLinks(node).ToList(),
                MediaLinks = GetMediaLinks(node).ToList(),
                ComputedValueLinks = GetComputedValueLinks(node).ToList()
            };
        }

        private string GetHashTag(XmlNode node)
        {
            string xpath = "descendant::a[1]/@name";
            XmlNode node2 = XmlHelper.SelectNode(node, xpath);
            return node2.Value;
        }

        private string GetName(XmlNode node)
        {
            string xpath = "dt";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetValue(XmlNode node)
        {
            string xpath = "descendant::tr[1]/td[2]";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetInitial(XmlNode node)
        {
            string xpath = "descendant::tr[2]/td[2]";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetAppliesTo(XmlNode node)
        {
            string xpath = "descendant::tr[3]/td[2]";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetInherited(XmlNode node)
        {
            string xpath = "descendant::tr[4]/td[2]";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetPercentages(XmlNode node)
        {
            string xpath = "descendant::tr[5]/td[2]";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetMedia(XmlNode node)
        {
            string xpath = "descendant::tr[6]/td[2]";
            string value = SelectText(node, xpath);
            return value;
        }

        private string GetComputedValue(XmlNode node)
        {
            string xpath = "descendant::tr[7]/td[2]";
            string value = SelectText(node, xpath);
            return value;
        }

        private IEnumerable<LinkModel> GetNameLinks(XmlNode node)
        {
            return GetLinks(node, "dt/descendant::a[@href]");
        }

        private IEnumerable<LinkModel> GetValueLinks(XmlNode node)
        {
            return GetLinks(node, "descendant::tr[1]/td[2]/descendant::a[@href]");
        }

        private IEnumerable<LinkModel> GetInitialLinks(XmlNode node)
        {
            return GetLinks(node, "descendant::tr[2]/td[2]/descendant::a[@href]");
        }

        private IEnumerable<LinkModel> GetAppliesToLinks(XmlNode node)
        {
            return GetLinks(node, "descendant::tr[3]/td[2]/descendant::a[@href]");
        }

        private IEnumerable<LinkModel> GetInheritedLinks(XmlNode node)
        {
            return GetLinks(node, "descendant::tr[4]/td[2]/descendant::a[@href]");
        }

        private IEnumerable<LinkModel> GetPercentagesLinks(XmlNode node)
        {
            return GetLinks(node, "descendant::tr[5]/td[2]/descendant::a[@href]");
        }

        private IEnumerable<LinkModel> GetMediaLinks(XmlNode node)
        {
            return GetLinks(node, "descendant::tr[6]/td[2]/descendant::a[@href]");
        }

        private IEnumerable<LinkModel> GetComputedValueLinks(XmlNode node)
        {
            return GetLinks(node, "descendant::tr[7]/td[2]/descendant::a[@href]");
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
