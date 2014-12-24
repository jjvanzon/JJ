using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using JJ.Models.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Models.QuestionAndAnswer.NHibernate.Mapping
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