using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace JJ.Framework.Data.NHibernate
{
    internal class SqlLoggingConfiguration
    {
        [XmlAttribute]
        public bool Enabled { get; set; }

        [XmlAttribute]
        public string FilePath { get; set; }
    }
}
