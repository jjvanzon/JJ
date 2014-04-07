using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Models.Canonical
{
    public class Result
    {
        public bool Successful { get; set; }
        public List<ValidationMessage> ValidationMessages { get; set; }
    }
}
