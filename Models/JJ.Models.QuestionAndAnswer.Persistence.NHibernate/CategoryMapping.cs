using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.Persistence.NHibernate
{
    public class CategoryMapping : ClassMap<Category>
    {
        public CategoryMapping()
        {
            Id(x => x.ID);
            Map(x => x.Name);

            References(x => x.ParentCategory, ColumnNames.ParentCategoryID);
            HasMany(x => x.SubCategories).KeyColumn(ColumnNames.ParentCategoryID).Inverse();
            HasMany(x => x.CategoryQuestions).KeyColumn(ColumnNames.QuestionCategoryID).Inverse();
        }
    }
}