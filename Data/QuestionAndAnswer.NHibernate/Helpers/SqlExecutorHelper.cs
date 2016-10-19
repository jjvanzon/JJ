using JJ.Data.QuestionAndAnswer.Sql;
using JJ.Framework.Data;
using JJ.Framework.Data.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Helpers
{
    internal static class SqlExecutorHelper
    {
        public static QuestionAndAnswerSqlExecutor CreateQuestionAndAnswerSqlExecutor(IContext context)
        {
            if (context == null) throw new NullException(() => context);

            var castedContext = (NHibernateContext)context;
            var sqlExecutor = new QuestionAndAnswerSqlExecutor(new NHibernateSqlExecutor(castedContext.Session));
            return sqlExecutor;
        }
    }
}
