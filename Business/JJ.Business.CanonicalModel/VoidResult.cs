using System.Collections.Generic;

namespace JJ.Business.CanonicalModel
{
    public class VoidResult : IResult
    {
        public bool Successful { get; set; }
        public IList<Message> Messages { get; set; }
    }
}
