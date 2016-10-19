using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using JJ.Data.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Mapping
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