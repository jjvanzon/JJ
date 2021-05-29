using FluentNHibernate.Mapping;
using JetBrains.Annotations;
using JJ.Data.QuestionAndAnswer.NHibernate.Names;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Mapping
{
    [UsedImplicitly]
    public class QuestionTypeMapping : ClassMap<QuestionType>
    {
        public QuestionTypeMapping()
        {
            Id(x => x.ID);

            Map(x => x.Name);

            HasMany(x => x.Questions).KeyColumn(ColumnNames.QuestionTypeID).Inverse();
        }
    }
}