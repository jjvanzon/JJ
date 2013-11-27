using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;
using JJ.Framework.Common;
using JJ.Framework.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Selectors
{
    public class W3CSpecCss3_SelectorIndex_CsvSelector : ISelector<W3CSpecCss3_SelectorIndex_ImportModel>
    {
        public IEnumerable<W3CSpecCss3_SelectorIndex_ImportModel> GetSelection(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            using (CsvReader reader = new CsvReader(stream))
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
        {
            return new W3CSpecCss3_SelectorIndex_ImportModel
            {
                Pattern = reader[0],
                Meaning = reader[1],
                DescribedInSection = reader[2],
                FirstDefinedInLevel = reader[3]
            };
        }
    }
}
