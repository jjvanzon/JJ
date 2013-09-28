using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.Persistence.NHibernate
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