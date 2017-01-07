using FluentNHibernate.Mapping;
using JJ.Data.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Mapping
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