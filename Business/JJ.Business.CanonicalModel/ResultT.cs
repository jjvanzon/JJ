using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Business.CanonicalModel
{
    public class Result<T> : Result
    {
        public T Data { get; set; }
    }
}
