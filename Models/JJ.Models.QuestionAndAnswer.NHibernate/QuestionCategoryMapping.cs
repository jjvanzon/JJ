using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.NHibernate
{
    public class QuestionCategoryMapping : ClassMap<QuestionCategory>
    {
        public QuestionCategoryMapping()
        {
            Id(x => x.ID);

            References(x => x.Category, ColumnNames.CategoryID);
            References(x => x.Question, ColumnNames.QuestionID);
        }
    }
}