﻿using FluentNHibernate.Mapping;
using JJ.Data.Synthesizer.NHibernate.Names;

namespace JJ.Data.Synthesizer.NHibernate.Mapping
{
    public class DimensionMapping : ClassMap<Dimension>
    {
        public DimensionMapping()
        {
            Id(x => x.ID).GeneratedBy.Assigned();
            Map(x => x.Name);
            HasMany(x => x.AsXDimensionInCurves).KeyColumn(ColumnNames.XDimensionID).Inverse();
            HasMany(x => x.AsYDimensionInCurves).KeyColumn(ColumnNames.YDimensionID).Inverse();
        }
    }
}
