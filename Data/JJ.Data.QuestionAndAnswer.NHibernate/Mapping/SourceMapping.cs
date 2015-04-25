using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using JJ.Data.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Mapping
{
    public class SourceMapping : ClassMap<Source>
    {
        public SourceMapping()
        {
            Id(x => x.ID);

            Map(x => x.Identifier);
            Map(x => x.Description);
            Map(x => x.Url);
            Map(x => x.IsActive);

            HasMany(x => x.Questions).KeyColumn(ColumnNames.QuestionID).Inverse();
        }
    }
}