using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex
{
    public interface ISelector
    {
        IEnumerable<ImportModel> GetSelection(Stream stream);
    }
}
