﻿using JJ.Framework.Common;
using JJ.Framework.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JJ.OneOff.QuestionAndAnswer.ImportW3CSpecCss3SelectorIndex
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
                Pattern = reader[0],
                Meaning = reader[1],
                DescribedInSection = reader[2],
                FirstDefinedInLevel = reader[3]
            };
        }
    }
}
