using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.Persistence.NHibernate
{
    public class SourceMapping : ClassMap<Source>
    {
        public SourceMapping()
        {
            Id(x => x.ID);

            Map(x => x.Identifier);
            Map(x => x.Description);
            Map(x => x.Url);

            HasMany(x => x.Questions).KeyColumn(ColumnNames.QuestionID).Inverse();
        }
    }
}