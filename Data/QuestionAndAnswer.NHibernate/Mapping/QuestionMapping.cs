﻿using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using JJ.Data.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Mapping
{
    public class QuestionMapping : ClassMap<Question>
    {
        public QuestionMapping()
        {
            Id(x => x.ID);

            Map(x => x.Text);
            Map(x => x.IsManual);
            Map(x => x.IsActive);

            References(x => x.QuestionType, ColumnNames.QuestionTypeID);
            References(x => x.Source, ColumnNames.SourceID);

            References(x => x.LastModifiedByUser, ColumnNames.LastModifiedByUserID);

            HasMany(x => x.Answers).KeyColumn(ColumnNames.QuestionID).Inverse();
            HasMany(x => x.QuestionCategories).KeyColumn(ColumnNames.QuestionID).Inverse();
            HasMany(x => x.QuestionLinks).KeyColumn(ColumnNames.QuestionID).Inverse();
            HasMany(x => x.UserAnswers).KeyColumn(ColumnNames.QuestionID).Inverse();
            HasMany(x => x.QuestionFlags).KeyColumn(ColumnNames.QuestionID).Inverse();
        }
    }
}