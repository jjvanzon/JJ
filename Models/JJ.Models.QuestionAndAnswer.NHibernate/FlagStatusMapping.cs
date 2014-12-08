using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.NHibernate
{
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