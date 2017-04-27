using System.Collections.Generic;
using JetBrains.Annotations;

namespace JJ.Data.Canonical
{
    public class Result<T> : IResult
    {
        public bool Successful { get; set; }
        public IList<Message> Messages { get; set; }

        [CanBeNull]
        public T Data { get; set; }
    }
}
