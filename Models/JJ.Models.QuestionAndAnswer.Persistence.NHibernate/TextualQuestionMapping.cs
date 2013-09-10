using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.Persistence.NHibernate
{
    public class TextualQuestionMapping : ClassMap<TextualQuestion>
    {
        public TextualQuestionMapping()
        {
            Id(x => x.ID);
            Map(x => x.Text);

            //References(x => x.TextualAnswer, ColumnNames.TextualAnswerID);
        }
    }
}