using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace JJ.Models.Canonical
{
    [DataContract]
    public class ValidationMessage
    {
        [DataMember]
        public string PropertyKey { get; set; }

        [DataMember]
        public string Text { get; set; }
    }
}
