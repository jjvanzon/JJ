using System.Runtime.Serialization;

namespace JJ.Data.Canonical
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string PropertyKey { get; set; }

        [DataMember]
        public string Text { get; set; }
    }
}
