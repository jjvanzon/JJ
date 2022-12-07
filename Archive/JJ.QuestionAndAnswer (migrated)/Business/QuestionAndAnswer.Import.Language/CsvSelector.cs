using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using JJ.Framework.Exceptions.Basic;
using JJ.Framework.IO;

namespace JJ.Business.QuestionAndAnswer.Import.Language
{
    [UsedImplicitly]
    internal class CsvSelector : ISelector<Model>
    {
        public IEnumerable<Model> GetSelection(Stream stream)
        {
            if (stream == null) throw new NullException(() => stream);

            using (var reader = new CsvReader(stream))
            {
                // Skip header
                reader.Read();

                while (reader.Read())
                {
                    Model model = AssertAndCreateImportModel(reader);
                    yield return model;
                }
            }
        }

        private Model AssertAndCreateImportModel(CsvReader reader)
        {
            // For columns, but fifth for remark.
            if (reader.ColumnCount < 4)
            {
                throw new Exception($"{new { reader.ColumnCount }} < 4 at {new { reader.CurrentLine }}.");
            }

            return new Model
            {
                CultureCodeA = reader[0],
                WordInCultureA = reader[1],
                CultureCodeB = reader[2],
                WordInCultureB = reader[3]
            };
        }
    }
}