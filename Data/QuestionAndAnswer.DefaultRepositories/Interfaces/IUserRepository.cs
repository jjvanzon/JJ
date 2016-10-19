using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Data;

namespace JJ.Data.QuestionAndAnswer.DefaultRepositories.Interfaces
{
    public interface IUserRepository : IRepository<User, int>
    {
        User TryGetByUserName(string userName);
        User GetByUserName(string userName);
    }
}
