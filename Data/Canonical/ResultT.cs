using System.Collections.Generic;

namespace JJ.Data.Canonical
{
    public class Result<T> : IResult
    {
        public bool Successful { get; set; }
        public IList<Message> Messages { get; set; }
        public T Data { get; set; }
    }
}
