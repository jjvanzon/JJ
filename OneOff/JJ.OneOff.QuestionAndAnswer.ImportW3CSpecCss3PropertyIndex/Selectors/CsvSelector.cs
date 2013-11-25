using JJ.Framework.Common;
using JJ.Framework.IO;
using JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3PropertyIndex.Selectors
{
    public class CsvSelector : ISelector
    {
        public IEnumerable<ImportModel> GetSelection(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            using (CsvReader reader = new CsvReader(stream))
            {
                // Skip header.
                reader.Read();

                while (reader.Read())
                {
                    ImportModel model = CreateImportModel(reader);

                    yield return model;
                }
            }
        }

        private ImportModel CreateImportModel(CsvReader reader)
        {
            return new ImportModel
            {
                Name = reader[0],
                Values = reader[1],
                InitialValue = reader[2],
                AppliesTo = reader[3],
                Inherited = reader[4],
                Percentages = reader[5],
                Media = reader[6],
            };
        }
    }
}
