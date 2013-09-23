using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.Persistence.NHibernate
{
    public class SourceMapping : ClassMap<Source>
    {
        public SourceMapping()
        {
            Id(x => x.ID);
            Map(x => x.Identifier);
            Map(x => x.Description);
            Map(x => x.Link);
        }
    }
}