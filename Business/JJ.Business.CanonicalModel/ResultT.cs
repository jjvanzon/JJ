using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Business.CanonicalModel
{
    public class Result<T> : IResult
    {
        public bool Successful { get; set; }
        public IList<Message> Messages { get; set; }
        public T Data { get; set; }
    }
}
