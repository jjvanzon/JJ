using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace JJ.Apps.QuestionAndAnswer.Mvc.Configuration
{
    internal class ConfigurationSection
    {
        [XmlAttribute]
        public int PageSize { get; set; }

        [XmlAttribute]
        public int MaxVisiblePageNumbers { get; set; }
    }
}