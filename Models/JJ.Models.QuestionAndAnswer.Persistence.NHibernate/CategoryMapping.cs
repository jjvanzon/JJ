using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace JJ.Models.QuestionAndAnswer.Persistence.NHibernate
{
    public class CategoryMapping : ClassMap<Category>
    {
        public CategoryMapping()
        {
            Id(x => x.ID);
            Map(x => x.Name);
        }
    }
}