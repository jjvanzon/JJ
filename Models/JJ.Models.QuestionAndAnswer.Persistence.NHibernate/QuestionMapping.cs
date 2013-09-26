using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.Persistence.NHibernate
{
    public class QuestionMapping : ClassMap<Question>
    {
        public QuestionMapping()
        {
            Id(x => x.ID);

            Map(x => x.Text);

            References(x => x.QuestionType, ColumnNames.QuestionTypeID);
            References(x => x.Source, ColumnNames.SourceID);

            HasMany(x => x.Answers).KeyColumn(ColumnNames.QuestionID).Inverse();
            HasMany(x => x.QuestionCategories).KeyColumn(ColumnNames.QuestionID).Inverse();
            HasMany(x => x.QuestionLinks).KeyColumn(ColumnNames.QuestionID).Inverse();
        }
    }
}