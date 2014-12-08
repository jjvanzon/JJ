using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.NHibernate
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