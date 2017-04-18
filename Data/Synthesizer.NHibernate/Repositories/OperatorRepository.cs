﻿using System.Collections.Generic;
using System.Linq;
using JJ.Data.Synthesizer.Entities;
using JJ.Data.Synthesizer.NHibernate.Helpers;
using JJ.Framework.Data;
using JJ.Framework.Data.NHibernate;

namespace JJ.Data.Synthesizer.NHibernate.Repositories
{
    public class OperatorRepository : DefaultRepositories.OperatorRepository
    {
        private new readonly NHibernateContext _context;

        public OperatorRepository(IContext context) 
            : base(context)
        {
            _context = (NHibernateContext)context;
        }

        public override IList<Operator> GetManyByOperatorTypeID(int operatorTypeID)
        {
            return _context.Session.QueryOver<Operator>()
                                   .Where(x => x.OperatorType.ID == operatorTypeID)
                                   .List();
        }

        public override IList<Operator> GetManyByOperatorTypeID_AndSingleDataKeyAndValue(int operatorTypeID, string dataKey, string dataValue)
        {
            var sqlExecutor = SqlExecutorHelper.CreateSynthesizerSqlExecutor(_context);
            
            int[] ids = sqlExecutor.Operator_GetIDs_ByOperatorTypeID_AndSingleDataKeyAndValue(operatorTypeID, dataKey, dataValue).ToArray();

            IList<Operator> entities = _context.Session.QueryOver<Operator>()
                                               .WhereRestrictionOn(x => x.ID)
                                               .IsIn(ids)
                                               .List();
            return entities;
        }
    }
}
