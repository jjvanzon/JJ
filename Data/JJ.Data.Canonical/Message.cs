using System.Diagnostics;
using System.Runtime.Serialization;
using JJ.Business.CanonicalModel.Helpers;

namespace JJ.Data.Canonical
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    [DataContract]
    public class Message
    {
        [DataMember]
        public string PropertyKey { get; set; }

        [DataMember]
        public string Text { get; set; }

        private string DebuggerDisplay => DebugHelper.GetDebuggerDisplay(this);
    }
}
