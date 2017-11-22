using JJ.Framework.Data;
using JJ.Framework.Data.NHibernate;

namespace JJ.Data.QuestionAndAnswer.NHibernate.Repositories
{
	public class SourceRepository : DefaultRepositories.SourceRepository
	{
		private new readonly NHibernateContext _context;

		public SourceRepository(IContext context)
			: base(context)
		{
			_context = (NHibernateContext)context;
		}

		public override Source TryGetByIdentifier(string identifier)
		{
			return _context.Session.QueryOver<Source>()
								   .Where(x => x.Identifier == identifier)
								   .SingleOrDefault();
		}
	}
}
