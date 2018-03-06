using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;
using JJ.Framework.Exceptions;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.Xml;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Selectors
{
	public class W3CSpecCss21_PropertyAspects_Selector : ISelector<PropertyAspectsImportModel>
	{
		public IEnumerable<PropertyAspectsImportModel> GetSelection(Stream stream)
		{
			if (stream == null) throw new NullException(() => stream);

			var streamReader = new StreamReader(stream);

			string html = streamReader.ReadToEnd();
			string xml = HtmlToXmlConverter.Convert(html);

			var doc = new XmlDocument();
			doc.LoadXml(xml);

			foreach (XmlNode node in GetRecords(doc))
			{
				PropertyAspectsImportModel importModel = CreatePropertyDefinitionModel(node);
				yield return importModel;
			}
		}

		private IEnumerable<XmlNode> GetRecords(XmlDocument doc)
		{
			string xpath = "//div[@class='propdef']/dl";
			XmlNodeList nodes = doc.SelectNodes(xpath);
			if (nodes == null)
			{
				throw new NullException(() => doc.SelectNodes(xpath));
			}
			return nodes.OfType<XmlNode>();
		}

		private PropertyAspectsImportModel CreatePropertyDefinitionModel(XmlNode node)
		{
			return new PropertyAspectsImportModel
			{
				HashTag = GetHashTag(node),

				PropertyName = GetName(node),
				PossibleValues = GetPossibleValues(node),
				InitialValue = GetInitialValue(node),
				AppliesTo = GetAppliesTo(node),
				IsInherited = GetIsInherited(node),
				Percentages = GetPercentages(node),
				Media = GetMedia(node),
				ComputedValue = GetComputedValue(node),

				NameLinks = GetNameLinks(node).ToList(),
				PossibleValuesLinks = GetPossibleValuesLinks(node).ToList(),
				InitialValueLinks = GetInitialValueLinks(node).ToList(),
				AppliesToLinks = GetAppliesToLinks(node).ToList(),
				IsInheritedLinks = GetIsInheritedLinks(node).ToList(),
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

		private string GetPossibleValues(XmlNode node)
		{
			string xpath = "descendant::tr[1]/td[2]";
			string value = SelectText(node, xpath);
			return value;
		}

		private string GetInitialValue(XmlNode node)
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

		private string GetIsInherited(XmlNode node)
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

		private IEnumerable<LinkModel> GetPossibleValuesLinks(XmlNode node)
		{
			return GetLinks(node, "descendant::tr[1]/td[2]/descendant::a[@href]");
		}

		private IEnumerable<LinkModel> GetInitialValueLinks(XmlNode node)
		{
			return GetLinks(node, "descendant::tr[2]/td[2]/descendant::a[@href]");
		}

		private IEnumerable<LinkModel> GetAppliesToLinks(XmlNode node)
		{
			return GetLinks(node, "descendant::tr[3]/td[2]/descendant::a[@href]");
		}

		private IEnumerable<LinkModel> GetIsInheritedLinks(XmlNode node)
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
			XmlNodeList childNodes = node.SelectNodes(xpath);
			if (childNodes == null)
			{
				throw new NullException(() => node.SelectNodes(xpath));
			}

			foreach (XmlNode node2 in childNodes)
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
