using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.QuestionAndAnswer.Import
{
    public interface IImporter
    {
        void Execute(string filePath, Action<string> progressCallback = null, Func<bool> isCancelledCallback = null);
        void Execute(Stream stream, Action<string> progressCallback = null, Func<bool> isCancelledCallback = null);
    }
}
