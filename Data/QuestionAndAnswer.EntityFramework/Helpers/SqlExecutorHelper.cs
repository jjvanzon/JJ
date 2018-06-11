using System.Data.SqlClient;
using JJ.Data.QuestionAndAnswer.Sql;
using JJ.Framework.Data;
using JJ.Framework.Data.EntityFramework;
using JJ.Framework.Data.SqlClient;
using JJ.Framework.Exceptions.TypeChecking;

namespace JJ.Data.QuestionAndAnswer.EntityFramework.Helpers
{
    internal static class SqlExecutorHelper
    {
        public static QuestionAndAnswerSqlExecutor CreateQuestionAndAnswerSqlExecutor(IContext context)
        {
            var entityFrameworkContext = (EntityFrameworkContext)context;

            if (!(entityFrameworkContext.Context.Database.Connection is SqlConnection sqlConnection))
            {
                throw new IsNotTypeException<SqlConnection>(() => entityFrameworkContext.Context.Database.Connection);
            }

            ISqlExecutor sqlExecutor = SqlExecutorFactory.CreateSqlExecutor(sqlConnection);
            var sqlExecutor2 = new QuestionAndAnswerSqlExecutor(sqlExecutor);
            return sqlExecutor2;
        }
    }
}