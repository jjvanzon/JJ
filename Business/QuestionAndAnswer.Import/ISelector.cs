using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Import
{
    public interface ISelector<TModel>
    {
        IEnumerable<TModel> GetSelection(Stream stream);
    }
}
