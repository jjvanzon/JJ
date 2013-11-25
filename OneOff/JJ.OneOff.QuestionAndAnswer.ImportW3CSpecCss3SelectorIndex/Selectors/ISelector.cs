using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex.Selectors
{
    public interface ISelector
    {
        IEnumerable<ImportModel> GetSelection(Stream stream);
    }
}
