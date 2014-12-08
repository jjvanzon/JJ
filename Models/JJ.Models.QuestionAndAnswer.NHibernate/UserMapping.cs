using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.NHibernate
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            Id(x => x.ID);

            Map(x => x.DisplayName);
            Map(x => x.UserName);
            Map(x => x.Password);
            Map(x => x.SecuritySalt);

            HasMany(x => x.Runs).KeyColumn(ColumnNames.UserID).Inverse();
            HasMany(x => x.UserAnswers).KeyColumn(ColumnNames.UserID).Inverse();
            HasMany(x => x.AsLastModifiedByInQuestions).KeyColumn(ColumnNames.ModifiedByUserID).Inverse();
            HasMany(x => x.AsFlaggedByInQuestionFlags).KeyColumn(ColumnNames.FlaggedByUserID).Inverse();
            HasMany(x => x.AsLastModifiedByInQuestionFlags).KeyColumn(ColumnNames.LastModifiedByUserID).Inverse();
        }
    }
}
