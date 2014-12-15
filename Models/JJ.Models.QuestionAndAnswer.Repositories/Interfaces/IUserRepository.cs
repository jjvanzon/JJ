using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Persistence;

namespace JJ.Models.QuestionAndAnswer.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User, int>
    {
        User TryGetByUserName(string userName);
        User GetByUserName(string userName);
    }
}
