using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.Persistence.NHibernate
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