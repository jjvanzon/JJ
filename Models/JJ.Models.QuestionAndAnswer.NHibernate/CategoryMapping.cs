using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.NHibernate
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
            HasMany(x => x.CategoryQuestions).KeyColumn(ColumnNames.QuestionCategoryID).Inverse();
            HasMany(x => x.CategoryRuns).KeyColumn(ColumnNames.QuestionRunID).Inverse();
        }
    }
}