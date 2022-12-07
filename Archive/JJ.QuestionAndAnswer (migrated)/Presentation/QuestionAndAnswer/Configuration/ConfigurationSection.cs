using System.Collections.Generic;
using System.Xml.Serialization;

namespace JJ.Presentation.QuestionAndAnswer.Configuration
{
    public class ConfigurationSection
    {
        [XmlAttribute]
        public int PageSize { get; set; }

        [XmlAttribute]
        public int MaxVisiblePageNumbers { get; set; }

        public IList<string> AvailableCultureCodes { get; set; }
    }
}