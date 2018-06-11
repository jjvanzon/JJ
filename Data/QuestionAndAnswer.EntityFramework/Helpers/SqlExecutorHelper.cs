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
			var entityFramework5Context = (EntityFrameworkContext)context;
			if (!(entityFramework5Context.Context.Database.Connection is SqlConnection sqlConnection))
			{
				throw new IsNotTypeException<SqlConnection>(() => entityFramework5Context.Context.Database.Connection);
			}
			ISqlExecutor sqlExecutor = SqlExecutorFactory.CreateSqlExecutor(sqlConnection);
			var sqlExecutor2 = new QuestionAndAnswerSqlExecutor(sqlExecutor);
			return sqlExecutor2;
		}
	}
}
