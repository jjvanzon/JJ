using System.Runtime.Serialization;

namespace JJ.Data.Canonical
{
    [DataContract]
    public class IDAndName
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
