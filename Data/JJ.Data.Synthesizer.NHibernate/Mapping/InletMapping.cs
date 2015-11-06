﻿using FluentNHibernate.Mapping;
using JJ.Data.Synthesizer.NHibernate.Names;

namespace JJ.Data.Synthesizer.NHibernate.Mapping
{
    public class InletMapping : ClassMap<Inlet>
    {
        public InletMapping()
        {
            Id(x => x.ID).GeneratedBy.Assigned();
            Map(x => x.Name);
            Map(x => x.ListIndex);
            References(x => x.Operator, ColumnNames.OperatorID);
            References(x => x.InputOutlet, ColumnNames.InputOutletID);
        }
    }
}
