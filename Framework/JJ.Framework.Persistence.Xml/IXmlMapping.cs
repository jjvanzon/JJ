﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Framework.Persistence.Xml
{
    public interface IXmlMapping
    {
        IdentityType IdentityType { get; }
        string IdentityPropertyName { get; }
        string ElementName { get; }
    }
}
