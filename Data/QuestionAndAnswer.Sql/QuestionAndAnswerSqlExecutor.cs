using System.Collections.Generic;
using JJ.Framework.Data.SqlClient;
using JJ.Framework.Exceptions.Basic;

namespace JJ.Data.QuestionAndAnswer.Sql
{
	public class QuestionAndAnswerSqlExecutor
	{
		private readonly ISqlExecutor _sqlExecutor;

		public QuestionAndAnswerSqlExecutor(ISqlExecutor sqlExecutor) => _sqlExecutor = sqlExecutor ?? throw new NullException(() => sqlExecutor);

	    public IEnumerable<int> Question_GetPageOfIDs(int firstIndex, int count)
			=> _sqlExecutor.ExecuteReader<int>(SqlEnum.Question_GetPageOfIDs, new { firstIndex, count });

		public int? Question_TryGetRandomID() => (int?)_sqlExecutor.ExecuteScalar(SqlEnum.Question_TryGetRandomID);

		public int Question_CountAll() => (int)_sqlExecutor.ExecuteScalar(SqlEnum.Question_CountAll);
	}
}