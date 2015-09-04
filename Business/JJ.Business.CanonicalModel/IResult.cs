using System.Collections.Generic;

namespace JJ.Business.CanonicalModel
{
    public interface IResult
    {
        bool Successful { get; set; }
        IList<Message> Messages { get; set; }
    }
}
