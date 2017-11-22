using JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Models;
using JJ.Framework.IO;
using JJ.Framework.Exceptions;
using System.Collections.Generic;
using System.IO;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3.Selectors
{
	public class W3CSpecCss3_PropertyIndex_CsvSelector : ISelector<PropertyAspectsImportModel>
	{
		public IEnumerable<PropertyAspectsImportModel> GetSelection(Stream stream)
		{
			if (stream == null) throw new NullException(() => stream);

			using (CsvReader reader = new CsvReader(stream))
			{
				// Skip header.
				reader.Read();

				while (reader.Read())
				{
					PropertyAspectsImportModel model = CreateImportModel(reader);

					yield return model;
				}
			}
		}

		private PropertyAspectsImportModel CreateImportModel(CsvReader reader)
		{
			return new PropertyAspectsImportModel
			{
				PropertyName = reader[0],
				PossibleValues = reader[1],
				InitialValue = reader[2],
				AppliesTo = reader[3],
				IsInherited = reader[4],
				Percentages = reader[5],
				Media = reader[6],
			};
		}
	}
}
