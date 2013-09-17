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
        [XmlArrayItem("persistenceContextType")]
        public string[] PersistenceContextTypes { get; set; }
    }
}
