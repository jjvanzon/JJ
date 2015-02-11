using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using JJ.Persistence.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Persistence.QuestionAndAnswer.NHibernate.Mapping
{
    public class QuestionFlagMapping : ClassMap<QuestionFlag>
    {
        public QuestionFlagMapping()
        {
            Id(x => x.ID);
            Map(x => x.Comment);
            Map(x => x.DateTime);

            References(x => x.Question, ColumnNames.QuestionID);
            References(x => x.FlaggedByUser, ColumnNames.FlaggedByUserID);
            References(x => x.LastModifiedByUser, ColumnNames.LastModifiedByUserID);
            References(x => x.FlagStatus, ColumnNames.FlagStatusID);
        }
    }
}