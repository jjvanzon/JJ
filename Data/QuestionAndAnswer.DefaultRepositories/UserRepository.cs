using JJ.Framework.Data;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using System.Linq;
using JJ.Framework.Exceptions;
using JJ.Framework.Exceptions.Aggregates;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
	public class UserRepository : RepositoryBase<User, int>, IUserRepository
	{
		public UserRepository(IContext context)
			: base(context)
		{ }

		public virtual User TryGetByUserName(string userName)
		{
			return _context.Query<User>().Where(x => x.UserName == userName).SingleOrDefault();
		}

		public User GetByUserName(string userName)
		{
			User user = TryGetByUserName(userName);

			if (user == null)
			{
				throw new NotFoundException<User>(new { userName });
			}

			return user;
		}
	}
}
