using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using FluentNHibernate;

namespace JJ.Models.QuestionAndAnswer.Persistence.NHibernate
{
    public class TextualQuestionMapping : ClassMap<TextualQuestion>
    {
        public TextualQuestionMapping()
        {
            Id(x => x.ID);
            Map(x => x.Text);

            References(x => x.Category, ColumnNames.CategoryID);
            References(x => x.Source, ColumnNames.SourceID);

           /*HasOne(Reveal.Property("Client"))
                .Constrained()
                .ForeignKey();*/

            //HasOne(x => x.TextualAnswer).Constrained().ForeignKey();
            //HasOne(x => x.TextualAnswer).ForeignKey();

            //HasOne(x => x.TextualAnswer).Constrained().ForeignKey();

            //Map(x => x.TextualAnswer, ColumnNames.TextualQuestionID);

            //HasOne(x => x.TextualAnswer).Cascade.All();

            //HasMany(x => x.TextualAnswers).KeyColumn(ColumnNames.TextualQuestionID).Inverse();
            HasMany(x => x.TextualAnswers).KeyColumn(ColumnNames.TextualQuestionID).Inverse();
        }
    }
}