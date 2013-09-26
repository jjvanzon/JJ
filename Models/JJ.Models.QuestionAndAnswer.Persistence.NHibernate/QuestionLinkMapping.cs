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
            Map(x => x.Link);

            References(x => x.Question, ColumnNames.QuestionID);
        }
    }
}