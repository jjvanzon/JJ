using JJ.Framework.Data;
using JJ.Framework.Data.NHibernate;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Repositories
{
	public class UserRepository : DefaultRepositories.UserRepository
	{
		private new readonly NHibernateContext _context;

		public UserRepository(IContext context)
			: base(context)
		{
			_context = (NHibernateContext)context;
		}

		public override User TryGetByUserName(string userName)
		{
			return _context.Session.QueryOver<User>()
								   .Where(x => x.UserName == userName)
								   .SingleOrDefault();
		}
	}
}
