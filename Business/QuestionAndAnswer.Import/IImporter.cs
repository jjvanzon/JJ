using System;
using System.IO;

namespace JJ.Business.QuestionAndAnswer.Import
{
    public interface IImporter
    {
        void Execute(string filePath, Action<string> progressCallback = null, Func<bool> isCancelledCallback = null);
        void Execute(Stream stream, Action<string> progressCallback = null, Func<bool> isCancelledCallback = null);
    }
}
