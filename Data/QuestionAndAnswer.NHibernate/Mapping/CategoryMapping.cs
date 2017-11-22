using FluentNHibernate.Mapping;
using JJ.Data.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Mapping
{
	public class CategoryMapping : ClassMap<Category>
	{
		public CategoryMapping()
		{
			Id(x => x.ID);

			Map(x => x.Identifier);
			Map(x => x.Description);
			Map(x => x.IsActive);

			References(x => x.ParentCategory, ColumnNames.ParentCategoryID);

			HasMany(x => x.SubCategories).KeyColumn(ColumnNames.ParentCategoryID).Inverse();
			HasMany(x => x.CategoryQuestions).KeyColumn(ColumnNames.CategoryID).Inverse();
			HasMany(x => x.CategoryRuns).KeyColumn(ColumnNames.CategoryID).Inverse();
		}
	}
}