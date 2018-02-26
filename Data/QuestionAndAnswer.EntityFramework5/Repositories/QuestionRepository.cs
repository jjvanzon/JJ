using JJ.Framework.Data;
using JJ.Data.QuestionAndAnswer.Sql;
using JJ.Data.QuestionAndAnswer.EntityFramework5.Helpers;

namespace JJ.Data.QuestionAndAnswer.EntityFramework5.Repositories
{
	public class QuestionRepository : DefaultRepositories.QuestionRepository
	{
		public QuestionRepository(IContext context)
			: base(context)
		{
		}

		public override Question TryGetRandomQuestion()
		{
			QuestionAndAnswerSqlExecutor sqlExecutor = SqlExecutorHelper.CreateQuestionAndAnswerSqlExecutor(_context);
			int? randomID = sqlExecutor.Question_TryGetRandomID();
			if (randomID.HasValue)
			{
				return Get(randomID.Value);
			}
			else
			{
				return null;
			}
		}

		public override int Count()
		{
			QuestionAndAnswerSqlExecutor sqlExecutor = SqlExecutorHelper.CreateQuestionAndAnswerSqlExecutor(_context);
			return sqlExecutor.Question_CountAll();
		}
	}
}
