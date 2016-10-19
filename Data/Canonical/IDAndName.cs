using System.Diagnostics;
using System.Runtime.Serialization;
using JJ.Business.CanonicalModel.Helpers;

namespace JJ.Data.Canonical
{
    [DataContract]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class IDAndName
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        private string DebuggerDisplay => DebugHelper.GetDebuggerDisplay(this);
    }
}
