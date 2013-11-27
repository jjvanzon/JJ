using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JJ.Apps.QuestionAndAnswer.Import.Configuration
{
    public class ImportConfiguration
    {
        [XmlArrayItem("importer")]
        public ImportConfigurationImporter[] Importers { get; set; }
    }
}
