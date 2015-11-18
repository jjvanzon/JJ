﻿using JJ.Framework.Data;
using JJ.Data.Synthesizer.Memory.Helpers;

namespace JJ.Data.Synthesizer.Memory.Repositories
{
    public class OutletTypeRepository : DefaultRepositories.OutletTypeRepository
    {
        public OutletTypeRepository(IContext context)
            : base(context)
        {
            RepositoryHelper.EnsureEnumEntity(this, 1, "Signal");
            RepositoryHelper.EnsureEnumEntity(this, 2, "Frequencies");
            RepositoryHelper.EnsureEnumEntity(this, 3, "Volumes");
       }
    }
}