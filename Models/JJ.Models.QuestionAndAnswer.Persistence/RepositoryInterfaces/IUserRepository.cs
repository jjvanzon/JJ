using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Models.QuestionAndAnswer.Persistence.RepositoryInterfaces
{
    public interface IUserRepository
    {
        User TryGetByUserName(string userName);
        User GetByUserName(string userName);
    }
}
