using System.Collections.Generic;

namespace JJ.Data.Canonical
{
    public interface IResult
    {
        bool Successful { get; set; }
        IList<Message> Messages { get; set; }
    }
}
