﻿using FluentNHibernate.Mapping;
using JJ.Data.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Mapping
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