using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using JJ.Data.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Mapping
{
    public class AnswerMapping : ClassMap<Answer>
    {
        public AnswerMapping()
        {
            Id(x => x.ID);
            Map(x => x.Text);
            Map(x => x.IsCorrectAnswer);

            References(x => x.Question, ColumnNames.QuestionID);
        }
    }
}