﻿using JJ.Framework.Persistence;
using JJ.Framework.Persistence.NHibernate;
using JJ.Framework.Persistence.SqlClient;
using JJ.Persistence.Synthesizer.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Persistence.Synthesizer.NHibernate.Repositories
{
    public class SampleRepository : JJ.Persistence.Synthesizer.DefaultRepositories.SampleRepository
    {
        private SynthesizerSqlExecutor _synthesizerSqlExecutor;

        public SampleRepository(IContext context)
            : base(context)
        { 
            NHibernateContext nhibernateContext = (NHibernateContext)context;
            ISqlExecutor sqlExecutor = new NHibernateSqlExecutor(nhibernateContext.Session);
            _synthesizerSqlExecutor = new SynthesizerSqlExecutor(sqlExecutor);
        }

        public override byte[] GetBinary(int id)
        {
            byte[] binary = _synthesizerSqlExecutor.Sample_TryGetBinary(id);
            if (binary == null)
            {
                throw new Exception(String.Format("Binary is null for Sample with id '{0}' or the Sample does not exist.", id));
            }
            return binary;
        }

        public override void SetBinary(int id, byte[] bytes)
        {
            Sample sample = Get(id); // Force an exception when the entity does not exist.

            _synthesizerSqlExecutor.Sample_TrySetBinary(id, bytes);
        }
    }
}
