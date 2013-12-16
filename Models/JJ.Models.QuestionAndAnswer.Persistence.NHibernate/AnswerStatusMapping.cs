using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.Persistence.NHibernate
{
    public class AnswerStatusMapping : ClassMap<AnswerStatus>
    {
        public AnswerStatusMapping()
        {
            Id(x => x.ID);
            Map(x => x.Description);

            HasMany(x => x.UserAnswers).KeyColumn(ColumnNames.AnswerStatusID).Inverse();
        }
    }
}