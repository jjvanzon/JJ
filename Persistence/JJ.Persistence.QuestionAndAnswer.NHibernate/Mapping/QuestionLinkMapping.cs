using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using JJ.Persistence.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Persistence.QuestionAndAnswer.NHibernate.Mapping
{
    public class QuestionLinkMapping : ClassMap<QuestionLink>
    {
        public QuestionLinkMapping()
        {
            Id(x => x.ID);
            Map(x => x.Url);
            Map(x => x.Description);

            References(x => x.Question, ColumnNames.QuestionID);
        }
    }
}