using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.Persistence.NHibernate
{
    public class QuestionTypeMapping : ClassMap<QuestionType>
    {
        public QuestionTypeMapping()
        {
            Id(x => x.ID);
            Map(x => x.Name);

            HasMany(x => x.Questions).KeyColumn(ColumnNames.QuestionID).Inverse();
        }
    }
}