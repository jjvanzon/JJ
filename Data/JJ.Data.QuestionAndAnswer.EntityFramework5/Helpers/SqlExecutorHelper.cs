using JJ.Data.QuestionAndAnswer.Sql;
using JJ.Framework.Data;
using JJ.Framework.Data.EntityFramework5;
using JJ.Framework.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Data.QuestionAndAnswer.EntityFramework5.Helpers
{
    internal static class SqlExecutorHelper
    {
        public static QuestionAndAnswerSqlExecutor CreateQuestionAndAnswerSqlExecutor(IContext context)
        {
            EntityFramework5Context castedContext = (EntityFramework5Context)context;
            SqlConnection sqlConnection = castedContext.Context.Database.Connection as SqlConnection;
            if (sqlConnection == null)
            {
                throw new Exception("EntityFramework5Context.Context.Database.Connection must be an SqlConnection.");
            }
            var sqlExecutor = new QuestionAndAnswerSqlExecutor(new SqlExecutor(sqlConnection));
            return sqlExecutor;
        }
    }
}
