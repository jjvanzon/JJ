using System.Collections.Generic;

namespace JJ.Data.Canonical
{
    public class VoidResult : IResult
    {
        public bool Successful { get; set; }
        public IList<Message> Messages { get; set; }
    }
}
