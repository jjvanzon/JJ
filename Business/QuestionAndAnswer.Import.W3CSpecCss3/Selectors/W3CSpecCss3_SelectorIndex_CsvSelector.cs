using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;
using JJ.Framework.IO;
using JJ.Framework.Exceptions;
using System.Collections.Generic;
using System.IO;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Selectors
{
	public class W3CSpecCss3_SelectorIndex_CsvSelector : ISelector<W3CSpecCss3_SelectorIndex_ImportModel>
	{
		public IEnumerable<W3CSpecCss3_SelectorIndex_ImportModel> GetSelection(Stream stream)
		{
			if (stream == null) throw new NullException(() => stream);

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
