using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Models.Canonical
{
    public class Result<T> : Result
    {
        public T Data { get; set; }
    }
}
