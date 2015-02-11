using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using JJ.Persistence.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Persistence.QuestionAndAnswer.NHibernate.Mapping
{
    public class RunCategoryMapping : ClassMap<RunCategory>
    {
        public RunCategoryMapping()
        {
            Id(x => x.ID);

            References(x => x.Category, ColumnNames.CategoryID);
            References(x => x.Run, ColumnNames.RunID);
        }
    }
}