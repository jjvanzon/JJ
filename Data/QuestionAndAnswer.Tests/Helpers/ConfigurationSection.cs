using JJ.Framework.Data;
using System.Xml.Serialization;

namespace JJ.Data.QuestionAndAnswer.Tests.Helpers
{
	public class ConfigurationSection
	{
		[XmlAttribute]
		public int ExistingQuestionID { get; set; }

		[XmlArrayItem("persistenceConfiguration")]
		public PersistenceConfiguration[] PersistenceConfigurations { get; set; }
	}
}
