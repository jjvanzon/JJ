using FluentNHibernate.Mapping;
using JetBrains.Annotations;
using JJ.Data.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Mapping
{
    [UsedImplicitly]
	public class FlagStatusMapping : ClassMap<FlagStatus>
	{
		public FlagStatusMapping()
		{
			Id(x => x.ID);

			Map(x => x.Description);

			HasMany(x => x.QuestionFlags).KeyColumn(ColumnNames.FlagStatusID).Inverse();
		}
	}
}