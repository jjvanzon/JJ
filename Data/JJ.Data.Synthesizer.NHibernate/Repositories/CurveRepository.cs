﻿using JJ.Data.Synthesizer.NHibernate.Helpers;
using JJ.Data.Synthesizer.SqlClient;
using JJ.Framework.Data;
using JJ.Framework.Data.NHibernate;
using JJ.Framework.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Data.Synthesizer.NHibernate.Repositories
{
    public class CurveRepository : JJ.Data.Synthesizer.DefaultRepositories.CurveRepository
    {
        public CurveRepository(IContext context)
            : base(context)
        { }
    }
}
