using JJ.Framework.Data;
using JJ.Framework.Data.NHibernate;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Repositories
{
	public class QuestionFlagRepository : DefaultRepositories.QuestionFlagRepository
	{
		private new readonly NHibernateContext _context;

		public QuestionFlagRepository(IContext context)
			: base(context)
		{
			_context = (NHibernateContext)context;
		}

		public override QuestionFlag TryGetByCriteria(int questionID, int flaggedByUserID)
		{
			Question q = null;
			User u = null;

			return _context.Session.QueryOver<QuestionFlag>()
								   .JoinAlias(x => x.Question, () => q)
								   .JoinAlias(x => x.FlaggedByUser, () => u)
								   .Where(() => q.ID == questionID)
								   .Where(() => u.ID == flaggedByUserID)
								   .SingleOrDefault();
		}
	}
}