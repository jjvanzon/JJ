using System.Xml.Serialization;

namespace JJ.Presentation.QuestionAndAnswer.Import.Configuration
{
	public class ImportConfiguration
	{
		[XmlArrayItem("importer")]
		public ImportConfigurationImporter[] Importers { get; set; }
	}
}
