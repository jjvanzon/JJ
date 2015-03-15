﻿using FluentNHibernate.Mapping;
using JJ.Persistence.Synthesizer.NHibernate.Names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Persistence.Synthesizer.NHibernate.Mapping
{
    public class ChannelMapping : ClassMap<Channel>
    {
        public ChannelMapping()
        {
            Id(x => x.ID);
            Map(x => x.Name);
            Map(x => x.IndexNumber);
            HasMany(x => x.SpeakerSetupChannels).KeyColumn(ColumnNames.ChannelID).Inverse();
        }
    }
}
