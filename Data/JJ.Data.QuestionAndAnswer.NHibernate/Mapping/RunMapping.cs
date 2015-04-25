using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using JJ.Data.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Mapping
{
    public class RunMapping : ClassMap<Run>
    {
        public RunMapping()
        {
            Id(x => x.ID);
            Map(x => x.Description);
            Map(x => x.IsActive);

            References(x => x.User, ColumnNames.UserID);

            HasMany(x => x.UserAnswers).KeyColumn(ColumnNames.RunID).Inverse();
            HasMany(x => x.RunCategories).KeyColumn(ColumnNames.RunID).Inverse();
        }
    }
}