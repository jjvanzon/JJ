using System.Collections.Generic;
using System.IO;

namespace JJ.Business.QuestionAndAnswer.Import
{
    public interface ISelector<TModel>
    {
        IEnumerable<TModel> GetSelection(Stream stream);
    }
}
