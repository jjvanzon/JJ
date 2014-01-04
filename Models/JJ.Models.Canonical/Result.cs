using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.Canonical
{
    // TODO: This class is not used yet, so may not be finished.

    public class Result
    {
        public bool Successful { get; set; }
        public List<ValidationMessage> ValidationMessages { get; set; }
    }
}
