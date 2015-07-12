﻿using FluentNHibernate.Mapping;
using JJ.Data.Synthesizer.NHibernate.Names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Data.Synthesizer.NHibernate.Mapping
{
    public class NodeMapping : ClassMap<Node>
    {
        public NodeMapping()
        {
            Id(x => x.ID).GeneratedBy.Assigned();
            Map(x => x.Time);
            Map(x => x.Value);
            Map(x => x.Direction);

            References(x => x.Curve, ColumnNames.CurveID);
            References(x => x.NodeType, ColumnNames.NodeTypeID);
        }
    }
}
