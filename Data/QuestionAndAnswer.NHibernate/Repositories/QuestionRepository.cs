using JJ.Framework.Data;
using JJ.Framework.Data.NHibernate;
using JJ.Data.QuestionAndAnswer.Sql;
using System.Collections.Generic;
using System.Linq;
using JJ.Data.QuestionAndAnswer.NHibernate.Helpers;
using NHibernate.Transform;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Repositories
{
	public class QuestionRepository : DefaultRepositories.QuestionRepository
	{
		private new NHibernateContext _context;

		public QuestionRepository(IContext context)
			: base(context)
		{
			_context = (NHibernateContext)context;
		}

		public override IList<Question> GetAll() => _context.Session.QueryOver<Question>().List();

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

		public override IList<Question> GetBySourceID(int sourceID)
		{
			return _context.Session.QueryOver<Question>()
								   .Where(x => x.Source.ID == sourceID)
								   .List();
		}

		public override IList<Question> GetByCriteria(bool mustFilterByFlagStatusID, int? flagStatusID)
		{
			if (!mustFilterByFlagStatusID)
			{
				return _context.Session.QueryOver<Question>().List();
			}
			else
			{
				if (!flagStatusID.HasValue)
				{
					QuestionFlag qf = null;

					return _context.Session.QueryOver<Question>()
										   .Left.JoinAlias(x => x.QuestionFlags, () => qf)
										   // TODO: Test if this works. It means 'has no question flags'
										   .Where(x => qf == null)
										   .TransformUsing(Transformers.DistinctRootEntity)
										   .List();
				}
				else
				{
					Question q = null;
					QuestionFlag fs = null;
					QuestionFlag qf = null;

					// TODO: Test this query.
					return _context.Session.QueryOver(() => q)
										   .JoinAlias(() => q.QuestionFlags, () => qf)
										   .JoinAlias(() => qf.FlagStatus, () => fs)
										   .Where(() => fs.ID == flagStatusID)
										   .TransformUsing(Transformers.DistinctRootEntity)
										   .List();
				}
			}
		}

		public override IList<Question> GetPage(int firstIndex, int count)
		{
			QuestionAndAnswerSqlExecutor sqlExecutor = SqlExecutorHelper.CreateQuestionAndAnswerSqlExecutor(_context);

			IList<Question> list = new List<Question>(count);

			IList<int> ids = sqlExecutor.Question_GetPageOfIDs(firstIndex, count).ToArray();
			foreach (int id in ids)
			{
				Question entity = Get(id);
				list.Add(entity);
			}

			return list;
		}

		public override int Count()
		{
			return _context.Session.QueryOver<Question>().RowCount();
		}
	}
}
