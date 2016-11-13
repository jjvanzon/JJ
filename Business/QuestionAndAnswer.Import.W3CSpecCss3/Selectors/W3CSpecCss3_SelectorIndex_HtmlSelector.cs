﻿using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;
using JJ.Framework.Common;
using JJ.Framework.Reflection.Exceptions;
using JJ.Framework.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Selectors
{
    public class W3CSpecCss3_SelectorIndex_HtmlSelector : ISelector<W3CSpecCss3_SelectorIndex_ImportModel>
    {
        public IEnumerable<W3CSpecCss3_SelectorIndex_ImportModel> GetSelection(Stream stream)
        {
            if (stream == null) throw new NullException(() => stream);

            using (var streamReader = new StreamReader(stream))
            {
                string html = streamReader.ReadToEnd();
                string xml = HtmlToXmlConverter.Convert(html);

                // Make sure <br /> comes back as whitespace of InnerText.
                // Most of all to keep 'plural' css selectors separated.
                xml = xml.Replace("<br />", " ");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                foreach (XmlNode node in GetRecords(doc))
                {
                    W3CSpecCss3_SelectorIndex_ImportModel importModel = CreateImportModel(node);
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

        private W3CSpecCss3_SelectorIndex_ImportModel CreateImportModel(XmlNode node)
        {
            var model = new W3CSpecCss3_SelectorIndex_ImportModel
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
            XmlNode node2 = XmlHelper.SelectNode(node, xpath);
            LinkModel model = CreateLinkModel(node2);
            return model;
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
            string text = ImportHelper.FormatHtmlText(node.InnerText);
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
            string text = ImportHelper.FormatHtmlText(node2.InnerText);
            return text;
        }
    }
}