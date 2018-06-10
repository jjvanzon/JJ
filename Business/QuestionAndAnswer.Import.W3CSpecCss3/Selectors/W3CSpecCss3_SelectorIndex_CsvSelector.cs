using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.IO;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Selectors
{
    [UsedImplicitly]
    public class W3CSpecCss3_SelectorIndex_CsvSelector : ISelector<W3CSpecCss3_SelectorIndex_ImportModel>
    {
        public IEnumerable<W3CSpecCss3_SelectorIndex_ImportModel> GetSelection(Stream stream)
        {
            if (stream == null) throw new NullException(() => stream);

            using (var reader = new CsvReader(stream))
            {
                // Skip header.
                reader.Read();

                while (reader.Read())
                {
                    W3CSpecCss3_SelectorIndex_ImportModel model = CreateImportModel(reader);

                    yield return model;
                }
            }
        }

        private W3CSpecCss3_SelectorIndex_ImportModel CreateImportModel(CsvReader reader)
            => new W3CSpecCss3_SelectorIndex_ImportModel
            {
                Pattern = reader[0],
                Meaning = reader[1],
                DescribedInSection = reader[2],
                FirstDefinedInLevel = reader[3]
            };
    }
}