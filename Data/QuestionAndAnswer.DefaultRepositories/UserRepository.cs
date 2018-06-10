using System.Linq;
using JetBrains.Annotations;
using JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces;
using JJ.Framework.Data;
using JJ.Framework.Exceptions.Aggregates;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories
{
    public class UserRepository : RepositoryBase<User, int>, IUserRepository
    {
        [UsedImplicitly]
        public UserRepository(IContext context) : base(context) { }

        public virtual User TryGetByUserName(string userName) => _context.Query<User>().Where(x => x.UserName == userName).SingleOrDefault();

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