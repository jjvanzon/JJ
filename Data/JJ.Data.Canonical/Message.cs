using System.Runtime.Serialization;

namespace JJ.Business.CanonicalModel
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
