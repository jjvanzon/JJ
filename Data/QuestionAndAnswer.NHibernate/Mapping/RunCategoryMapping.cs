using FluentNHibernate.Mapping;
using JetBrains.Annotations;
using JJ.Data.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Mapping
{
    [UsedImplicitly]
	public class RunCategoryMapping : ClassMap<RunCategory>
	{
		public RunCategoryMapping()
		{
			Id(x => x.ID);

			References(x => x.Category, ColumnNames.CategoryID);
			References(x => x.Run, ColumnNames.RunID);
		}
	}
}