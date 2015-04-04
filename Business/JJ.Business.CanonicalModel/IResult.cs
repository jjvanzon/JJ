using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Business.CanonicalModel
{
    public interface IResult
    {
        bool Successful { get; set; }
        IList<ValidationMessage> ValidationMessages { get; set; }
    }
}
