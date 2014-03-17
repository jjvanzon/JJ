using JJ.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JJ.Models.QuestionAndAnswer.Persistence.Tests.Helpers
{
    public class ConfigurationSection
    {
        [XmlAttribute]
        public int ExistingQuestionID { get; set; }

        [XmlArrayItem("persistenceConfiguration")]
        public PersistenceConfiguration[] PersistenceConfigurations { get; set; }
    }
}
