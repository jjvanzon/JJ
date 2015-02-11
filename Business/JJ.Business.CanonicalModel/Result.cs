using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Business.CanonicalModel
{
    public class Result
    {
        public bool Successful { get; set; }
        public List<ValidationMessage> ValidationMessages { get; set; }
    }
}
