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
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Data.QuestionAndAnswer.EntityFramework5.Helpers
{
    internal static class SqlExecutorHelper
    {
        public static QuestionAndAnswerSqlExecutor CreateQuestionAndAnswerSqlExecutor(IContext context)
        {
            EntityFramework5Context entityFramework5Context = (EntityFramework5Context)context;
            SqlConnection sqlConnection = entityFramework5Context.Context.Database.Connection as SqlConnection;
            if (sqlConnection == null)
            {
                throw new IsNotTypeException<SqlConnection>(() => entityFramework5Context.Context.Database.Connection);
            }
            ISqlExecutor sqlExecutor = SqlExecutorFactory.CreateSqlExecutor(sqlConnection);
            var sqlExecutor2 = new QuestionAndAnswerSqlExecutor(sqlExecutor);
            return sqlExecutor2;
        }
    }
}
