using JJ.Business.QuestionAndAnswer.Import;
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
    public class W3CSpecCss3_PropertyIndex_CsvSelector : ISelector<W3CSpecCss3_PropertyIndex_ImportModel>
    {
        public IEnumerable<W3CSpecCss3_PropertyIndex_ImportModel> GetSelection(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            using (CsvReader reader = new CsvReader(stream))
            {
                // Skip header.
                reader.Read();

                while (reader.Read())
                {
                    W3CSpecCss3_PropertyIndex_ImportModel model = CreateImportModel(reader);

                    yield return model;
                }
            }
        }

        private W3CSpecCss3_PropertyIndex_ImportModel CreateImportModel(CsvReader reader)
        {
            return new W3CSpecCss3_PropertyIndex_ImportModel
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
