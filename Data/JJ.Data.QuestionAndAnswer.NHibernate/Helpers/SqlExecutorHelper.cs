using JJ.Data.QuestionAndAnswer.Sql;
using JJ.Framework.Data;
using JJ.Framework.Data.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Helpers
{
    internal static class SqlExecutorHelper
    {
        public static QuestionAndAnswerSqlExecutor CreateQuestionAndAnswerSqlExecutor(IContext context)
        {
            var castedContext = (NHibernateContext)context;
            var sqlExecutor = new QuestionAndAnswerSqlExecutor(new NHibernateSqlExecutor(castedContext.Session));
            return sqlExecutor;
        }
    }
}
