﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace JJ.Framework.Xml.Tests.Mocks
{
    internal class Element_WithAttribute_WithExplicitName
    {
        [XmlAttribute("Attribute_WithExplicitName")]
        public int Attribute_WithExplicitName { get; set; }
    }
}
